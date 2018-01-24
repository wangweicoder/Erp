using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 管理员权限表
    /// </summary>
    public class AdminJurisdiction
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
        /// 菜单代码
        /// </summary>
        public string MenuID { set; get; }
        /// <summary>
        /// 按钮代码
        /// </summary>
        public string ButtonID { set; get; }

        /// <summary>
        /// 添加时间(数据库默认为插入时间)
        /// </summary>
        public DateTime AddTime { set; get; }
    }
}
