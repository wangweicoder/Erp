using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Salesman
    {
        public int ID { set; get; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { set; get; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { set; get; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { set; get; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { set; get; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { set; get; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdCard { set; get; }


        /// <summary>
        /// 负责区域
        /// </summary>
        public string ResponsibleArea { set; get; }

        /// <summary>
        /// 上月业绩
        /// </summary>
        public string LastMonthResults { set; get; }
        /// <summary>
        /// 本月目标
        /// </summary>
        public string GoalsMonth { set; get; }

        /// <summary>
        /// 本月业绩
        /// </summary>
        public string PerformanceMonth { set; get; }

        /// <summary>
        /// 提成比例
        /// </summary>
        public string RoyaltyRatio { set; get; }

        /// <summary>
        /// 提成金额
        /// </summary>
        public string Royalty { set; get; }

        /// <summary>
        /// 客户来源
        /// </summary>
        public string CustomerSource { set; get; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { set; get; }
    }
}
