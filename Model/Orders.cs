using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 订单表
    /// </summary>
    public class Orders
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int id { set; get; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderId { set; get; }

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
        /// 下单时间
        /// </summary>
        public DateTime CreateTime { set; get; }

        /// <summary>
        /// 下单会员
        /// </summary>
        public int UsersId { set; get; }

        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string ConsigneeName { set; get; }

        /// <summary>
        /// 收货人电话
        /// </summary>
        public string ConsigneePhone { set; get; }

        /// <summary>
        /// 收货人地址
        /// </summary>
        public string ConsigneAaddress { set; get; }

        /// <summary>
        /// 订单状态  1.待支付  2.待发货  3.已发货  4.已签收  5.取消订单
        /// </summary>
        public int OrdersState { set; get; }
        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime PayTime { set; get; }

        /// <summary>
        /// 支付流水号
        /// </summary>
        public string PayOrdersNum { set; get; }

        /// <summary>
        /// 物流公司编号
        /// </summary>
        public string LogisticsCompanyNumber { set; get; }

        /// <summary>
        /// 物流单号
        /// </summary>
        public string LogisticsNumber { set; get; }

        /// <summary>
        /// 发货时间
        /// </summary>
        public DateTime OrderDelivery { set; get; }

        /// <summary>
        /// 商品总数
        /// </summary>
        public int GoodsSum { set; get; }

        /// <summary>
        /// 会员真实姓名
        /// </summary>
        public string RealName { set; get; }
        /// <summary>
        /// 登陆账号
        /// </summary>
        public string UserName { set; get; }
    }
}
