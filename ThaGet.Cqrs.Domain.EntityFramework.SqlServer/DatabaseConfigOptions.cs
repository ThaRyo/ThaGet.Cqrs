using ThaGet.Shared;

namespace ThaGet.Cqrs.Domain.EntityFramework
{
    public class DatabaseConfigOptions : Core.DatabaseConfigOptions
    {
        public string Schema { get; set; } = "dbo";
        public bool IsSensitiveDataLoggingEnabled { get; set; }

        public override bool IsValid()
        {
            return !(
                base.IsValid()
                || ArgumentHelper.IsNullOrEmpty(Schema)
            );
        }

        public override string BuildConnectionString()
        {
            return $"Server={Server},{Port ?? 1433};Database={Database};User Id={User};Password={Password}";
        }
    }
}
