using System;

namespace Zoobook.Core
{
    public class DatabaseSetting : IDatabaseSetting
    {
        public string Name { get; set; }

        public string Host { get; set; }

        public string User { get; set; }

        public string Password { get; set; }

        public uint Port { get; set; }

        public bool Pooling { get; set; }

        public string ConnectionString()
        {
            return $"Server={Host};" +
                    $"Initial Catalog={Name};" +
                    $"User ID={User};" +
                    $"Password={Password};" +
                    $"Persist Security Info=True;" +
                    $"MultipleActiveResultSets=True;" +
                    $"Encrypt = True;" +
                    $"TrustServerCertificate={(this.Host.Equals("localhost", StringComparison.OrdinalIgnoreCase) ? "True" : "False")};" +
                    $"Connection Timeout = 30;";
        }
    }
}
