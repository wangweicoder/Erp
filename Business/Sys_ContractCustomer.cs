using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class Sys_ContractCustomer
    {
        private string SQLConString = System.Configuration.ConfigurationManager.AppSettings["SQLConString"];

        /// <summary>
        /// 获得总记录数
        /// </summary>
        /// <param name="StrWhere"></param>
        /// <returns></returns>
        public int GetListCount(string StrWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(ID) as ID FROM ContractCustomer");
            if (!string.IsNullOrEmpty(StrWhere))
            {
                strSql.Append(" where  1=1 " + StrWhere);
            }
            List<Model.ContractCustomer> ContractCustomer = Factory.DBHelper.Query<Model.ContractCustomer>(SQLConString, strSql.ToString(), new DynamicParameters(new { StrWhere }));
            return ContractCustomer.Count() > 0 ? ContractCustomer[0].ID : 0;
        }

        /// <summary>
        /// 分页获得数据信息
        /// </summary>
        /// <param name="limit">页码大小</param>
        /// <param name="offset">第几页</param>
        /// <param name="UserName">用户名</param>
        /// <param name="Role">角色ID</param>
        /// <returns></returns>
        public List<Model.ContractCustomer> GetList( int offset, string StrWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( SELECT ROW_NUMBER() over(order by id desc) as rn ,* FROM ContractCustomer");
            if (!string.IsNullOrEmpty(StrWhere))
            {
                strSql.Append(" where  1=1 " + StrWhere);
            }
            strSql.Append(")T where t.rn between   @offset and (@offset+9)");
            return Factory.DBHelper.Query<Model.ContractCustomer>(SQLConString, strSql.ToString(), new DynamicParameters(new { offset }));
        }


        public bool AddInfo(Model.ContractCustomer ContractCustomer) 
        {
            const string sql =
@"INSERT INTO ContractCustomer(RealName,Sex,phone,Address,CooperationDate,DockingProject,DockingprojectLeader,IsSign)
  VALUES(@RealName,@Sex,@phone,@Address,@CooperationDate,@DockingProject,@DockingprojectLeader,@IsSign)";

            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                ContractCustomer.RealName,
                ContractCustomer.Sex,
                ContractCustomer.phone,
                ContractCustomer.Address,
                ContractCustomer.CooperationDate,
                ContractCustomer.DockingProject,
                ContractCustomer.DockingprojectLeader,
                ContractCustomer.IsSign,
            }));
        }

        public bool UpdateInfo(Model.ContractCustomer ContractCustomer)
        {
            const string sql =
@"UPDATE ContractCustomer SET RealName=@RealName,Sex=@Sex,phone=@phone,Address=@Address,CooperationDate=@CooperationDate,DockingProject=@DockingProject,
DockingprojectLeader=@DockingprojectLeader,IsSign=@IsSign  WHERE ID=@ID";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                ContractCustomer.RealName,
                ContractCustomer.Sex,
                ContractCustomer.phone,
                ContractCustomer.Address,
                ContractCustomer.CooperationDate,
                ContractCustomer.DockingProject,
                ContractCustomer.DockingprojectLeader,
                ContractCustomer.IsSign,
                ContractCustomer.ID,
            }));
        }

        public bool DeleteInfo(string id)
        {
            const string sql =
@"DELETE FROM  ContractCustomer WHERE ID=@ID";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                id
            }));
        }

        public Model.ContractCustomer GetInfo(string id)
        {
            const string sql =
@"SELECT * FROM ContractCustomer WHERE ID=@ID";
            List<Model.ContractCustomer> ContractCustomer = Factory.DBHelper.Query<Model.ContractCustomer>(SQLConString, sql.ToString(), new DynamicParameters(new { id }));
            return ContractCustomer.Count() > 0 ? ContractCustomer[0]: null;
        }
    }
}
