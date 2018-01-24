using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class Sys_SettlementSituation
    {
        private string SQLConString = System.Configuration.ConfigurationManager.AppSettings["SQLConString"];


        /// <summary>
        /// 获得一个记录
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Model.SettlementSituation GetInfo(string ID)
        {
            const string sql =
@"SELECT * FROM SettlementSituation WHERE ID=@ID ";
            List<Model.SettlementSituation> SettlementSituation = Factory.DBHelper.Query<Model.SettlementSituation>(SQLConString, sql.ToString(), new DynamicParameters(new { ID }));
            return SettlementSituation.Count() > 0 ? SettlementSituation[0] : null;
        }
        /// <summary>
        /// 获得总记录数
        /// </summary>
        /// <param name="StrWhere"></param>
        /// <returns></returns>
        public int GetListCount(string StrWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(ID) as ID FROM SettlementSituation");
            if (!string.IsNullOrEmpty(StrWhere))
            {
                strSql.Append(" where  1=1 " + StrWhere);
            }
            List<Model.SettlementSituation> SettlementSituation = Factory.DBHelper.Query<Model.SettlementSituation>(SQLConString, strSql.ToString(), new DynamicParameters(new { StrWhere }));
            return SettlementSituation.Count() > 0 ? SettlementSituation[0].ID : 0;
        }

        /// <summary>
        /// 分页获得数据信息
        /// </summary>
        /// <param name="limit">页码大小</param>
        /// <param name="offset">第几页</param>
        /// <param name="UserName">用户名</param>
        /// <param name="Role">角色ID</param>
        /// <returns></returns>
        public List<Model.SettlementSituation> GetList(int offset, string StrWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( SELECT ROW_NUMBER() over(order by id desc) as rn ,* FROM SettlementSituation");
            if (!string.IsNullOrEmpty(StrWhere))
            {
                strSql.Append(" where  1=1 " + StrWhere);
            }
            strSql.Append(")T where t.rn between   @offset and (@offset+9)");
            return Factory.DBHelper.Query<Model.SettlementSituation>(SQLConString, strSql.ToString(), new DynamicParameters(new { offset }));
        }

        /// <summary>
        /// 添加一行记录
        /// </summary>
        /// <param name="SettlementSituation"></param>
        /// <returns></returns>
        public bool AddInfo(Model.SettlementSituation SettlementSituation)
        {

            const string sql =
@"INSERT INTO SettlementSituation(BillingInformation,MonthlyRent,PayType,AmountPayable,AmountPaid,
BillType,Remark,WhatMonth,SettlementDay,CustomerAbbreviation,Isrenew,CompanyName) 
 VALUES(@BillingInformation,@MonthlyRent,@PayType,@AmountPayable,@AmountPaid,@BillType,
@Remark,@WhatMonth,@SettlementDay,@CustomerAbbreviation,@Isrenew,@CompanyName)";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                SettlementSituation.BillingInformation,
                SettlementSituation.MonthlyRent,
                SettlementSituation.PayType,
                SettlementSituation.AmountPayable,
                SettlementSituation.AmountPaid,
                SettlementSituation.BillType,
                SettlementSituation.Remark,
                SettlementSituation.WhatMonth,
                SettlementSituation.SettlementDay,
                SettlementSituation.CustomerAbbreviation,
                SettlementSituation.Isrenew,
                SettlementSituation.CompanyName,
            }));
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="SettlementSituation"></param>
        /// <returns></returns>
        public bool UpdateInfo(Model.SettlementSituation SettlementSituation) 
        {
            const string sql =
@"UPDATE SettlementSituation SET  BillingInformation=@BillingInformation,MonthlyRent=@MonthlyRent,PayType=@PayType,AmountPayable=@AmountPayable,AmountPaid=@AmountPaid,
BillType=@BillType,Remark=@Remark,WhatMonth=@WhatMonth,SettlementDay=@SettlementDay,CustomerAbbreviation=@CustomerAbbreviation,
Isrenew=@Isrenew,CompanyName=@CompanyName  where id=@ID";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                SettlementSituation.BillingInformation,
                SettlementSituation.MonthlyRent,
                SettlementSituation.PayType,
                SettlementSituation.AmountPayable,
                SettlementSituation.AmountPaid,
                SettlementSituation.BillType,
                SettlementSituation.Remark,
                SettlementSituation.WhatMonth,
                SettlementSituation.SettlementDay,
                SettlementSituation.CustomerAbbreviation,
                SettlementSituation.Isrenew,
                SettlementSituation.CompanyName,
                SettlementSituation.ID,
            }));
        }

        public bool DeleteInfo(string ID)
        {
            const string sql =
@"DELETE FROM  SettlementSituation WHERE ID=@ID";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                ID
            }));
        }
    }
}
