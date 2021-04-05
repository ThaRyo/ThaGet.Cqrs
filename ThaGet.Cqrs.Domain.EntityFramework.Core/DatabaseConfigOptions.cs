using ThaGet.Shared;

namespace ThaGet.Cqrs.Domain.EntityFramework.Core
{
    public abstract class DatabaseConfigOptions
    {
        public string Server { get; set; }
        public int? Port { get; set; }
        public string Database { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

        public virtual bool IsValid()
        {
            return !(
                ArgumentHelper.IsNullOrEmpty(Server)
                || ArgumentHelper.IsNullOrEmpty(Database)
                || ArgumentHelper.IsNullOrEmpty(User)
                || ArgumentHelper.IsNullOrEmpty(Password)
            );
        }

        public abstract string BuildConnectionString();
    }
}
