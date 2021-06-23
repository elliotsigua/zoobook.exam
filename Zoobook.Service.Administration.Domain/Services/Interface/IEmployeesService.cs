using System.Collections.Generic;
using System.Threading.Tasks;
using Zoobook.Core;
using Zoobook.Models;

namespace Zoobook.Service.Administration.Domain
{
    public interface IEmployeesService : IServiceBase
    {
        Task<Response<List<EmployeeDto>>> SearchAsync(EmployeeRequestDto request);

        Task<Response<EmployeeDto>> CreateAsync(EmployeeCreateDto payload);

        Task<Response<EmployeeDto>> UpdateAsync(long entityId, EmployeeUpdateDto payload);

        Task<Response<EmployeeDto>> DeleteAsync(long entityId);
    }
}
