using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
     public class ContractCustomer
    {
         /// <summary>
         /// 主键ID
         /// </summary>
         public int ID { set; get; }


         /// <summary>
         /// 真实姓名
         /// </summary>
         public string RealName { set; get; }

         /// <summary>
         /// 性别
         /// </summary>
         public string Sex { set; get; }

         /// <summary>
         /// 电话号码
         /// </summary>
         public string phone { set; get; }

         /// <summary>
         /// 详细地址
         /// </summary>
         public string Address { set; get; }

         /// <summary>
         /// 签约日期
         /// </summary>
         public string CooperationDate { set; get; }

         /// <summary>
         /// 签约项目
         /// </summary>
         public string DockingProject { set; get; }

         /// <summary>
         /// 签约项目负责人
         /// </summary>
         public string DockingprojectLeader { set; get; }

         /// <summary>
         /// 是否签约
         /// </summary>
         public string IsSign { set; get; }
     }
}
