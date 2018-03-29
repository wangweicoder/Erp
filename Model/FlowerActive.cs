using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class FlowerActive
    {   
        /// <summary>
        /// 主键ID
        /// </summary>
        public int Id { set; get; }

        /// <summary>
        /// 关联花卉编号
        /// </summary>
        public string FlowerId { set; get; }
        /// <summary>
        /// 关联花卉名称
        /// </summary>
        public string FlowerWatchName { get; set; }
        /// <summary>
        /// 用户编号
        /// </summary>
        public string UsersId { set; get; }
        /// <summary>
        /// 内容
        /// </summary>       
        public string Content { set; get; }

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
