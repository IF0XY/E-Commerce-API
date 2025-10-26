using AutoMapper;
using Domain.Models.IdentityModule;
using Shared.IdentityModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.MappingProfiles
{
    internal class IdentituProfile : Profile
    {
        public IdentituProfile()
        {
            CreateMap<Address, AddressDto>().ReverseMap();
        }
    }
}
