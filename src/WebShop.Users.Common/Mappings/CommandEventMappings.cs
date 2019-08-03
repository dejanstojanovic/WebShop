using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using WebShop.Users.Common.Commands;
using WebShop.Users.Common.Events;

namespace WebShop.Users.Common.Mappings
{
    public class CommandEventMappings: Profile
    {
        public CommandEventMappings()
        {
            CreateMap<RegisterUserCommand, UserRegisteredEvent>()
           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
           .ForAllOtherMembers(x => x.UseDestinationValue());
        }
    }
}
