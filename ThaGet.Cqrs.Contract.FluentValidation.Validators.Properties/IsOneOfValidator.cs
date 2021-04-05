using System;
using System.Globalization;
using System.Linq;
using FluentValidation.Validators;

namespace ThaGet.Cqrs.Contract.FluentValidation.Validators.Properties
{
    public class IsOneOfValidator<T> : PropertyValidator
    {
        private const string CURRENT_VALUE = "CurrentValue";
        private const string INVALID_VALUE = "InvalidValue";
        private const string ALLOWED_VALUES = "AllowedValues";

        private readonly T[] _values;

        public IsOneOfValidator(T[] values) : base()
        {
            Options.ErrorCode = INVALID_VALUE;
            _values = values;
        }

        protected override string GetDefaultMessageTemplate() => $"Invalid value '{{{CURRENT_VALUE}}}'. Has to be one of: {{{ALLOWED_VALUES}}}";

        protected override bool IsValid(PropertyValidatorContext context)
        {
            if (context.PropertyValue == null)
                return true;

            var currentValueFormat = string.Format(CultureInfo.InvariantCulture, "{0}", context.PropertyValue);
            var allowedValuesFormat = string.Join(", ", _values.Select(x => string.Format(CultureInfo.InvariantCulture, "{0}", x)));

            context.MessageFormatter.AppendArgument(CURRENT_VALUE, currentValueFormat);
            context.MessageFormatter.AppendArgument(ALLOWED_VALUES, allowedValuesFormat);
            
            var isAllowedValue = (context.PropertyValue is T value) && _values.Contains(value);
            return isAllowedValue;
        }
    }
}
