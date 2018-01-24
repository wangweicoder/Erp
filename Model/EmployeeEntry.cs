using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 员工入职
    /// </summary>
    public class EmployeeEntry
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int ID { set; get; }
        /// <summary>
        /// 编号
        /// </summary>
        public string Number { set; get; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { set; get; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime Birthday { set; get; }

        /// <summary>
        /// 民族
        /// </summary>
        public string Nation { set; get; }

        /// <summary>
        /// 婚否
        /// </summary>
        public string Marriage { set; get; }

        /// <summary>
        /// 学历
        /// </summary>
        public string Education { set; get; }

        /// <summary>
        /// 专业
        /// </summary>
        public string Major { set; get; }

        /// <summary>
        /// 何种语言
        /// </summary>
        public string WhatLanguage { set; get; }

        /// <summary>
        /// 籍贯
        /// </summary>
        public string PlaceOfOrigin { set; get; }

        /// <summary>
        /// 户口所在地
        /// </summary>
        public string RegisteredResidence { set; get; }

        /// <summary>
        /// 现住址电话
        /// </summary>
        public int PresentAddressTelephoneNumber { set; get; }

        /// <summary>
        /// 身份证号码
        /// </summary>
        public string IDCard { set; get; }

        /// <summary>
        /// 暂住证号码
        /// </summary>
        public string TemporaryResidencePermitNumber { set; get; }

        /// <summary>
        /// 应急联系人
        /// </summary>
        public string EmergencyContact { set; get; }
        /// <summary>
        /// 应急联系人电话
        /// </summary>
        public int EmergencyContactTelephone { set; get; }
        /// <summary>
        /// 联系人单位
        /// </summary>
        public string ContactUnit { set; get; }

        /// <summary>
        /// 申请职位
        /// </summary>
        public string ApplyForPosition { set; get; }

        /// <summary>
        /// 月薪
        /// </summary>
        public decimal MonthlySalary { set; get; }

        /// <summary>
        /// 家庭主要成员姓名
        /// </summary>
        public string NameOfFamilyMember { set; get; }

        /// <summary>
        /// 家庭主要成员与本人关系
        /// </summary>
        public string FamilyMembersAndMyself { set; get; }
        /// <summary>
        /// 家庭主要成员单位与职务
        /// </summary>
        public string FamilyMembersUnitAndPosition{ set; get; }
        /// <summary>
        /// 家庭主要成员姓名2
        /// </summary>
        public string NameOfFamilyMember1 { set; get; }

        /// <summary>
        /// 家庭主要成员与本人关系2
        /// </summary>
        public string FamilyMembersAndMyself1 { set; get; }
        /// <summary>
        /// 家庭主要成员单位与职务2
        /// </summary>
        public string FamilyMembersUnitAndPosition1 { set; get; }
        /// <summary>
        /// 家庭主要成员姓名3
        /// </summary>
        public string NameOfFamilyMember2 { set; get; }

        /// <summary>
        /// 家庭主要成员与本人关系3
        /// </summary>
        public string FamilyMembersAndMyself2{ set; get; }
        /// <summary>
        /// 家庭主要成员单位与职务3
        /// </summary>
        public string FamilyMembersUnitAndPosition2 { set; get; }
        /// <summary>
        /// 工作开始时间
        /// </summary>
        public string BeginningTimeOfWork { set; get; }
        /// <summary>
        /// 工作结束时间
        /// </summary>
        public string EndingTimeOfWork { set; get; }

        /// <summary>
        /// 工作单位
        /// </summary>
        public string WorkUnit { set; get; }
        /// <summary>
        /// 工作开始时间2
        /// </summary>
        public string BeginningTimeOfWork1 { set; get; }
        /// <summary>
        /// 工作结束时间2
        /// </summary>
        public string EndingTimeOfWork1 { set; get; }

        /// <summary>
        /// 工作单位2
        /// </summary>
        public string WorkUnit1 { set; get; }
        /// <summary>
        /// 工作开始时间3
        /// </summary>
        public string BeginningTimeOfWork2 { set; get; }
        /// <summary>
        /// 工作结束时间3
        /// </summary>
        public string EndingTimeOfWork2 { set; get; }

        /// <summary>
        /// 工作单位3
        /// </summary>
        public string WorkUnit2 { set; get; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { set; get; }

        /// <summary>
        /// 信息类型
        /// </summary>
        public string DataType { set; get; }
    }
}
