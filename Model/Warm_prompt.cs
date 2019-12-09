using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 温馨提示
    /// </summary>
    public class Warm_prompt
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int ID { set; get; }        

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
