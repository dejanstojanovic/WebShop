using WebShop.Users.Common.Commands;
using WebShop.Users.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebShop.Messaging;

namespace WebShop.Users.Common.Handlers
{
    public class UpdateProfileHandler : ICommandHandler<UpdateUserInfoCommand>
    {
        private readonly IApplicationUsersUnitOfWork _applicationUsersUnitOfWork;
        public UpdateProfileHandler(IApplicationUsersUnitOfWork applicationUsersUnitOfWork)
        {
            _applicationUsersUnitOfWork = applicationUsersUnitOfWork;
        }

        public async Task HandleAsync(UpdateUserInfoCommand command)
        {
            await _applicationUsersUnitOfWork.ApplicationUsers.UpdateProfile(
                userId: command.Id,
                firstName: command.FirstName,
                lastName: command.LastName,
                occupation: command.Occupation,
                education: command.Education
                );

            await _applicationUsersUnitOfWork.SaveAsync();

        }

        public void Dispose() {
            _applicationUsersUnitOfWork.Dispose();
        }
    }
}
