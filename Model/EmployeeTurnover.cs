using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 员工转正
    /// </summary>
    public class EmployeeTurnover
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ID { set; get; }

        /// <summary>
        /// 编号
        /// </summary>
        public string Number { set; get; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { set; get; }


        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { set; get; }

        /// <summary>
        /// 出生年月
        /// </summary>
        public DateTime Birthday { set; get; }

        /// <summary>
        /// 所属部门
        /// </summary>
        public string SubordinateSector { set; get; }

        /// <summary>
        /// 职务
        /// </summary>
        public string Post { set; get; }

        /// <summary>
        /// 职称
        /// </summary>
        public string PostTitle { set; get; }

        /// <summary>
        /// 入职时间
        /// </summary>
        public DateTime InductionTime { set; get; }

        /// <summary>
        /// 试用期开始时间
        /// </summary>
        public DateTime StartTimeOfProbation { set; get; }

        /// <summary>
        /// 试用期结束时间
        /// </summary>
        public DateTime EndOfProbationPeriod { set; get; }

        /// <summary>
        /// 本人述职
        /// </summary>
        public string WorkRepor { set; get; }
        /// <summary>
        /// 上级主管意见
        /// </summary>
        public string SuperiorOpinion { set; get; }

        /// <summary>
        /// 行政人事部意见
        /// </summary>
        public string AdministrationPersonnelDepartmentOpinion { set; get; }

        /// <summary>
        /// 总经理意见
        /// </summary>
        public string GeneralManagerOpinion { set; get; }
        /// <summary>
        /// 信息类型
        /// </summary>
        public string DataType { set; get; }
    }
}
