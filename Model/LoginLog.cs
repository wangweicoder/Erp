using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 登录日志
    /// </summary>
    public class LoginLog
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { set; get; }

        /// <summary>
        /// 账号密码
        /// </summary>
        public string UserName { set; get; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { set; get; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public string LoginTime { set; get; }

        /// <summary>
        /// 登录IP 
        /// </summary>
        public string IP { set; get; }
    }
}
