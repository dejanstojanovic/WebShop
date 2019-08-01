using WebShop.Email.Api.Events;
using WebShop.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShop.Mailing;

namespace WebShop.Email.Api.Handlers
{
    public class UserRegisteredEventHandler: IEventHandler<UserRegisteredEvent>
    {
        readonly IMailService _mailService;
        public UserRegisteredEventHandler(IMailService mailer)
        {
            _mailService = mailer;
        }

        public async Task HandleAsync(UserRegisteredEvent @event)
        {
            await _mailService.SendAsync(new MailMessage()
            {
                Body = $"Welcome to WebShop {@event.FirstName} {@event.LastName}",
                From = "noreply@WebShop.net",
                Subject = "Welcome to WebShop",
                To = new String[] { @event.Email }
            });
        }

        public void Dispose()
        {

        }
    }
}
