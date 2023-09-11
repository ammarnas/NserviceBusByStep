using Billing.Messages;
using NServiceBus.Logging;
using Sales.Messages;

namespace Billing
{
    public class Program
    {
        static ILog log = LogManager.GetLogger<Program>();
        static async Task Main()
        {
            Console.Title = "Billing";

            var endpointConfiguration = new EndpointConfiguration("Billing");
            // Choose JSON to serialize and deserialize messages
            endpointConfiguration.UseSerialization<SystemJsonSerializer>();

            var transport = endpointConfiguration.UseTransport<LearningTransport>();

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();

            

            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}