using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public  class Sys_FlowerArrangement
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
            strSql.Append("SELECT COUNT(ID) as id FROM FlowerArrangement");
            if (!string.IsNullOrEmpty(StrWhere))
            {
                strSql.Append(" where  1=1 " + StrWhere);
            }
            List<Model.FlowerArrangement> UserAdminList = Factory.DBHelper.Query<Model.FlowerArrangement>(SQLConString, strSql.ToString(), new DynamicParameters(new { StrWhere }));
            return UserAdminList.Count() > 0 ? UserAdminList[0].id : 0;
        }

        /// <summary>
        /// 分页获得数据信息
        /// </summary>
        /// <param name="limit">页码大小</param>
        /// <param name="offset">第几页</param>
        /// <param name="UserName">用户名</param>
        /// <param name="Role">角色ID</param>
        /// <author>wangwei</author>
        /// <returns></returns>
        public List<Model.FlowerArrangement> GetList(int limit, int offset, string StrWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( SELECT ROW_NUMBER() over(order by f1.id desc) as rn , ");
            strSql.Append(@"f1.Id,f1.ShopId,f1.arrangement,f1.FlowerType,f1.Specifications,f1.UnitPrice,
            f1.Count,f1.Total,f1.Remark,f1.ImgORCodePath,f1.belongUsersId,FL.FlowerWatchPhoto as Photo,");
            strSql.Append("US.OwnedCompany,FL.FlowerWatchName,FL.FlowerWatchType  FROM FlowerArrangement f1 ");
            strSql.Append("left join  UserAdmin Us  on f1.belongUsersId=Us.id left join Flower FL on f1.ShopId=FL.ID");
            if (!string.IsNullOrEmpty(StrWhere))
            {
                strSql.Append(" where  1=1 " + StrWhere);
            }
            strSql.Append(")T where t.rn between   @offset and (@offset+@limit-1)");
            return Factory.DBHelper.Query<Model.FlowerArrangement>(SQLConString, strSql.ToString(), new DynamicParameters(new { offset,limit }));
        }
        /// <summary>
        /// 删除时获得多条记录
        /// </summary>      
        /// <param name="StrWhere"></param>
        /// <returns></returns>
        public List<Model.FlowerArrangement> GetList(string StrWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT  *  FROM FlowerArrangement");            
            if (!string.IsNullOrEmpty(StrWhere))
            {
                strSql.Append(" where  1=1 " + StrWhere);
            }           
            return Factory.DBHelper.Query<Model.FlowerArrangement>(SQLConString, strSql.ToString(), new DynamicParameters(new { }));
        }
        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <author>wangwei</author>
        /// <returns></returns>
        public Model.FlowerArrangement GetModel(string id) 
        {
            const string sql =
            @"SELECT  f1.Id,f1.ShopId,f1.arrangement,f1.FlowerType,f1.Specifications,f1.UnitPrice,
            f1.Count,f1.Total,f1.Remark,f1.ImgORCodePath,f1.belongUsersId,FL.FlowerWatchPhoto as Photo,
            FL.FlowerWatchName,FL.FlowerWatchType,FL.XiXin,FL.YangHuFangFa,FL.FlowerSalesPrice,
            us.OwnedCompany,us.Weekly FROM FlowerArrangement  f1 left join Flower FL on f1.ShopId=FL.ID 
            left join useradmin  us on us.id= f1.belongUsersId  WHERE f1.Id=@id";
            List<Model.FlowerArrangement> FlowerArrangementList = Factory.DBHelper.Query<Model.FlowerArrangement>(SQLConString, sql.ToString(), new DynamicParameters(new { id }));
            return FlowerArrangementList.Count() > 0 ? FlowerArrangementList[0] : null;
        }

        /// <summary>
        /// 查询养护时间
        /// </summary>
        /// <param name="belongUsersId">所属用户id</param>
        /// <author>wangwei</author>
        /// <returns></returns>
        public Model.FlowerTreatment GetFlowerTreatmentModel(string belongUsersId) 
        {
            const string sql =
            @"SELECT  max([time]) as time FROM [FlowerTreatment] where OwnedUsersId=@belongUsersId";
            List<Model.FlowerTreatment> FlowerTreatmentList = Factory.DBHelper.Query<Model.FlowerTreatment>(SQLConString, sql.ToString(), new DynamicParameters(new { belongUsersId }));
            return FlowerTreatmentList.Count() > 0 ? FlowerTreatmentList[0] : null;
        }

        public int Add(Model.FlowerArrangement FlowerArrangement)
        {
            const string sql =
@"INSERT INTO FlowerArrangement(arrangement,FlowerType,Photo,Specifications,UnitPrice,Count,Total,Remark,belongUsersId,ShopId) 
 VALUES(@arrangement,@FlowerType,@Photo,@Specifications,@UnitPrice,@Count,@Total,@Remark,@belongUsersId,@ShopId);SELECT @@identity";
            //var p = new DynamicParameters();
            //p.Add("@id", dbType: DbType.Int32, direction: ParameterDirection.Output);
            var info= Factory.DBHelper.ExecSqlGetId(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                FlowerArrangement.arrangement,
                FlowerArrangement.FlowerType,
                FlowerArrangement.Photo,
                FlowerArrangement.Specifications,
                FlowerArrangement.UnitPrice,
                FlowerArrangement.Count,
                FlowerArrangement.Total,
                FlowerArrangement.Remark,             
                FlowerArrangement.belongUsersId,
                FlowerArrangement.ShopId,

            }));     
            return info;
        }

        public bool Edit(Model.FlowerArrangement FlowerArrangement)
        {
            const string sql =
@" UPDATE  FlowerArrangement SET arrangement=@arrangement,FlowerType=@FlowerType, Photo=@Photo,Specifications=@Specifications,
UnitPrice=@UnitPrice,Count=@Count,Total=@Total,Remark=@Remark,belongUsersId=@belongUsersId,ShopId=@ShopId  WHERE Id=@id";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                FlowerArrangement.arrangement,
                FlowerArrangement.FlowerType,
                FlowerArrangement.Photo,
                FlowerArrangement.Specifications,
                FlowerArrangement.UnitPrice,
                FlowerArrangement.Count,
                FlowerArrangement.Total,
                FlowerArrangement.Remark,              
                FlowerArrangement.belongUsersId,  
                FlowerArrangement.ShopId,
                FlowerArrangement.id
            }));
        }

        public bool Delete(string id)
        {
            const string sql =
@"DELETE FROM  FlowerArrangement  WHERE Id=@id";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                id
            }));
        }

        public bool UpdateImgORCodePath(string path,int id)
        {
            const string sql =
@"UPDATE  FlowerArrangement SET  ImgORCodePath=@path where id=@id";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                path,id
            }));
        }



        /// <summary>
        /// 更改摆放位置图片url
        /// </summary>
        /// <param name="path"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool UpdateUploadImg(string path, int id)
        {
            const string sql =
 @"UPDATE  FlowerArrangement SET  Photo=@path where id=@id";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                path,
                id
            }));
        }

    }
}
