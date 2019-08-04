using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebShop.Messaging;
using WebShop.Users.Common.Queries;
using WebShop.Users.Data;

namespace WebShop.Users.Common.Handlers.Users
{
    public class UserRolesGetHandler : IQueryHandler<UserRolesGetQuery, IEnumerable<String>>
    {
        private readonly IApplicationUsersUnitOfWork _applicationUsersUnitOfWork;
        public UserRolesGetHandler(IApplicationUsersUnitOfWork applicationUsersUnitOfWork)
        {
            _applicationUsersUnitOfWork = applicationUsersUnitOfWork;
        }
        public async Task<IEnumerable<string>> HandleAsync(UserRolesGetQuery query)
        {
            return await _applicationUsersUnitOfWork.ApplicationUsers.GetRoles(query.UserId);
        }
    }
}
