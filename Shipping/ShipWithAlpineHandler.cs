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
    public class ShipWithAlpineHandler
    {
        static ILog log = LogManager.GetLogger<ShipWithAlpineHandler>();

        const int MaximumTimeAlpineMightRespond = 30;
        static Random random = new Random();

        public async Task Handle(ShipWithAlpine message, IMessageHandlerContext context)
        {
            var waitingTime = random.Next(MaximumTimeAlpineMightRespond);

            log.Info($"ShipWithAlpineHandler: Delaying Order [{message.OrderId}] {waitingTime} seconds.");

            await Task.Delay(waitingTime * 1000);

            await context.Reply(new ShipmentAcceptedByAlpine());
        }
    }
}
