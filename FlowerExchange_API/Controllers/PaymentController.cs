﻿using Application.Payment.Commands.CreatePaymentUrl;
using Application.Payment.Commands.CreatePostServicePaymentTransaction;
using Application.Payment.Commands.CreateTransaction;
using Application.Payment.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Quic;
using Application.Payment.Commands.CreateFlowerServicePaymentTransaction;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/payment")]
    public class PaymentController : APIControllerBase
    {
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(ILogger<PaymentController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePaymentUrl([FromBody] WalletDepositRequest request)
        {
            try
            {
                var requestUrl = HttpContext.Request;
                //var currentPath = $"{requestUrl.Scheme}://{requestUrl.Host}{requestUrl.PathBase}{requestUrl.Path}";
                var currentPath = $"{requestUrl.Scheme}://{requestUrl.Host}";

                var response = await Mediator.Send(new CreatePaymentUrlCommand(request, HttpContext, currentPath));

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("ipn")]
        public async Task<IActionResult> PaymentCallBack()
        {
            var query = Request.Query;
            if (query == null)
                return Ok(new IPNResponseVNPAY
                {
                    RspCode = "01",
                    Message = "The queries is null!"
                });

            try
            {
                var response = await Mediator.Send(new CreateTransactionCommand(query));
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new IPNResponseVNPAY
                {
                    RspCode = "01",
                    Message = ex.Message
                });
            }
        }

        [HttpPost("post-service")]
        [Authorize]
        public async Task<IActionResult> CreatePostServicePayment([FromBody] PostServicePaymentRequest request)
        {
            try
            {
                // Take userid from token
                var userIdClaim = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Jti);
                if (userIdClaim == null)
                {
                    return Unauthorized();
                }
                var userId = Guid.Parse(userIdClaim.Value);

                await Mediator.Send(new CreatePostServicePaymentTransactionCommand(request, userId));
                return Ok(new {message = "Create payment post service success!"});
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        
        [HttpPost("flower-service")]
        [Authorize]
        public async Task<IActionResult> CreateFlowerServicePayment([FromBody] FlowerServicePaymentRequest request)
        {
            try
            {
                // Take userid from token
                var userIdClaim = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Jti);
                if (userIdClaim == null)
                {
                    return Unauthorized();
                }
                var userId = Guid.Parse(userIdClaim.Value);

                await Mediator.Send(new CreateFlowerServicePaymentTransactionCommand(request.PostId, userId));
                return Ok(new {message = "Create payment flower service success!"});
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
