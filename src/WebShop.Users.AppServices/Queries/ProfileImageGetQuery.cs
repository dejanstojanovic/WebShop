using System;
using System.Collections.Generic;
using System.Text;
using WebShop.Messaging;

namespace WebShop.Users.AppServices.Queries
{
    public class ProfileImageGetQuery:IQuery<byte[]>
    {
        public Guid UserId { get; set; }

        public ProfileImageGetQuery()
        {

        }

        public ProfileImageGetQuery(Guid userId)
        {
            this.UserId = userId;
        }
    }
}
