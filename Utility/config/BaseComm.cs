using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.config
{
    public class BaseComm
    {
        /// <summary>
        /// 读取WebConfig内设置节点值
        /// </summary>
        /// <param name="key">WebConfig中配置的Key节点</param>
        /// <param name="defaultValue">为空时默认值</param>
        /// <returns></returns>
        public static string GetAppSettings(string key, string defaultValue)
        {
            string sValue = System.Configuration.ConfigurationManager.AppSettings[key];

            if (string.IsNullOrEmpty(sValue))
            {
                if (!string.IsNullOrEmpty(defaultValue))
                {
                    sValue = defaultValue;
                }
                else
                {
                    sValue = string.Empty;
                }
            }
            return sValue;
        }
        /// <summary>
        /// 读取WebConfig内设置节点值
        /// </summary>
        /// <param name="key">WebConfig中配置的Key节点</param>
        /// <param name="defaultValue">为空时默认值</param>
        /// <returns></returns>
        public static int GetAppSettings(string key, int defaultValue)
        {
            string sValue = GetAppSettings(key, "");
            if (string.IsNullOrEmpty(sValue))
            {
                int nPort = Convert.ToInt32(sValue, 10);
                if (nPort >= 0)
                    return nPort;
            }
            return defaultValue;
        }
    }
}
