using AutoMapper;
using WebShop.Messaging;
using WebShop.Users.Services.Queries;
using WebShop.Users.Dtos.ApplicationUser;
using WebShop.Users.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Users.Services.Handlers
{
    public class ProfileBrowseHandler : IQueryHandler<ProfileBrowseQuery, IEnumerable<ProfileView>>
    {
        private readonly IApplicationUsersUnitOfWork _applicationUsersUnitOfWork;
        private readonly IMapper _mapper;

        public ProfileBrowseHandler(IApplicationUsersUnitOfWork applicationUsersUnitOfWork, IMapper mapper)
        {
            _applicationUsersUnitOfWork = applicationUsersUnitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProfileView>> HandleAsync(ProfileBrowseQuery query)
        {
            return _mapper.Map<IEnumerable<ProfileView>>(await _applicationUsersUnitOfWork.ApplicationUsers.GetUsers(
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
