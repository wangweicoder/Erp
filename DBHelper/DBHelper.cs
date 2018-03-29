using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Factory
{
    /// <summary>
    /// 所有数据库操作的统一入口
    /// </summary>
    public static class DBHelper
    {
        //配置默认数据库类型
        private static readonly string dataAccessType = System.Configuration.ConfigurationManager.AppSettings["DataAccessType"];
        private static IDataBase _obj;
        private static IDataBase Obj
        {
            get
            {
                if (_obj == null)
                {
                    //配置数据库类型 调用底层类
                    string m_className = "Factory." + dataAccessType;
                    _obj = (IDataBase)Assembly.Load("DBHelper").CreateInstance(m_className);

                    //MSSQL
                    //_obj = new SqlHelper();
                }
                return _obj;
            }
        }

       
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Server"></param>
        /// <param name="strSql"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public static List<T> Query<T>(string Server, string strSql, DynamicParameters Parameters)
        {
            return Obj.Query<T>(Server, strSql, Parameters);
        }

        //查询返回多表数据
        public static IEnumerable<dynamic> QueryTable(string Server, string strSql, DynamicParameters Parameters)
        {
            return Obj.QueryTable(Server, strSql, Parameters);
        }
       
       
        /// <summary>
        ///  Server实现数据库读写分离 拆分库
        /// </summary>
        /// <param name="Server"></param>
        /// <param name="strSql"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public static bool ExecSQL(string Server, string strSql, DynamicParameters Parameters)
        {
            return Obj.ExecSQL(Server, strSql, Parameters);
        }
        //Server实现数据库读写分离 拆分库
        public static int ExecSqlGetId(string Server, string strSql, DynamicParameters Parameters)
        {
            return Obj.ExecSqlGetId(Server, strSql, Parameters);
        }
        //Server实现批量插入数据
        public static void BulkToDB(string Server, string TableName, DataTable dt)
        {
            Obj.BulkToDB(Server, TableName, dt);
        }
        /// <summary>
        /// 批量执行SQL 语句
        /// </summary>
        /// <param name="Server"></param>
        /// <param name="strSql"></param>
        /// <param name="list">List<Class>  集合类别</param>
        /// <returns></returns>
        public static int ExecuteSQLBatch(string Server, string strSql, object list)
        {
            return Obj.ExecuteSQLBatch(Server, strSql, list);
        }
        
        /// <summary>
        ///  事务执行SQL
        /// </summary>
        /// <param name="Server"></param>
        /// <param name="StrSQL"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public static bool ExecTranSQL(string Server, string StrSQL, DynamicParameters Parameters)
        {
            return Obj.ExecTranSQL(Server, StrSQL, Parameters);
        }

        //执行无返回值存储过程 默认超时5000
        public static bool ExecProduceSQL(string Server, string ProduceName, DynamicParameters Parameters, int? CommondTimeOut = 5000)
        {
            return Obj.ExecProduceSQL(Server, ProduceName, Parameters, CommondTimeOut);
        }

        //执行存储过程PageList分页 默认超时100000
        public static List<T> ExecProduceSQL<T>(string Server, string SQL, string Sort, int PageSize, int PageIndex, string Where, int? CommondTimeOut = 100000)
        {
            return Obj.ExecProduceSQL<T>(Server, SQL, Sort, PageSize, PageIndex,Where, CommondTimeOut);
        }
    }
}