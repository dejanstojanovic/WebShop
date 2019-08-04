using AutoMapper;
using WebShop.Users.Common.Queries;
using WebShop.Users.Common.Dtos.Users;
using WebShop.Users.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebShop.Messaging;

namespace WebShop.Users.Common.Handlers
{
    public class UserGetHandler : IQueryHandler<UserGetQuery, UserInfoDetailsViewDto>
    {

        private readonly IApplicationUsersUnitOfWork _applicationUsersUnitOfWork;
        private readonly IMapper _mapper;
        public UserGetHandler(IApplicationUsersUnitOfWork applicationUsersUnitOfWork, IMapper mapper)
        {
            _applicationUsersUnitOfWork = applicationUsersUnitOfWork;
            _mapper = mapper;
        }

        public async Task<UserInfoDetailsViewDto> HandleAsync(UserGetQuery query)
        {
            return _mapper.Map<UserInfoDetailsViewDto>(await _applicationUsersUnitOfWork.ApplicationUsers.GetUser(query.Id));
        }
    }
}
