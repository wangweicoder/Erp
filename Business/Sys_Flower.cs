using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    /// <summary>
    /// 花卉业务处理类
    /// </summary>
    public  class Sys_Flower
    {
        private string SQLConString = System.Configuration.ConfigurationManager.AppSettings["SQLConString"];

           /// <summary>
        /// 获得总记录数
        /// </summary>
        /// <param name="StrWhere"></param>
        /// <returns></returns>
        public int GetFlowerListCount(string StrWhere) 
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(ID) as id FROM Flower");
            if (!string.IsNullOrEmpty(StrWhere))
            {
                strSql.Append(" where  1=1 " + StrWhere);
            }
            List<Model.Flower> FlowerList = Factory.DBHelper.Query<Model.Flower>(SQLConString, strSql.ToString(), new DynamicParameters(new { StrWhere }));
            return FlowerList.Count() > 0 ? FlowerList[0].id : 0;
        }

        /// <summary>
        /// 分页获得数据信息
        /// </summary>
        /// <param name="limit">页码大小</param>
        /// <param name="offset">第几页</param>
        /// <param name="UserName">用户名</param>
        /// <param name="Role">角色ID</param>
        /// <returns></returns>
        public List<Model.Flower> GetFlowerList ( int offset, string StrWhere)  
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( SELECT ROW_NUMBER() over(order by Flower.id desc) as rn ,* FROM Flower");
            if (!string.IsNullOrEmpty(StrWhere))
            {
                strSql.Append(" where  1=1 " + StrWhere);
            }
            strSql.Append(")T where t.rn between   @offset and (@offset+9)");
            return Factory.DBHelper.Query<Model.Flower>(SQLConString, strSql.ToString(), new DynamicParameters(new { offset }));
        }
        /// <summary>
        /// 验证花卉名称是否存在
        /// </summary>
        /// <param name="FlowerWatchName"></param>
        /// <returns></returns>
        public bool CheckFlowerWatchName(string FlowerWatchName) 
        {
            const string sql =
@"SELECT * FROM Flower WHERE FlowerWatchName=@FlowerWatchName";
            List<Model.Flower> FlowerList = Factory.DBHelper.Query<Model.Flower>(SQLConString, sql.ToString(), new DynamicParameters(new { FlowerWatchName }));
            return FlowerList.Count() > 0 ? true : false;
        }
        /// <summary>
        /// 验证花卉编号是否存在
        /// </summary>
        /// <param name="FlowerWatchName"></param>
        /// <returns></returns>
        public bool CheckFlowerNumber(string FlowerNumber)
        {
            const string sql =
@"SELECT * FROM Flower WHERE FlowerNumber=@FlowerNumber";
            List<Model.Flower> FlowerList = Factory.DBHelper.Query<Model.Flower>(SQLConString, sql.ToString(), new DynamicParameters(new { FlowerNumber }));
            return FlowerList.Count() > 0 ? true : false;
        }
        /// <summary>
        /// 通过ID 查询花卉信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Model.Flower GetFlower(string id) 
        {
            const string sql ="SELECT * FROM  Flower WHERE id=@id";
            List<Model.Flower> FlowerList = Factory.DBHelper.Query<Model.Flower>(SQLConString, sql.ToString(), new DynamicParameters(new { id }));
            return FlowerList.Count() > 0 ? FlowerList[0] : null;
        }
       
        /// <summary>
        /// 通过花卉(商品)编号获得花卉(商品信息）
        /// </summary>
        /// <param name="FlowerNumber"></param>
        /// <returns></returns>
        public Model.Flower GetFlowerByFlowerNumber(string FlowerNumber)
        {
            const string sql ="SELECT * FROM  Flower WHERE FlowerNumber=@FlowerNumber";
            List<Model.Flower> FlowerList = Factory.DBHelper.Query<Model.Flower>(SQLConString, sql.ToString(), new DynamicParameters(new { FlowerNumber }));
            return FlowerList.Count() > 0 ? FlowerList[0] : null;
        }

        /// <summary>
        /// 写入一条记录
        /// </summary>
        /// <param name="Flower"></param>
        /// <returns></returns>
        public bool InsertFlowerWatch(Model.Flower Flower) 
        {
            const string sql =
@"INSERT INTO Flower(FlowerWatchName,FlowerWatchPhoto,FlowerCostPrice,FlowerSalesPrice,FlowerStock,FlowerIntroduction,FlowerWatchType,XiXin,YangHuFangFa,FlowerNumber) 
 VALUES(@FlowerWatchName,@FlowerWatchPhoto,@FlowerCostPrice,@FlowerSalesPrice,@FlowerStock,@FlowerIntroduction,@FlowerWatchType,@XiXin,@YangHuFangFa,@FlowerNumber)";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                Flower.FlowerWatchName,
                Flower.FlowerWatchPhoto,
                Flower.FlowerCostPrice,
                Flower.FlowerSalesPrice,
                Flower.FlowerStock,
                Flower.FlowerIntroduction,
                Flower.FlowerWatchType,
                Flower.XiXin,
                Flower.YangHuFangFa,
                Flower.FlowerNumber,
            }));
        }
        /// <summary>
        /// 修改一条记录
        /// </summary>
        /// <param name="Flower"></param>
        /// <returns></returns>
        public bool UpdateFlowerWatch(Model.Flower Flower) 
        {
            const string sql =
@"UPDATE  Flower SET FlowerWatchName=@FlowerWatchName,FlowerWatchPhoto=@FlowerWatchPhoto,FlowerCostPrice=@FlowerCostPrice,FlowerSalesPrice=@FlowerSalesPrice,
FlowerStock=@FlowerStock,FlowerIntroduction=@FlowerIntroduction,FlowerWatchType=@FlowerWatchType,XiXin=@XiXin,YangHuFangFa=@YangHuFangFa  WHERE id=@id";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                Flower.FlowerWatchName,
                Flower.FlowerWatchPhoto,
                Flower.FlowerCostPrice,
                Flower.FlowerSalesPrice,
                Flower.FlowerStock,
                Flower.FlowerIntroduction,
                Flower.FlowerWatchType,
                Flower.XiXin,
                Flower.YangHuFangFa,
                Flower.id,
            }));
        }

        /// <summary>
        /// 删除一个花卉
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteFlowerWatch(string id)
        {
            const string sql =
@"DELETE from Flower WHERE id=@id";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                id,
            }));
        }


        public List<Model.Flower> GetFlowerList() 
        {
            const string sql =@"SELECT * FROM Flower";
            return Factory.DBHelper.Query<Model.Flower>(SQLConString, sql.ToString(), new DynamicParameters(new {  }));
        }
    }
}
