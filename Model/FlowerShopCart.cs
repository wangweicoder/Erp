using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class FlowerShopCart
    {   
        /// <summary>
        /// 主键ID
        /// </summary>
        public int Id { set; get; }

        /// <summary>
        /// 花卉编号
        /// </summary>
        public string FlowerId { set; get; }
        /// <summary>
        /// 用户编号
        /// </summary>
        public string UsersId { set; get; }
        /// <summary>
        /// 购买数量
        /// </summary>
        public int Num { set; get; }
        /// <summary>
        /// 状态 1正常 0禁用 -1 删除
        /// </summary>
        public int Status { set; get; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { set; get; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateTime { set; get; }
    }
}
