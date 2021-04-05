namespace ThaGet.Cqrs.Domain.EntityFramework.MySql
{
    public class DatabaseConfigOptions : Core.DatabaseConfigOptions
    {
        public override string BuildConnectionString()
        {
            return $"server={Server};port={Port ?? 3306};database={Database};user={User};password={Password}";
        }
    }
}
