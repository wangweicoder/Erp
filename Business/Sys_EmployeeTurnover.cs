using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    /// <summary>
    /// 员工转正信息类
    /// </summary>
    public  class Sys_EmployeeTurnover
    {
        private string SQLConString = System.Configuration.ConfigurationManager.AppSettings["SQLConString"];

        /// <summary>
        /// 写入一条记录
        /// </summary>
        /// <param name="EmployeeTurnover"></param>
        /// <returns></returns>
        public bool InsertEmployeeTurnover(Model.EmployeeTurnover EmployeeTurnover)
        {
            const string sql =
@"INSERT INTO EmployeeTurnover(Number,CreateTime,Name,Sex,Birthday,SubordinateSector,Post,PostTitle,InductionTime,
StartTimeOfProbation,EndOfProbationPeriod,WorkRepor,SuperiorOpinion,AdministrationPersonnelDepartmentOpinion,GeneralManagerOpinion)
 VALUES(@Number,@CreateTime,@Name,@Sex,@Birthday,@SubordinateSector,@Post,@PostTitle,@InductionTime,
@StartTimeOfProbation,@EndOfProbationPeriod,@WorkRepor,@SuperiorOpinion,@AdministrationPersonnelDepartmentOpinion,@GeneralManagerOpinion)";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                EmployeeTurnover.Number,
                EmployeeTurnover.CreateTime,
                EmployeeTurnover.Name,
                EmployeeTurnover.Sex,
                EmployeeTurnover.Birthday,
                EmployeeTurnover.SubordinateSector,
                EmployeeTurnover.Post,
                EmployeeTurnover.PostTitle,
                EmployeeTurnover.InductionTime,
                EmployeeTurnover.StartTimeOfProbation,
                EmployeeTurnover.EndOfProbationPeriod,
                EmployeeTurnover.WorkRepor,
                EmployeeTurnover.SuperiorOpinion,
                EmployeeTurnover.AdministrationPersonnelDepartmentOpinion,
                EmployeeTurnover.GeneralManagerOpinion,
            }));
        }

        /// <summary>
        /// 通过编号获得详细信息
        /// </summary>
        /// <param name="Number"></param>
        /// <returns></returns>
        public Model.EmployeeTurnover GetInfoByNumber(string Number)
        {
            const string sql =
@"SELECT * FROM  EmployeeTurnover WHERE Number=@Number";
            List<Model.EmployeeTurnover> EmployeeTurnoverList = Factory.DBHelper.Query<Model.EmployeeTurnover>(SQLConString, sql.ToString(), new DynamicParameters(new { Number }));
            return EmployeeTurnoverList.Count() > 0 ? EmployeeTurnoverList[0] : null;
        }


        public bool UpdateInfo(Model.EmployeeTurnover EmployeeTurnover) 
        {
            const string sql =
@"update   EmployeeTurnover  set  Name=@Name,Sex=@Sex,Birthday=@Birthday,SubordinateSector=@SubordinateSector,Post=@Post,PostTitle=@PostTitle,InductionTime=@InductionTime,
StartTimeOfProbation=@StartTimeOfProbation,EndOfProbationPeriod=@EndOfProbationPeriod,WorkRepor=@WorkRepor,SuperiorOpinion=@SuperiorOpinion,AdministrationPersonnelDepartmentOpinion=@AdministrationPersonnelDepartmentOpinion,
GeneralManagerOpinion=@GeneralManagerOpinion  where Number=@Number";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
             
                EmployeeTurnover.Name,
                EmployeeTurnover.Sex,
                EmployeeTurnover.Birthday,
                EmployeeTurnover.SubordinateSector,
                EmployeeTurnover.Post,
                EmployeeTurnover.PostTitle,
                EmployeeTurnover.InductionTime,
                EmployeeTurnover.StartTimeOfProbation,
                EmployeeTurnover.EndOfProbationPeriod,
                EmployeeTurnover.WorkRepor,
                EmployeeTurnover.SuperiorOpinion,
                EmployeeTurnover.AdministrationPersonnelDepartmentOpinion,
                EmployeeTurnover.GeneralManagerOpinion,
                EmployeeTurnover.Number,
            }));
        }
    }
}
