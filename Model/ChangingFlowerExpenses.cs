using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public  class ChangingFlowerExpenses
    {

        public int ID { set; get; }

        /// <summary>
        /// 花卉名
        /// </summary>
        public string FlowerName { set; get; }
        /// <summary>
        /// 花卉编号
        /// </summary>
        public string FlowerNUm { set; get; }
        /// <summary>
        /// 花卉数量
        /// </summary>
        public string Number { set; get; }
        /// <summary>
        /// 成本价
        /// </summary>
        public decimal CostPrice { set; get; }
        /// <summary>
        /// 销售价
        /// </summary>
        public decimal SalePrice { set; get; }

        /// <summary>
        /// 更换地址
        /// </summary>
        public string ChangeAddress { set; get; }
        /// <summary>
        /// 更换原因
        /// </summary>
        public string ReplacementReason { set; get; }
        /// <summary>
        /// 更换人手机号
        /// </summary>
        public string ReplacementTelephone { set; get; }

        /// <summary>
        /// 更换人姓名
        /// </summary>
        public string replacementName { set; get; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime CreateTime { set; get; }
        /// <summary>
        /// 更换地所属单位
        /// </summary>
        public string ReplacementUnit { set; get; }
    }
}
