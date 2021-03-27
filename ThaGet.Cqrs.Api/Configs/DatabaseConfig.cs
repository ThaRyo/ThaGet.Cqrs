using System;
using System.Collections.Generic;
using System.Text;

namespace ThaGet.Cqrs.Api.Configs
{
    public class DatabaseConfig
    {
        public string ConnectionString { get; set; }
        public string Schema { get; set; }
    }
}
