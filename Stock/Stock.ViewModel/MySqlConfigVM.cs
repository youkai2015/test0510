using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.ViewModel
{
    public class MySqlConfigVM
    {
        /// <summary>
        /// 检测传入的商户对应的链接串是否配置
        /// </summary>
        /// <param name="ConnectionStringCode">商户编码</param>
        /// <returns>true:已配置，false：未配置</returns>
        public static bool TestingCongfig(string ConnectionStringCode)
        {
          
            if (!string.IsNullOrEmpty(ConnectionStringCode))
            {
                var s = System.Configuration.ConfigurationManager.AppSettings[ConnectionStringCode];
                if (s==null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
