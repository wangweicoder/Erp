using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 统一显示员工信息类
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string Number { set; get; }

        /// <summary>
        /// 员工姓名
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { set; get; }

        /// <summary>
        /// 资料类型
        /// </summary>
        public string DataType { set; get; }

        public int ID { set; get; }
    }
}
