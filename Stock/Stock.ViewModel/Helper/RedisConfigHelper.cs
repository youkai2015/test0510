using System.Collections.Generic;

namespace Stock.ViewModel.Helper
{
    public class RedisConfigHelper
    {
        public static Dictionary<string, string> dic = new Dictionary<string, string>();
        public static string GetConnection(string configKey)
        {
            if (dic.ContainsKey(configKey))
            {
                return dic[configKey]?.ToString();
            }
            else
            {
                var conntionString = System.Configuration.ConfigurationManager.AppSettings[configKey];
                dic.Add(configKey, conntionString);
                return conntionString;
            }
        }
    }
}
