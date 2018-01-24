using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    /// <summary>
    /// 员工入职业务处理
    /// </summary>
    public class Sys_EmployeeEntry
    {
        private string SQLConString = System.Configuration.ConfigurationManager.AppSettings["SQLConString"];

        public bool UpdateEmployeeEntry(Model.EmployeeEntry EmployeeEntry)
        {
            const string sql =
@"update  EmployeeEntry set Name=@Name,Sex=@Sex,Birthday=@Birthday,Nation=@Nation,Marriage=@Marriage,
Education=@Education,Major=@Major,WhatLanguage=@WhatLanguage,PlaceOfOrigin=@PlaceOfOrigin,RegisteredResidence=@RegisteredResidence,
PresentAddressTelephoneNumber=@PresentAddressTelephoneNumber,IDCard=@IDCard,TemporaryResidencePermitNumber=@TemporaryResidencePermitNumber,
EmergencyContact=@EmergencyContact,EmergencyContactTelephone=@EmergencyContactTelephone,ContactUnit=@ContactUnit,
ApplyForPosition=@ApplyForPosition,MonthlySalary=@MonthlySalary,NameOfFamilyMember=@NameOfFamilyMember,FamilyMembersAndMyself=@FamilyMembersAndMyself,
BeginningTimeOfWork=@BeginningTimeOfWork,EndingTimeOfWork=@EndingTimeOfWork,WorkUnit=@WorkUnit,FamilyMembersUnitAndPosition=@FamilyMembersUnitAndPosition,
NameOfFamilyMember1=@NameOfFamilyMember1,FamilyMembersAndMyself1=@FamilyMembersAndMyself1,FamilyMembersUnitAndPosition1=@FamilyMembersUnitAndPosition1,
NameOfFamilyMember2=@NameOfFamilyMember2,FamilyMembersAndMyself2=@FamilyMembersAndMyself2,FamilyMembersUnitAndPosition2=@FamilyMembersUnitAndPosition2,
BeginningTimeOfWork1=@BeginningTimeOfWork1,EndingTimeOfWork1=@EndingTimeOfWork1,WorkUnit1=@WorkUnit1,BeginningTimeOfWork2=@BeginningTimeOfWork2,
EndingTimeOfWork2=@EndingTimeOfWork2,WorkUnit2=@WorkUnit2  where Number=@Number";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                EmployeeEntry.Name,
                EmployeeEntry.Sex,
                EmployeeEntry.Birthday,
                EmployeeEntry.Nation,
                EmployeeEntry.Marriage,
                EmployeeEntry.Education,
                EmployeeEntry.Major,
                EmployeeEntry.WhatLanguage,
                EmployeeEntry.PlaceOfOrigin,
                EmployeeEntry.RegisteredResidence,
                EmployeeEntry.PresentAddressTelephoneNumber,
                EmployeeEntry.IDCard,
                EmployeeEntry.TemporaryResidencePermitNumber,
                EmployeeEntry.EmergencyContact,
                EmployeeEntry.EmergencyContactTelephone,
                EmployeeEntry.ContactUnit,
                EmployeeEntry.ApplyForPosition,
                EmployeeEntry.MonthlySalary,
                EmployeeEntry.NameOfFamilyMember,
                EmployeeEntry.FamilyMembersAndMyself,
                EmployeeEntry.BeginningTimeOfWork,
                EmployeeEntry.EndingTimeOfWork,
                EmployeeEntry.WorkUnit,
                EmployeeEntry.FamilyMembersUnitAndPosition,
              

                EmployeeEntry.NameOfFamilyMember1,
                EmployeeEntry.FamilyMembersAndMyself1,
                EmployeeEntry.FamilyMembersUnitAndPosition1,
                EmployeeEntry.NameOfFamilyMember2,
                EmployeeEntry.FamilyMembersAndMyself2,
                EmployeeEntry.FamilyMembersUnitAndPosition2,
                EmployeeEntry.BeginningTimeOfWork1,
                EmployeeEntry.EndingTimeOfWork1,

                EmployeeEntry.WorkUnit1,
                EmployeeEntry.BeginningTimeOfWork2,
                EmployeeEntry.EndingTimeOfWork2,
                EmployeeEntry.WorkUnit2,
                EmployeeEntry.Number,
            }));
        }

        /// <summary>
        /// 写入员工入职表信息
        /// </summary>
        /// <param name="EmployeeEntry"></param>
        /// <returns></returns>
        public bool InsertEmployeeEntry(Model.EmployeeEntry EmployeeEntry) 
        {
            const string sql =
@"INSERT INTO EmployeeEntry(Name,Sex,Birthday,Nation,Marriage,Education,Major,WhatLanguage,PlaceOfOrigin,RegisteredResidence,
PresentAddressTelephoneNumber,IDCard,TemporaryResidencePermitNumber,EmergencyContact,EmergencyContactTelephone,ContactUnit,
ApplyForPosition,MonthlySalary,NameOfFamilyMember,FamilyMembersAndMyself,BeginningTimeOfWork,EndingTimeOfWork,WorkUnit,FamilyMembersUnitAndPosition,Number,
NameOfFamilyMember1,FamilyMembersAndMyself1,FamilyMembersUnitAndPosition1,NameOfFamilyMember2,FamilyMembersAndMyself2,FamilyMembersUnitAndPosition2,
BeginningTimeOfWork1,EndingTimeOfWork1,WorkUnit1,BeginningTimeOfWork2,EndingTimeOfWork2,WorkUnit2)
 VALUES(@Name,@Sex,@Birthday,@Nation,@Marriage,@Education,@Major,@WhatLanguage,@PlaceOfOrigin,@RegisteredResidence,
@PresentAddressTelephoneNumber,@IDCard,@TemporaryResidencePermitNumber,@EmergencyContact,@EmergencyContactTelephone,@ContactUnit,
@ApplyForPosition,@MonthlySalary,@NameOfFamilyMember,@FamilyMembersAndMyself,@BeginningTimeOfWork,@EndingTimeOfWork,@WorkUnit,@FamilyMembersUnitAndPosition,@Number,
@NameOfFamilyMember1,@FamilyMembersAndMyself1,@FamilyMembersUnitAndPosition1,@NameOfFamilyMember2,@FamilyMembersAndMyself2,@FamilyMembersUnitAndPosition2,
@BeginningTimeOfWork1,@EndingTimeOfWork1,@WorkUnit1,@BeginningTimeOfWork2,@EndingTimeOfWork2,@WorkUnit2)";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                EmployeeEntry.Name,
                EmployeeEntry.Sex,
                EmployeeEntry.Birthday,
                EmployeeEntry.Nation,
                EmployeeEntry.Marriage,
                EmployeeEntry.Education,
                EmployeeEntry.Major,
                EmployeeEntry.WhatLanguage,
                EmployeeEntry.PlaceOfOrigin,
                EmployeeEntry.RegisteredResidence,
                EmployeeEntry.PresentAddressTelephoneNumber,
                EmployeeEntry.IDCard,
                EmployeeEntry.TemporaryResidencePermitNumber,
                EmployeeEntry.EmergencyContact,
                EmployeeEntry.EmergencyContactTelephone,
                EmployeeEntry.ContactUnit,
                EmployeeEntry.ApplyForPosition,
                EmployeeEntry.MonthlySalary,
                EmployeeEntry.NameOfFamilyMember,
                EmployeeEntry.FamilyMembersAndMyself,
                EmployeeEntry.BeginningTimeOfWork,
                EmployeeEntry.EndingTimeOfWork,
                EmployeeEntry.WorkUnit,
                EmployeeEntry.FamilyMembersUnitAndPosition,
                EmployeeEntry.Number,

                EmployeeEntry.NameOfFamilyMember1,
                EmployeeEntry.FamilyMembersAndMyself1,
                EmployeeEntry.FamilyMembersUnitAndPosition1,
                EmployeeEntry.NameOfFamilyMember2,
                EmployeeEntry.FamilyMembersAndMyself2,
                EmployeeEntry.FamilyMembersUnitAndPosition2,
                EmployeeEntry.BeginningTimeOfWork1,
                EmployeeEntry.EndingTimeOfWork1,

                EmployeeEntry.WorkUnit1,
                EmployeeEntry.BeginningTimeOfWork2,
                EmployeeEntry.EndingTimeOfWork2,
                EmployeeEntry.WorkUnit2,
            }));
        }


        /// <summary>
        /// 通过编号获得详细信息
        /// </summary>
        /// <param name="Number"></param>
        /// <returns></returns>
        public Model.EmployeeEntry GetInfoByNumber(string Number) 
        {
            const string sql =
@"SELECT * FROM  EmployeeEntry WHERE Number=@Number";
            List<Model.EmployeeEntry> EmployeeEntryList = Factory.DBHelper.Query<Model.EmployeeEntry>(SQLConString, sql.ToString(), new DynamicParameters(new { Number }));
            return EmployeeEntryList.Count > 0 ? EmployeeEntryList[0] : null;
        }
    }
}
