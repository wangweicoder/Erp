using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class Sys_Salesman
    {
        private string SQLConString = System.Configuration.ConfigurationManager.AppSettings["SQLConString"];

        /// <summary>
        /// 获得总记录数
        /// </summary>
        /// <param name="StrWhere"></param>
        /// <returns></returns>
        public int GetSalesmanListCount(string StrWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(ID) as ID FROM Salesman");
            if (!string.IsNullOrEmpty(StrWhere))
            {
                strSql.Append(" where  1=1 " + StrWhere);
            }
            List<Model.Salesman> Salesman = Factory.DBHelper.Query<Model.Salesman>(SQLConString, strSql.ToString(), new DynamicParameters(new { StrWhere }));
            return Salesman.Count() > 0 ? Salesman[0].ID : 0;
        }

        /// <summary>
        /// 分页获得数据信息
        /// </summary>
        /// <param name="limit">页码大小</param>
        /// <param name="offset">第几页</param>
        /// <param name="UserName">用户名</param>
        /// <param name="Role">角色ID</param>
        /// <returns></returns>
        public List<Model.Salesman> SalesmanList(int offset, string StrWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( SELECT ROW_NUMBER() over(order by Salesman.id desc) as rn ,* FROM Salesman");
            if (!string.IsNullOrEmpty(StrWhere))
            {
                strSql.Append(" where  1=1 " + StrWhere);
            }
            strSql.Append(")T where t.rn between   @offset and (@offset+9)");
            return Factory.DBHelper.Query<Model.Salesman>(SQLConString, strSql.ToString(), new DynamicParameters(new { offset }));
        }

        public Model.Salesman GetInfo(string ID)
        {
            const string sql =
@"SELECT  * FROM  Salesman WHERE ID=@ID";
            List<Model.Salesman> Salesman = Factory.DBHelper.Query<Model.Salesman>(SQLConString, sql.ToString(), new DynamicParameters(new { ID }));
            return Salesman.Count() > 0 ? Salesman[0] : null;
        }

        /// <summary>
        /// 添加一条记录
        /// </summary>
        /// <param name="Salesman"></param>
        /// <returns></returns>
        public bool AddSalesman(Model.Salesman Salesman)
        {
            const string sql =
@"INSERT INTO Salesman (RealName,Phone,Address,Sex,Email,IdCard,ResponsibleArea,LastMonthResults,
GoalsMonth,PerformanceMonth,RoyaltyRatio,Royalty,CustomerSource,Remark)  VALUES(@RealName,@Phone,@Address,@Sex,@Email,@IdCard,@ResponsibleArea,
@LastMonthResults,@GoalsMonth,@PerformanceMonth,@RoyaltyRatio,@Royalty,@CustomerSource,@Remark)";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                Salesman.RealName,
                Salesman.Phone,
                Salesman.Address,
                Salesman.Sex,
                Salesman.Email,

                Salesman.IdCard,
                Salesman.ResponsibleArea,
                Salesman.LastMonthResults,
                Salesman.GoalsMonth,
                Salesman.PerformanceMonth,


                Salesman.RoyaltyRatio,
                Salesman.Royalty,
                Salesman.CustomerSource,
                Salesman.Remark,
            }));
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="Salesman"></param>
        /// <returns></returns>
        public bool EditSalesman(Model.Salesman Salesman)
        {
            const string sql =
@"UPDATE Salesman  SET RealName=@RealName,Phone=@Phone,Address=@Address,Sex=@Sex,Email=@Email,IdCard=@IdCard,ResponsibleArea=@ResponsibleArea,
LastMonthResults=@LastMonthResults,GoalsMonth=@GoalsMonth,PerformanceMonth=@PerformanceMonth,RoyaltyRatio=@RoyaltyRatio,
Royalty=@Royalty,CustomerSource=@CustomerSource,Remark=@Remark

WHERE ID=@ID";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                Salesman.RealName,
                Salesman.Phone,
                Salesman.Address,
                Salesman.Sex,
                Salesman.Email,

                Salesman.IdCard,
                Salesman.ResponsibleArea,
                Salesman.LastMonthResults,
                Salesman.GoalsMonth,
                Salesman.PerformanceMonth,

                Salesman.RoyaltyRatio,
                Salesman.Royalty,
                Salesman.CustomerSource,
                Salesman.Remark,
                Salesman.ID,
            }));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteSalesman(string id)
        {
            const string sql =
@"DELETE FROM  Salesman  WHERE ID=@id";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                id
            }));
        }
    }
}
