using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    /// <summary>
    /// 登录日志
    /// </summary>
    public class Sys_LoginLog
    {
        private string SQLConString = System.Configuration.ConfigurationManager.AppSettings["SQLConString"];

        /// <summary>
        /// 写入登录日志
        /// </summary>
        /// <returns></returns>
        public bool InsertLoginLog(Model.LoginLog LoginLog) 
        {
            const string sql =
@"INSERT INTO LoginLog(UserName,RealName,LoginTime,IP) VALUES(@UserName,@RealName,@LoginTime,@IP)";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                LoginLog.UserName,LoginLog.RealName,LoginLog.LoginTime,LoginLog.IP
            }));
        }
    }
}
