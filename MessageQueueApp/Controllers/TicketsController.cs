using MassTransit;
using MessageQueueApp.Events;
using Microsoft.AspNetCore.Mvc;

namespace MessageQueueApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromServices] IBus bus, [FromBody] CreateTicketEvent @event)
        {
            await bus.Publish(@event);

            return Created();
        }
    }
}
