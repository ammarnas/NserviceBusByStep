using Messages;
using NServiceBus.Logging;
using Sales.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales
{
    class BuyersRemorsePolicy : 
        Saga<BuyersRemorseState>,
        IAmStartedByMessages<PlaceOrder>,
        IHandleTimeouts<BuyersRemorseIsOver>,
        IHandleMessages<CancelOrder>
    {
        static ILog log = LogManager.GetLogger<BuyersRemorsePolicy>();

        public async Task Handle(PlaceOrder message, IMessageHandlerContext context)
        {
            log.Info($"Received PlaceOrder, OrderId = {message.OrderId}");
            Data.OrderId = message.OrderId;

            log.Info($"Starting cool down period for order #{Data.OrderId}.");
            await RequestTimeout(context, TimeSpan.FromSeconds(20),new BuyersRemorseIsOver());
        }

        public Task Handle(CancelOrder message, IMessageHandlerContext context)
        {
            log.Info($"Order #{message.OrderId} was cancelled.");

            //TODO: Possibly publish an OrderCancelled event?

            MarkAsComplete();

            return Task.CompletedTask;
        }

        public async Task Timeout(BuyersRemorseIsOver timeout, IMessageHandlerContext context)
        {
            log.Info($"Cooling down period for order #{Data.OrderId} has elapsed.");

            var orderPlaced = new OrderPlaced
            {
                OrderId = Data.OrderId
            };

            await context.Publish(orderPlaced);

            MarkAsComplete();
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<BuyersRemorseState> mapper)
        {
            mapper.MapSaga(saga => saga.OrderId)
            .ToMessage<PlaceOrder>(message => message.OrderId)
            .ToMessage<CancelOrder>(message => message.OrderId);
        }
    }
    class BuyersRemorseIsOver
    {
    }
    public class BuyersRemorseState : ContainSagaData
    {
        public string OrderId { get; set; }
    }
}
