using AutoMapper;
using Core.DTOs;
using Infrastructure.Entities;

namespace EmployeeManagement.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Employee, EmployeeDto>().ReverseMap();
        }
    }
}
