using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GetBee.Email.Api.Events;
using GetBee.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;

namespace GetBee.Email.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailsController : ControllerBase
    {
        readonly MessageSubscriber<UserRegisteredEvent> _messageSubscriber;

        public EmailsController(MessageSubscriber<UserRegisteredEvent> messageSubscriber)
        {
            _messageSubscriber = messageSubscriber;
        }

        [HttpPost]
        public async Task<IActionResult> SendAsync(UserRegisteredEvent accountCreatedEvent)
        {
            await _messageSubscriber.HandleAsync(accountCreatedEvent);
            return Ok();
        }
   }
}
