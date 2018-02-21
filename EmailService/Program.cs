using System;
using System.Net.Http;
using System.Threading.Tasks;
using MassTransit;

namespace EmailService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Task.WaitAll(WaitUntilRabbitIsUp());

            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host("rabbit", "dockerdemo", h =>
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

        private static async Task WaitUntilRabbitIsUp()
        {
            var rabbitOn = false;
            var counter = 0;

            while (!rabbitOn)
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(1);
                    Console.WriteLine($"Checking RabbitMq healthcheck enpoint, try: {counter++}");
                    try
                    {
                        var response = await client.GetAsync("http://docker:docker@rabbit:15672/api/healthchecks/node");
                        if (response.IsSuccessStatusCode)
                        {
                            Console.WriteLine("RabbitMq IS UP!");
                            Console.WriteLine(await response.Content.ReadAsStringAsync());
                            rabbitOn = true;
                        }
                        else
                        {
                            Console.WriteLine($"RabbitMq still down, resposne code: {response.StatusCode}");
                            await Task.Delay(5000);
                        }
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine(ex);
                        Console.WriteLine("RabbitMq still down.");
                        await Task.Delay(5000);
                    }
                }
            }
        }
    }
}
