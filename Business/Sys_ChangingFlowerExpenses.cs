using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class Sys_ChangingFlowerExpenses
    {
        private string SQLConString = System.Configuration.ConfigurationManager.AppSettings["SQLConString"];


        /// <summary>
        /// 获得总记录数
        /// </summary>
        /// <param name="StrWhere"></param>
        /// <returns></returns>
        public int GetListCount(string StrWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(ID) as ID FROM ChangingFlowerExpenses");
            if (!string.IsNullOrEmpty(StrWhere))
            {
                strSql.Append(" where  1=1 " + StrWhere);
            }
            List<Model.ChangingFlowerExpenses> ChangingFlowerExpenses = Factory.DBHelper.Query<Model.ChangingFlowerExpenses>(SQLConString, strSql.ToString(), new DynamicParameters(new { StrWhere }));
            return ChangingFlowerExpenses.Count() > 0 ? ChangingFlowerExpenses[0].ID : 0;
        }

        /// <summary>
        /// 分页获得数据信息
        /// </summary>
        /// <param name="limit">页码大小</param>
        /// <param name="offset">第几页</param>
        /// <param name="UserName">用户名</param>
        /// <param name="Role">角色ID</param>
        /// <returns></returns>
        public List<Model.ChangingFlowerExpenses> GetList( int offset, string StrWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( SELECT ROW_NUMBER() over(order by id desc) as rn ,* FROM ChangingFlowerExpenses");
            if (!string.IsNullOrEmpty(StrWhere))
            {
                strSql.Append(" where  1=1 " + StrWhere);
            }
            strSql.Append(")T where t.rn between   @offset and (@offset+9)");
            return Factory.DBHelper.Query<Model.ChangingFlowerExpenses>(SQLConString, strSql.ToString(), new DynamicParameters(new { offset }));
        }

        public bool InsertChangingFlowerExpenses(Model.ChangingFlowerExpenses ChangingFlowerExpenses)
        {
            const string sql =
@"INSERT INTO ChangingFlowerExpenses(FlowerName,FlowerNUm,Number,CostPrice,SalePrice,ChangeAddress,ReplacementReason,replacementName,
ReplacementTelephone,ReplacementUnit)  VALUES(@FlowerName,@FlowerNUm,@Number,@CostPrice,@SalePrice,@ChangeAddress,@ReplacementReason,
@replacementName,@ReplacementTelephone,@ReplacementUnit)";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                ChangingFlowerExpenses.FlowerName,
                ChangingFlowerExpenses.FlowerNUm,
                ChangingFlowerExpenses.Number,
                ChangingFlowerExpenses.CostPrice,
                ChangingFlowerExpenses.SalePrice,
                ChangingFlowerExpenses.ChangeAddress,
                ChangingFlowerExpenses.ReplacementReason,
                ChangingFlowerExpenses.replacementName,
                ChangingFlowerExpenses.ReplacementTelephone,
                ChangingFlowerExpenses.ReplacementUnit,
            }));
        }

        public bool UpdateChangingFlowerExpenses(Model.ChangingFlowerExpenses ChangingFlowerExpenses)
        {
            const string sql =
@"UPDATE  ChangingFlowerExpenses  SET FlowerName=@FlowerName,FlowerNUm=@FlowerNUm,Number=@Number,CostPrice=@CostPrice,
SalePrice=@SalePrice,ChangeAddress=@ChangeAddress,ReplacementReason=@ReplacementReason,replacementName=@replacementName,
ReplacementTelephone=@ReplacementTelephone,ReplacementUnit=@ReplacementUnit  WHERE ID=@ID";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                ChangingFlowerExpenses.FlowerName,
                ChangingFlowerExpenses.FlowerNUm,
                ChangingFlowerExpenses.Number,
                ChangingFlowerExpenses.CostPrice,
                ChangingFlowerExpenses.SalePrice,
                ChangingFlowerExpenses.ChangeAddress,
                ChangingFlowerExpenses.ReplacementReason,
                ChangingFlowerExpenses.replacementName,
                ChangingFlowerExpenses.ReplacementTelephone,
                ChangingFlowerExpenses.ReplacementUnit,
                ChangingFlowerExpenses.ID
            }));
        }

        public bool DeleteInfo(string ID) 
        {
            const string sql =
@"DELETE FROM ChangingFlowerExpenses WHERE ID=@ID ";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                ID
            }));
        }

        public Model.ChangingFlowerExpenses GetModel(string id)
        {
            const string sql =
@"SELECT * FROM  ChangingFlowerExpenses WHERE ID=@ID";
            List<Model.ChangingFlowerExpenses> ChangingFlowerExpenses = Factory.DBHelper.Query<Model.ChangingFlowerExpenses>(SQLConString, sql.ToString(), new DynamicParameters(new { id }));
            return ChangingFlowerExpenses.Count() > 0 ? ChangingFlowerExpenses[0] : null;
        }
    }
}
