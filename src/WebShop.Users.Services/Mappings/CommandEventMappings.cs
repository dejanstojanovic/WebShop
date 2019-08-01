using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using WebShop.Users.Services.Commands;
using WebShop.Users.Services.Events;

namespace WebShop.Users.Services.Mappings
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
