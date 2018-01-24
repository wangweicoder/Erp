using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 订单明细表
    /// </summary>
    public class OrdersDetails
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int id { set; get; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderId { set; get; }

        /// 花卉名
        /// </summary>
        public string FlowerWatchName { set; get; }
        /// <summary>
        /// 花卉照片
        /// </summary>
        public string FlowerWatchPhoto { set; get; }

        /// <summary>
        /// 商品(花卉)编号
        /// </summary>
        public string FlowerNumber { set; get; }

        /// <summary>
        /// 销售价
        /// </summary>
        public decimal SellingPrice { set; get; }
        /// <summary>
        /// 成本价
        /// </summary>
        public decimal CostPrice { set; get; }

        /// <summary>
        /// 利润
        /// </summary>
        public decimal profit { set; get; }

        /// <summary>
        /// 销售数量
        /// </summary>
        public int SellingNum { set; get; }
    }
}
