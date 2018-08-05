using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class Sys_Adviertisement
   {
       private string SQLConString = System.Configuration.ConfigurationManager.AppSettings["SQLConString"];

       /// <summary>
       /// 获得总记录数
       /// </summary>
       /// <param name="StrWhere"></param>
       /// <returns></returns>
       public int GetAdviertisementListCount(string StrWhere)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append("SELECT COUNT(ID) as id FROM Adviertisement");
           if (!string.IsNullOrEmpty(StrWhere))
           {
               strSql.Append(" where  1=1 " + StrWhere);
           }
           List<Model.Adviertisement> FlowerList = Factory.DBHelper.Query<Model.Adviertisement>(SQLConString, strSql.ToString(), new DynamicParameters(new { StrWhere }));
           return FlowerList.Count() > 0 ? FlowerList[0].ID : 0;
       }

       /// <summary>
       /// 通过Id查询信息
       /// </summary>
       /// <param name="id"></param>
       /// <returns>Adviertisement记录</returns>
       public Model.Adviertisement GetModel(string AdvId)
       {
           const string sql = @"SELECT * FROM  Adviertisement WHERE Id=@AdvId";
           List<Model.Adviertisement> FlowerList = Factory.DBHelper.Query<Model.Adviertisement>(SQLConString, sql.ToString(),
               new DynamicParameters(new
               {
                   AdvId
               }));
           return FlowerList.Count() > 0 ? FlowerList[0] : null;
       }
       
       /// <summary>
       /// 写入一条记录
       /// </summary>
       /// <param name="Adviertisement"></param>
       /// <returns></returns>
       public bool InsertAdviertisement(Model.Adviertisement adv)
       {
           const string sql = @"INSERT INTO Adviertisement(Title,UsersId,Content,CreateTime,UpdateTime,Picture,Pictures) 
            VALUES(@Title,@UsersId,@Content,@CreateTime,@UpdateTime,@Picture,@Pictures)";
           return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
           {
               adv.Title,
               adv.UsersId,
               adv.Content,
               adv.CreateTime,
               adv.UpdateTime,
               adv.Picture,
               adv.Pictures
           }));
       }
       /// <summary>
       /// 修改一条记录
       /// </summary>
       /// <param name="Flower"></param>
       /// <returns></returns>
       public bool UpdateAdviertisemente(Model.Adviertisement adv)
       {
           const string sql = @"UPDATE  Adviertisement SET Title=@Title,UsersId=@UsersId,Content=@Content,
                UpdateTime=@UpdateTime,Picture=@Picture,Pictures=@Pictures  WHERE Id=@Id";
           return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
           {
               adv.Title,
               adv.UsersId,
               adv.Content,             
               adv.UpdateTime,
               adv.Picture,
               adv.Pictures,
               adv.ID,
           }));
       }

       /// <summary>
       /// 删除一个广告
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
       public bool DeleteAdviertisement(int id)
        {
            const string sql = @"DELETE from Adviertisement WHERE Id=@id";
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
         public List<Model.Adviertisement> GetAdviertisementList(int limit, int offset, string StrWhere)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT * FROM (SELECT ROW_NUMBER() over(order by UpdateTime desc) as rn,* FROM Adviertisement");
            if (!string.IsNullOrEmpty(StrWhere))
            {
                sql.Append(" where  1=1 " + StrWhere);
            }
            sql.Append(")t where t.rn between   @offset and (@offset+@limit)");
            return Factory.DBHelper.Query<Model.Adviertisement>(SQLConString, sql.ToString(), new DynamicParameters(new { limit, offset }));
        }
    }
}
