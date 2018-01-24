using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public  class Sys_Role
    {
        private string SQLConString = System.Configuration.ConfigurationManager.AppSettings["SQLConString"];
        #region 读
        /// <summary>
        /// 获得全部的角色列表
        /// </summary>
        /// <returns></returns>
        public List<Model.RoleInfo> UserRoleList()
        {
            const string sql =
@"SELECT * FROM RoleInfo order by id desc";
            return Factory.DBHelper.Query<Model.RoleInfo>(SQLConString, sql, new DynamicParameters(new { }));
        }


        /// 分页获得数据信息
        /// </summary>
        /// <param name="limit">页码大小</param>
        /// <param name="offset">第几页</param>
        /// <param name="UserName">用户名</param>
        /// <param name="Role">角色ID</param>
        /// <returns></returns>
        public List<Model.RoleInfo> UserRoleList(int limit, int offset, string StrWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( SELECT ROW_NUMBER() over(order by id desc) as rn ,* FROM RoleInfo");
            if (!string.IsNullOrEmpty(StrWhere))
            {
                strSql.Append(" where  1=1 " + StrWhere);
            }
            strSql.Append(")T where t.rn between  (@offset-1)* @limit+1 and @offset*@limit");
            return Factory.DBHelper.Query<Model.RoleInfo>(SQLConString, strSql.ToString(), new DynamicParameters(new { offset, limit }));
        }
        /// <summary>
        /// 获得总记录数
        /// </summary>
        /// <param name="StrWhere"></param>
        /// <returns></returns>
        public int GetRoleLisCount(string StrWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(ID) as ID FROM RoleInfo");
            if (!string.IsNullOrEmpty(StrWhere))
            {
                strSql.Append(" where  1=1 " + StrWhere);
            }
            List<Model.RoleInfo> Role = Factory.DBHelper.Query<Model.RoleInfo>(SQLConString, strSql.ToString(), new DynamicParameters(new { StrWhere }));
            return Role.Count() > 0 ? Role[0].ID : 0;
        }

        public Model.RoleInfo GetRoleInfoByRoleID(string id)
        {
            const string sql =
@"SELECT * FROM  ROLEINFO   where id=@id";
            List<Model.RoleInfo> Role = Factory.DBHelper.Query<Model.RoleInfo>(SQLConString, sql.ToString(), new DynamicParameters(new { id }));
            return Role.Count() > 0 ? Role[0] : null;
        }

        public bool CheckRoleInfo(string RoleCode, string RoleName)
        {
            const string sql =
@"SELECT * FROM  ROLEINFO   where RoleCode=@RoleCode or RoleName=@RoleName ";
            List<Model.RoleInfo> Role = Factory.DBHelper.Query<Model.RoleInfo>(SQLConString, sql.ToString(), new DynamicParameters(new { RoleCode, RoleName }));
            return Role.Count() > 0 ? true : false;
        }

        public Model.RoleInfo GetRoleInfoByRoleCode(string RoleCode) 
        {
            const string sql =
@"SELECT * FROM  ROLEINFO   where RoleCode=@RoleCode";
            List<Model.RoleInfo> Role = Factory.DBHelper.Query<Model.RoleInfo>(SQLConString, sql.ToString(), new DynamicParameters(new { RoleCode }));
            return Role.Count() > 0 ? Role[0] : null;
        }
        #endregion

        #region 写
        /// <summary>
        /// 添加一个角色
        /// </summary>
        /// <param name="RoleInfo"></param>
        /// <returns></returns>
        public bool InsertRole(Model.RoleInfo RoleInfo)
        {
            const string sql =
@"INSERT INTO RoleInfo(RoleCode,RoleName,MenuIdList) VALUES(@RoleCode,@RoleName,0)";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                RoleInfo.RoleCode,
                RoleInfo.RoleName
            }));
        }

        /// <summary>
        /// 修改菜单集合
        /// </summary>
        /// <param name="RoleId"></param>
        /// <param name="MenuIdList"></param>
        /// <returns></returns>
        public bool UpdateRoleMenuIdList(string RoleId, string MenuIdList) 
        {
            const string sql =
@"UPDATE ROLEINFO SET MenuIdList=@MenuIdList WHERE id=@RoleId";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
               RoleId,
               MenuIdList
            }));
        }
        #endregion
    }
}
