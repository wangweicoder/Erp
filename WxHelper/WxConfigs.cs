using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxHelper
{
    /// <summary>
    /// 微信基本配置页面
    /// </summary>
    public class WxConfigs
    {
        /// <summary>
        /// 微信APPID
        /// </summary>
        public static readonly string WxAppId = System.Configuration.ConfigurationManager.AppSettings["WxAppId"];
        /// <summary>
        /// 微信secret
        /// </summary>
        public static readonly string appsecret = System.Configuration.ConfigurationManager.AppSettings["appsecret"];
    }

}
