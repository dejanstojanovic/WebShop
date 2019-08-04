using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebShop.Messaging;
using WebShop.Users.Common.Commands;
using WebShop.Users.Data;

namespace WebShop.Users.Common.Handlers.Users
{
    public class UserRoleAddHandler : ICommandHandler<AddUserRoleCommand>
    {

        private readonly IApplicationUsersUnitOfWork _applicationUsersUnitOfWork;
        public UserRoleAddHandler(IApplicationUsersUnitOfWork applicationUsersUnitOfWork)
        {
            _applicationUsersUnitOfWork = applicationUsersUnitOfWork;
        }


        public async Task HandleAsync(AddUserRoleCommand message)
        {
           await _applicationUsersUnitOfWork.ApplicationUsers.AddRole(message.UserId, message.RoleName);
        }

        public void Dispose()
        {
            _applicationUsersUnitOfWork.Dispose();
        }
    }
}
