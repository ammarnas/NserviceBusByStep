using NServiceBus;
using NServiceBus.Logging;
using Sales.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping
{
    public class OrderPlacedHandler
        //: IHandleMessages<OrderPlaced>
    {
        static ILog log = LogManager.GetLogger<OrderPlacedHandler>();

        public Task Handle(OrderPlaced message, IMessageHandlerContext context)
        {
            log.Info($"Shipping - Received OrderPlaced, OrderId = {message.OrderId} - Charging credit card...");

            return Task.CompletedTask;
        }
    }
}
