using NServiceBus;
using Sales.Messages;
using System.Net;

namespace Shipping
{
    internal class Program
    {
        static async Task Main()
        {
            Console.Title = "Shipping";

            var endpointConfiguration = new EndpointConfiguration("Shipping");
            // Choose JSON to serialize and deserialize messages
            endpointConfiguration.UseSerialization<SystemJsonSerializer>();

            var transport = endpointConfiguration.UseTransport<LearningTransport>();

            var routing = transport.Routing();
            routing.RouteToEndpoint(typeof(ShipOrder), "Shipping");
            routing.RouteToEndpoint(typeof(ShipWithMaple), "Shipping");
            routing.RouteToEndpoint(typeof(ShipWithAlpine), "Shipping");

            var persistence = endpointConfiguration.UsePersistence<LearningPersistence>();

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();

            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}