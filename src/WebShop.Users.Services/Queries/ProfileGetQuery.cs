using WebShop.Users.Dtos.ApplicationUser;
using System;
using System.Collections.Generic;
using System.Text;
using WebShop.Messaging;

namespace WebShop.Users.Services.Queries
{
    public class ProfileGetQuery:IQuery<ProfileView>
    {
        public Guid Id { get; set; }
    }
}
