using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MassTransit;
using Messages;
using MimeKit;
using Newtonsoft.Json;

namespace EmailService
{
    public class UserProfileHandler : IConsumer<ISendUserProfile>
    {
        private readonly string _mailHost = "mail";
        private readonly int _mailPort = 1025;

        public async Task Consume(ConsumeContext<ISendUserProfile> context)
        {
            Console.WriteLine("<++++++ Message RECEIVED over RabbitMq/MassTransit.");

            await SendEmail(context.Message.UserProfile);

            await Task.Delay(2000);

            Console.WriteLine("<++++++ Email sent.");

            //return Task.CompletedTask;
        }

        private async Task SendEmail(UserProfile profile)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Docker demo sender", "dockerdemo@test.com"));
            message.To.Add(new MailboxAddress("Docker demo receiver", "dockerdemoreceiver@fake.net"));
            message.Subject = "User profile data";

            message.Body = new TextPart("plain")
            {
                Text = "Yo, here's your user profile data: " + JsonConvert.SerializeObject(profile)
            };
            using (var mailClient = new SmtpClient())
            {
                await mailClient.ConnectAsync(_mailHost, _mailPort, SecureSocketOptions.None);
                await mailClient.SendAsync(message);
                await mailClient.DisconnectAsync(true);
            }
        }
    }
 
}
