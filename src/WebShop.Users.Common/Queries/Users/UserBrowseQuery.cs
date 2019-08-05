using WebShop.Users.Common.Dtos.Users;
using System;
using System.Collections.Generic;
using System.Text;
using WebShop.Messaging;

namespace WebShop.Users.Common.Queries
{
    public class UserBrowseQuery : IQuery<IEnumerable<UserInfoDetailsViewDto>>
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Email { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public DateTime DateOfBirth { get; set; }
        public String Occupation { get; set; }
        public String Education { get; set; }


        public UserBrowseQuery(UserFilterDto userFilter)
        {
            this.Email = userFilter.Email;
            this.DateOfBirth = userFilter.DateOfBirth;
            this.Education = userFilter.Education;
            this.FirstName = userFilter.FirstName;
            this.LastName = userFilter.LastName;
            this.PageIndex = userFilter.PageIndex;
            this.PageSize = userFilter.PageSize;
        }

    }
}
