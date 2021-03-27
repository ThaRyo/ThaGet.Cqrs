using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TestClasses;
using ThaGet.Cqrs.Domain.Entities.Abstractions;
using ThaGet.Cqrs.Domain.Repositories.Abstractions;
using ThaGet.Cqrs.Filter.Abstractions;
using ThaGet.Cqrs.Include.Abstractions;
using ThaGet.Cqrs.Sort.Abstractions;

namespace HandlerSample
{
    class Program
    {
        private class LoggerMock : ILogger<object>
        {
            public IDisposable BeginScope<TState>(TState state)
            {
                throw new NotImplementedException();
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                throw new NotImplementedException();
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                throw new NotImplementedException();
            }
        }
        private class MapperMock : IMapper
        {
            public IConfigurationProvider ConfigurationProvider => throw new NotImplementedException();

            public Func<Type, object> ServiceCtor => throw new NotImplementedException();

            public TDestination Map<TDestination>(object source, Action<IMappingOperationOptions<object, TDestination>> opts)
            {
                throw new NotImplementedException();
            }

            public TDestination Map<TSource, TDestination>(TSource source, Action<IMappingOperationOptions<TSource, TDestination>> opts)
            {
                throw new NotImplementedException();
            }

            public TDestination Map<TSource, TDestination>(TSource source, TDestination destination, Action<IMappingOperationOptions<TSource, TDestination>> opts)
            {
                throw new NotImplementedException();
            }

            public object Map(object source, Type sourceType, Type destinationType, Action<IMappingOperationOptions<object, object>> opts)
            {
                throw new NotImplementedException();
            }

            public object Map(object source, object destination, Type sourceType, Type destinationType, Action<IMappingOperationOptions<object, object>> opts)
            {
                throw new NotImplementedException();
            }

            public TDestination Map<TDestination>(object source)
            {
                throw new NotImplementedException();
            }

            public TDestination Map<TSource, TDestination>(TSource source)
            {
                throw new NotImplementedException();
            }

            public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
            {
                throw new NotImplementedException();
            }

            public object Map(object source, Type sourceType, Type destinationType)
            {
                throw new NotImplementedException();
            }

            public object Map(object source, object destination, Type sourceType, Type destinationType)
            {
                throw new NotImplementedException();
            }

            public IQueryable<TDestination> ProjectTo<TDestination>(IQueryable source, object parameters = null, params Expression<Func<TDestination, object>>[] membersToExpand)
            {
                throw new NotImplementedException();
            }

            public IQueryable<TDestination> ProjectTo<TDestination>(IQueryable source, IDictionary<string, object> parameters, params string[] membersToExpand)
            {
                throw new NotImplementedException();
            }

            public IQueryable ProjectTo(IQueryable source, Type destinationType, IDictionary<string, object> parameters = null, params string[] membersToExpand)
            {
                throw new NotImplementedException();
            }
        }
        private class RepositoryMock : IRepository<TestEntity, int>
        {
            public IUnitOfWork UnitOfWork => throw new NotImplementedException();

            public TestEntity Create(TestEntity entity)
            {
                throw new NotImplementedException();
            }

            public void Delete(TestEntity entity)
            {
                throw new NotImplementedException();
            }

            public Task<bool> Exists(int id)
            {
                throw new NotImplementedException();
            }

            public Task<ICollection<TestEntity>> FindAllByAsync(IIncludeExpression<TestEntity, int> includeExpression, IFilterExpression<TestEntity, int> filterExpression)
            {
                throw new NotImplementedException();
            }

            public Task<ICollection<TResult>> FindAllByAsync<TResult>(ThaGet.Cqrs.Selector.Abstractions.ISelectExpression<TestEntity, TResult, int> selectExpression, IFilterExpression<TestEntity, int> filterExpression) where TResult : class
            {
                throw new NotImplementedException();
            }

            public Task<ICollection<TResult>> FindAllByAsync<TResult>(ThaGet.Cqrs.Selector.Abstractions.ISelectExpression<TestEntity, TResult, int> selectExpression, IIncludeExpression<TestEntity, int> includeExpression, IFilterExpression<TestEntity, int> filterExpression) where TResult : class
            {
                throw new NotImplementedException();
            }

            public Task<ICollection<TResult>> FindAllByAsync<TResult>(ThaGet.Cqrs.Selector.Abstractions.ISelectExpression<TestEntity, TResult, int> selectExpression, IIncludeExpression<TestEntity, int> includeExpression, IFilterExpression<TestEntity, int> filterExpression, ISortExpression<TestEntity, int> sortExpression) where TResult : class
            {
                throw new NotImplementedException();
            }

            public Task<TestEntity> FindByAsync(IFilterExpression<TestEntity, int> filterExpression)
            {
                throw new NotImplementedException();
            }

            public Task<TResult> FindByAsync<TResult>(ThaGet.Cqrs.Selector.Abstractions.ISelectExpression<TestEntity, TResult, int> selectExpression, IFilterExpression<TestEntity, int> filterExpression) where TResult : class
            {
                throw new NotImplementedException();
            }

            public Task<TResult> FindByAsync<TResult>(ThaGet.Cqrs.Selector.Abstractions.ISelectExpression<TestEntity, TResult, int> selectExpression, IIncludeExpression<TestEntity, int> includeExpression, IFilterExpression<TestEntity, int> filterExpression) where TResult : class
            {
                throw new NotImplementedException();
            }

            public Task<TestEntity> FindByIdAsync(int id)
            {
                throw new NotImplementedException();
            }

            public Task<TestEntity> FindByIdAsync(int id, IIncludeExpression<TestEntity, int> includeExpression)
            {
                throw new NotImplementedException();
            }

