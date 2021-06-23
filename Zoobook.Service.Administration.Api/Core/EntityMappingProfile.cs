using AutoMapper;
using Zoobook.Models;
using Zoobook.Service.Administration.DataLayer;

namespace Zoobook.Service.Administration.Api
{
    public class EntityMappingProfile : Profile
    {
        public EntityMappingProfile()
        {
            CreateMap<Employee, EmployeeDto>(MemberList.Destination).ReverseMap();
            CreateMap<Employee, EmployeeCreateDto>(MemberList.Destination).ReverseMap();
            CreateMap<Employee, EmployeeUpdateDto>(MemberList.Destination).ReverseMap();
        }
    }
}
