using System;
using System.Collections.Generic;
using System.Text;
using WebShop.Messaging;

namespace WebShop.Users.Common.Queries
{
    public class UserRolesGetQuery:IQuery<IEnumerable<String>>
    {
        public Guid UserId { get; set; }
    }
}
