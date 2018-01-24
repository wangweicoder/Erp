using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class Sys_ClockAttendance
    {
        private string SQLConString = System.Configuration.ConfigurationManager.AppSettings["SQLConString"];

        /// <summary>
        /// 写入一条考勤记录
        /// </summary>
        /// <param name="ClockAttendance"></param>
        /// <returns></returns>
        public bool InsertClockAttendance(Model.ClockAttendance ClockAttendance) 
        {
            const string sql =
@"INSERT INTO ClockAttendance(UsersId,RealName,Createtime,Address,CheckAddress) VALUES(@UsersId,@RealName,GETDATE(),@Address,@CheckAddress)";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                ClockAttendance.UsersId,
                ClockAttendance.RealName,
                ClockAttendance.Address,
                ClockAttendance.CheckAddress,
            }));
        }

        public object GetClockAttendanceListCount(string StrWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(ID) as id FROM ClockAttendance");
            if (!string.IsNullOrEmpty(StrWhere))
            {
                strSql.Append(" where  1=1 " + StrWhere);
            }
            List<Model.ClockAttendance> ClockAttendanceList = Factory.DBHelper.Query<Model.ClockAttendance>(SQLConString, strSql.ToString(), new DynamicParameters(new { StrWhere }));
            return ClockAttendanceList.Count() > 0 ? ClockAttendanceList[0].id : 0;
        }

        public object UserClockAttendanceList( int offset, string StrWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from (select ROW_NUMBER() over (order by ClockAttendance.id  desc) as rn ,ClockAttendance.id,UserAdmin.UserName,UserAdmin.RealName,ClockAttendance.Address,ClockAttendance.CheckAddress,ClockAttendance.Createtime from  ClockAttendance inner join UserAdmin  on ClockAttendance.UsersId=UserAdmin.ID ");
            if (!string.IsNullOrEmpty(StrWhere))
            {
                strSql.Append(" where  1=1 " + StrWhere);
            }
            strSql.Append(")T where t.rn between  @offset and (@offset+9)");
            return Factory.DBHelper.Query<Model.ProblemsAndSuggestions>(SQLConString, strSql.ToString(), new DynamicParameters(new { offset }));
        }
    }
}
