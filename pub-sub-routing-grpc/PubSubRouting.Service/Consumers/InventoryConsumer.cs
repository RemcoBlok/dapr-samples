using MassTransit;
using PubSubRouting.Interfaces;

namespace PubSubRouting.Service.Consumers
{
    public class InventoryConsumer :
        IConsumer<Product>,
        IConsumer<Widget>,
        IConsumer<Gadget>
    {
        public Task Consume(ConsumeContext<Product> context)
        {
            Product product = context.Message;
            Console.WriteLine("Subscriber received (default) : " + product);
            return Task.CompletedTask;
        }

        public Task Consume(ConsumeContext<Widget> context)
        {
            Widget widget = context.Message;
            Console.WriteLine("Subscriber received : " + widget);
            return Task.CompletedTask;
        }

        public Task Consume(ConsumeContext<Gadget> context)
        {
            Gadget gadget = context.Message;
            Console.WriteLine("Subscriber received : " + gadget);
            return Task.CompletedTask;
        }
    }
}
