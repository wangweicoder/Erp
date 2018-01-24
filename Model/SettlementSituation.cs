using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class SettlementSituation
    {
        public int ID { set; get; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { set; get; }

        /// <summary>
        /// 开票信息
        /// </summary>
        public string BillingInformation { set; get; }

        /// <summary>
        /// 月租金
        /// </summary>
        public decimal MonthlyRent { set; get; }

        /// <summary>
        /// 结算方式
        /// </summary>
        public string PayType { set; get; }

        /// <summary>
        /// 应付金额
        /// </summary>
        public decimal AmountPayable { set; get; }

        /// <summary>
        /// 实付金额
        /// </summary>
        public decimal AmountPaid { set; get; }

        /// <summary>
        /// 票据类型
        /// </summary>
        public string BillType { set; get; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { set; get; }

        /// <summary>
        /// 续费月份
        /// </summary>
        public string WhatMonth { set; get; }

        /// <summary>
        /// 结算日
        /// </summary>
        public string SettlementDay { set; get; }

        /// <summary>
        /// 客户简称
        /// </summary>
        public string CustomerAbbreviation { set; get; }

        /// <summary>
        /// 是否续费
        /// </summary>
        public string Isrenew { set; get; }

    }
}
