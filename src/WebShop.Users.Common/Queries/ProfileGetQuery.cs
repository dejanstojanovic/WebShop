using WebShop.Users.Common.Dtos.ApplicationUser;
using System;
using System.Collections.Generic;
using System.Text;
using WebShop.Messaging;

namespace WebShop.Users.Common.Queries
{
    public class ProfileGetQuery:IQuery<UserInfoDetailsViewDto>
    {
        public Guid Id { get; set; }
    }
}
