using Messages;
using NServiceBus;
using NServiceBus.Logging;
using Sales.Messages;

namespace ClientUI
{
    internal class Program
    {
        static async Task Main()
        {
            Console.Title = "ClientUI";

            var endpointConfiguration = new EndpointConfiguration("ClientUI");

            // Choose JSON to serialize and deserialize messages
            endpointConfiguration.UseSerialization<SystemJsonSerializer>();

            //endpointConfiguration.EnableInstallers();

            //endpointConfiguration.AuditProcessedMessagesTo("audit");
            //endpointConfiguration.SendHeartbeatTo("Particular.ServiceControl");



            //var metrics = endpointConfiguration.EnableMetrics();
            //metrics.SendMetricDataToServiceControl("Particular.Monitoring", TimeSpan.FromMilliseconds(500));

            //var transport = endpointConfiguration.UseTransport<LearningTransport>();
            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            //transport.UseConventionalRoutingTopology(QueueType.Quorum);
            //transport.ConnectionString("host=localhost");

            var routing = transport.Routing();
            routing.RouteToEndpoint(typeof(PlaceOrder), "Sales");
            routing.RouteToEndpoint(typeof(CancelOrder), "Sales");


            

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            //Console.WriteLine("Press Enter to exit...");
            //Console.ReadLine();

            await RunLoop(endpointInstance)
                .ConfigureAwait(false);

            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
        static ILog log = LogManager.GetLogger<Program>();

        static async Task RunLoop(IEndpointInstance endpointInstance)
        {
            var lastOrder = string.Empty;

            while (true)
            {
                log.Info("Press 'P' to place an order, 'C' to cancel last order, or 'Q' to quit.");
                var key = Console.ReadKey();
                Console.WriteLine();

                switch (key.Key)
                {
                    case ConsoleKey.P:
                        // Instantiate the command
                        var command = new PlaceOrder
                        {
                            OrderId = Guid.NewGuid().ToString()
                        };

                        // Send the command
                        log.Info($"Sending PlaceOrder command, OrderId = {command.OrderId}");
                        await endpointInstance.Send(command)
                            .ConfigureAwait(false);

                        lastOrder = command.OrderId; // Store order identifier to cancel if needed.
                        break;

                    case ConsoleKey.C:
                        var cancelCommand = new CancelOrder
                        {
                            OrderId = lastOrder
                        };
                        await endpointInstance.Send(cancelCommand)
                            .ConfigureAwait(false);
                        log.Info($"Sent a correlated message to {cancelCommand.OrderId}");
                        break;

                    case ConsoleKey.Q:
                        return;

                    default:
                        log.Info("Unknown input. Please try again.");
                        break;
                }
            }
        }
    }
}