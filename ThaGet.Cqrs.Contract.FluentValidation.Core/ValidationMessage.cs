namespace ThaGet.Cqrs.Contract.FluentValidation
{
    public class ValidationMessage
    {
        private const string PREFIX = "Validation_";

        public ValidationMessage(string key)
        {
            Key = key;
        }

        public ValidationMessage(string key, string argument)
        {
            Key = PREFIX + key;
            Argument = PREFIX + argument;
        }

        public string Key { get; set; } // = "NotNull" => "{0} darf nicht {1} sein"
        public string Argument { get; set; } // = "@ValuatorId" | "123"
    }
}
