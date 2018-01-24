using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public  class Sys_FlowerChange
    {
        private string SQLConString = System.Configuration.ConfigurationManager.AppSettings["SQLConString"];



        /// <summary>
        /// 
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="StrWhere"></param>
        /// <returns></returns>
        public List<Model.FlowerChange> GetList(int limit, int offset, string StrWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( SELECT ROW_NUMBER() over(order by id desc) as rn ,* FROM FlowerChange");
            if (!string.IsNullOrEmpty(StrWhere))
            {
                strSql.Append(" where  1=1 " + StrWhere);
            }
            strSql.Append(")T where t.rn between   @offset and (@offset+9)");
            return Factory.DBHelper.Query<Model.FlowerChange>(SQLConString, strSql.ToString(), new DynamicParameters(new { offset }));
        }



        /// <summary>
        /// 获得总记录数
        /// </summary>
        /// <param name="StrWhere"></param>
        /// <returns></returns>
        public int GetFlowerChangeListCount(string StrWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(id) as id FROM FlowerChange");
            if (!string.IsNullOrEmpty(StrWhere))
            {
                strSql.Append(" where  1=1 " + StrWhere);
            }
            List<Model.FlowerChange> FlowerChangeList = Factory.DBHelper.Query<Model.FlowerChange>(SQLConString, strSql.ToString(), new DynamicParameters(new { StrWhere }));
            return FlowerChangeList.Count() > 0 ? FlowerChangeList[0].id : 0;
        }



        /// <summary>
        /// 写入一条记录
        /// </summary>
        /// <param name="FlowerChange"></param>
        /// <returns></returns>
        public bool InsertFlowerChange(Model.FlowerChange FlowerChange) 
        {
            const string sql =
@"INSERT INTO FlowerChange(FlowerNumber,UsersId,FlowerTreatmentType,ContentMsg,Photo,ChangePhoto,Number,WorkUsersId,OwnedCompany,OwnedUsersId,WorkUsersRealName,FlowerType,PlacingPosition,Sum,Reamrk,State) 
 VALUES(@FlowerNumber,@UsersId,@FlowerTreatmentType,@ContentMsg,@Photo,@ChangePhoto,@Number,@WorkUsersId,@OwnedCompany,@OwnedUsersId,@WorkUsersRealName,@FlowerType,@PlacingPosition,@Sum,@Reamrk,@State)";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                FlowerChange.FlowerNumber,
                FlowerChange.UsersId,
                FlowerChange.FlowerTreatmentType,
                FlowerChange.ContentMsg,
                FlowerChange.Photo,
                FlowerChange.ChangePhoto,
                FlowerChange.Number,
                FlowerChange.WorkUsersId,
                FlowerChange.OwnedCompany,
                FlowerChange.OwnedUsersId,
                FlowerChange.WorkUsersRealName,
                FlowerChange.FlowerType,
                FlowerChange.PlacingPosition,
                FlowerChange.Sum,
                FlowerChange.Reamrk,
                FlowerChange.State,
            }));
        }

        public bool AddFlowerPhotoInfo(string Number, string ChangePhoto)
        {
            const string sql =
@"UPDATE FlowerChange SET ChangePhoto=@ChangePhoto,state='已更换'  WHERE id=@Number";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                ChangePhoto,
                Number
            }));
        }


        public bool UpdateFlowerPhoto(string id,string imgurl)
        {
            const string sql =
  @"UPDATE FlowerChange SET ChangePhoto=@imgurl  WHERE id=@id";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                imgurl,
                id
            }));
        }
    }
}
