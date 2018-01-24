using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ProblemsAndSuggestions
    {
        public int id { set; get; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public int UsersId { set; get; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { set; get; }

        /// <summary>
        /// 问题
        /// </summary>
        public string Problems { set; get; }

        /// <summary>
        /// 建议
        /// </summary>
        public string Suggestions { set; get; }

        /// <summary>
        /// 照片集合
        /// </summary>
        public string PhotoList { set; get; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string phone { set; get; }

        /// <summary>
        /// 联系地址
        /// </summary>
        public string Address { set; get; }

        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime CreateTime { set; get; }

        /// <summary>
        /// 1 待处理  2 已处理
        /// </summary>
        public int State { set; get; }
    }
}
