using NServiceBus.Logging;
using NServiceBus;
using Sales.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping
{
    class ShipOrderHandler 
        //: IHandleMessages<ShipOrder>
    {
        //static ILog log = LogManager.GetLogger<ShipOrderHandler>();

        //public Task Handle(ShipOrder message, IMessageHandlerContext context)
        //{
        //    log.Info($"Order [{message.OrderId}] - Successfully shipped.");
        //    return Task.CompletedTask;
        //}
    }
}
