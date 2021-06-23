namespace Zoobook.Shared
{
    public class MessageUtility
    {
        public static string GetString(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return key;
            return key;
        }

        public static string GetString(string key, params object[] items)
        {
            if (string.IsNullOrEmpty(key)) return key;
            return string.Format(key, items);
        }
    }
}
