using Billing.Messages;
using NServiceBus;
using NServiceBus.Logging;

namespace Shipping
{
    public class OrderBilledHandler
    //: IHandleMessages<OrderBilled>    
    {
        static ILog log = LogManager.GetLogger<OrderPlacedHandler>();

        public Task Handle(OrderBilled message, IMessageHandlerContext context)
        {
            log.Info($"Shipping - Received OrderBilled, OrderId = {message.OrderId} - Charging credit card...");

            return Task.CompletedTask;
        }

      

    }
}
