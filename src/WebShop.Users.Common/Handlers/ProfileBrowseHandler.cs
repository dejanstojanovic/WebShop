using AutoMapper;
using WebShop.Messaging;
using WebShop.Users.Common.Queries;
using WebShop.Users.Common.Dtos.ApplicationUser;
using WebShop.Users.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Users.Common.Handlers
{
    public class ProfileBrowseHandler : IQueryHandler<ProfileBrowseQuery, IEnumerable<UserInfoDetailsViewDto>>
    {
        private readonly IApplicationUsersUnitOfWork _applicationUsersUnitOfWork;
        private readonly IMapper _mapper;

        public ProfileBrowseHandler(IApplicationUsersUnitOfWork applicationUsersUnitOfWork, IMapper mapper)
        {
            _applicationUsersUnitOfWork = applicationUsersUnitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserInfoDetailsViewDto>> HandleAsync(ProfileBrowseQuery query)
        {
            return _mapper.Map<IEnumerable<UserInfoDetailsViewDto>>(await _applicationUsersUnitOfWork.ApplicationUsers.GetUsers(
                query.FirstName,
                query.LastName,
                query.Occupation,
                query.Education,
                query.Email,
                query.PageIndex,
                query.PageSize
                ));
        }
    }
}
