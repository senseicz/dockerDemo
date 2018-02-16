using System;
using System.Threading.Tasks;
using MassTransit;
using Messages;

namespace EmailService
{
    public class UserProfileHandler : IConsumer<ISendUserProfile>
    {
        public Task Consume(ConsumeContext<ISendUserProfile> context)
        {
            Console.WriteLine($"Adding user {context.Message.UserProfile.Name} {context.Message.UserProfile.Email}");
            return Task.CompletedTask;
        }
    }
 
}
