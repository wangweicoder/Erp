using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 花卉养护类 养护 
    /// </summary>
    public class FlowerTreatment
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int id { set; get; }

        /// <summary>
        /// 花卉编号
        /// </summary>
        public string FlowerNumber { set; get; }
        /// <summary>
        /// 摆放id
        /// </summary>
        public string ArrangementId { set; get; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime time { set; get; }
       
        /// <summary>
        /// 开始养护时间
        /// </summary>
        public DateTime? starttime { set; get; }
       
        /// <summary>
        /// 结束养护时间
        /// </summary>
        public DateTime? endtime { set; get; }

        /// <summary>
        /// 状态  
        /// </summary>
        public string State { set; get; }
        /// <summary>
        /// 会员编号
        /// </summary>
        public int UsersId { set; get; }

        /// <summary>
        /// 处理类型  例如 养护花卉 
        /// </summary>
        public string FlowerTreatmentType { set; get; }

        /// <summary>
        /// 操作内容
        /// </summary>
        public string ContentMsg { set; get; }

        /// <summary>
        /// 图片
        /// </summary>
        public string Photo { set; get; }
        /// <summary>
        /// 更换后图片
        /// </summary>
        public string ChangePhoto { set; get; }
        /// <summary>
        /// 所属客户ID
        /// </summary>
        public string OwnedUsersId { set; get; }

        /// <summary>
        /// 所属客户真实姓名
        /// </summary>
        public string OwnedUsersRealName { set; get; }

        /// <summary>
        /// 花卉摆放地址
        /// </summary>
        public string FlowerTreatmentAddress { set; get; }

        /// <summary>
        /// 养护人姓名
        /// </summary>
        public string UserRealName { set; get; }

        /// <summary>
        /// 所属客户LOGO
        /// </summary>
        public string LogoPhoto { set; get; }

        /// <summary>
        /// 所属客户公司名称
        /// </summary>
        public string OwnedCompany { set; get; }
        /// <summary>
        /// 外键用户表的公司名称
        /// </summary>
        public string CompanyName { set; get; }
    }
}
