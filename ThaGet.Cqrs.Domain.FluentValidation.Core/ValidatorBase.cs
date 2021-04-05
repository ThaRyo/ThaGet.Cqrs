using ThaGet.Cqrs.Domain.Entities.Abstractions;
using FluentValidation;

namespace ThaGet.Cqrs.Domain.FluentValidation
{
    public abstract class ValidatorBase<TEntity, TId> : AbstractValidator<TEntity>
        where TEntity : IEntity<TId>
        where TId : struct
    {
        protected ValidatorBase()
        {
            RuleSet("default", DefaultRules);
            RuleSet("create", CreateRules);
            RuleSet("update", UpdateRules);
            RuleSet("delete", DeleteRules);
        }

        protected virtual void DefaultRules()
        {

        }

        protected virtual void CreateRules()
        {

        }

        protected virtual void UpdateRules()
        {

        }

        protected virtual void DeleteRules()
        {

        }
    }
}
