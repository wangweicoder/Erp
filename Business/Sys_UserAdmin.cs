using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class Sys_UserAdmin
    {
        private string SQLConString = System.Configuration.ConfigurationManager.AppSettings["SQLConString"];
        
        #region 读
        /// <summary>
        /// 根据管理员ID 获得管理员信息
        /// </summary>
        /// <param name="UsersId"></param>
        /// <returns></returns>
        public Model.UserAdmin GetUserAdminByUserId(int UsersUserAdminId) 
        {
            const string sql =@"SELECT * FROM UserAdmin where id=@UsersUserAdminId";
            List<Model.UserAdmin> UserAdminList = Factory.DBHelper.Query<Model.UserAdmin>(SQLConString, sql.ToString(), new DynamicParameters(new { UsersUserAdminId }));
            return UserAdminList.Count > 0 ? UserAdminList[0] : null;
        }
        /// <summary>
        /// 通过OpendId获得对应信息
        /// </summary>
        /// <param name="OpendId"></param>
        /// <returns></returns>
        public Model.UserAdmin GetUserAdminByOpendId(string OpendId) 
        {
            const string sql = @"SELECT  * FROM UserAdmin where OpenId=@OpendId";
            List<Model.UserAdmin> UserAdminList = Factory.DBHelper.Query<Model.UserAdmin>(SQLConString, sql.ToString(), new DynamicParameters(new { OpendId }));
            return UserAdminList.Count > 0 ? UserAdminList[0] : null;
        }

        /// <summary>
        /// 后台登录
        /// </summary>
        /// <returns></returns>
        public Model.UserAdmin AdminLogin(Model.UserAdmin UserAdmin) 
        {
            const string sql =@"SELECT * FROM UserAdmin WHERE UserName=@UserName AND PassWord=@PassWord";
            List<Model.UserAdmin> UserAdminList = Factory.DBHelper.Query<Model.UserAdmin>(SQLConString, sql.ToString(), new DynamicParameters(new { UserAdmin.UserName, UserAdmin.PassWord }));
            return UserAdminList.Count > 0 ? UserAdminList[0] : null;
        }
        /// <summary>
        /// 验证账号信息是否存在
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public bool CheckUserAdminInfo(string UserName) 
        {
            const string sql =@"SELECT ID FROM UserAdmin WHERE  UserName=@UserName";
            List<Model.UserAdmin> UserAdminList = Factory.DBHelper.Query<Model.UserAdmin>(SQLConString, sql.ToString(), new DynamicParameters(new { UserName }));
            return UserAdminList.Count() > 0 ? true : false;
        }

        /// <summary>
        /// 查询指定角色的用户
        /// </summary>
        /// <param name="RoleCode"></param>
        /// <returns></returns>
        public List<Model.UserAdmin> GetUserAdminListByRoleCode(string RoleCode, int WorkUsersId) 
        {
             string sql =
@"SELECT * FROM UserAdmin WHERE  charindex( ','+'" + WorkUsersId + "'+',',','+cast(WorkUsersId as varchar)+',')>0 AND RoleCode=@RoleCode order by Weekly";
            return Factory.DBHelper.Query<Model.UserAdmin>(SQLConString, sql.ToString(), new DynamicParameters(new { RoleCode }));
        }
        /// <summary>
        /// 查询客户本身
        /// </summary>
        /// <param name="RoleCode"></param>
        /// <returns></returns>
        public List<Model.UserAdmin> GetUserAdminUsByRoleCode(string RoleCode, int WorkUsersId)
        {
            string sql =@"SELECT * FROM UserAdmin WHERE  RoleCode=@RoleCode AND id=@WorkUsersId order by Weekly";
            return Factory.DBHelper.Query<Model.UserAdmin>(SQLConString, sql.ToString(), new DynamicParameters(new { RoleCode, WorkUsersId }));
        }
        /// <summary>
        /// 查询客户列表
        /// </summary>
        public List<Model.UserAdmin> GetAdminInfoList(string RoleCode)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT * FROM UserAdmin WHERE  RoleCode=@RoleCode ");            
            sql.Append("order by Weekly");
            return Factory.DBHelper.Query<Model.UserAdmin>(SQLConString, sql.ToString(), new DynamicParameters(new { RoleCode }));
        }
        /// <summary>
        /// 查询客户列表
        /// </summary>
        /// <param name="RoleCode">角色编号</param>
        /// <param name="week">周次</param>
        /// <returns></returns> 
        public List<Model.UserAdmin> GetAdminInfoListbyweek(string RoleCode,int week)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT * FROM UserAdmin WHERE  RoleCode=@RoleCode ");
            if(week!=0){
                sql.Append("AND  Weekly=@week");
            }
            else
             sql.Append(" order by Weekly");
            return Factory.DBHelper.Query<Model.UserAdmin>(SQLConString, sql.ToString(), new DynamicParameters(new { RoleCode ,week}));
        }
        /// <summary>
        /// 查询业负责的Customer
        /// </summary>
        /// <param name="RoleCode"></param>
        /// <returns></returns>
        public List<Model.UserAdmin> GetUserAdminListByWorkUsersId(string RoleCode,int WorkUsersId,int week)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT * FROM UserAdmin WHERE charindex( ','+'" + WorkUsersId + "'+',',','+cast(WorkUsersId as varchar)+',')>0 AND  RoleCode=@RoleCode ");
            if (week != 0)
            {
                sql.Append("AND Weekly=@week ");
            }
            else
            sql.Append("order by Weekly");
            return Factory.DBHelper.Query<Model.UserAdmin>(SQLConString, sql.ToString(), new DynamicParameters(new { RoleCode,week }));
        }


        /// <summary>
        /// 查询排除角色以外的并且不是游客的用户
        /// </summary>
        /// <param name="RoleCode"></param>
        /// <returns></returns>
        public List<Model.UserAdmin> GetUserAdminListByRoleCodeNo(string RoleCode)
        {
            const string sql = @"SELECT * FROM UserAdmin WHERE  RoleCode!=@RoleCode and RoleCode!='Tourist'";
            return Factory.DBHelper.Query<Model.UserAdmin>(SQLConString, sql.ToString(), new DynamicParameters(new { RoleCode }));
        }

        /// <summary>
        /// 获得总记录数
        /// </summary>
        /// <param name="StrWhere"></param>
        /// <returns></returns>
        public int GetUserAdminListCount(string StrWhere) 
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(ID) as ID FROM UserAdmin");
            if (!string.IsNullOrEmpty(StrWhere))
            {
                strSql.Append(" where  1=1 " + StrWhere);
            }
            List<Model.UserAdmin> UserAdminList = Factory.DBHelper.Query<Model.UserAdmin>(SQLConString, strSql.ToString(), new DynamicParameters(new { StrWhere }));
            return UserAdminList.Count() > 0 ? UserAdminList[0].ID : 0;
        }

        /// <summary>
        /// 分页获得数据信息
        /// </summary>
        /// <param name="limit">页码大小</param>
        /// <param name="offset">第多少条</param>        
        /// <returns></returns>
        public List<Model.UserAdmin> UserAdminList(int limit, int offset, string StrWhere)  
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( SELECT ROW_NUMBER() over(order by UserAdmin.id desc) as rn ,");
            strSql.Append(" UserAdmin.IsEnable,UserAdmin.ID,UserAdmin.UserName,UserAdmin.RealName,");
            strSql.Append(" UserAdmin.CheckAddress,UserAdmin.RoleName,");
            strSql.Append(" UserAdmin.OwnedCompany,UserAdmin.WorkRealName,");
            strSql.Append(" UserAdmin.WorkUsersId,UserAdmin.LogoPhoto,UserAdmin.Weekly  FROM UserAdmin");
            if (!string.IsNullOrEmpty(StrWhere))
            {
                strSql.Append(" where  1=1 " + StrWhere);
            }
            strSql.Append(")T where t.rn between   @offset and (@offset+@limit-1)");
            return Factory.DBHelper.Query<Model.UserAdmin>(SQLConString, strSql.ToString(), new DynamicParameters(new { offset,limit
            }));
        }
        /// <summary>
        /// 获得数据
        /// </summary>       
        /// <returns></returns>
        public List<Model.UserAdmin> GetUserAdminList( string StrWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT  UserAdmin.IsEnable,UserAdmin.ID,UserAdmin.UserName,UserAdmin.RealName,");
            strSql.Append(" UserAdmin.CheckAddress,UserAdmin.RoleName,");
            strSql.Append(" UserAdmin.OwnedCompany,UserAdmin.WorkRealName,");
            strSql.Append(" UserAdmin.WorkUsersId,UserAdmin.LogoPhoto,UserAdmin.Weekly  FROM UserAdmin");
            if (!string.IsNullOrEmpty(StrWhere))
            {
                strSql.Append(" where  1=1 " + StrWhere);
            }          
            return Factory.DBHelper.Query<Model.UserAdmin>(SQLConString, strSql.ToString(),new DynamicParameters ());
        }

        #endregion

        #region 写
        /// <summary>
        /// 添加一个后台管理员
        /// </summary>
        /// <param name="UserAdmin"></param>
        /// <returns></returns>
        public bool InsertUserAdmin(Model.UserAdmin UserAdmin) 
        {
            const string sql =
@"INSERT INTO UserAdmin(UserName,PassWord,RealName,CheckAddress,OwnedCompany,RoleName,RoleCode,OpenId) VALUES(
@UserName,@PassWord,@RealName,@CheckAddress,@OwnedCompany,@RoleName,@RoleCode,@OpenId)";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                UserAdmin.UserName,
                UserAdmin.PassWord,
                UserAdmin.RealName,
                UserAdmin.CheckAddress,
                UserAdmin.OwnedCompany,
                UserAdmin.RoleName,
                UserAdmin.RoleCode,
                UserAdmin.OpenId,
            }));
        }
        /// <summary>
        /// 添加一个后台管理员
        /// </summary>
        /// <param name="UserAdmin"></param>
        /// <returns></returns>
        public int InsertUserAdminGetId(Model.UserAdmin UserAdmin)
        {
            const string sql =
@"INSERT INTO UserAdmin(UserName,PassWord,RealName,CheckAddress,OwnedCompany,RoleName,RoleCode,OpenId) VALUES(
@UserName,@PassWord,@RealName,@CheckAddress,@OwnedCompany,@RoleName,@RoleCode,@OpenId)";
            return Factory.DBHelper.ExecSqlGetId(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                UserAdmin.UserName,
                UserAdmin.PassWord,
                UserAdmin.RealName,
                UserAdmin.CheckAddress,
                UserAdmin.OwnedCompany,
                UserAdmin.RoleName,
                UserAdmin.RoleCode,
                UserAdmin.OpenId,
            }));
        }
        /// <summary>
        /// 修改后台管理员信息
        /// </summary>
        /// <param name="UserAdmin"></param>
        /// <returns></returns>
        public bool UpdateUserAdmin(Model.UserAdmin UserAdmin) 
        {
            const string sql =@"UPDATE UserAdmin SET PassWord=@PassWord,RealName=@RealName,CheckAddress=@CheckAddress,
OwnedCompany=@OwnedCompany,RoleName=@RoleName,RoleCode=@RoleCode WHERE ID=@ID";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                UserAdmin.PassWord,
                UserAdmin.RealName,
                UserAdmin.CheckAddress,
                UserAdmin.OwnedCompany,
                UserAdmin.RoleName,
                UserAdmin.RoleCode,
                UserAdmin.ID,
            }));
        }

        public bool UpdateOpenId(string OpenId,int UsersId)
        {
            const string sql =@"UPDATE UserAdmin SET OPENID=@OpenId  WHERE ID=@UsersId";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                OpenId,
                UsersId
            }));
        }
        /// <summary>
        /// 是否启用
        /// </summary>
        /// <param name="IsEnable"></param>
        /// <returns></returns>
        public bool UpdateUserAdminIsEnable(int IsEnable,int ID) 
        {
            const string sql =@"UPDATE UserAdmin SET IsEnable=@IsEnable WHERE ID=@ID";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                IsEnable,ID
            }));
        }

        /// <summary>
        /// 删除一掉记录
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool DeleteUserAdminInfo(int ID) 
        {
            const string sql =@"DELETE FROM UserAdmin WHERE ID=@ID";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                ID
            }));
        }

        public bool SetUserAdminRole(string id, string RoleCode, string RoleName) 
        {
            const string sql =@"UPDATE UserAdmin SET RoleCode=@RoleCode,RoleName=@RoleName WHERE ID=@id ";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                id,
                RoleCode,
                RoleName
            }));
        }


        public bool SetWorkName(string id,string WorkRealName,string WorkUsersId)
        {
            const string sql =@"UPDATE UserAdmin SET WorkUsersId=@WorkUsersId,WorkRealName=@WorkRealName WHERE ID=@id";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {

                WorkUsersId,
                WorkRealName,
                id,
            }));
        }
        #endregion


        public bool AddLogoPhoto(string id,string urlpath) 
        {
            const string sql =
@"UPDATE UserAdmin  SET LogoPhoto=@urlpath  WHERE ID=@id ";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {

                urlpath,
                id,
            }));
        }

        public bool SetWeekly(string id, string Weekly) 
        {
            const string sql =@"UPDATE UserAdmin  SET Weekly=@Weekly  WHERE ID=@id ";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                Weekly,
                id,
            }));
        }
    }
}
