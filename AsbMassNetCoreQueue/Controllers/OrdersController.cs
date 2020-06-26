using System;
using System.Threading.Tasks;
using AsbMassNetCoreQueue.Contracts;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace AsbMassNetCoreQueue.Sender.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController
        : ControllerBase
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly Random _random;

        public OrdersController(
        ISendEndpointProvider sendEndpointProvider)
        {
            _sendEndpointProvider = sendEndpointProvider;
            _random = new Random();
        }

        [HttpPost()]
        public async Task<IActionResult> NewOrder()
        {
            var sendEndpoint =
             await _sendEndpointProvider.GetSendEndpoint(
                 new Uri("sb://servicebusqueuesnetcore.servicebus.windows.net/new-orders"));

            await sendEndpoint.Send(
                                        new Order
                                        {
                                            OrderId = Guid.NewGuid(),
                                            Timestamp = DateTime.UtcNow,
                                            PublicOrderId = _random.Next(1, 999).ToString()
                                        });

            return Ok();
        }
    }
}
