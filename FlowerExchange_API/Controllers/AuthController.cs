using Application.SystemUser.Commands.UpdateUser;
using Application.SystemUser.DTOs;
using Application.UserIdentity.Commands.ConfirmEmail;
using Application.UserIdentity.Commands.Login;
using Application.UserIdentity.Commands.RefreshUserAccessToken;
using Application.UserIdentity.Commands.Register;
using Application.UserIdentity.Commands.SendConfirmEmail;
using Domain.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AuthController : APIControllerBase
    {
        [HttpPost("register")]
        public async Task<string> Register([FromBody] RegisterCommand command)
        {
           return await this.Mediator.Send(command);
           
        }

        [HttpGet("confirm-email-registration")]
        public async Task<string> ConfirmEmailWhenRegister([FromQuery] ConfirmEmailCommand command)
        {
            return await this.Mediator.Send(command);
        }

        [HttpPost("send-email-confirmation")]
        public async Task<string> SendEmailConfirmation([FromQuery] SendConfirmEmailSignUpCommand command)
        {
            return await this.Mediator.Send(command);
        }

        [HttpPost("resend-email-confirmation")]
        public async Task<string> ReSendEmailConfirmation([FromQuery] SendConfirmEmailSignUpCommand command)
        {
            return await this.Mediator.Send(command);
        }

        [HttpPost("login")]
        public async Task<AuthenticatedToken> Loginlogin([FromBody] UsernamePasswordLoginCommand command)
        {
            return await this.Mediator.Send(command);
        }

        [HttpPost("refresh-token")]
        public async Task<AuthenticatedToken> RefreshToken([FromBody] RefreshTokenCommand command)
        {
            return await this.Mediator.Send(command);
        }
        [HttpPut("update-account")]
        public async Task<string> UpdateAccount([FromBody] UpdateUserAccountDTO dto)
        {
            var command = new UpdateUserAccountCommand(dto);
            return await this.Mediator.Send(command);
        }


    }


}
