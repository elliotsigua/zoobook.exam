using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Zoobook.Core;
using Zoobook.Models;
using Zoobook.Service.Administration.DataLayer;
using Zoobook.Shared;
using Microsoft.Extensions.Logging;

namespace Zoobook.Service.Administration.Domain
{
    public class EmployeesService : ServiceBase<ZoobookAdministrationDbContext>, IEmployeesService
    {
        public EmployeesService(
            IMapper mapper,
            IUrlSetting urlSetting,
            ILogger<EmployeesService> logger,
            HttpClient httpClient,
            ZoobookAdministrationDbContext context)
                : base(logger, context, mapper, httpClient, urlSetting.Base, Constants.ApiRouteEmployees)  // TODO: Add Base URL
        { }

        public async Task<Response<List<EmployeeDto>>> SearchAsync(EmployeeRequestDto request)
        {
            var query = new GetEmployeesQuery(DbContext, request);
            var response = await query.ExecuteQueryAsync();
            if (response.Failed)
                return Response<List<EmployeeDto>>.BadRequestError(response.ErrorMessage);

            var entitiDtos = Mapper.Map<List<EmployeeDto>>(response.Data);
            return Response<List<EmployeeDto>>.Success(entitiDtos, response.TotalCount);
        }

        public async Task<Response<EmployeeDto>> CreateAsync(EmployeeCreateDto payload)
        {
            return await CreateEntityAsync<Employee, EmployeeCreateDto, EmployeeDto>(payload);
        }

        public async Task<Response<EmployeeDto>> UpdateAsync(long entityId, EmployeeUpdateDto payload)
        {
            return await UpdateEntityAsync<Employee, EmployeeUpdateDto, EmployeeDto>(entityId, payload);
        }

        public async Task<Response<EmployeeDto>> DeleteAsync(long entityId)
        {
            return await DeleteEntityAsync<Employee, EmployeeDto>(entityId);
        }
    }
}
