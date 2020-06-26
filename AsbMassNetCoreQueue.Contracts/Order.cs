using System;

namespace AsbMassNetCoreQueue.Contracts
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public string PublicOrderId { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
