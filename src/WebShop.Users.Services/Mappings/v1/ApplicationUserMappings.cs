﻿using AutoMapper;
using WebShop.Users.Dtos.ApplicationUser;
using WebShop.Users.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebShop.Users.Services.Mappings.v1
{
    public class ApplicationUserMappings : Profile
    {
        public ApplicationUserMappings()
        {
            //Map user entity to user dto
            CreateMap<ApplicationUser, ProfileView>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForAllOtherMembers(x => x.UseDestinationValue());
        }
    }
}