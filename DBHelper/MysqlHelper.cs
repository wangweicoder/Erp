using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using System.Data;
using MySql.Data.MySqlClient;

namespace Factory
{
    class MysqlHelper : IDataBase
    {
        //Server查询数据
        public List<T> Query<T>(string Server, string strSql, DynamicParameters Parameters)
        {
            using (IDbConnection conn = new MySqlConnection(Server))
            {
                return SqlMapper.Query<T>(conn, strSql, Parameters).AsList();
            }
        }

        //Server多表查询数据
        public IEnumerable<dynamic> QueryTable(string Server, string strSql, DynamicParameters Parameters)
        {
            using (IDbConnection conn = new MySqlConnection(Server))
            {
                return conn.Query(strSql, Parameters).ToList();
            }
        }

        //Server实现数据库读写分离 拆分库
        public bool ExecSQL(string Server, string strSql, DynamicParameters Parameters)
        {
            //Server写 获取一个数据库连接
            using (IDbConnection connection = new MySqlConnection(Server))
            {
                connection.Open();
                if (connection.Execute(strSql, Parameters) > 0)
                {
                    return true;
                }
                return false;
            }
        }
        /// <summary>
        /// 增删改且返回对应ID
        /// </summary>
        public int ExecSqlGetId(string Server, string strSql, DynamicParameters Parameters)
        {
            //执行SQL语句公共方法
            using (IDbConnection connection = new MySqlConnection(Server))
            {
                connection.Open();
                return connection.Execute(strSql, Parameters);
            }
        }
        /// <summary>
        /// 批量执行SQL 语句
        /// </summary>
        /// <param name="Server"></param>
        /// <param name="strSql"></param>
        /// <param name="list">List<Class>  集合类别</param>
        /// <returns></returns>
        public int ExecuteSQLBatch(string Server, string strSql, object list)
        {
            //执行SQL语句公共方法
            using (IDbConnection connection = new MySqlConnection(Server))
            {
                connection.Open();
                return connection.Execute(strSql, list);
            }
        }
        //Server实现批量插入数据
        public void BulkToDB(string Server, string TableName, DataTable dt)
        {

        }
        // 事务执行SQL
        public bool ExecTranSQL(string Server, string StrSQL, DynamicParameters Parameters)
        {
            using (IDbConnection dbConnection = new MySqlConnection(Server))
            {
                dbConnection.Open();
                IDbTransaction transaction = dbConnection.BeginTransaction();
                try
                {
                    dbConnection.Execute(StrSQL, Parameters, transaction);
                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

        //执行无返回值存储过程 默认超时5000
        public bool ExecProduceSQL(string Server, string ProduceName, DynamicParameters Parameters, int? CommondTimeOut = 5000)
        {
            using (IDbConnection con = new MySqlConnection(Server))
            {
                if (con.Execute(ProduceName, Parameters, null, CommondTimeOut, CommandType.StoredProcedure) > 0) { return true; }
                else { return false; }
            }
        }

        //执行存储过程PageList分页 默认超时100000
        public List<T> ExecProduceSQL<T>(string Server, string SQL, string Sort, int PageSize, int PageIndex, string Where, int? CommondTimeOut = 100000)
        {
            using (IDbConnection con = new MySqlConnection(Server))
            {
                DynamicParameters Parameters = new DynamicParameters();
                Parameters.Add("@Sql", SQL);
                Parameters.Add("@Sort", Sort);
                Parameters.Add("@PageIndex", PageIndex);
                Parameters.Add("@PageSize", PageSize);
                Parameters.Add("@Where", Where);
                return con.Query<T>("PageList", Parameters, null, true, CommondTimeOut, CommandType.StoredProcedure).ToList();//执行分页存储过程
            }
        }
    }
}
