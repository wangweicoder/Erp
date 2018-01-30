using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class FlowerCartVM
    {   
        /// <summary>
        /// 主键ID
        /// </summary>
        public int Id { set; get; }

        /// <summary>
        /// 花卉名称
        /// </summary>
        public string FlowerWatchName { set; get; }
        /// <summary>
        /// 花卉图片
        /// </summary>
        public string FlowerWatchPhoto { set; get; }
        /// <summary>
        /// 购买数量
        /// </summary>
        public int Num { set; get; }
        /// <summary>
        /// 状态 1正常 0禁用 -1 删除
        /// </summary>
        public int Status { set; get; }
        /// <summary>
        /// 花卉销售价
        /// </summary>
        public decimal FlowerSalesPrice { set; get; }
        /// <summary>
        /// 花卉文字介绍
        /// </summary>
        public string FlowerIntroduction { set; get; }        
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateTime { set; get; }
    }
}
