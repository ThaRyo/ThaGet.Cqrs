namespace ThaGet.Cqrs.Exceptions
{
    public class ApiError
    {
        public int ErrorCode { get; }

        public ApiError(int errorCode)
        {
            ErrorCode = errorCode;
        }
    }
}
