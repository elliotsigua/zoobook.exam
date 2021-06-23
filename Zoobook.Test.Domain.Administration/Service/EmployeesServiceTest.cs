using LightInject;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Xunit;
using Zoobook.Models;
using Zoobook.Service.Administration.DataLayer;
using Zoobook.Service.Administration.Domain;
using Zoobook.Shared;

namespace Zoobook.Test.Domain.Administration
{
    public class EmployeesServiceTest
    {
        private readonly ZoobookAdministrationDbContext _context;
        private readonly IEmployeesService _employeesService;

        public EmployeesServiceTest()
        {
            var container = new ServiceContainer();
            container.RegisterFrom<CompositionRoot>();
            container.BeginScope();

            _context = container.GetInstance<ZoobookAdministrationDbContext>();
            _employeesService = container.GetInstance<IEmployeesService>();
        }

        #region CreateAsync
        [Fact]
        public async void CreateAsync_EmptyLastName_ShouldReturnUnprocessable()
        {
            // Arrange
            var payload = new EmployeeCreateDto()
            {
                Email = "robert.baratheon@house-baratheon.got",
                LastName = string.Empty,
                MiddleName = "Usurper",
                FirstName = "Robert",
                MobilePhone = "",
                Address = "Baratheon House"
            };

            // Act
            var entity = await _employeesService.CreateAsync(payload);

            // Assert
            Assert.True(entity.Failed);
            Assert.Equal(HttpStatusCode.UnprocessableEntity, entity.StatusCode);
            Assert.Null(entity.Data);
        }

        [Fact]
        public async void CreateAsync_EmptyFirstName_ShouldReturnUnprocessable()
        {
            // Arrange
            var payload = new EmployeeCreateDto()
            {
                Email = "robert.baratheon@house-baratheon.got",
                LastName = "Baratheon",
                MiddleName = "Usurper",
                FirstName = string.Empty,
                MobilePhone = "",
                Address = "Baratheon House"
            };

            // Act
            var entity = await _employeesService.CreateAsync(payload);

            // Assert
            Assert.True(entity.Failed);
            Assert.Equal(HttpStatusCode.UnprocessableEntity, entity.StatusCode);
            Assert.Null(entity.Data);
        }

        [Fact]
        public async void CreateAsync_EmptyEmail_ShouldReturnUnprocessable()
        {
            // Arrange
            var payload = new EmployeeCreateDto()
            {
                Email = string.Empty,
                LastName = "Baratheon",
                MiddleName = "Usurper",
                FirstName = "Robert",
                MobilePhone = "",
                Address = "Baratheon House"
            };

            // Act
            var entity = await _employeesService.CreateAsync(payload);

            // Assert
            Assert.True(entity.Failed);
            Assert.Equal(HttpStatusCode.UnprocessableEntity, entity.StatusCode);
            Assert.Null(entity.Data);
        }

        [Fact]
        public async void CreateAsync_InvalidEmail_ShouldReturnUnprocessable()
        {
            // Arrange
            var payload = new EmployeeCreateDto()
            {
                Email = "12345",
                LastName = "Baratheon",
                MiddleName = "Usurper",
                FirstName = "Robert",
                MobilePhone = "",
                Address = "Baratheon House"
            };

            // Act
            var entity = await _employeesService.CreateAsync(payload);

            // Assert
            Assert.True(entity.Failed);
            Assert.Equal(HttpStatusCode.UnprocessableEntity, entity.StatusCode);
            Assert.Null(entity.Data);
        }

        [Fact]
        public async void CreateAsync_ProperInput_ShouldReturnOk()
        {
            // Arrange
            var payload = new EmployeeCreateDto()
            {
                Email = "robert.baratheon@house-baratheon.got",
                LastName = "Baratheon",
                MiddleName = "Usurper",
                FirstName = "Robert",
                MobilePhone = "",
                Address = "Baratheon House"
            };

            // Act
            var entity = await _employeesService.CreateAsync(payload);

            // Assert
            Assert.False(entity.Failed);
            Assert.Equal(HttpStatusCode.OK, entity.StatusCode);
            Assert.NotNull(entity.Data);
            Assert.NotEqual(0, entity.Data.Id);
        }
        #endregion

        #region GetAsync
        [Fact]
        public async void SearchAsync_PageOneSizeOne_MustGetPage1With1Data()
        {
            // Arrange
            var request = new EmployeeRequestDto()
            {
                Paging = new Paging(1, 1)
            };

            // Act
            var entities = await _employeesService.SearchAsync(request);

            // Assert
            Assert.NotEmpty(entities.Data);
            Assert.Single(entities.Data);
            Assert.Equal(1, entities.Data.Single().Id);
        }

        [Fact]
        public async void SearchAsync_PageTwoSizeOne_MustGetPage2With1Data()
        {
            // Arrange
            var request = new EmployeeRequestDto()
            {
                Paging = new Paging(2, 1)
            };

            // Act
            var entities = await _employeesService.SearchAsync(request);

            // Assert
            Assert.NotEmpty(entities.Data);
            Assert.Single(entities.Data);
            Assert.Equal(2, entities.Data.Single().Id);
        }

