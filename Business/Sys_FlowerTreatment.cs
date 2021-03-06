﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class Sys_FlowerTreatment
    {
        private string SQLConString = System.Configuration.ConfigurationManager.AppSettings["SQLConString"];

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="StrWhere"></param>
        /// <returns></returns>
        public List<Model.FlowerTreatment> FlowerTreatmentList(int limit, int offset, string StrWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT u.OwnedCompany as CompanyName,T.* FROM ( SELECT ROW_NUMBER() over(order by id desc) as rn ,* FROM FlowerTreatment");
            if (!string.IsNullOrEmpty(StrWhere))
            {
                strSql.Append(" where  1=1 " + StrWhere);
            }
            strSql.Append(")T inner join [dbo].[UserAdmin] u on u.ID=T.OwnedUsersId  where t.rn between   @offset and (@offset+@limit-1)");
            
            return Factory.DBHelper.Query<Model.FlowerTreatment>(SQLConString, strSql.ToString(), new DynamicParameters(new { offset,limit }));
        }
        /// <summary>
        /// 服务记录
        /// </summary>
        /// <param name="pagesize"></param>
        /// <param name="pagenumber"></param>
        /// <param name="StrWhere"></param>
        /// <returns></returns>
        public List<Model.FlowerTreatment> MFlowerTreatmentList(int pagesize, int pagenumber, string StrWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT u.FlowerWatchName as CompanyName,T.* FROM ( SELECT ROW_NUMBER() over(order by id desc) as rn ,* FROM FlowerTreatment");
            if (!string.IsNullOrEmpty(StrWhere))
            {
                strSql.Append(" where  FlowerTreatmentType like '服务%'  " + StrWhere);
            }
            strSql.Append(")T inner join [dbo].[Flower] u on u.ID=T.FlowerNumber   where t.rn between  (@pagesize*(@pagenumber-1)+1) and (@pagesize*@pagenumber)");

            return Factory.DBHelper.Query<Model.FlowerTreatment>(SQLConString, strSql.ToString(), new DynamicParameters(new { pagenumber, pagesize }));
        }            
        /// <summary>
        /// 获得总记录数
        /// </summary>
        /// <param name="StrWhere"></param>
        /// <returns></returns>
        public int GetFlowerTreatmentListCount(string StrWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(id) as id FROM FlowerTreatment");
            if (!string.IsNullOrEmpty(StrWhere))
            {
                strSql.Append(" where  1=1 " + StrWhere);
            }
            List<Model.FlowerTreatment> FlowerTreatmentList = Factory.DBHelper.Query<Model.FlowerTreatment>(SQLConString, strSql.ToString(), new DynamicParameters(new { StrWhere }));
            return FlowerTreatmentList.Count() > 0 ? FlowerTreatmentList[0].id : 0;
        }
        /// <summary>
        /// 按条件查询
        /// </summary>
        /// <param name="StrWhere"></param>
        /// <returns></returns>
        public List<Model.FlowerTreatment> FlowerTreatmentList(string StrWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM FlowerTreatment t");
            if (!string.IsNullOrEmpty(StrWhere))
            {
                strSql.Append(" where  1=1 and" + StrWhere);
            }            
            return Factory.DBHelper.Query<Model.FlowerTreatment>(SQLConString, strSql.ToString(), new DynamicParameters(new { StrWhere }));
        }
        public List<Model.FlowerTreatment> GetFlowerTreatmentName() 
        {
            const string sql =
@"SELECT DISTINCT  OwnedUsersId,OwnedUsersRealName FROM FlowerTreatment  ";
            return Factory.DBHelper.Query<Model.FlowerTreatment>(SQLConString, sql.ToString(), new DynamicParameters(new {  }));
        }


        /// <summary>
        /// 写入一条记录
        /// </summary>
        /// <param name="FlowerTreatment"></param>
        /// <returns></returns>
        public bool InsertFlowerTreatment(Model.FlowerTreatment FlowerTreatment)
        {
            const string sql=
        @"INSERT INTO FlowerTreatment(FlowerNumber,UsersId,FlowerTreatmentType,ContentMsg,Photo,OwnedUsersId,OwnedUsersRealName,FlowerTreatmentAddress,UserRealName,
            OwnedCompany,LogoPhoto,State,starttime,ArrangementId) VALUES(@FlowerNumber,@UsersId,@FlowerTreatmentType,@ContentMsg,@Photo,@OwnedUsersId,@OwnedUsersRealName,
            @FlowerTreatmentAddress,@UserRealName,@OwnedCompany,@LogoPhoto,@State,@starttime,@ArrangementId)";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                FlowerTreatment.FlowerNumber,
                FlowerTreatment.UsersId,
                FlowerTreatment.FlowerTreatmentType,
                FlowerTreatment.ContentMsg,
                FlowerTreatment.Photo,
                FlowerTreatment.OwnedUsersId,
                FlowerTreatment.OwnedUsersRealName,
                FlowerTreatment.FlowerTreatmentAddress,
                FlowerTreatment.UserRealName ,
                FlowerTreatment.OwnedCompany,
                FlowerTreatment.LogoPhoto,
                FlowerTreatment.State,
                FlowerTreatment.starttime,
                FlowerTreatment.ArrangementId
            }));
        }


        public bool Update(Model.FlowerTreatment FlowerTreatment)
        {
            const string sql =
@"UPDATE  FlowerTreatment  SET FlowerNumber=@FlowerNumber,UserRealName=@UserRealName,UsersId=@UsersId,FlowerTreatmentAddress=@FlowerTreatmentAddress,
ContentMsg=@ContentMsg,OwnedUsersRealName=@OwnedUsersRealName,OwnedUsersId=@OwnedUsersId  WHERE id=@id";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                FlowerTreatment.FlowerNumber,
                FlowerTreatment.UserRealName,
                FlowerTreatment.UsersId,
                FlowerTreatment.FlowerTreatmentAddress,
                FlowerTreatment.ContentMsg,
                FlowerTreatment.OwnedUsersRealName,
                FlowerTreatment.OwnedUsersId,
                FlowerTreatment.id,
            }));
        }

        public bool DeleteInfo(string id) 
        {
            const string sql =
@"DELETE FROM  FlowerTreatment  where id=@id";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                id
            }));
        }

        public Model.FlowerTreatment GetModel(string ID) 
        {
            const string sql =
@"SELECT * FROM  FlowerTreatment WHERE ID=@ID";
            List<Model.FlowerTreatment> FlowerTreatmentList = Factory.DBHelper.Query<Model.FlowerTreatment>(SQLConString, sql.ToString(), new DynamicParameters(new { ID }));
            return FlowerTreatmentList.Count() > 0 ? FlowerTreatmentList[0] : null;
        }
        /// <summary>
        /// 通过花卉id查询服务前记录
        /// </summary>        
        /// <returns></returns>
        public Model.FlowerTreatment GetModelbyArrid(string ArrangementId, string ownedUsersId ,string usersid)
        {
            const string sql = @"SELECT top 1 * FROM  FlowerTreatment WHERE ArrangementId=@ArrangementId and OwnedUsersId=@ownedUsersId and FlowerTreatmentType ='服务前'  order by time desc ";
            List<Model.FlowerTreatment> FlowerTreatmentList = Factory.DBHelper.Query<Model.FlowerTreatment>(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                ArrangementId,
                ownedUsersId,
                usersid
            }));
            return FlowerTreatmentList.Count() > 0 ? FlowerTreatmentList[0] : null;
        }
        /// <summary>
        /// 通过摆放id查询开始养护记录
        /// </summary>        
        /// <returns></returns>
        public Model.FlowerTreatment GetModelbyid(string ArrangementId, string ownedUsersId, string usersid)
        {
            const string sql = @"SELECT top 1 * FROM  FlowerTreatment WHERE ArrangementId=@ArrangementId and OwnedUsersId=@ownedUsersId and FlowerTreatmentType ='开始养护'  order by time desc ";
            List<Model.FlowerTreatment> FlowerTreatmentList = Factory.DBHelper.Query<Model.FlowerTreatment>(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                ArrangementId,
                ownedUsersId,
                usersid
            }));
            return FlowerTreatmentList.Count() > 0 ? FlowerTreatmentList[0] : null;
        }
        /// <summary>
        /// 通过客户id查询养护记录(公司有一盆花开始养护，其他花也在养护)
        /// </summary>        
        /// <returns></returns>
        public Model.FlowerTreatment GetModelbyOwnid(string ownedUsersId)
        {
            const string sql = @"SELECT top 1 * FROM  FlowerTreatment WHERE OwnedUsersId=@ownedUsersId and FlowerTreatmentType ='开始养护' order by time desc ";
            List<Model.FlowerTreatment> FlowerTreatmentList = Factory.DBHelper.Query<Model.FlowerTreatment>(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                ownedUsersId,                
            }));
            return FlowerTreatmentList.Count() > 0 ? FlowerTreatmentList[0] : null;
        }
        /// <summary>
        /// 结束养护花卉
        /// </summary>     
        public bool UpdateServer(Model.FlowerTreatment FlowerTreatment)
        {
            const string sql = @"UPDATE  FlowerTreatment  SET endtime=@endtime,FlowerTreatmentType=@FlowerTreatmentType,State=@State  WHERE id=@id";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                FlowerTreatment.endtime,
                FlowerTreatment.FlowerTreatmentType,
                FlowerTreatment.State,
                FlowerTreatment.id,
            }));
        }
        /// <summary>
        /// 养护花卉服务后
        /// </summary>     
        public bool AddServerPhoto(Model.FlowerTreatment FlowerTreatment)
        {
            const string sql = @"UPDATE  FlowerTreatment  SET FlowerTreatmentType=@FlowerTreatmentType,ChangePhoto=@ChangePhoto,endtime=@endtime,State=@State  WHERE id=@id";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                FlowerTreatment.FlowerTreatmentType,
                FlowerTreatment.ChangePhoto,
                FlowerTreatment.id,
                FlowerTreatment.endtime,
                FlowerTreatment.State
            }));
        }
        /// <summary>
        /// 养护花卉服务前后
        /// </summary>     
        public bool UpdateServerPhoto(Model.FlowerTreatment FlowerTreatment)
        {
            const string sql = @"UPDATE  FlowerTreatment  SET Photo=@Photo,ChangePhoto=@ChangePhoto  WHERE id=@id";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                FlowerTreatment.Photo,
                FlowerTreatment.ChangePhoto,
                FlowerTreatment.id
                
            }));
        }
    }
}
