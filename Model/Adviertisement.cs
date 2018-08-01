using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 广告
    /// </summary>
    public class Adviertisement
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
        /// 标题
        /// </summary>
        public string  Title { set; get; }

        /// <summary>
        /// 图片
        /// </summary>
        public string Picture { set; get; }

        /// <summary>
        /// 图片1
        /// </summary>
        public string Picturef { set; get; }

        /// <summary>
        /// 图片2
        /// </summary>
        public string Pictures { set; get; }

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
