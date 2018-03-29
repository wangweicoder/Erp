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
    public class ViewOrders
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
        /// 下单时间
        /// </summary>
        public DateTime CreateTime { set; get; }

        /// <summary>
        /// 下单会员
        /// </summary>
        public int UsersId { set; get; }

        /// <summary>
        /// 订单状态  1.待支付  2.待发货  3.已发货  4.已签收  5.取消订单
        /// </summary>
        public int OrdersState { set; get; }
        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime PayTime { set; get; }
        
        /// <summary>
        /// 会员真实姓名
        /// </summary>
        public string Url { set; get; }
        /// <summary>
        /// 登陆账号
        /// </summary>
        public string UserName { set; get; }
        /// <summary>
        /// 订单详情列表
        /// </summary>
        public List<Model.OrdersDetails> ListDetails { get; set; }
    }
}
