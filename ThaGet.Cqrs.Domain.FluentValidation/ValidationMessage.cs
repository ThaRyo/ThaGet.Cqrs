namespace ThaGet.Cqrs.Domain.FluentValidation
{
    public class ValidationMessage
    {
        public string PropertyName { get; set; }
        public string Message { get; set; }
        public string ErrorCode { get; set; }
    }
}
