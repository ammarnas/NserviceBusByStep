using Billing.Messages;
using NServiceBus.Logging;
using NServiceBus;
using Sales.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus.Unicast.Subscriptions;

namespace Shipping
{
    public class ShippingPolicy : 
        Saga<ShippingPolicyData>,
         IAmStartedByMessages<OrderPlaced>, // This can start the saga
    //   IHandleMessages<OrderBilled>       // But surely, not this one!?
        IAmStartedByMessages<OrderBilled>  // I can start the saga too!

    //  1
    // IHandleMessages<OrderPlaced>,
    // IHandleMessages<OrderBilled>
    {
        static ILog log = LogManager.GetLogger<ShippingPolicy>();

        public Task Handle(OrderPlaced message, IMessageHandlerContext context)
        {
            log.Info($"Received OrderPlaced, OrderId = {message.OrderId}");
            Data.IsOrderPlaced = true;
            return ProcessOrder(context);
        }

        public Task Handle(OrderBilled message, IMessageHandlerContext context)
        {
            log.Info($"Received OrderBilled, OrderId = {message.OrderId}");
            Data.IsOrderBilled = true;
            return ProcessOrder(context);
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ShippingPolicyData> mapper)
        {
            mapper.MapSaga(sagaData => sagaData.OrderId)
                    .ToMessage<OrderPlaced>(message => message.OrderId)
                    .ToMessage<OrderBilled>(message => message.OrderId);
        }
        private async Task ProcessOrder(IMessageHandlerContext context)
        {
            if (Data.IsOrderPlaced && Data.IsOrderBilled)
            {
                await context.SendLocal(new ShipOrder() { OrderId = Data.OrderId });
                MarkAsComplete();
            }
        }
    }
}
