using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 花卉类别
    /// </summary>
    public class FlowerCategory
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int id { set; get; }

        /// <summary>
        /// 具体类别名称
        /// </summary>
        public string FlowerCategoryType { set; get; }
    }
}
