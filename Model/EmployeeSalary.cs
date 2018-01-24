using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
     public class EmployeeSalary
    {
         public int ID { set; get; }
         /// <summary>
         /// 姓名
         /// </summary>
         public string Name { set; get; }
         /// <summary>
         /// 性别
         /// </summary>
         public string Sex { set; get; }
         /// <summary>
         /// 年龄
         /// </summary>
         public int age { set; get; }
         /// <summary>
         /// 入职时间
         /// </summary>
         public DateTime InductionTime { set; get; }
         /// <summary>
         /// 基本工资
         /// </summary>
         public decimal Basicsalary { set; get; }
         /// <summary>
         /// 提成
         /// </summary>
         public decimal Commission { set; get; }
         /// <summary>
         /// 奖励
         /// </summary>
         public decimal Reward { set; get; }
         /// <summary>
         /// 惩罚
         /// </summary>
         public decimal Punishment { set; get; }
         /// <summary>
         /// 薪水
         /// </summary>
         public decimal salary { set; get; }
         /// <summary>
         /// 月份
         /// </summary>
         public int Month { set; get; }
         /// <summary>
         /// 备注
         /// </summary>
         public string Remark { set; get; }
    }
}
