using WebShop.Users.Services.Commands;
using WebShop.Users.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebShop.Messaging;

namespace WebShop.Users.Services.Handlers
{
    public class UpdatePasswordHandler : ICommandHandler<UpdatePasswordCommand>
    {
        private readonly IApplicationUsersUnitOfWork _applicationUsersUnitOfWork;
        public UpdatePasswordHandler(IApplicationUsersUnitOfWork applicationUsersUnitOfWork)
        {
            _applicationUsersUnitOfWork = applicationUsersUnitOfWork;
        }
        public async Task HandleAsync(UpdatePasswordCommand command)
        {
            await _applicationUsersUnitOfWork.ApplicationUsers.UpdatePassword(command.Id, command.OldPassword, command.NewPassword);
            await _applicationUsersUnitOfWork.SaveAsync();
        }

        public void Dispose() {
            _applicationUsersUnitOfWork.Dispose();
        }
    }
}
