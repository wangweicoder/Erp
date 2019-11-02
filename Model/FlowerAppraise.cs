using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 评价
    /// </summary>
    public class FlowerAppraise
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int ID { set; get; }

        /// <summary>
        /// 评价人的账号
        /// </summary>
        public string UsersId { set; get; }

        /// <summary>
        /// 花卉摆放的id
        /// </summary>
        public string ArrangementId{ set; get; }

        /// <summary>
        /// 图片
        /// </summary>
        public string Pictures { set; get; }

        /// <summary>
        /// 点赞是1，差评是0
        /// </summary>
        public string IsGood { set; get; }   
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { set; get; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { set; get; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdateTime { set; get; }
        
    }
}
