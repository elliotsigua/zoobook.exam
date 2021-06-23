namespace Zoobook.Core
{
    public interface IDatabaseSetting
    {
        string Name { get; set; }

        string Host { get; set; }

        string User { get; set; }

        string Password { get; set; }

        uint Port { get; set; }

        bool Pooling { get; }

        string ConnectionString();
    }
}
