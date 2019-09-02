using System;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace ECommerce_Shop.Helpers
{
    public static class SendEmail
    {
        //Send Mail based on subject, HTML body content and TO(Email) passed | Godaddy
        public static bool Send(string subject, string body, string to)
        {
            bool response;
            try
            { 
                var client = new SendGridClient("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");
                var from = new EmailAddress("noreply@bosskinds.com", "BossKinds");
                var toAddress = new EmailAddress(to, "BossKinds");
                var plainTextContent = string.Empty;
                var htmlContent = body;
                var msg = MailHelper.CreateSingleEmail(from, toAddress, subject, plainTextContent, htmlContent);
                var sendEmailAsync = client.SendEmailAsync(msg);
                response = true;
            }
            catch (Exception e)
            {
                response = false;
            }
            return response;
        }

    }
}