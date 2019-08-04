using WebShop.Users.Common.Commands;
using WebShop.Users.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebShop.Messaging;

namespace WebShop.Users.Common.Handlers
{
    public class UserUpdatePasswordHandler : ICommandHandler<UpdateUserPasswordCommand>
    {
        private readonly IApplicationUsersUnitOfWork _applicationUsersUnitOfWork;
        public UserUpdatePasswordHandler(IApplicationUsersUnitOfWork applicationUsersUnitOfWork)
        {
            _applicationUsersUnitOfWork = applicationUsersUnitOfWork;
        }
        public async Task HandleAsync(UpdateUserPasswordCommand command)
        {
            await _applicationUsersUnitOfWork.ApplicationUsers.UpdatePassword(command.UserId, command.OldPassword, command.NewPassword);
            await _applicationUsersUnitOfWork.SaveAsync();
        }

        public void Dispose() {
            _applicationUsersUnitOfWork.Dispose();
        }
    }
}
