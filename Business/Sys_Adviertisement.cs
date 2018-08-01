using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
   public class Sys_FlowerActive
   {
       private string SQLConString = System.Configuration.ConfigurationManager.AppSettings["SQLConString"];

       /// <summary>
       /// 获得总记录数
       /// </summary>
       /// <param name="StrWhere"></param>
       /// <returns></returns>
       public int GetFlowerActiveListCount(string StrWhere)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append("SELECT COUNT(ID) as id FROM FlowerActive");
           if (!string.IsNullOrEmpty(StrWhere))
           {
               strSql.Append(" where  1=1 " + StrWhere);
           }
           List<Model.FlowerActive> FlowerList = Factory.DBHelper.Query<Model.FlowerActive>(SQLConString, strSql.ToString(), new DynamicParameters(new { StrWhere }));
           return FlowerList.Count() > 0 ? FlowerList[0].Id : 0;
       }
              
       /// <summary>
       /// 通过UserId和FlowerId查询信息
       /// </summary>
       /// <param name="id"></param>
       /// <returns>同一用户本花卉的记录</returns>
       public Model.FlowerActive GetFlowerActive(string FlowerId, string userid)
       {
           const string sql = @"SELECT * FROM  FlowerActive WHERE FlowerId=@FlowerId and UsersId=@UserID";
           List<Model.FlowerActive> FlowerList = Factory.DBHelper.Query<Model.FlowerActive>(SQLConString, sql.ToString(),
               new DynamicParameters(new
               {
                   FlowerId,
                   userid
               }));
           return FlowerList.Count() > 0 ? FlowerList[0] : null;
       }
       /// <summary>
       /// 通过Id查询信息
       /// </summary>
       /// <param name="id"></param>
       /// <returns>花卉的记录</returns>
       public Model.FlowerActive GetFlowerActiveById(int Id)
       {
           const string sql = @"SELECT * FROM  FlowerActive WHERE Id=@Id";
           List<Model.FlowerActive> FlowerList = Factory.DBHelper.Query<Model.FlowerActive>(SQLConString, sql.ToString(),
               new DynamicParameters(new
               {
                   Id
               }));
           return FlowerList.Count() > 0 ? FlowerList[0] : null;
       }
       /// <summary>
       /// 写入一条记录
       /// </summary>
       /// <param name="Flower"></param>
       /// <returns></returns>
       public bool InsertFlowerActive(Model.FlowerActive Flower)
       {
           const string sql = @"INSERT INTO FlowerActive(FlowerId,UsersId,Content,CreateTime,UpdateTime) 
            VALUES(@FlowerId,@UsersId,@Content,@CreateTime,@UpdateTime)";
           return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
           {
               Flower.FlowerId,               
               Flower.UsersId,
               Flower.Content,
               Flower.CreateTime,
               Flower.UpdateTime,
           }));
       }
       /// <summary>
       /// 修改一条记录
       /// </summary>
       /// <param name="Flower"></param>
       /// <returns></returns>
       public bool UpdateFlowerActive(Model.FlowerActive Flower)
       {
           const string sql = @"UPDATE  FlowerActive SET FlowerId=@FlowerId,UsersId=@UsersId,Content=@Content,
                UpdateTime=@UpdateTime  WHERE Id=@Id";
           return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
           {
               Flower.FlowerId,              
               Flower.UsersId,
               Flower.Content,               
               Flower.UpdateTime,
               Flower.Id,
           }));
       }

       /// <summary>
       /// 删除一个花卉
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
        public bool DeleteFlowerActive(int id)
        {
            const string sql =@"DELETE from FlowerActive WHERE Id=@id";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                id,
            }));
        }
        

        /// <summary>
        /// 获得数据信息
        /// </summary>
        /// <param name="limit">(@limit)必须加括号，否则报错</param>        
        /// <returns></returns>
        public List<Model.FlowerActive> FlowerActiveList(int limit, int offset, string StrWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT  top (@limit) * FROM (SELECT ROW_NUMBER() over(order by b.UpdateTime desc) as rn, b.Id, x.FlowerWatchName,");
            strSql.Append("b.FlowerId,b.Content,b.UpdateTime,b.CreateTime,b.UsersId FROM Flower x inner join FlowerActive b on x.id=b.FlowerId");
	       
            if (!string.IsNullOrEmpty(StrWhere))
            {
                strSql.Append(" where  1=1 " + StrWhere);
            }
            strSql.Append(") t where t.rn between   @offset and (@offset+@limit)");
            return Factory.DBHelper.Query<Model.FlowerActive>(SQLConString, strSql.ToString(), new DynamicParameters(new { limit, offset }));
        }

        public List<Model.FlowerActive> GetFlowerActiveList(int limit, int offset, string StrWhere)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT * FROM (SELECT ROW_NUMBER() over(order by UpdateTime desc) as rn,* FROM FlowerActive");
            if (!string.IsNullOrEmpty(StrWhere))
            {
                sql.Append(" where  1=1 " + StrWhere);
            }
            sql.Append(")t where t.rn between   @offset and (@offset+@limit)");
            return Factory.DBHelper.Query<Model.FlowerActive>(SQLConString, sql.ToString(), new DynamicParameters(new { limit,offset }));
        }
    }
}
