using System;
using System.Linq;
using System.Threading.Tasks;
using Zoobook.Core;
using Zoobook.Models;
using Zoobook.Shared;

namespace Zoobook.Service.Administration.DataLayer
{
    public class GetEmployeesQuery : QueryBase<Employee, EmployeeRequestDto>
    {
        private readonly ZoobookAdministrationDbContext _context;

        public GetEmployeesQuery(
            ZoobookAdministrationDbContext context,
            EmployeeRequestDto request)
                : base(request)
        {
            _context = context;
        }

        protected override async Task<IQueryable<Employee>> PerformQueryAsync()
        {
            var entities = _context.Employees
                .ConditionalWhere(() => !Request.Ids.IsNullOrEmpty(),
                    entity => Request.Ids.Contains(entity.Id))
                .ConditionalWhere(() => !Request.Emails.IsNullOrEmpty(),
                    entity => Request.Emails.Contains(entity.Email))
                .OrderBy(entity => entity.Id);

            return await Task.Run(() => entities); ;
        }
    }
}
