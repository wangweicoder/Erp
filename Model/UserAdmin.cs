using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 管理员后台
    /// </summary>
    public class UserAdmin
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int ID { set; get; }

        /// <summary>
        /// 账号
        /// </summary>
        public string UserName { set; get; }

        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord { set; get; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { set; get; }

        /// <summary>
        /// 是否启用 0启用 1禁用
        /// </summary>
        public int IsEnable { set; get; }

        /// <summary>
        /// 微信OpenId
        /// </summary>
        public string OpenId { set; get; }
        /// <summary>
        /// 设定的考勤地址
        /// </summary>
        public string CheckAddress { set; get; }

        /// <summary>
        /// 角色代码
        /// </summary>
        public string RoleCode { set; get; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { set; get; }

        /// <summary>
        /// 所属公司
        /// </summary>
        public string OwnedCompany { set; get; }


        /// <summary>
        /// 对应业务负责人主键ID
        /// </summary>
        public string WorkUsersId { set; get; }

        /// <summary>
        /// 对应业务负责人真实姓名
        /// </summary>
        public string WorkRealName { set; get; }

        /// <summary>
        /// LOGO图标
        /// </summary>
        public string LogoPhoto { set; get; }

        /// <summary>
        /// 周次
        /// </summary>
        public string Weekly { set; get; }
    }
}
