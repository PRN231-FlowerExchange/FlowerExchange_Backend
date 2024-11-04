using Application.UserIdentity.Commands.ConfirmEmail;
using Application.UserIdentity.Commands.ExternalLogin;
using Application.UserIdentity.Commands.ForgotPassword;
using Application.UserIdentity.Commands.Login;
using Application.UserIdentity.Commands.Logout;
using Application.UserIdentity.Commands.RefreshUserAccessToken;
using Application.UserIdentity.Commands.Register;
using Application.UserIdentity.Commands.SendConfirmEmail;
using Application.UserIdentity.DTOs;
using Application.UserIdentity.Queries.CurrentUser;
using Application.UserIdentity.Queries.ExternalLogin;
using Application.UserIdentity.Queries.GetUser;
using Domain.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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


        [HttpGet("current-user")]
        [Authorize]
        public async Task<CurrentUserModel> CurrentUser()
        {
            return await Mediator.Send(new CurrentUserQuery());
        }

        [HttpGet("google-login")]
        public async Task<IActionResult> LoginByGoogle([FromQuery(Name = "redirect_uri")] string redirect_uri = null, [FromQuery(Name = "purpose_get_token")] bool purpose_get_token = false, [FromQuery(Name = "provider")] string externalLoginProvider = "Google")
        {
            if(HttpContext.Request.Cookies.Any(x => x.Key.Equals("Identity.External")))
            {
                AuthenticatedToken token = await Mediator.Send(new CallbackExternalLoginCommand());
                if(redirect_uri != null)
                {
                    return Redirect(redirect_uri + "?accessToken=" + token.AccessToken + "&refreshToken=" + token.RefreshToken + "&tokenType=" + token.TokenType);
                }
                else
                {
                    return Unauthorized();
                }
            }
            else
            {
                string callbackTokenUrl = Url.Action(nameof(this.LoginByGoogle), "Auth") + "?redirect_uri=" + redirect_uri + "&purpose_get_token=" + true;
                Console.WriteLine("Callback: ", callbackTokenUrl);
                AuthenticationProperties properties = await this.Mediator.Send(new ExternalLoginRedirectQuery() { AuthenticationScheme = externalLoginProvider, RedirectUrl = callbackTokenUrl });
                ChallengeResult challengeResult = Challenge(properties, externalLoginProvider);
                return challengeResult;
            }

            
        }

        [HttpGet("external-login-provider-options")]
        public async Task<IActionResult> GetExternalLoginProviderOptions()
        {
            IList<AuthenticationScheme> schemes = await Mediator.Send(new ExternalLoginProvidersQuery());
            return Ok(schemes.Select(x => new { x.Name, }).ToList());

        }


        [HttpPost("send-email-reset-password-code")]
        public async Task<IActionResult> SendEmaiResetPasswordCode(SendEmailResetPasswordCodeCommand command)
        {
            await Mediator.Send(command);
            return Ok("Code has been sent already");

        }

        [HttpPut("verify-reset-password-code")]
        public async Task<IActionResult> VeriryEmaiResetPasswordCode(VerifyResetPasswordCodeCommand command)
        {
            bool success = await Mediator.Send(command);
            return Ok("Verify reset password code success");
        }

        [HttpGet("logout-current-user")]
        [Authorize]
        public async Task<IActionResult> LogoutCurrentUser()
        {
            await Mediator.Send(new RevokeTokenAfterLogOutCommand());
            return Ok("Log Out Successfully");
        }

        [HttpGet("user-id/{userId}")]
        public async Task<CurrentUserModel> GetUserById([FromRoute] Guid userId)
        {
            return await Mediator.Send(new GetUserByIDPublicQuery() { UserID = userId });
        }

    }


}
