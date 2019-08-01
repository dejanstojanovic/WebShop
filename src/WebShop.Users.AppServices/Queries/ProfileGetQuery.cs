using WebShop.Users.Contracts.ApplicationUser;
using System;
using System.Collections.Generic;
using System.Text;
using WebShop.Messaging;

namespace WebShop.Users.AppServices.Queries
{
    public class ProfileGetQuery:IQuery<ProfileView>
    {
        public Guid Id { get; set; }
    }
}
