﻿using System;
 using System.Net.Mail;
 using System.Threading.Tasks;
 using Microsoft.Extensions.Configuration;

 namespace BeautySalonSystem.Email
{
    public interface IEmailService
    {
        Task SendEmail(string templateName, string receiverEmail);
    }
    
    public class EmailService : IEmailService
    {
        public EmailService(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public IConfiguration Configuration {
            get;
            set;
        }
        public Task SendEmail(string templateName, string receiverEmail)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(Configuration["EmailSettings:Server"]);

                mail.From = new MailAddress(Configuration["EmailSettings:SenderEmail"]);
                mail.To.Add(receiverEmail);
                mail.Subject = "Часът ви е потвърден";
                mail.Body = "Вашият час е потвърден.";

                SmtpServer.Port = int.Parse(Configuration["EmailSettings:Port"]);
                SmtpServer.Credentials = new System.Net.NetworkCredential(Configuration["EmailSettings:SenderEmail"], Configuration["EmailSettings:Password"]);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                Console.WriteLine("Email Sent");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
            return Task.CompletedTask;
        }
    }
}