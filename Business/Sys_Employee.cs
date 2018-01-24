using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    /// <summary>
    /// 员工同一查询显示
    /// </summary>
    public class Sys_Employee
    {
        private string SQLConString = System.Configuration.ConfigurationManager.AppSettings["SQLConString"];

        /// <summary>
        /// 获得列表信息
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="StrWhere"></param>
        /// <returns></returns>
        public List<Model.Employee> GetList(int limit, int offset, string StrWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY CreateTime DESC) as rn,* FROM (");
            if (string.IsNullOrEmpty(StrWhere))
            {
                strSql.Append("SELECT Name,Number,CreateTime,DataType  FROM EmployeeTurnover  where  1=1 ");
                strSql.Append("UNION ALL SELECT  Name,Number,CreateTime,DataType  FROM EmployeeEntry  where  1=1 ");
            }
            if (!string.IsNullOrEmpty(StrWhere))
            {
                strSql.Append(" SELECT Name,Number,CreateTime,DataType  FROM EmployeeTurnover  where  1=1 " + StrWhere);
                strSql.Append(" UNION ALL SELECT  Name,Number,CreateTime,DataType  FROM EmployeeEntry where  1=1 " + StrWhere);
            }
            strSql.Append(")  T  ) as t where  t.rn between   @offset and (@offset+9)");
            return Factory.DBHelper.Query<Model.Employee>(SQLConString, strSql.ToString(), new DynamicParameters(new { offset }));
        }

        /// <summary>
        /// 获得总记录数
        /// </summary>
        /// <param name="StrWhere"></param>
        /// <returns></returns>
        public int GetCount(string StrWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(Name)  as ID from( SELECT  id,Name,Number,CreateTime,DataType  FROM EmployeeTurnover  where  1=1 " + StrWhere);
            strSql.Append(" UNION ALL SELECT  id,Name,Number,CreateTime,DataType  FROM EmployeeEntry where  1=1 " + StrWhere);
            strSql.Append(") T");
            List<Model.Employee> EmployeeList = Factory.DBHelper.Query<Model.Employee>(SQLConString, strSql.ToString(), new DynamicParameters(new { StrWhere }));
            return EmployeeList.Count() > 0 ? EmployeeList[0].ID : 0;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteInfo(string Number)
        {
            const string sql =
@"DELETE FROM  EmployeeTurnover  WHERE Number=@Number";
            Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                Number
            }));
            const string sql1 =
@"DELETE FROM  EmployeeEntry  WHERE Number=@Number";
            return Factory.DBHelper.ExecSQL(SQLConString, sql1.ToString(), new DynamicParameters(new
            {
                Number
            }));
        }
    }
}
