using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using ThaGet.Cqrs.Domain.Abstractions;
using ThaGet.Cqrs.Domain.Entities.Abstractions;
using ThaGet.Cqrs.Domain.EntityFramework.Abstractions;
using ThaGet.Cqrs.Domain.FluentValidation;
using ThaGet.Cqrs.Domain.FluentValidation.Exceptions;
using ThaGet.Cqrs.Domain.Repositories.Abstractions;
using ThaGet.Cqrs.Filter;
using ThaGet.Cqrs.Filter.Abstractions;
using ThaGet.Cqrs.Include.Abstractions;
using ThaGet.Cqrs.Include.EntityFramework;
using ThaGet.Cqrs.Selector;
using ThaGet.Cqrs.Selector.Abstractions;
using ThaGet.Cqrs.Sort.Abstractions;
using ThaGet.Shared;
using ThaGet.Shared.Extensions;

namespace ThaGet.Cqrs.Domain.EntityFramework.Repositories
{
    public abstract class BaseRepository<TEntity, TId> : IBaseRepository<TEntity, TId>
        where TEntity : class, IEntity<TId>
        where TId : struct
    {
        protected readonly IValidator<TEntity> _validator;

        protected BaseRepository(IDbContextBase context, IValidator<TEntity> validator = null)
        {
            _validator = validator;
            Context = context;
            UnitOfWork = context;
        }

        //public IDbContextBase<TDbContext, TId> Context { get; }
        public IDbContextBase Context { get; }
        public virtual IUnitOfWork UnitOfWork { get; }
        protected virtual IQueryable<TEntity> FilteredSet { get => Context.Set<TEntity>(); }

        public virtual TEntity Create(TEntity entity)
        {
            EnsureValidation(entity, "default", "create");
            return Context.Set<TEntity>()
                .Add(entity)
                .Entity;
        }

        public virtual void Update(TEntity entity)
        {
            EnsureValidation(entity, "default", "update");
            Context.Entry(entity)
                .Property(x => x.Timestamp)
                .OriginalValue = entity.Timestamp;
            Context.Entry(entity)
                .State = EntityState.Modified;
        }

        public virtual void Delete(TEntity entity)
        {
            EnsureValidation(entity, "delete");
            Context.Set<TEntity>()
                .Remove(entity);
        }

        protected virtual void EnsureValidation(TEntity entity, params string[] ruleSets)
        {
            if (_validator == null)
                return;

            foreach (var ruleSet in ruleSets)
            {
                var validationResult = _validator.Validate(entity, options =>
                {
                    options.IncludeRuleSets(ruleSet);
                });

                if (!validationResult.IsValid)
                {
                    throw new DomainValidationException(
                        $"Unexpected domain validation error for entity '{entity}' on validation ruleSet '{ruleSet}'.",
                        validationResult.Errors.Select(x => new ValidationMessage
                        {
                            PropertyName = x.PropertyName,
                            ErrorCode = x.ErrorCode,
                            Message = x.ErrorMessage
                        }));
                }
            }
        }

        // TODO Check if we actually want this, ever. Shouldn't there always be some kind of filter?
        public virtual async Task<ICollection<TEntity>> GetAllAsync(bool trackEntities = false)
        {
            //return await FindAllByAsync(null, x => x);
            IQueryable<TEntity> query = trackEntities
                ? FilteredSet
                : FilteredSet.AsNoTracking();

            return await query.ToListAsync();
        }

        public virtual async Task<TEntity> GetByIdAsync(TId id)
        {
            var selectExpression = SelectExpression<TEntity, TEntity, TId>.WithMap(x => x);
            return await GetByIdAsync(id, selectExpression);
        }

        public virtual async Task<TResult> GetByIdAsync<TResult>(
            TId id,
            ISelectExpression<TEntity, TResult, TId> selectExpression)
            where TResult : class
        {
            return await GetByIdAsync(id, selectExpression, includeExpression: null);
        }

        public virtual async Task<TEntity> GetByIdAsync(
            TId id,
            IIncludeExpression<TEntity, TId> includeExpression)
        {
            var selectExpression = SelectExpression<TEntity, TEntity, TId>.WithMap(x => x);
            return await GetByIdAsync(id, selectExpression, includeExpression);
        }

        public virtual async Task<TResult> GetByIdAsync<TResult>(
            TId id,
            ISelectExpression<TEntity, TResult, TId> selectExpression,
            IIncludeExpression<TEntity, TId> includeExpression)
            where TResult : class
        {
            var filterExpression = new FilterExpression<TEntity, TId>() { (x) => x.Id.Equals(id) };
            return await GetByAsync(selectExpression, includeExpression, filterExpression);
        }

        public virtual async Task<TEntity> GetByAsync(IFilterExpression<TEntity, TId> filterExpression)
        {
            return await GetByAsync(null, filterExpression);
        }

        public virtual async Task<TResult> GetByAsync<TResult>(
            ISelectExpression<TEntity, TResult, TId> selectExpression,
            IFilterExpression<TEntity, TId> filterExpression)
            where TResult : class
        {
            return await GetByAsync(selectExpression, filterExpression);
        }

        public virtual async Task<TEntity> GetByAsync(
            IIncludeExpression<TEntity, TId> includeExpression,
            IFilterExpression<TEntity, TId> filterExpression)
        {
            var selectExpression = SelectExpression<TEntity, TEntity, TId>.WithMap(x => x);
            return await GetByAsync(selectExpression, includeExpression, filterExpression);
        }

        public virtual async Task<TResult> GetByAsync<TResult>(
            ISelectExpression<TEntity, TResult, TId> selectExpression,
            IIncludeExpression<TEntity, TId> includeExpression,
            IFilterExpression<TEntity, TId> filterExpression)
           where TResult : class
        {
            var query = BuildQuery(selectExpression, includeExpression, filterExpression);
            return await query.SingleAsync();
        }

        public virtual async Task<TEntity> FindByIdAsync(TId id)
        {
            var selectExpression = SelectExpression<TEntity, TEntity, TId>.WithMap(x => x);
            return await FindByIdAsync(id, selectExpression);
        }

        public virtual async Task<TResult> FindByIdAsync<TResult>(
            TId id,
            ISelectExpression<TEntity, TResult, TId> selectExpression)
            where TResult : class
        {
            return await FindByIdAsync(id, selectExpression, null);
        }

        public virtual async Task<TEntity> FindByIdAsync(
            TId id,
            IIncludeExpression<TEntity, TId> includeExpression)
        {
            var selectExpression = SelectExpression<TEntity, TEntity, TId>.WithMap(x => x);
            return await FindByIdAsync(id, selectExpression, includeExpression);
        }

        public virtual async Task<TResult> FindByIdAsync<TResult>(
            TId id,
            ISelectExpression<TEntity, TResult, TId> selectExpression,
            IIncludeExpression<TEntity, TId> includeExpression)
            where TResult : class
        {
            var filterExpression = new FilterExpression<TEntity, TId>()
            {
                (x) => x.Id.Equals(id)
            };
            return await FindByAsync(selectExpression, includeExpression, filterExpression);
        }

        public virtual async Task<TEntity> FindByAsync(IFilterExpression<TEntity, TId> filterExpression)
        {
            var selectExpression = SelectExpression<TEntity, TEntity, TId>.WithMap(x => x);
            return await FindByAsync(selectExpression, filterExpression);
        }

        public virtual async Task<TEntity> FindByAsync(
            IIncludeExpression<TEntity, TId> includeExpression,
            IFilterExpression<TEntity, TId> filterExpression)
        {
            var selectExpression = SelectExpression<TEntity, TEntity, TId>.WithMap(x => x);
            return await FindByAsync(selectExpression, includeExpression, filterExpression);
        }

        public virtual async Task<TResult> FindByAsync<TResult>(
            ISelectExpression<TEntity, TResult, TId> selectExpression,
            IFilterExpression<TEntity, TId> filterExpression)
            where TResult : class
        {
            return await FindByAsync(selectExpression, includeExpression: null, filterExpression: filterExpression);
        }

        public virtual async Task<TResult> FindByAsync<TResult>(
            ISelectExpression<TEntity, TResult, TId> selectExpression,
            IIncludeExpression<TEntity, TId> includeExpression,
            IFilterExpression<TEntity, TId> filterExpression)
            where TResult : class
        {
            var query = BuildQuery(selectExpression, includeExpression, filterExpression);
            return await query.SingleOrDefaultAsync();
        }

        public virtual async Task<ICollection<TResult>> FindAllByAsync<TResult>(
            ISelectExpression<TEntity, TResult, TId> selectExpression,
            IFilterExpression<TEntity, TId> filterExpression)
            where TResult : class
        {
            return await FindAllByAsync(selectExpression, includeExpression: null, filterExpression: filterExpression);
        }

        public virtual async Task<ICollection<TResult>> FindAllByAsync<TResult>(
            ISelectExpression<TEntity, TResult, TId> selectExpression,
            IIncludeExpression<TEntity, TId> includeExpression,
            IFilterExpression<TEntity, TId> filterExpression)
            where TResult : class
        {
            return await FindAllByAsync(selectExpression, includeExpression, filterExpression, null);
        }

        public virtual async Task<ICollection<TResult>> FindAllByAsync<TResult>(
            ISelectExpression<TEntity, TResult, TId> selectExpression,
            IIncludeExpression<TEntity, TId> includeExpression,
            IFilterExpression<TEntity, TId> filterExpression,
            ISortExpression<TEntity, TId> sortExpression)
            where TResult : class
        {
            var query = BuildQuery(selectExpression, includeExpression, filterExpression, sortExpression);
            return await query.ToListAsync();
        }

        public virtual async Task<ICollection<TEntity>> FindAllByAsync(
            IIncludeExpression<TEntity, TId> includeExpression,
            IFilterExpression<TEntity, TId> filterExpression)
        {
            var selectExpression = SelectExpression<TEntity, TEntity, TId>.WithMap(x => x);

            var query = BuildQuery(selectExpression, includeExpression, filterExpression);
            return await query.ToListAsync();
        }

        public virtual async Task<IPagination<TResultType>> GetPaginationAsync<TResultType>(
            ISelectExpression<TEntity, TResultType, TId> selectExpression,
            IIncludeExpression<TEntity, TId> includeExpression,
            IFilterExpression<TEntity, TId> filterExpression,
            ISortExpression<TEntity, TId> sortExpression,
            int skip,
            int take)
            where TResultType : class
        {
            var paginationResult = new Pagination<TResultType>();
            var query = BuildQuery(selectExpression, includeExpression, filterExpression, sortExpression);

            // Get total Count
            // TODO Check if select gives performance boost (check if it is even used in sql query)
            // TODO Check if sorting (applied in BuildQuery) results in performance loss
            paginationResult.TotalCount = query
                //.Select(x => x.Id)
                .Count();

            if (skip > 0)
                query = query.Skip(skip);

            if (take > 0)
                query = query.Take(take);

            paginationResult.Items = await query.ToListAsync();

            return paginationResult;
        }

        private IQueryable<TResultType> BuildQuery<TResultType>(
            ISelectExpression<TEntity, TResultType, TId> selectExpression,
            IIncludeExpression<TEntity, TId> includeExpression = null,
            IFilterExpression<TEntity, TId> filterExpression = null,
            ISortExpression<TEntity, TId> sortExpression = null)
            where TResultType : class
        {
            ArgumentHelper.ThrowIfNull(selectExpression, nameof(selectExpression));

            IQueryable<TEntity> query = FilteredSet;

            if (includeExpression != null)
                query = includeExpression.Apply(query);

            if (filterExpression != null)
                query = filterExpression.Apply(query);

            if (sortExpression != null)
                query = sortExpression.Apply(query);

            return selectExpression.Apply(query);
        }

        // TODO Check if this actually works...
        public async Task<TProperty> LazyLoad_NEW<TSomeEntity, TProperty>(TSomeEntity entity, Expression<Func<TSomeEntity, TProperty>> propertyExpression)
            where TSomeEntity : class, IEntity<TId>
            where TProperty : class
        {
            if (Context.Entry(entity).State == EntityState.Detached)
                return null;

            var propertyPath = propertyExpression.AsPath();
            await Context.Entry(entity).Navigation(propertyPath).LoadAsync();

            return propertyExpression.Compile().Invoke(entity);
        }

        // TODO Find a better way (for whole paths) if above _NEW version doesnt work
        public async Task<TProperty> LazyLoad_OLD<TSomeEntity, TProperty>(TSomeEntity entity, Expression<Func<TSomeEntity, TProperty>> propertyExpression)
            where TSomeEntity : class, IEntity<TId>
            where TProperty : class
        {
            if (Context.Entry(entity).State == EntityState.Detached)
                return null;

            if (typeof(IEnumerable).IsAssignableFrom(typeof(TProperty)))
            {
                var propertyName = propertyExpression.GetPropertyAccess().GetSimpleMemberName();
                await Context.Entry(entity).Collection(propertyName).LoadAsync();
            }
            else
            {
                await Context.Entry(entity).Reference(propertyExpression).LoadAsync();
            }

            return propertyExpression.Compile().Invoke(entity);
        }

        public virtual async Task<bool> Exists(TId id)
        {
            var entity = await FindByIdAsync(id);
            return entity != null;
        }

        public void UpdateReferences<TRef>(TEntity entity, Func<TEntity, ICollection<TRef>> references, Func<TRef, IComparable> getIdFunc, IEnumerable<TRef> newValues, Action<TRef, TRef> updateAction = null, Func<TRef, TRef, bool> isModifiedFunc = null, Action<TRef> deleteAction = null)
            where TRef : class, IEntity<TId>
        {
            if (isModifiedFunc == null && updateAction != null || isModifiedFunc != null && updateAction == null)
                throw new ArgumentException($"If one of {nameof(isModifiedFunc)} or {nameof(updateAction)} is set the other parameter has to be provided too");

            GetReferenceChanges(references(entity).ToArray(), newValues.ToArray(), getIdFunc, isModifiedFunc, out var toAdd, out var toRemove, out var toUpdate);
            UpdateReferences(entity, references, getIdFunc, toAdd, toRemove, toUpdate, updateAction, deleteAction);
        }

        public void UpdateReferences<TRef>(TEntity entity, Func<TEntity, ICollection<TRef>> references, Func<TRef, IComparable> getIdFunc, IEnumerable<TRef> toAdd, IEnumerable<TRef> toRemove, IEnumerable<TRef> toUpdate, Action<TRef, TRef> updateAction = null, Action<TRef> deleteAction = null)
            where TRef : class, IEntity<TId>
        {
            foreach (var item in toRemove)
            {
                deleteAction?.Invoke(item);
                references(entity).Remove(item);
                Context.Remove(item);
            }

            foreach (var item in toAdd)
            {
                references(entity).Add(item);
                Context.Add(item);
            }

            if (toUpdate == null || updateAction == null)
                return;

            foreach (var itemToUpdate in toUpdate)
            {
                var originalItem = references(entity).Single(x => Equals(getIdFunc(x), getIdFunc(itemToUpdate)));
                updateAction(originalItem, itemToUpdate);
                Context.Update(originalItem);
            }
        }

        // TODO Use "out" or return a tuple/object?
        protected void GetReferenceChanges<TRef>(TRef[] currentItems, TRef[] targetItems, Func<TRef, IComparable> getIdFunc, Func<TRef, TRef, bool> isModifiedFunc,
            out IEnumerable<TRef> newItems, out IEnumerable<TRef> removedItems, out IEnumerable<TRef> updatedItems)
            where TRef : class, IEntity<TId>
        {
            var currentIds = currentItems.Select(getIdFunc).ToArray();
            var targetIds = targetItems.Select(getIdFunc).ToArray();

            var removedIds = currentIds.Except(targetIds);
            var newIds = targetIds.Except(currentIds);

            newItems = targetItems.Where(x => newIds.Contains(getIdFunc(x))).ToList();
            removedItems = currentItems.Where(x => removedIds.Contains(getIdFunc(x))).ToList();

            var remainingItemIds = currentIds.Intersect(targetIds);
            var remainingItems = currentItems.Where(x => remainingItemIds.Contains(getIdFunc(x))).ToList();

            updatedItems = null;
            if (isModifiedFunc == null)
                return;

            var itemsToUpdate = new List<TRef>();
            foreach (var sourceItem in remainingItems)
            {
                var targetItem = targetItems.Single(x => Equals(getIdFunc(x), getIdFunc(sourceItem)));
                if (isModifiedFunc(sourceItem, targetItem))
                    itemsToUpdate.Add(targetItem);
            }

            updatedItems = itemsToUpdate.Any() ? itemsToUpdate : null;
        }
    }

    // TODO Test if this does something
    public static class MemberInfoExtensions
    {
        public static string GetSimpleMemberName(this MemberInfo member)
        {
            var name = member.Name;
            var index = name.LastIndexOf('.');

            return index >= 0 ? name[(index + 1)..] : name;
        }
    }
}
