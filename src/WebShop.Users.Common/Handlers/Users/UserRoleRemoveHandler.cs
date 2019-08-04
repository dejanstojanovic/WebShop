using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebShop.Messaging;
using WebShop.Users.Common.Commands;
using WebShop.Users.Data;

namespace WebShop.Users.Common.Handlers.Users
{
    public class UserRoleRemoveHandler : ICommandHandler<RemoveUserRoleCommand>
    {
        private readonly IApplicationUsersUnitOfWork _applicationUsersUnitOfWork;
        public UserRoleRemoveHandler(IApplicationUsersUnitOfWork applicationUsersUnitOfWork)
        {
            _applicationUsersUnitOfWork = applicationUsersUnitOfWork;
        }

        public async Task HandleAsync(RemoveUserRoleCommand message)
        {
            await _applicationUsersUnitOfWork.ApplicationUsers.RemoveRole(message.UserId, message.RoleName);
        }

        public void Dispose()
        {
            _applicationUsersUnitOfWork.Dispose();
        }
    }
}
