﻿using System;
using System.Threading.Tasks;

namespace BeautySalonSystem.Email
{
    public interface IEmailService
    {
        Task SendEmail(string templateName, string receiverEmail);
    }
    
    public class EmailService : IEmailService
    {
        public Task SendEmail(string templateName, string receiverEmail)
        {
            Console.WriteLine("Email Sent");
            return Task.CompletedTask;
        }
    }
}