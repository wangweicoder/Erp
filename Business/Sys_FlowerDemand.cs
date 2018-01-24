using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    /// <summary>
    /// 最终花卉处理业务类
    /// </summary>
    public class Sys_FlowerDemand
    {
        private string SQLConString = System.Configuration.ConfigurationManager.AppSettings["SQLConString"];
        /// <summary>
        /// 写入一条需求记录
        /// </summary>
        /// <param name="FlowerDemand"></param>
        /// <returns></returns>
        public bool InsertFlowerDemand(Model.FlowerDemand FlowerDemand)
        {
            const string sql =
@"INSERT INTO FlowerDemand(FlowerCategoryType,ContactsName,ContactsPhone,ContactsAddress,ContentMsg,State) 
 VALUES(@FlowerCategoryType,@ContactsName,@ContactsPhone,@ContactsAddress,@ContentMsg,@State)";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                FlowerDemand.FlowerCategoryType,
                FlowerDemand.ContactsName,
                FlowerDemand.ContactsPhone,
                FlowerDemand.ContactsAddress,
                FlowerDemand.ContentMsg,
                FlowerDemand.State
            }));
        }

        /// <summary>
        /// 分页获得数据信息
        /// </summary>
        /// <param name="limit">页码大小</param>
        /// <param name="offset">第几页</param>
        /// <param name="UserName">用户名</param>
        /// <param name="Role">角色ID</param>
        /// <returns></returns>
        public List<Model.FlowerDemand> GetList( int offset, string StrWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( SELECT ROW_NUMBER() over(order by FlowerDemand.id desc) as rn ,* FROM FlowerDemand");
            if (!string.IsNullOrEmpty(StrWhere))
            {
                strSql.Append(" where  1=1 " + StrWhere);
            }
            strSql.Append(")T where t.rn between   @offset and (@offset+9)");
            return Factory.DBHelper.Query<Model.FlowerDemand>(SQLConString, strSql.ToString(), new DynamicParameters(new { offset }));
        }
        /// <summary>
        /// 获得总记录数
        /// </summary>
        /// <param name="StrWhere"></param>
        /// <returns></returns>
        public int GetFlowerDemandListCount(string StrWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(ID) as id FROM FlowerDemand");
            if (!string.IsNullOrEmpty(StrWhere))
            {
                strSql.Append(" where  1=1 " + StrWhere);
            }
            List<Model.FlowerDemand> FlowerDemand = Factory.DBHelper.Query<Model.FlowerDemand>(SQLConString, strSql.ToString(), new DynamicParameters(new { StrWhere }));
            return FlowerDemand.Count() > 0 ? FlowerDemand[0].id : 0;
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="state"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool EditState(string state ,string id) 
        {
            const string sql =
"update FlowerDemand  set state=@state where id=@id";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                state,id
            }));
        }
    }
}
