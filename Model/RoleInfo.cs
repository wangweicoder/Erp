using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 角色表
    /// </summary>
    public  class RoleInfo
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int ID { set; get; }

        /// <summary>
        /// 角色代码
        /// </summary>
        public string RoleCode { set; get; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { set; get; }
        /// <summary>
        /// 菜单ID集合
        /// </summary>
        public string MenuIdList { set; get; }
    }
}
