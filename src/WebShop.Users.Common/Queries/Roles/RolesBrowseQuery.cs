using System;
using System.Collections.Generic;
using System.Text;
using WebShop.Messaging;

namespace WebShop.Users.Common.Queries
{
    public class RolesBrowseQuery:IQuery<IEnumerable<String>>
    {
        public String RoleName { get; set; }
    }
}
