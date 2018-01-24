using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 订单日志表
    /// </summary>
    public class OrdersLog
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int id { set; get; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrdersId { set; get; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime Time { set; get; }

        /// <summary>
        /// 操作人账号
        /// </summary>
        public string UserName { set; get; }

        /// <summary>
        /// 订单状态  1.待支付  2.已支付  3.待发货  4.已发货  5.取消订单
        /// </summary>
        public int OrdersState { set; get; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { set; get; }
    }
}
