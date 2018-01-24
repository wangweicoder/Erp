using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    /// <summary>
    /// 权限表
    /// </summary>
    public class Sys_AdminJurisdiction
    {
        private string SQLConString = System.Configuration.ConfigurationManager.AppSettings["SQLConString"];
   
        #region 读
        /// <summary>
        /// 分页获得权限表信息
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="StrWhere"></param>
        /// <returns></returns>
        public List<Model.AdminJurisdiction> GetAdminJurisdictionList(int limit, int offset, string StrWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( SELECT ROW_NUMBER() over(order by id desc) as rn ,* FROM AdminJurisdiction");
            if (!string.IsNullOrEmpty(StrWhere))
            {
                strSql.Append(" where  1=1 " + StrWhere);
            }
            strSql.Append(")T where t.rn between  (@offset-1)* @limit+1 and @offset*@limit");
            return Factory.DBHelper.Query<Model.AdminJurisdiction>(SQLConString, strSql.ToString(), new DynamicParameters(new { offset, limit }));
        }

        /// <summary>
        /// 获得总记录数
        /// </summary>
        /// <param name="StrWhere"></param>
        /// <returns></returns>
        public int GetAdminJurisdictionListCount(string StrWhere) 
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(ID) as ID FROM AdminJurisdiction");
            if (!string.IsNullOrEmpty(StrWhere))
            {
                strSql.Append(" where  1=1 " + StrWhere);
            }
            List<Model.AdminJurisdiction> AdminJurisdiction = Factory.DBHelper.Query<Model.AdminJurisdiction>(SQLConString, strSql.ToString(), new DynamicParameters(new { StrWhere }));
            return AdminJurisdiction.Count() > 0 ? AdminJurisdiction[0].ID : 0;
        }

        /// <summary>
        /// 根据菜单代码获得对应权限集合
        /// </summary>
        /// <param name="RoleCode"></param>
        /// <returns></returns>
        public List<Model.AdminJurisdiction> GetAdminJurisdictionListByRoleCode(string RoleCode)
        {
            const string sql =
@"SELECT * FROM AdminJurisdiction  WHERE RoleCode=@RoleCode";
            return Factory.DBHelper.Query<Model.AdminJurisdiction>(SQLConString, sql, new DynamicParameters(new { RoleCode }));
        }

        #endregion


        #region 写

        /// <summary>
        /// 执行设置菜单权限
        /// </summary>
        /// <param name="RoleCode"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public int UpdateAdminJurisdiction(IList<Model.AdminJurisdiction> list) 
        {
            const string sql =
@"UPDATE AdminJurisdiction SET MenuID=@MenuID  WHERE RoleCode=@RoleCode AND MenuID=@MenuID ";
            return Factory.DBHelper.ExecuteSQLBatch(SQLConString, sql.ToString(), list);
        }

        #endregion
    }
}
