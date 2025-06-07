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
    public class BoxProfile : Profile
    {
        public BoxProfile()
        {
            CreateMap<BoxDTO, Box>();
            CreateMap<Box, BoxDTO>();
        }
    }
}
