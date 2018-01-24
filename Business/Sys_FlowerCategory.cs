using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    /// <summary>
    /// 花卉类别
    /// </summary>
    public  class Sys_FlowerCategory
    {
        private string SQLConString = System.Configuration.ConfigurationManager.AppSettings["SQLConString"];

        /// <summary>
        /// 获得花卉类别集合
        /// </summary>
        /// <returns></returns>
        public List<Model.FlowerCategory> GetListModel() 
        {
            const string sql =
@"SELECT  * FROM FlowerCategory";
            return Factory.DBHelper.Query<Model.FlowerCategory>(SQLConString, sql.ToString(), new DynamicParameters(new { }));
        }

        public Model.FlowerCategory Get(string id) 
        {
            const string sql =
@"SELECT  * FROM FlowerCategory  WHERE ID=@id";
               return Factory.DBHelper.Query<Model.FlowerCategory>(SQLConString, sql.ToString(), new DynamicParameters(new {id }))[0];
        }

        /// 分页获得数据信息
        /// </summary>
        /// <param name="limit">页码大小</param>
        /// <param name="offset">第几页</param>
        /// <param name="UserName">用户名</param>
        /// <param name="Role">角色ID</param>
        /// <returns></returns>
        public List<Model.FlowerCategory> FlowerCategoryList( int offset, string StrWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( SELECT ROW_NUMBER() over(order by FlowerCategory.id desc) as rn ,* FROM FlowerCategory");
            if (!string.IsNullOrEmpty(StrWhere))
            {
                strSql.Append(" where  1=1 " + StrWhere);
            }
            strSql.Append(")T where t.rn between   @offset and (@offset+9)");
            return Factory.DBHelper.Query<Model.FlowerCategory>(SQLConString, strSql.ToString(), new DynamicParameters(new { offset }));
        }

        /// <summary>
        /// 获得总记录数
        /// </summary>
        /// <param name="StrWhere"></param>
        /// <returns></returns>
        public int GetFlowerCategoryListCount(string StrWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(ID) as ID FROM FlowerCategory");
            if (!string.IsNullOrEmpty(StrWhere))
            {
                strSql.Append(" where  1=1 " + StrWhere);
            }
            List<Model.FlowerCategory> UserAdminList = Factory.DBHelper.Query<Model.FlowerCategory>(SQLConString, strSql.ToString(), new DynamicParameters(new { StrWhere }));
            return UserAdminList.Count() > 0 ? UserAdminList[0].id : 0;
        }

        public bool Check(string FlowerCategoryType)
        {
            const string sql =
@"SELECT * FROM FlowerCategory WHERE FlowerCategoryType=@FlowerCategoryType";
            List<Model.FlowerCategory> FlowerCategoryS = Factory.DBHelper.Query<Model.FlowerCategory>(SQLConString, sql.ToString(), new DynamicParameters(new { FlowerCategoryType }));
            return FlowerCategoryS.Count() > 0 ? true : false;
        }

        public bool Add(string FlowerCategoryType) 
        {
            const string sql =
@"INSERT INTO  FlowerCategory(FlowerCategoryType)  VALUES(@FlowerCategoryType)";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                FlowerCategoryType
            }));
        }

        public bool Edit(string FlowerCategoryType, int ID)
        {
            const string sql =
@"UPDATE  FlowerCategory  SET FlowerCategoryType=@FlowerCategoryType WHERE ID=@ID";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                FlowerCategoryType,
                ID
            }));
        }

        public bool Del(string ID) 
        {
            const string sql =
@"DELETE FROM  FlowerCategory WHERE ID=@ID";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                ID
            }));
        }
    }
}
