using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public  class Sys_ServiceIntroduction
    {
        private string SQLConString = System.Configuration.ConfigurationManager.AppSettings["SQLConString"];

        public Model.ServiceIntroduction GetModel() 
        {
            const string sql =
@"SELECT * FROM  ServiceIntroduction";
            List<Model.ServiceIntroduction> ServiceIntroduction = Factory.DBHelper.Query<Model.ServiceIntroduction>(SQLConString, sql.ToString(), new DynamicParameters(new { }));
            return ServiceIntroduction.Count > 0 ? ServiceIntroduction[0] : null;
        }

        public bool InsertModel(string msg) 
        {
            const string sql =
@"INSERT INTO ServiceIntroduction(MSG)  VALUES(@msg)";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                msg
            }));
        }

        public bool UpdateModel(string  msg) 
        {
            const string sql =
@"UPDATE ServiceIntroduction  SET msg=@msg where id=1";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                msg
            }));
        }
    }
}
