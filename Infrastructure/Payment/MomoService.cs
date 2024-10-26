using Domain.Payment;
using Domain.Payment.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Payment
{
    public class MomoService : IMomoService
    {
        private readonly IOptions<MomoOptionModel> _options;

        public MomoService(IOptions<MomoOptionModel> options)
        {
            _options = options;
        }

        public async Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(WalletDeposit model, string currentPath)
        {
            // Get value
            var partnerCode = _options.Value.PartnerCode;
            var requestId = Guid.NewGuid().ToString();
            var amount = (long)model.Amount;
            var orderId = Guid.NewGuid().ToString();
            var orderInfo = $"Nạp {model.Amount} VND vào wallet.";
            var redirectUrl = "https://mealhunt.vercel.app/";
            //var ipnUrl = currentPath + "/api/ipn";
            var ipnUrl = "https://9a43-1-53-159-90.ngrok-free.app/api/ipn";
            var requestType = _options.Value.RequestType;
            var extraData = "";
            var lang = "vi";
            var accessKey = _options.Value.AccessKey;
            var secretKey = _options.Value.SecretKey;
            string signature;

            // Handle raw data
            //var rawData =
            //    $"partnerCode={_options.Value.PartnerCode}&accessKey={_options.Value.AccessKey}&requestId={model.OrderId}&amount={model.Amount}&orderId={model.OrderId}&orderInfo={model.OrderInfo}&returnUrl={returnUrl}&notifyUrl={_options.Value.NotifyUrl}&extraData=";

            var rawData = $"accessKey={accessKey}&amount={amount}&extraData={extraData}&ipnUrl={ipnUrl}&orderId={orderId}&orderInfo={orderInfo}&partnerCode={partnerCode}&redirectUrl={redirectUrl}&requestId={requestId}&requestType={requestType}";

            // Compute signature
            signature = ComputeHmacSha256(rawData, secretKey);

            // Initial client of Momo api
            var client = new RestClient(_options.Value.MomoApiUrl);
            var request = new RestRequest() { Method = Method.Post };
            request.AddHeader("Content-Type", "application/json; charset=UTF-8");

            // Create an object representing the request data
            var requestData = new
            {
                partnerCode,
                requestId,
                amount,
                orderId,
                orderInfo,
                redirectUrl,
                ipnUrl,
                requestType,
                extraData,
                lang,
                signature
            };

            // Add request data
            request.AddParameter("application/json", JsonConvert.SerializeObject(requestData), ParameterType.RequestBody);

            // Execute request
            var response = await client.ExecuteAsync(request);

            return JsonConvert.DeserializeObject<MomoCreatePaymentResponseModel>(response.Content);
        }

        private string ComputeHmacSha256(string message, string secretKey)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);
            var messageBytes = Encoding.UTF8.GetBytes(message);

            byte[] hashBytes;

            using (var hmac = new HMACSHA256(keyBytes))
            {
                hashBytes = hmac.ComputeHash(messageBytes);
            }

            var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            return hashString;
        }
    }
}
