using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 花卉需求表
    /// </summary>
    public class FlowerDemand
    {

        public int id { set; get; }

        /// <summary>
        /// 花卉类别名称
        /// </summary>
        public string FlowerCategoryType { set; get; }

        /// <summary>
        /// 具体需求内容
        /// </summary>
        public string ContentMsg { set; get; }
        /// <summary>
        /// 联系人姓名
        /// </summary>
        public string ContactsName { set; get; }

        /// <summary>
        /// 联系人电话
        /// </summary>
        public string ContactsPhone { set; get; }
        /// <summary>
        /// 联系人地址
        /// </summary>
        public string ContactsAddress { set; get; }

        /// <summary>
        /// 状态  //0 待处理  1 已处理
        /// </summary>
        public int State { set; get; }
    }
}
