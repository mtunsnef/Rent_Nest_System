using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RentNest.Common.Helper.Mail;
using MimeKit;
using MailKit.Security;
namespace RentNest.Service.Implements
{
    public class MailService
    {
        MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        public async Task<bool> SendMail(MailContent mailContent)
        {
            var email = new MimeMessage();
            email.Sender = new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail);
            email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));

            email.To.Add(new MailboxAddress(mailContent.To, mailContent.To));
            email.Subject = mailContent.Subject;

            var builder = new BodyBuilder();

            builder.HtmlBody = mailContent.Body;

            email.Body = builder.ToMessageBody();
            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(email);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi xác thực SMTP: " + ex.Message);
                return false;
            }

            smtp.Disconnect(true);
            return true;
        }
    }
}
public class MailContent
{
    public string? To { get; set; }
    public string? Subject { get; set; }
    public string? Body { get; set; }
}