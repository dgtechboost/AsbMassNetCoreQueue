using System.Threading.Tasks;
using AsbMassNetCoreQueue.Contracts;
using MassTransit;

namespace AsbMassNetCoreQueue.Consumer
{
    public class OrderConsumer 
        : IConsumer<Order>
    {
        public Task Consume(ConsumeContext<Order> context)
        {
            System.Threading.Thread.Sleep(60000);//Wait for one minnute

            //by returning a completed task service bus removes message from the queue
            return Task.CompletedTask;
        }
    }
}
