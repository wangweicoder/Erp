using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 菜单
    /// </summary>
    public  class Menu
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int id { set; get; }

        /// <summary>
        /// 菜单代码
        /// </summary>
        public string MenuCode { set; get; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { set; get; }

        /// <summary>
        /// URL地址
        /// </summary>
        public string UrlPath { set; get; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public int OrderBy { set; get; }

        /// <summary>
        /// 层级
        /// </summary>
        public int Hierarchy { set; get; }

        /// <summary>
        /// 上级菜单ID
        /// </summary>
        public int SuperiorMenuID { set; get; }

        /// <summary>
        /// 终端 PC 或者 手机
        /// </summary>
        public string Terminal { set; get; }
    }
}
