using System;
using System.Linq.Expressions;
using ThaGet.Cqrs.Domain.Entities.Abstractions;
using ThaGet.Cqrs.Domain.Repositories.Abstractions;
using ThaGet.Cqrs.Filter;
using ThaGet.Cqrs.Filter.Abstractions;
using FluentValidation.Validators;
using ThaGet.Shared;

namespace ThaGet.Cqrs.Contract.FluentValidation.Validators.Properties
{
    public class EntityExistsValidator<TEntity, TId> : PropertyValidator
           where TEntity : IEntity<TId>
        where TId : struct
    {
        private const string PROPERTY_NAME = "PropertyName";
        private const string PROPERTY_VALUE = "PropertyValue";
        private const string RECORD_DOES_NOT_EXIST = "RecordDoesNotExist";

        private readonly IRepository<TEntity, TId> _repository;
        private readonly Expression<Func<TEntity, object>> _predicate;

        public EntityExistsValidator(IRepository<TEntity, TId> repository) : base()
        {
            Options.ErrorCode = RECORD_DOES_NOT_EXIST;
            _repository = repository;
        }

        public EntityExistsValidator(IRepository<TEntity, TId> repository, Expression<Func<TEntity, object>> predicate) : this(repository)
        {
            _predicate = predicate;
        }

        protected override string GetDefaultMessageTemplate() => $"Record with '{{{PROPERTY_NAME}}}' {{{PROPERTY_VALUE}}} not found";

        protected override bool IsValid(PropertyValidatorContext context)
        {
            // TODO Should be handled by NotEmpty, NotNull... and expect here to have a value?
            // TODO Check if that works...
            if (context.Rule.TypeToValidate == typeof(TId) && ArgumentHelper.IsNullOrEmpty((TId)context.PropertyValue))
                return true;

            if (_predicate == null)
            {   // Check if id (property value) exists
                var id = (TId)context.PropertyValue;
                var exists = _repository.Exists(id).Result;

                return exists;
            }

            // Check if given condition has a result
            var buildPredicate = GeneratorEqualityTest(_predicate, context.PropertyValue);
            var item = _repository.FindByAsync(buildPredicate).Result;

            return item != null;

        }

        private IFilterExpression<TEntity, TId> GeneratorEqualityTest<TProperty>(Expression<Func<TEntity, TProperty>> accessor, TProperty expectedValue)
        {
            var body = Expression.Equal(accessor.Body, Expression.Constant(expectedValue));
            var predicate = Expression.Lambda<Func<TEntity, bool>>(body, accessor.Parameters);

            return new FilterExpression<TEntity, TId>
            {
                predicate
            };
        }
    }
}
