using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class Sys_UsersLoginLog
    {
        private string SQLConString = System.Configuration.ConfigurationManager.AppSettings["SQLConString"];
        
        #region 读
        /// <summary>
        /// 根据usersID 获得某个用户的登录信息
        /// </summary>
        /// <param name="UsersId"></param>
        /// <returns></returns>
        public List<Model.UsersLoginLog> GetUsersIdList(int UsersId) 
        {
            const string sql = @"SELECT * FROM UsersLoginLog where UsersId=@UsersId";
            List<Model.UsersLoginLog> UsersList = Factory.DBHelper.Query<Model.UsersLoginLog>(SQLConString, sql.ToString(), new DynamicParameters(new { UsersId }));
            return UsersList;
        }
        /// <summary>
        /// 根据 Month 获得某个用户的登录信息
        /// </summary>
        /// <param name="Month"></param>
        /// <returns></returns>
        public List<Model.UsersLoginLog> GetUsersListByMonth(string Month)
        {
            const string sql = @"SELECT * FROM UsersLoginLog where Month=@Month";
            List<Model.UsersLoginLog> UsersList = Factory.DBHelper.Query<Model.UsersLoginLog>(SQLConString, sql.ToString(), new DynamicParameters(new { Month }));
            return UsersList;
        }
        /// <summary>
        /// 根据 Day 获得某个用户的登录信息
        /// </summary>
        /// <param name="Day"></param>
        /// <returns></returns>
        public List<Model.UsersLoginLog> GetUsersListByDay(string Day)
        {
            const string sql = @"SELECT * FROM UsersLoginLog where Day=@Day";
            List<Model.UsersLoginLog> UsersList = Factory.DBHelper.Query<Model.UsersLoginLog>(SQLConString, sql.ToString(), new DynamicParameters(new { Day }));
            return UsersList;
        }
        /// <summary>
        /// 获得总记录数
        /// </summary>
        /// <param name="StrWhere"></param>
        /// <returns></returns>
        public int GetUsersLoginLogCount(string StrWhere) 
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(ID) as ID FROM UsersLoginLog");
            if (!string.IsNullOrEmpty(StrWhere))
            {
                strSql.Append(" where  1=1 " + StrWhere);
            }
            List<Model.UsersLoginLog> UsersLoginLogList = Factory.DBHelper.Query<Model.UsersLoginLog>(SQLConString, strSql.ToString(), new DynamicParameters(new { StrWhere }));
            return UsersLoginLogList.Count() > 0 ? UsersLoginLogList[0].ID : 0;
        }       
        /// <summary>
        /// 分页获得数据信息
        /// </summary>
        /// <param name="limit">页码大小</param>
        /// <param name="offset">第几页</param>
        /// <param name="UserName">用户名</param>
        /// <param name="Role">角色ID</param>
        /// <returns></returns>
        public List<Model.UsersLoginLog> UserAdminList(int limit, int offset, string StrWhere)  
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( SELECT ROW_NUMBER() over(order by UsersLoginLog.id desc) as rn ,");               
            strSql.Append(" UserAdmin.UserName,UserAdmin.RealName  FROM UserAdmin");
            if (!string.IsNullOrEmpty(StrWhere))
            {
                strSql.Append(" where  1=1 " + StrWhere);
            }
            strSql.Append(")T where t.rn between   @offset and (@offset+9)");
            return Factory.DBHelper.Query<Model.UsersLoginLog>(SQLConString, strSql.ToString(), new DynamicParameters(new { offset }));
        }
        #endregion

        #region 写
        /// <summary>
        /// 添加一个后台管理员
        /// </summary>
        /// <param name="UserAdmin"></param>
        /// <returns></returns>
        public bool InsertUsersLoginLog(Model.UsersLoginLog UserAdmin) 
        {
            const string sql =
@"INSERT INTO UsersLoginLog(UsersId,Year,Month,PhoneNum,Day,LoginTime,Content) VALUES(
@UsersId,@Year,@Month,@PhoneNum,@Day,@LoginTime,@Content)";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                UserAdmin.UsersId,
                UserAdmin.Year,
                UserAdmin.Month,
                UserAdmin.PhoneNum,
                UserAdmin.Day,
                UserAdmin.LoginTime,
                UserAdmin.Content,               
            }));
        }       
        /// <summary>
        /// 修改后台管理员信息
        /// </summary>
        /// <param name="UserAdmin"></param>
        /// <returns></returns>
        public bool UpdateLoginLog(Model.UsersLoginLog UserAdmin) 
        {
            const string sql = @"UPDATE UsersLoginLog SET UsersId=@UsersId,Year=@Year,Month=@Month,
PhoneNum=@PhoneNum,RoleName=@RoleName,Day=@Day,LoginTime=@LoginTime,Content=@Content WHERE ID=@ID";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                UserAdmin.UsersId,
                UserAdmin.Year,
                UserAdmin.Month,
                UserAdmin.PhoneNum,
                UserAdmin.Day,
                UserAdmin.LoginTime,
                UserAdmin.Content,
                UserAdmin.ID,
            }));
        }     
      

        /// <summary>
        /// 删除一掉记录
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool DeleteUsersLoginLogInfo(int ID) 
        {
            const string sql = @"DELETE FROM UsersLoginLog WHERE ID=@ID";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                ID
            }));
        }
        
        #endregion


    }
}
