namespace ThaGet.Cqrs.Domain.EntityFramework.Extensions
{
    public class DbContextConfigOptions
    {
        public string EntitySchema { get; set; }
        public string ConnectionString { get; set; }
        public bool IsSensitiveDataLoggingEnabled { get; set; }
    }
}
