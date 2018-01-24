using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class Sys_EmployeeSalary
    {
        private string SQLConString = System.Configuration.ConfigurationManager.AppSettings["SQLConString"];

        /// <summary>
        /// 获得总记录数
        /// </summary>
        /// <param name="StrWhere"></param>
        /// <returns></returns>
        public int GetUserAdminListCount(string StrWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(ID) as ID FROM EmployeeSalary");
            if (!string.IsNullOrEmpty(StrWhere))
            {
                strSql.Append(" where  1=1 " + StrWhere);
            }
            List<Model.EmployeeSalary> UserAdminList = Factory.DBHelper.Query<Model.EmployeeSalary>(SQLConString, strSql.ToString(), new DynamicParameters(new { StrWhere }));
            return UserAdminList.Count() > 0 ? UserAdminList[0].ID : 0;
        }

        /// <summary>
        /// 分页获得数据信息
        /// </summary>
        /// <param name="limit">页码大小</param>
        /// <param name="offset">第几页</param>
        /// <param name="UserName">用户名</param>
        /// <param name="Role">角色ID</param>
        /// <returns></returns>
        public List<Model.EmployeeSalary> UserAdminList(int limit, int offset, string StrWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( SELECT ROW_NUMBER() over(order by id desc) as rn ,*  FROM EmployeeSalary");
            if (!string.IsNullOrEmpty(StrWhere))
            {
                strSql.Append(" where  1=1 " + StrWhere);
            }
            strSql.Append(")T where t.rn between   @offset and (@offset+9)");
            return Factory.DBHelper.Query<Model.EmployeeSalary>(SQLConString, strSql.ToString(), new DynamicParameters(new { offset }));
        }

        public bool Update(Model.EmployeeSalary EmployeeSalary) 
        {
            const string sql =
@"UPDATE  EmployeeSalary SET Name=@Name,Sex=@Sex,age=@age,InductionTime=@InductionTime,Basicsalary=@Basicsalary,
 Commission=@Commission,Reward=@Reward,Punishment=@Punishment,salary=@salary,Month=@Month,Remark=@Remark  WHERE ID=@ID";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                EmployeeSalary.Name,
                EmployeeSalary.Sex,
                EmployeeSalary.age,
                EmployeeSalary.InductionTime,
                EmployeeSalary.Basicsalary,

                EmployeeSalary.Commission,
                EmployeeSalary.Reward,

                EmployeeSalary.Punishment,
                EmployeeSalary.salary,

                EmployeeSalary.Month,
                EmployeeSalary.Remark,
                EmployeeSalary.ID,

            }));
        }
        public bool Add(Model.EmployeeSalary EmployeeSalary) 
        {
            const string sql =
@"INSERT INTO EmployeeSalary(Name,Sex,age,InductionTime,Basicsalary,Commission,Reward,Punishment,salary,Month,Remark) 
 VALUES(@Name,@Sex,@age,@InductionTime,@Basicsalary,@Commission,@Reward,@Punishment,@salary,@Month,@Remark)";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                EmployeeSalary.Name,
                EmployeeSalary.Sex,
                EmployeeSalary.age,
                EmployeeSalary.InductionTime,
                EmployeeSalary.Basicsalary,

                EmployeeSalary.Commission,
                EmployeeSalary.Reward,

                EmployeeSalary.Punishment,
                EmployeeSalary.salary,

                EmployeeSalary.Month,
                EmployeeSalary.Remark,
            }));
        }


        public Model.EmployeeSalary GetModel(string id)
        {
            const string sql =
@"SELECT * FROM  EmployeeSalary  WHERE ID=@id";
            List<Model.EmployeeSalary> UserAdminList = Factory.DBHelper.Query<Model.EmployeeSalary>(SQLConString, sql.ToString(), new DynamicParameters(new { id }));
            return UserAdminList.Count() > 0 ? UserAdminList[0] :null;
        }

        public bool Delete(string id)
        {
            const string sql =
@"DELETE FROM EmployeeSalary  WHERE id=@id";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                id
            }));
        }
    }
}
