using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 管理员后台
    /// </summary>
    public class UsersLoginLog
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int ID { set; get; }

        /// <summary>
        /// 账号
        /// </summary>
        public string UsersId { set; get; }

        /// <summary>
        /// 年
        /// </summary>
        public string  Year { set; get; }

        /// <summary>
        /// 月
        /// </summary>
        public string Month { set; get; }
        
        /// <summary>
        /// 日
        /// </summary>
        public string Day { set; get; }
        
        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNum　 { set; get; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { set; get; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public string LoginTime { set; get; }
        /// <summary>
        /// UserName
        /// </summary>
        public string UserName { set; get; }
        /// <summary>
        ///  RealName
        /// </summary>
        public string RealName { set; get; }
    }
}
