using AutoMapper;
using CRUD_API.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_API.Services.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EmplyeeModel, CRUD_API.Data.Entities.Employee>().ReverseMap();

        }
    }
}
