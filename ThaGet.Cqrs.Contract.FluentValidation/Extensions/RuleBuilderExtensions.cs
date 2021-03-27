using System;
using System.Linq.Expressions;
using ThaGet.Cqrs.Contract.FluentValidation.PropertyValidators;
using ThaGet.Cqrs.Domain.Entities.Abstractions;
using ThaGet.Cqrs.Domain.Repositories.Abstractions;
using FluentValidation;

namespace ThaGet.Cqrs.Contract.FluentValidation.Extensions
{
    public static class RuleBuilderExtensions
    {
        public static IRuleBuilderOptions<T, TId> MustExist<T, TEntity, TId>(this IRuleBuilder<T, TId> ruleBuilder, IRepository<TEntity, TId> repository)
            where TEntity : IEntity<TId>
            where TId : struct
        {
            return ruleBuilder.NotEmpty().DependentRules(() =>
            {
                ruleBuilder.SetValidator(new EntityExistsValidator<TEntity, TId>(repository));
            });
        }

        public static IRuleBuilderOptions<T, object> MustExist<T, TEntity, TId>(this IRuleBuilder<T, object> ruleBuilder, IRepository<TEntity, TId> repository, Expression<Func<TEntity, object>> propertyExpression)
            where TEntity : IEntity<TId>
            where TId : struct
        {
            return ruleBuilder.NotEmpty().DependentRules(() =>
            {
                ruleBuilder.SetValidator(new EntityExistsValidator<TEntity, TId>(repository, propertyExpression));
            });
        }

        public static IRuleBuilderOptions<T, TProperty> NotReferenced<T, TProperty, TEntity, TId>(
            this IRuleBuilder<T, TProperty> ruleBuilder, IRepository<TEntity, TId> repository)
            where TEntity : IEntity<TId>
            where TId : struct
        {
            return ruleBuilder.SetValidator(new EntityNotReferencedValidator<TEntity, TId>(repository));
        }

        public static IRuleBuilderOptions<T, TProperty> IsOneOf<T, TProperty>(
            this IRuleBuilder<T, TProperty> ruleBuilder, TProperty[] values)
        {
            return ruleBuilder.SetValidator(new IsOneOfValidator<TProperty>(values));
        }
    }
}
