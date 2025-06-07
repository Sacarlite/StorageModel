using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Warehouse.Domain.Models;
using Warehouse.Infrastructure.Models;

namespace Warehouse.Application.Mappings
{
    public class PalletProfile : Profile
    {
        public PalletProfile()
        {
            CreateMap<PalletDTO, Pallet>()
                .ForMember(dest => dest.Boxes, opt => opt.MapFrom(src => src.Boxes)).ReverseMap();
        }
    }
}
