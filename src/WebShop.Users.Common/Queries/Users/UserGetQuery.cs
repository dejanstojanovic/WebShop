using WebShop.Users.Common.Dtos.Users;
using System;
using System.Collections.Generic;
using System.Text;
using WebShop.Messaging;

namespace WebShop.Users.Common.Queries
{
    public class UserGetQuery:IQuery<UserInfoDetailsViewDto>
    {
        public Guid Id { get; set; }
    }
}
