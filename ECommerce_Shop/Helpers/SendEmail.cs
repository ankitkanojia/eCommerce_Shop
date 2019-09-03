using SendGrid;
using SendGrid.Helpers.Mail;
using System;

namespace ECommerce_Shop.Helpers
{
    public static class SendEmail
    {
        //Send Mail based on subject, HTML body content and TO(Email) passed
        public static bool Send(string subject, string body, string toEmail)
        {
            bool response;
            try
            {
                var client = new SendGridClient("YOUR SEND-GRID KEY");
                var from = new EmailAddress("FROM EMAIL", "FROM NAME");
                var toAddress = new EmailAddress(toEmail, "TO NAME");
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