using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Zoobook.Models;
using Zoobook.Shared;
using Microsoft.Extensions.Logging;
using Zoobook.Core.Properties;

namespace Zoobook.Core
{
    public class ServiceBase<TDbContext> : IServiceBase
        where TDbContext : ZoobookContextBase
    {
        private readonly HttpClient _apiHttpClient;

        protected IMapper Mapper { get; }

        protected readonly TDbContext DbContext;

        protected readonly ILogger<ServiceBase<TDbContext>> Logger;

        protected string ApiRoute { get; }

        public ServiceBase(
            ILogger<ServiceBase<TDbContext>> logger,
            TDbContext dbContext,
            IMapper mapper,
            HttpClient httpClient,
            string baseUrl,
            string baseRoute)
        {
            Logger = logger;
            ApiRoute = baseRoute;
            DbContext = dbContext;
            Mapper = mapper;
            
            _apiHttpClient = httpClient;
            _apiHttpClient.BaseAddress = new Uri(baseUrl);
        }

        public HttpClient ApiHttpClient()
        {
            _apiHttpClient.DefaultRequestHeaders.Clear();
            _apiHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            _apiHttpClient.DefaultRequestHeaders.UserAgent.TryParseAdd("request");

            return _apiHttpClient;
        }

        protected static HttpContent SerializeRequestPayload<TEntity>(TEntity entity)
        {
            if (Equals(entity, null)) return null;
            var myContent = entity.SerializeEntity();
            var buffer = Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return byteContent;
        }

        protected static async Task<TEntity> MapApiEntityAsync<TEntity>(HttpContent content)
            where TEntity : class
        {
            if (Equals(content, null)) return null;
            var contentJson = await content.ReadAsStringAsync();
            return contentJson.DeserializeEntity<TEntity>();
        }

        protected static async Task<IEnumerable<TEntity>> MapApiCollectionAsync<TEntity>(HttpContent content)
        {
            if (Equals(content, null)) return null;
            var contentJson = await content.ReadAsStringAsync();
            return contentJson.DeserializeEntity<IEnumerable<TEntity>>();
        }

        #region CRUD
        protected async Task<Response<TDto>> CreateEntityAsync<TEntity, TCreateDto, TDto>(
            TCreateDto createEntity)
            where TEntity : EntityBase, new()
            where TCreateDto : class, new()
            where TDto : EntityBaseDto, new()
        {
            try
            {
                var validationResult = ValidationManager
                    .InitializeFactories()
                    .ValidateEntityProperties(createEntity);
                if (!validationResult.IsNullOrEmpty())
                {
                    return Response<TDto>.UnprocessableEntityError(
                        string.Join(" ", validationResult.Select(result => result.ErrorMessage)));
                }

                var createPayload = Mapper.Map<TEntity>(createEntity);
                var createRequest = new CreateEntitiesCommand<TEntity>(DbContext, createPayload);
                var createResult = await createRequest.ExecuteAsync();
                if (createResult.Failed)
                    return Response<TDto>.BadRequestError(createResult.ErrorMessage);

                var mappedEntity = Mapper.Map<TEntity, TDto>(createResult.Data?.FirstOrDefault());
                return Response<TDto>.Success(mappedEntity);
            }
            catch (Exception exception)
            {
                var error = exception.InnerException?.Message ?? exception.Message;
                return Response<TDto>.BadRequestError(error);
            }
        }

        protected async Task<Response<TDto>> UpdateEntityAsync<TEntity, TUpdateDto, TDto>(
            long id,
            TUpdateDto updateEntity)
            where TEntity : EntityBase, new()
            where TUpdateDto : class, new()
            where TDto : EntityBaseDto, new()
        {
            try
            {
                var validationResult = ValidationManager
                    .InitializeFactories()
                    .ValidateEntityProperties(updateEntity);
                if (!validationResult.IsNullOrEmpty())
                {
                    return Response<TDto>.UnprocessableEntityError(
                        string.Join(" ", validationResult.Select(result => result.ErrorMessage)));
                }

                var obtainedEntity = DbContext.Set<TEntity>().FirstOrDefault(entity => Equals(entity.Id, id));
                if (Equals(obtainedEntity, null))
                    return Response<TDto>.UnprocessableEntityError(string.Format(Resources.EntityNotFound, id));

                var entity = Mapper.Map(updateEntity, obtainedEntity);
                var updateRequest = new UpdateEntitiesCommand<TEntity>(DbContext, entity);
                var updateResult = await updateRequest.ExecuteAsync();
                if (updateResult.Failed)
                    return Response<TDto>.BadRequestError(updateResult.ErrorMessage);

                var mappedEntity = Mapper.Map<TEntity, TDto>(
                    updateResult.Data?.FirstOrDefault());

                return Response<TDto>.Success(mappedEntity);
            }
            catch (Exception exception)
            {
                var error = exception.InnerException?.Message ?? exception.Message;
                return Response<TDto>.BadRequestError(error);
            }
        }

        protected async Task<Response<TDetailsDto>> DeleteEntityAsync<TEntity, TDetailsDto>(long id)
            where TEntity : EntityBase, new()
            where TDetailsDto : EntityBaseDto, new()
        {
            try
            {
                var obtainedEntity = DbContext.Set<TEntity>().FirstOrDefault(entity => Equals(entity.Id, id));
                if (Equals(obtainedEntity, null))
                    return Response<TDetailsDto>.UnprocessableEntityError(string.Format(Resources.EntityNotFound, id));

                // Execute deletion of entity
                var deleteRequest = new DeleteEntitiesCommand<TEntity>(DbContext, obtainedEntity);
                var deleteResult = await deleteRequest.ExecuteAsync();
                if (deleteResult.Failed)
                    return Response<TDetailsDto>.BadRequestError(deleteResult.ErrorMessage);

                var mappedEntity = Mapper.Map<TEntity, TDetailsDto>(
                    deleteResult.Data?.FirstOrDefault());

                return Response<TDetailsDto>.Success(mappedEntity);
            }
            catch (Exception exception)
            {
                var error = exception.InnerException?.Message ?? exception.Message;
                return Response<TDetailsDto>.BadRequestError(error);
            }
        }
        #endregion
    }
}
