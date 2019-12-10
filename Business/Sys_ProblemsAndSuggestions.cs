using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class Sys_ProblemsAndSuggestions
    {

        private string SQLConString = System.Configuration.ConfigurationManager.AppSettings["SQLConString"];

        public Model.ProblemsAndSuggestions GetModelById(string ID) 
        {
            const string sql =
@"SELECT * FROM ProblemsAndSuggestions WHERE ID=@ID";
            List<Model.ProblemsAndSuggestions> ProblemsAndSuggestionsList = Factory.DBHelper.Query<Model.ProblemsAndSuggestions>(SQLConString, sql.ToString(), new DynamicParameters(new { ID }));
            return ProblemsAndSuggestionsList.Count() > 0 ? ProblemsAndSuggestionsList[0] : null;
        }



        /// <summary>
        /// 写入一条记录
        /// </summary>
        /// <param name="ProblemsAndSuggestions"></param>
        /// <returns></returns>
        public bool InsertProblemsAndSuggestions(Model.ProblemsAndSuggestions ProblemsAndSuggestions)
        {
            const string sql=
@"INSERT INTO ProblemsAndSuggestions(UsersId,RealName,Problems,Suggestions,PhotoList,phone,Address,State) 
 VALUES(@UsersId,@RealName,@Problems,@Suggestions,@PhotoList,@phone,@Address,@State)";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                ProblemsAndSuggestions.UsersId,
                ProblemsAndSuggestions.RealName,
                ProblemsAndSuggestions.Problems,
                ProblemsAndSuggestions.Suggestions,
                ProblemsAndSuggestions.PhotoList,
                ProblemsAndSuggestions.phone,
                ProblemsAndSuggestions.Address,
                ProblemsAndSuggestions.State
            }));

        }

        public int GetProblemsAndSuggestionsListCount(string StrWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(ID) as id FROM ProblemsAndSuggestions");
            if (!string.IsNullOrEmpty(StrWhere))
            {
                strSql.Append(" where  1=1 " + StrWhere);
            }
            List<Model.ProblemsAndSuggestions> ProblemsAndSuggestionsList = Factory.DBHelper.Query<Model.ProblemsAndSuggestions>(SQLConString, strSql.ToString(), new DynamicParameters(new { StrWhere }));
            return ProblemsAndSuggestionsList.Count() > 0 ? ProblemsAndSuggestionsList[0].id : 0;
        }

        public List<Model.ProblemsAndSuggestions> UserProblemsAndSuggestionsList(int limit, int offset, string StrWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from (select ROW_NUMBER() over (order by ProblemsAndSuggestions.id desc) as rn ,ProblemsAndSuggestions.UsersId,ProblemsAndSuggestions.id,UserAdmin.UserName,UserAdmin.RealName,ProblemsAndSuggestions.phone,ProblemsAndSuggestions.Address,ProblemsAndSuggestions.CreateTime,ProblemsAndSuggestions.PhotoList,ProblemsAndSuggestions.Problems,ProblemsAndSuggestions.Suggestions,ProblemsAndSuggestions.State from ProblemsAndSuggestions inner join UserAdmin  on ProblemsAndSuggestions.UsersId=UserAdmin.ID ");
            if (!string.IsNullOrEmpty(StrWhere))
            {
                strSql.Append(" where  1=1 " + StrWhere);
            }
            strSql.Append(")T where t.rn between   @offset and (@offset+9)");
            return Factory.DBHelper.Query<Model.ProblemsAndSuggestions>(SQLConString, strSql.ToString(), new DynamicParameters(new { offset }));
        }


        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="state"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool UpdateState(string state,string id)
        {
            const string sql =
@"UPDATE  ProblemsAndSuggestions SET STATE=@state  WHERE ID=@id";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                state,id
            }));
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(string id)
        {
            const string sql = @"DELETE from ProblemsAndSuggestions WHERE id=@id";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                id,
            }));
        }
    }
}
