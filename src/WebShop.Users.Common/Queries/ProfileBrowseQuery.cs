using WebShop.Users.Common.Dtos.ApplicationUser;
using System;
using System.Collections.Generic;
using System.Text;
using WebShop.Messaging;

namespace WebShop.Users.Common.Queries
{
    public class ProfileBrowseQuery : IQuery<IEnumerable<UserInfoDetailsViewDto>>
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Email { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public DateTime DateOfBirth { get; set; }
        public String Occupation { get; set; }
        public String Education { get; set; }


        public ProfileBrowseQuery(UserInfoViewDto profileBrowse)
        {
            this.Email = profileBrowse.Email;
            this.DateOfBirth = profileBrowse.DateOfBirth;
            this.Education = profileBrowse.Education;
            this.FirstName = profileBrowse.FirstName;
            this.LastName = profileBrowse.LastName;
            this.PageIndex = profileBrowse.PageIndex;
            this.PageSize = profileBrowse.PageSize;
        }

    }
}
