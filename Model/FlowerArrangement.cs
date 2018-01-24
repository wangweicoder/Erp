using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public  class FlowerArrangement
    {
        public int id { set; get; }

        /// <summary>
        /// 摆放位置
        /// </summary>
        public string arrangement { set; get; }

       
        /// <summary>
        /// 照片
        /// </summary>
        public string Photo { set; get; }

        /// <summary>
        /// 规格
        /// </summary>
        public string Specifications { set; get; }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal UnitPrice { set; get; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Count { set; get; }
        /// <summary>
        /// 合计
        /// </summary>
        public decimal Total { set; get; }
        /// <summary>
        /// 所属公司
        /// </summary>
        public string OwnedCompany { set; get; }
        /// <summary>
        /// 花卉名
        /// </summary>
        public string FlowerWatchName { set; get; }

        /// <summary>
        /// 花卉种类
        /// </summary>
        public string FlowerWatchType { set; get; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { set; get; }

        /// <summary>
        /// 所属用户
        /// </summary>
        public int belongUsersId { set; get; }
        /// <summary>
        /// 周次
        /// </summary>
        public string Weekly { set; get; }
        
        /// <summary>
        /// 所对应的花卉(商品ID)
        /// </summary>
        public int ShopId { set; get; }

        
        /// <summary>
        /// 种类
        /// </summary>
        public string FlowerType { set; get; }

        /// <summary>
        /// 二维码地址
        /// </summary>
        public string ImgORCodePath { set; get; }

        /// <summary>
        /// 习性s
        /// </summary>
        public string XiXin { set; get; }
        /// <summary>
        /// 习性s
        /// </summary>
        public string YangHuFangFa { set; get; }
        /// <summary>
        /// 销售价格
        /// </summary>
        public decimal FlowerSalesPrice { set; get; }

    }
}
