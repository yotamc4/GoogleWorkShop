using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace YOTY.Service.Core.Services.Mail
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        private readonly MailSecrets _mailSecrets;

        public MailService(IOptions<MailSettings> mailSettings, IOptions<MailSecrets> mailSecrets)
        {
            _mailSettings = mailSettings.Value;
            _mailSecrets = mailSecrets.Value;
        }

        public MailService(MailSettings mailSettings, MailSecrets mailSecrets)
        {
            _mailSettings = mailSettings;
            _mailSecrets = mailSecrets;
        }

        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
            if (mailRequest.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in mailRequest.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSecrets.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
