using ThaGet.Cqrs.Domain.Entities.Abstractions;
using ThaGet.Cqrs.Domain.Repositories.Abstractions;
using FluentValidation.Validators;
using ThaGet.Shared;

namespace ThaGet.Cqrs.Contract.FluentValidation.Validators.Properties
{
    public class EntityNotReferencedValidator<TEntity, TId> : PropertyValidator
        where TEntity : IEntity<TId>
        where TId : struct
    {
        private const string PROPERTY_NAME = "PropertyName";
        private const string PROPERTY_VALUE = "PropertyValue";
        private const string RECORD_IN_USE = "RecordInUse";

        private readonly IRepository<TEntity, TId> _repository;

        public EntityNotReferencedValidator(IRepository<TEntity, TId> repository) : base()
        {
            Options.ErrorCode = RECORD_IN_USE;
            _repository = repository;
        }

        protected override string GetDefaultMessageTemplate() => $"Record with '{{{PROPERTY_NAME}}}' {{{PROPERTY_VALUE}}} is used by other records";

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var id = (TId)context.PropertyValue;

            // TODO Should be handled by NotEmpty, NotNull... and expect here to have a value?
            if (ArgumentHelper.IsNullOrEmpty(id))
                // TODO Shouldn't that return false / throw an exception instead?
                return true;

            var exists = _repository.Exists(id).Result;
            return !exists;
        }
    }
}
