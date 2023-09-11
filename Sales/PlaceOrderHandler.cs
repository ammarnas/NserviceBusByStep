using ClientUI;
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
    //We now have a working buyer's remorse policy so we don't need our existing PlaceOrderhandler.
    //Delete this class from the Sales project.
    public class PlaceOrderHandler 
        //: IHandleMessages<PlaceOrder>
    {
        //static readonly ILog log = LogManager.GetLogger<PlaceOrderHandler>();

        //public Task Handle(PlaceOrder message, IMessageHandlerContext context)
        //{
        //    log.Info($"Slaes - Received PlaceOrder, OrderId = {message.OrderId}");
        //    //throw new Exception("BOOM");

        //    //This is normally where some business logic would occur

        //   var orderPlaced = new OrderPlaced
        //   {
        //       OrderId = message.OrderId
        //   };
        //    return context.Publish(orderPlaced);
        //}
    }
}
