using Domain.Entities;
using Infrastructure.EmailProvider.Gmail;
using Microsoft.Extensions.Options;
using System.Configuration;

namespace Presentation.OptionsSetup
{
    public class EmailOptionsSetup : IConfigureOptions<GmailOptions>
    {
        private const string SectionName = "GmailOptions";
        private readonly IConfiguration _configuration;
        
        public EmailOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(GmailOptions options)
        {
            options.HostType = Enum.Parse<EmailHostType>(_configuration.GetValue<string>("GmailOptions:HostType"));
            options.SmtpServer = _configuration.GetValue<string>("GmailOptions:SmtpServer");
            options.Port = _configuration.GetValue<int>("GmailOptions:Port");
            options.AccountName = _configuration.GetValue<string>("GmailOptions:AccountName");
            options.Password = _configuration.GetValue<string>("GmailOptions:Password");
            options.EnableSSL = _configuration.GetValue<bool>("GmailOptions:EnableSSL");
            options.UseCredential = _configuration.GetValue<bool>("GmailOptions:UseCredential");
            options.FromEmailAddress = _configuration.GetValue<string>("GmailOptions:FromEmailAddress");
            options.FromDisplayName = _configuration.GetValue<string>("GmailOptions:FromDisplayName");

            //OR
            //_configuration.GetSection(SectionName).Bind(options);
        }
    }
}
