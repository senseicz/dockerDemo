using MassTransit;

namespace EmailService
{
    class Program
    {
        static void Main(string[] args)
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host("rabbit", "dockerhost", h =>
                {
                    h.Username("docker");
                    h.Password("docker");
                });

                cfg.ReceiveEndpoint(host, "SendUserProfile", e =>
                {
                    e.Consumer<UserProfileHandler>();
                });
            });
            busControl.Start();
        }
    }
}
