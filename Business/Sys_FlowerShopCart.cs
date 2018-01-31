using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    /// <summary>
    /// 购物车处理类
    /// </summary>
    public  class Sys_FlowerShopCart
    {
        private string SQLConString = System.Configuration.ConfigurationManager.AppSettings["SQLConString"];

        /// <summary>
        /// 获得总记录数
        /// </summary>
        /// <param name="StrWhere"></param>
        /// <returns></returns>
        public int GetFlowerShopCartListCount(string StrWhere) 
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(ID) as id FROM FlowerShopCart");
            if (!string.IsNullOrEmpty(StrWhere))
            {
                strSql.Append(" where  1=1 " + StrWhere);
            }
            List<Model.FlowerShopCart> FlowerList = Factory.DBHelper.Query<Model.FlowerShopCart>(SQLConString, strSql.ToString(), new DynamicParameters(new { StrWhere }));
            return FlowerList.Count() > 0 ? FlowerList[0].Id : 0;
        }

        /// <summary>
        /// 获得数据信息
        /// </summary>
        /// <param name="Userid">用户名</param>        
        /// <returns></returns>
        public List<Model.FlowerCartVM> FlowerShopCartList(string StrWhere)  
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT b.Id, x.FlowerWatchName,x.FlowerWatchPhoto,x.FlowerSalesPrice,x.FlowerIntroduction");
            strSql.Append(",b.FlowerId,b.Num,b.Status,b.UpdateTime FROM Flower x inner join FlowerShopCart b on x.id=b.FlowerId");
	       
            if (!string.IsNullOrEmpty(StrWhere))
            {
                strSql.Append(" where  1=1 " + StrWhere);
            }

            return Factory.DBHelper.Query<Model.FlowerCartVM>(SQLConString, strSql.ToString(), new DynamicParameters(new { }));
        }       
        /// <summary>
        /// 通过UserId和FlowerId查询信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns>同一用户本花卉的记录</returns>
        public Model.FlowerShopCart GetFlowerShopCart(string FlowerId,string userid) 
        {
            const string sql = @"SELECT * FROM  FlowerShopCart WHERE FlowerId=@FlowerId and UsersId=@UserID";
            List<Model.FlowerShopCart> FlowerList = Factory.DBHelper.Query<Model.FlowerShopCart>(SQLConString, sql.ToString(),
                new DynamicParameters(new { 
                    FlowerId ,
                    userid  
                }));
            return FlowerList.Count() > 0 ? FlowerList[0] : null;
        }
        /// <summary>
        /// 通过Id查询信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns>花卉的记录</returns>
        public Model.FlowerShopCart GetFlowerShopCartById(string Id)
        {
            const string sql = @"SELECT * FROM  FlowerShopCart WHERE Id=@Id";
            List<Model.FlowerShopCart> FlowerList = Factory.DBHelper.Query<Model.FlowerShopCart>(SQLConString, sql.ToString(),
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
        public bool InsertFlowerShopCart(Model.FlowerShopCart Flower) 
        {
            const string sql =@"INSERT INTO FlowerShopCart(FlowerId,Num,UsersId,Status,CreateTime,UpdateTime) 
            VALUES(@FlowerId,@Num,@UsersId,@Status,@CreateTime,@UpdateTime)";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                Flower.FlowerId,
                Flower.Num,
                Flower.UsersId,
                Flower.Status,
                Flower.CreateTime,
                Flower.UpdateTime,
            }));
        }
        /// <summary>
        /// 修改一条记录
        /// </summary>
        /// <param name="Flower"></param>
        /// <returns></returns>
        public bool UpdateFlowerShopCart(Model.FlowerShopCart Flower) 
        {
            const string sql = @"UPDATE  FlowerShopCart SET FlowerId=@FlowerId,Num=@Num,UsersId=@UsersId,Status=@Status,
                CreateTime=@CreateTime,UpdateTime=@UpdateTime  WHERE Id=@Id";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                Flower.FlowerId,
                Flower.Num,
                Flower.UsersId,
                Flower.Status,
                Flower.CreateTime,
                Flower.UpdateTime,
                Flower.Id,
            }));
        }

        /// <summary>
        /// 删除一个花卉
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteFlowerShopCart(string id)
        {
            const string sql =@"DELETE from FlowerShopCart WHERE Id=@id";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                id,
            }));
        }


        public List<Model.FlowerShopCart> GetFlowerList() 
        {
            const string sql =@"SELECT * FROM FlowerShopCart";
            return Factory.DBHelper.Query<Model.FlowerShopCart>(SQLConString, sql.ToString(), new DynamicParameters(new { }));
        }
    }
}
