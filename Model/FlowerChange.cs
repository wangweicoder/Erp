using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 更换花卉
    /// </summary>
    public class FlowerChange
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int id { set; get; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Number { set; get; }

        /// <summary>
        /// 花卉编号
        /// </summary>
        public string FlowerNumber { set; get; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime time { set; get; }

        /// <summary>
        /// 会员编号
        /// </summary>
        public int UsersId { set; get; }

        /// <summary>
        /// 处理类型  例如 更换花卉
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
        /// 更换花卉工作人ID
        /// </summary>
        public int WorkUsersId { set; get; }

        /// <summary>
        /// 更换花卉工作人真实姓名
        /// </summary>
        public string WorkUsersRealName { set; get; }
                      
        /// <summary>
        /// 客户所属公司
        /// </summary>
        public string OwnedCompany { set; get; }
        /// <summary>
        /// 客户UsersId
        /// </summary>
        public int OwnedUsersId { set; get; }

        /// <summary>
        /// 花卉种类
        /// </summary>
        public string FlowerType { set; get; }

        /// <summary>
        /// 摆放位置
        /// </summary>
        public string PlacingPosition { set; get; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Sum { set; get; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Reamrk { set; get; }

        /// <summary>
        /// 状态  
        /// </summary>
        public string State { set; get; }
    }
}
