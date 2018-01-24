using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Factory
{
    /// 利用接口实现多种数据库类型的灵活更换
    public interface IDataBase
    {
        //查询数据返回实体对象
        List<T> Query<T>(string Server, string strSql, DynamicParameters Parameters);
       
        //多表联合查询数据
        IEnumerable<dynamic> QueryTable(string Server, string strSql, DynamicParameters Parameters);

        //增删改数据
        bool ExecSQL(string Server, string strSql, DynamicParameters Parameters);
        //增删改数据返回对应主键
        int ExecSqlGetId(string Server, string strSql, DynamicParameters Parameters);
        //Server实现批量插入数据
        void BulkToDB(string Server, string TableName, DataTable dt);
        //批量执行SQL语句
        int ExecuteSQLBatch(string Server, string strSql, object list);
        // 事务执行SQL
        bool ExecTranSQL(string Server, string StrSQL, DynamicParameters Parameters);

        //执行无返回值存储过程 默认超时5000
        bool ExecProduceSQL(string Server, string ProduceName, DynamicParameters Parameters, int? CommondTimeOut = 5000);
        
        //执行存储过程PageList分页 默认超时100000
        List<T> ExecProduceSQL<T>(string Server, string SQL, string Sort, int PageSize, int PageIndex,string Where, int? CommondTimeOut = 100000);
    }
}
