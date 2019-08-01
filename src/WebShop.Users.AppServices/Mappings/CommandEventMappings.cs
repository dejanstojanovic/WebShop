using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using WebShop.Users.AppServices.Commands;
using WebShop.Users.AppServices.Events;

namespace WebShop.Users.AppServices.Mappings
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
