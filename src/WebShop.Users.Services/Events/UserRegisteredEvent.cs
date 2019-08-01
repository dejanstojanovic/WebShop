using WebShop.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebShop.Users.Services.Events
{
    public class UserRegisteredEvent : IEvent
    {
        public Guid Id { get;  }
        public String Email { get; }
        public String FirstName { get;  }
        public String LastName { get; }

        [JsonConstructor]
        public UserRegisteredEvent(Guid id, String email, String firstName, String lastName)
        {
            this.Id = id;
            this.Email = email;
            this.FirstName = firstName;
            this.LastName = lastName;
        }
    }
}
