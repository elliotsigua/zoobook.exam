using AutoMapper;
using Zoobook.Models;
using Zoobook.Service.Administration.DataLayer;

namespace Zoobook.Test.Domain.Administration
{
    public class MockMappingProfile : Profile
    {
        public MockMappingProfile()
        {
            CreateMap<Employee, EmployeeDto>(MemberList.Destination).ReverseMap();
            CreateMap<Employee, EmployeeCreateDto>(MemberList.Destination).ReverseMap();
            CreateMap<Employee, EmployeeUpdateDto>(MemberList.Destination).ReverseMap();
        }
    }
}
