using Domain.Payment;
using Domain.Payment.Models;
using Infrastructure.Payment.Libraries;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Payment
{
    public class VNPAYService : IVNPAYService
    {
        private readonly IConfiguration _configuration;

        public VNPAYService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreatePaymentUrl(WalletDeposit walletDeposit, HttpContext context, string currentPath)
        {
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            //var tick = DateTime.Now.Ticks.ToString();
            var depositId = Guid.NewGuid().ToString();
            var pay = new VNPAYLibrary();
            var userInfo = "Nap tien vao vi dien tu cua nguoi dung: " + walletDeposit.UserId;
            var description = "So tien " + walletDeposit.Amount + " VND";

            var urlCallBack = "http://localhost:5173/charge/noti";

            pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", (walletDeposit.Amount * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", $"{userInfo}. {description}");
            pay.AddRequestData("vnp_OrderType", "other");
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
            pay.AddRequestData("vnp_TxnRef", depositId);

            var paymentUrl =
                pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);

            return paymentUrl;
        }

        public VNPAYPaymentResponseModel? PaymentExecute(IQueryCollection collections)
        {
            try
            {
                var pay = new VNPAYLibrary();
                var response = pay.GetFullResponseData(collections, _configuration["Vnpay:HashSecret"]);

                // Payment unsuccess
                if (!response.Success)
                {
                    return null;
                }

                if (!response.ResponseCode.Equals("00"))
                {
                    return null;
                }

                // Payment success
                var userInfo = response.OrderInfo.Split('.')[0].Trim();
                var userIdString = userInfo.Split(": ")[1].Trim();

                // Parse user id to Guid
                Guid userId;
                var parseGuidSuccess = Guid.TryParse(userIdString, out userId);
                if (!parseGuidSuccess)
                {
                    throw new Exception("Invalid user id!");
                }

                response.UserId = userId;

                return response;
            }
            catch
            {
                throw;
            }
        }
    }
}
