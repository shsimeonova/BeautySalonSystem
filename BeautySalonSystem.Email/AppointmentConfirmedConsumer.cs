using System.Threading.Tasks;
using BeautySalonSystem.Messages;
using MassTransit;

namespace BeautySalonSystem.Email
{
    public class AppointmentConfirmedConsumer : IConsumer<AppointmentConfirmedMessage>
    {
        private IEmailService _emailService;

        public AppointmentConfirmedConsumer(IEmailService emailService)
        {
            _emailService = emailService;
        }
        
        public async Task Consume(ConsumeContext<AppointmentConfirmedMessage> context)
        {
            var message = context.Message;
            await _emailService.SendEmail("blabla", "blala");
        }
    }
}