            public Task<TResult> FindByIdAsync<TResult>(int id, ThaGet.Cqrs.Selector.Abstractions.ISelectExpression<TestEntity, TResult, int> selectExpression) where TResult : class
            {
                throw new NotImplementedException();
            }

            public Task<TResult> FindByIdAsync<TResult>(int id, ThaGet.Cqrs.Selector.Abstractions.ISelectExpression<TestEntity, TResult, int> selectExpression, IIncludeExpression<TestEntity, int> includeExpression) where TResult : class
            {
                throw new NotImplementedException();
            }

            public Task<ICollection<TestEntity>> GetAllAsync(bool trackEntities = false)
            {
                throw new NotImplementedException();
            }

            public Task<TestEntity> GetByAsync(IFilterExpression<TestEntity, int> filterExpression)
            {
                throw new NotImplementedException();
            }

            public Task<TResult> GetByAsync<TResult>(ThaGet.Cqrs.Selector.Abstractions.ISelectExpression<TestEntity, TResult, int> selectExpression, IFilterExpression<TestEntity, int> filterExpression) where TResult : class
            {
                throw new NotImplementedException();
            }

            public Task<TResult> GetByAsync<TResult>(ThaGet.Cqrs.Selector.Abstractions.ISelectExpression<TestEntity, TResult, int> selectExpression, IIncludeExpression<TestEntity, int> includeExpression, IFilterExpression<TestEntity, int> filterExpression) where TResult : class
            {
                throw new NotImplementedException();
            }

            public Task<TestEntity> GetByIdAsync(int id)
            {
                throw new NotImplementedException();
            }

            public Task<TestEntity> GetByIdAsync(int id, IIncludeExpression<TestEntity, int> includeExpression)
            {
                throw new NotImplementedException();
            }

            public Task<TResult> GetByIdAsync<TResult>(int id, ThaGet.Cqrs.Selector.Abstractions.ISelectExpression<TestEntity, TResult, int> selectExpression) where TResult : class
            {
                throw new NotImplementedException();
            }

            public Task<TResult> GetByIdAsync<TResult>(int id, ThaGet.Cqrs.Selector.Abstractions.ISelectExpression<TestEntity, TResult, int> selectExpression, IIncludeExpression<TestEntity, int> includeExpression) where TResult : class
            {
                throw new NotImplementedException();
            }

            public Task<ThaGet.Cqrs.Domain.Abstractions.IPagination<TResultType>> GetPaginationAsync<TResultType>(ThaGet.Cqrs.Selector.Abstractions.ISelectExpression<TestEntity, TResultType, int> selectExpression, IIncludeExpression<TestEntity, int> includeExpression, IFilterExpression<TestEntity, int> filterExpression, ISortExpression<TestEntity, int> sortExpression, int skip, int take) where TResultType : class
            {
                throw new NotImplementedException();
            }

            public void Update(TestEntity entity)
            {
                throw new NotImplementedException();
            }

            Task<TProperty> IRepository<TestEntity, int>.LazyLoad_NEW<TSomeEntity, TProperty>(TSomeEntity entity, Expression<Func<TSomeEntity, TProperty>> propertyExpression)
            {
                throw new NotImplementedException();
            }

            Task<TProperty> IRepository<TestEntity, int>.LazyLoad_OLD<TSomeEntity, TProperty>(TSomeEntity entity, Expression<Func<TSomeEntity, TProperty>> propertyExpression)
            {
                throw new NotImplementedException();
            }

            void IRepository<TestEntity, int>.UpdateReferences<TRef>(TestEntity entity, Func<TestEntity, ICollection<TRef>> references, Func<TRef, IComparable> getIdFunc, IEnumerable<TRef> newValues, Action<TRef, TRef> updateAction, Func<TRef, TRef, bool> isModifiedFunc, Action<TRef> deleteAction)
            {
                throw new NotImplementedException();
            }

            void IRepository<TestEntity, int>.UpdateReferences<TRef>(TestEntity entity, Func<TestEntity, ICollection<TRef>> references, Func<TRef, IComparable> getIdFunc, IEnumerable<TRef> toAdd, IEnumerable<TRef> toRemove, IEnumerable<TRef> toUpdate, Action<TRef, TRef> updateAction, Action<TRef> deleteAction)
            {
                throw new NotImplementedException();
            }
        }
        private class FilterServiceMock : IFilterService<int>
        {
            public void AddFilter<TQuery, TEntity>(IFilterExpression<TEntity, int> expression)
                where TQuery : class
                where TEntity : IEntity<int>
            {
                throw new NotImplementedException();
            }

            public IReadOnlyCollection<IFilterDefinition<TEntity, int>> GetFilterList<TQuery, TEntity>()
                where TQuery : class
                where TEntity : IEntity<int>
            {
                throw new NotImplementedException();
            }
        }
        private class SortServiceMock : ISortService<int>
        {
            public void AddSort<TQuery, TEntity>(ISortExpression<TEntity, int> expression)
                where TQuery : class
                where TEntity : IEntity<int>
            {
                throw new NotImplementedException();
            }

            public IReadOnlyCollection<ISortDefinition<TEntity, int>> GetSortList<TQuery, TEntity>()
                where TQuery : class
                where TEntity : IEntity<int>
            {
                throw new NotImplementedException();
            }
        }

        static void Main(string[] args)
        {
            var logger = new LoggerMock();
            var mapper = new MapperMock();
            var repository = new RepositoryMock();
            var filterService = new FilterServiceMock();
            var sortService = new SortServiceMock();

            new TestEntityQuerySingleHandler(logger, mapper, repository, filterService, sortService);
        }
    }
}
