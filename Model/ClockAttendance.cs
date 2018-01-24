using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public  class ClockAttendance
    {
        public int id { set; get; }

        public int UsersId { set; get; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { set; get; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string Createtime { set; get; }

        /// <summary>
        /// 考勤地址
        /// </summary>
        public string Address { set; get; }

        /// <summary>
        /// 设定的考勤地址
        /// </summary>
        public string CheckAddress { set; get; }
    }
}
