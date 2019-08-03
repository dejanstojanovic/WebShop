using AutoMapper;
using WebShop.Users.Common.Queries;
using WebShop.Users.Common.Dtos.ApplicationUser;
using WebShop.Users.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebShop.Messaging;

namespace WebShop.Users.Common.Handlers
{
    public class ProfileGetHandler : IQueryHandler<ProfileGetQuery, UserInfoDetailsViewDto>
    {

        private readonly IApplicationUsersUnitOfWork _applicationUsersUnitOfWork;
        private readonly IMapper _mapper;
        public ProfileGetHandler(IApplicationUsersUnitOfWork applicationUsersUnitOfWork, IMapper mapper)
        {
            _applicationUsersUnitOfWork = applicationUsersUnitOfWork;
            _mapper = mapper;
        }

        public async Task<UserInfoDetailsViewDto> HandleAsync(ProfileGetQuery query)
        {
            return _mapper.Map<UserInfoDetailsViewDto>(await _applicationUsersUnitOfWork.ApplicationUsers.GetUser(query.Id));
        }
    }
}
