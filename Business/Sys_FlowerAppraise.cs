using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class Sys_FlowerAppraise
    {
       private string SQLConString = System.Configuration.ConfigurationManager.AppSettings["SQLConString"];

       /// <summary>
       /// 获得总记录数
       /// </summary>
       /// <param name="StrWhere"></param>
       /// <returns></returns>
       public List<Model.FlowerAppraise> GetFlowerAppraiseCount(string StrWhere)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append("SELECT * FROM FlowerAppraise");
           if (!string.IsNullOrEmpty(StrWhere))
           {
               strSql.Append(" where  1=1 " + StrWhere);
           }
           List<Model.FlowerAppraise> FlowerList = Factory.DBHelper.Query<Model.FlowerAppraise>(SQLConString, strSql.ToString(), new DynamicParameters(new { StrWhere }));
           return FlowerList;
       }

       /// <summary>
       /// 通过Id查询信息
       /// </summary>
       /// <param name="id"></param>
       /// <returns>Adviertisement记录</returns>
       public Model.Adviertisement GetModel(string AdvId)
       {
           const string sql = @"SELECT * FROM  FlowerAppraise WHERE Id=@AdvId";
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
        /// <param name="FlowerAppraise"></param>
        /// <returns></returns>
        public bool InsertFlowerAppraise(Model.FlowerAppraise adv)
       {
           const string sql = @"INSERT INTO FlowerAppraise(ArrangementId,UsersId,Content,CreateTime,UpdateTime,IsGood,Pictures) 
            VALUES(@ArrangementId,@UsersId,@Content,@CreateTime,@UpdateTime,@IsGood,@Pictures)";
           return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
           {
               adv.ArrangementId,
               adv.UsersId,
               adv.Content,
               adv.CreateTime,
               adv.UpdateTime,
               adv.IsGood,
               adv.Pictures
           }));
       }
       /// <summary>
       /// 修改一条记录
       /// </summary>
       /// <param name="Flower"></param>
       /// <returns></returns>
       public bool UpdateFlowerAppraise(Model.FlowerAppraise adv)
       {
           const string sql = @"UPDATE  Adviertisement SET Title=@Title,UsersId=@UsersId,Content=@Content,
                UpdateTime=@UpdateTime,Picture=@Picture,Pictures=@Pictures  WHERE Id=@Id";
           return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
           {
               adv.ArrangementId,
               adv.UsersId,
               adv.Content,             
               adv.UpdateTime,              
               adv.Pictures,
               adv.ID,
           }));
       }

       /// <summary>
       /// 删除
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
       public bool DeleteFlowerAppraise(int id)
        {
            const string sql = @"DELETE from FlowerAppraise WHERE Id=@id";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                id,
            }));
        }        

        
    }
}
