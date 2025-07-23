using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace TechXpress.Core.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string toEmail, string subject, string htmlMessage);
        Task SendDynamicEmailAsync(string toEmail, string subject, string templateId, object templateData);
    }

    public class SendGridEmailSender : IEmailSender
    {
        private readonly IConfiguration _config;
        public SendGridEmailSender(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string confirmationLink)
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("keronedaaedu@gmail.com", "TechXpress");
            var to = new EmailAddress(toEmail);

            var msg = MailHelper.CreateSingleTemplateEmail(from, to, "d-8f54da15ccfb4583851af2129f8a8991", new
            {
                confirmation_link = confirmationLink
            });

            var response = await client.SendEmailAsync(msg);
            System.Diagnostics.Debug.WriteLine($"SendGrid Status Code: {response.StatusCode}");
            System.Diagnostics.Debug.WriteLine($"SendGrid Body: {await response.Body.ReadAsStringAsync()}");
        }

        public async Task SendDynamicEmailAsync(string toEmail, string subject, string templateId, object templateData)
        {
            var apiKey = _config["SendGrid:ApiKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("keronedaaedu@gmail.com", "TechXpress");
            var to = new EmailAddress(toEmail);

            var msg = MailHelper.CreateSingleTemplateEmail(from, to, templateId, templateData);
            await client.SendEmailAsync(msg);
        }
    }
}
