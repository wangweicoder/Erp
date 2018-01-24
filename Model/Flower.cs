using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 花卉表
    /// </summary>
    public class Flower
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int id { set; get; }

        /// <summary>
        /// 花卉名
        /// </summary>
        public string FlowerWatchName { set; get; }
        /// <summary>
        /// 花卉照片
        /// </summary>
        public string FlowerWatchPhoto { set; get; }
        /// <summary>
        /// 花卉成本价
        /// </summary>
        public decimal FlowerCostPrice { set; get; }
        /// <summary>
        /// 花卉销售价
        /// </summary>
        public decimal FlowerSalesPrice { set; get; }

        /// <summary>
        /// 花卉库存
        /// </summary>
        public int FlowerStock { set; get; }
        /// <summary>
        /// 花卉文字介绍
        /// </summary>
        public string FlowerIntroduction { set; get; }



        /// <summary>
        /// 花卉品种
        /// </summary>
        public string FlowerWatchType { set; get; }

        /// <summary>
        /// 花卉编号
        /// </summary>
        public string FlowerNumber { set; get; }

        /// <summary>
        /// 习性
        /// </summary>
        public string XiXin { set; get; }

        /// <summary>
        /// 养护方法
        /// </summary>
        public string YangHuFangFa { set; get; }
    }
}
