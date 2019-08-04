using System;
using System.Collections.Generic;
using System.Text;
using WebShop.Messaging;

namespace WebShop.Users.Common.Queries
{
    public class UserImageGetQuery:IQuery<byte[]>
    {
        public Guid UserId { get; set; }

        public UserImageGetQuery()
        {

        }

        public UserImageGetQuery(Guid userId)
        {
            this.UserId = userId;
        }
    }
}