        [Fact]
        public async void SearchAsync_PageSizeTwo_MustGetTwoData()
        {
            // Arrange
            var request = new EmployeeRequestDto()
            {
                Paging = new Paging(1, 2)
            };

            // Act
            var entities = await _employeesService.SearchAsync(request);

            // Assert
            Assert.NotEmpty(entities.Data);
            Assert.Equal(2, entities.Data.Count);
        }

        [Fact]
        public async void SearchAsync_EmptyRequest_MustReturnAll()
        {
            // Arrange
            var request = new EmployeeRequestDto();

            // Act
            var entities = await _employeesService.SearchAsync(request);

            // Assert
            Assert.NotEmpty(entities.Data);
            Assert.Equal(_context.Employees.Count(), entities.Data.Count);
        }

        [Fact]
        public async void SearchAsync_WithUnregisteredRequest_MustEmpty()
        {
            // Arrange
            var request = new EmployeeRequestDto() { Ids = new List<long>() { 999 } };

            // Act
            var entities = await _employeesService.SearchAsync(request);

            // Assert
            Assert.Empty(entities.Data);
        }

        [Fact]
        public async void SearchAsync_WithProperRequest_MustReturnData()
        {
            // Arrange
            var request = new EmployeeRequestDto() { Ids = new List<long>() { 1 } };

            // Act
            var entities = await _employeesService.SearchAsync(request);

            // Assert
            Assert.NotEmpty(entities.Data);
            Assert.Single(entities.Data);
            Assert.Equal(1, entities.Data.Single().Id);
        }
        #endregion

        #region UpdateAsync
        [Fact]
        public async void UpdateAsync_EmptyLastName_ShouldReturnUnprocessable()
        {
            // Arrange
            var payload = new EmployeeUpdateDto()
            {
                FirstName = "Robb",
                LastName = string.Empty,
                Status = EmployeeStatus.None
            };

            // Act
            var entity = await _employeesService.UpdateAsync(1, payload);

            // Assert
            Assert.True(entity.Failed);
            Assert.Equal(HttpStatusCode.UnprocessableEntity, entity.StatusCode);
            Assert.Null(entity.Data);
        }

        [Fact]
        public async void UpdateAsync_EmptyFirstName_ShouldReturnUnprocessable()
        {
            // Arrange
            var payload = new EmployeeUpdateDto()
            {
                FirstName = string.Empty,
                LastName = "Stark",
                Status = EmployeeStatus.None
            };

            // Act
            var entity = await _employeesService.UpdateAsync(1, payload);

            // Assert
            Assert.True(entity.Failed);
            Assert.Equal(HttpStatusCode.UnprocessableEntity, entity.StatusCode);
            Assert.Null(entity.Data);
        }

        [Fact]
        public async void UpdateAsync_InvalidStatus_ShouldReturnUnprocessable()
        {
            // Arrange
            var payload = new EmployeeUpdateDto()
            {
                FirstName = "Robb",
                LastName = "Stark",
                Status = EmployeeStatus.None
            };

            // Act
            var entity = await _employeesService.UpdateAsync(1, payload);

            // Assert
            Assert.True(entity.Failed);
            Assert.Equal(HttpStatusCode.UnprocessableEntity, entity.StatusCode);
            Assert.Null(entity.Data);
        }

        [Fact]
        public async void UpdateAsync_UnregisteredId_ShouldReturnUnprocessable()
        {
            // Arrange
            var payload = new EmployeeUpdateDto()
            {
                FirstName = "Robb",
                LastName = "Stark",
                Status = EmployeeStatus.Inactive
            };

            // Act
            var entity = await _employeesService.UpdateAsync(999, payload);

            // Assert
            Assert.True(entity.Failed);
            Assert.Equal(HttpStatusCode.UnprocessableEntity, entity.StatusCode);
            Assert.Null(entity.Data);
        }

        [Fact]
        public async void UpdateAsync_ProperInput_ShouldReturnOk()
        {
            // Arrange
            var payload = new EmployeeUpdateDto()
            {
                FirstName = "Robb",
                LastName = "Stark",
                Status = EmployeeStatus.Inactive
            };

            // Act
            var entity = await _employeesService.UpdateAsync(1, payload);

            // Assert
            Assert.False(entity.Failed);
            Assert.Equal(HttpStatusCode.OK, entity.StatusCode);
            Assert.NotNull(entity.Data);
            Assert.Equal(EmployeeStatus.Inactive, entity.Data.Status);
        }
        #endregion

        #region DeleteAsync
        [Fact]
        public async void DeleteAsync_UnregisteredId_ShouldReturnUnprocessable()
        {
            // Arrange
            var id = 999;

            // Act
            var entity = await _employeesService.DeleteAsync(id);

            // Assert
            Assert.True(entity.Failed);
            Assert.Equal(HttpStatusCode.UnprocessableEntity, entity.StatusCode);
            Assert.Null(entity.Data);
        }

        [Fact]
        public async void DeleteAsync_RegisteredId_ShouldDelete()
        {
            // Arrange
            var id = 4;

            // Act
            var deletedEntity = await _employeesService.DeleteAsync(id);
            var entity = _context.Employees.FirstOrDefault(entry => Equals(entry.Id, id));

            // Assert
            Assert.False(deletedEntity.Failed);
            Assert.Equal(HttpStatusCode.OK, deletedEntity.StatusCode);
            Assert.Null(entity);
        }
        #endregion
    }
}
