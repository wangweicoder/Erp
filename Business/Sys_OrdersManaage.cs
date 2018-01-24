using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    /// <summary>
    /// 订单统一事务处理
    /// </summary>
    public class Sys_OrdersManaage
    {
        private string SQLConString = System.Configuration.ConfigurationManager.AppSettings["SQLConString"];

        /// <summary>
        /// 查询订单
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="StrWhere"></param>
        /// <returns></returns>
        public object GetOrdersList(int limit, int offset, string StrWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from (select ROW_NUMBER() over (order by Orders.id desc ) as rn ,OrderId,SellingPrice,CostPrice,profit,GoodsSum,CreateTime,UserAdmin.UserName,UserAdmin.RealName,OrdersState from Orders");
            strSql.Append(" inner join UserAdmin  on Orders.UsersId=UserAdmin.ID");
            if (!string.IsNullOrEmpty(StrWhere))
            {
                strSql.Append(" where  1=1 " + StrWhere);
            }
            strSql.Append(")T where t.rn between   @offset and (@offset+9)");
            return Factory.DBHelper.Query<Model.Orders>(SQLConString, strSql.ToString(), new DynamicParameters(new { offset }));
        
        }

        /// <summary>
        /// 查询订单
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="StrWhere"></param>
        /// <returns></returns>
        public int GetOrdersCount(string StrWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(ID) as id FROM Orders");
            if (!string.IsNullOrEmpty(StrWhere))
            {
                strSql.Append(" where  1=1 " + StrWhere);
            }
            List<Model.Orders> OrdersList = Factory.DBHelper.Query<Model.Orders>(SQLConString, strSql.ToString(), new DynamicParameters(new { StrWhere }));
            return OrdersList.Count() > 0 ? OrdersList[0].id : 0;
        }

        /// <summary>
        /// 通过订单号获得订单主表信息
        /// </summary>
        /// <param name="Ordersid"></param>
        /// <returns></returns>
        public Model.Orders GetOrdersInfoByOrdersId(string OrderId) 
        {
            const string sql =
@"SELECT * FROM  Orders WHERE OrderId=@OrderId";
            List<Model.Orders>  OrdersList= Factory.DBHelper.Query<Model.Orders>(SQLConString, sql.ToString(), new DynamicParameters(new { OrderId }));
            return OrdersList.Count() > 0 ? OrdersList[0] : null;
        }

        /// <summary>
        /// 通过订单号获得订单日志信息
        /// </summary>
        /// <param name="Ordersid"></param>
        /// <returns></returns>
        public List<Model.OrdersLog> GetOrdersLogByOrdersId(string OrderId)
        {
            const string sql =
@"SELECT * FROM  OrdersLog WHERE OrdersId=@OrderId";
            List<Model.OrdersLog> OrdersList = Factory.DBHelper.Query<Model.OrdersLog>(SQLConString, sql.ToString(), new DynamicParameters(new { OrderId }));
            return OrdersList;
        }



        /// <summary>
        /// 通过订单号获得订单日志信息
        /// </summary>
        /// <param name="Ordersid"></param>
        /// <returns></returns>
        public List<Model.OrdersDetails> GetOrdersDetailsByOrdersId(string OrderId)
        {
            const string sql =
@"SELECT * FROM  OrdersDetails WHERE OrderId=@OrderId";
            List<Model.OrdersDetails> OrdersDetails = Factory.DBHelper.Query<Model.OrdersDetails>(SQLConString, sql.ToString(), new DynamicParameters(new { OrderId }));
            return OrdersDetails;
        }


        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="OrdersId"></param>
        /// <param name="UserName"></param>
        /// <param name="OrdersState"></param>
        /// <returns></returns>
        public bool ClaceOrders(string OrdersId, string UserName, int OrdersState)
        {
            const string Orders =
  @"UPDATE Orders SET OrdersState=@OrdersState  WHERE OrderId=@OrdersId";
            Factory.DBHelper.ExecSQL(SQLConString, Orders.ToString(), new DynamicParameters(new
            {
                OrdersState,
                OrdersId,
            }));
            const string OrdersLog =
@"insert into OrdersLog(OrdersId,UserName,OrdersState)
        values(@OrdersId,@UserName,@OrdersState)";
            Factory.DBHelper.ExecSQL(SQLConString, OrdersLog.ToString(), new DynamicParameters(new
            {
                OrdersId,
                UserName,
                OrdersState
            }));
            return
               true;
        }


        public int GetPayOrdersSum(string OrdersId) 
        {
            const string sql =
@"SELECT SUM(OrdersDetails.SellingNum) as id FROM OrdersDetails   WHERE OrderId=@OrdersId ";
            List<Model.OrdersDetails> OrdersDetails = Factory.DBHelper.Query<Model.OrdersDetails>(SQLConString, sql.ToString(), new DynamicParameters(new { OrdersId }));
            return OrdersDetails.Count()>0?OrdersDetails[0].id:0;
        
        }

        public bool AddInfo(string OrdersID, string DetailedAddress, string ConsigneeName, string ConsigneePhone) 
        {
            const string sql =
@"UPDATE  ORDERS SET ConsigneeName=@ConsigneeName,ConsigneePhone=@ConsigneePhone,ConsigneAaddress=@DetailedAddress WHERE OrderId=@OrdersID";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                ConsigneeName,
                ConsigneePhone,
                DetailedAddress,
                OrdersID
            }));
        }

        /// <summary>
        /// 订单支付
        /// </summary>
        /// <param name="OrdersId"></param>
        /// <returns></returns>
        public bool OrdersPay(string OrdersId, string PayOrdersNum, string UserName, int OrdersState)
        {
            const string Orders =
@"UPDATE Orders SET OrdersState=2,PayTime=getdate(),profit=SellingPrice-CostPrice,PayOrdersNum=@PayOrdersNum  WHERE OrderId=@OrdersId";
            Factory.DBHelper.ExecSQL(SQLConString, Orders.ToString(), new DynamicParameters(new
            {
                OrdersId,
                PayOrdersNum
            }));
            const string OrdersDetails =
@"UPDATE OrdersDetails SET profit=SellingPrice-CostPrice  WHERE OrderId=@OrdersId";
            Factory.DBHelper.ExecSQL(SQLConString, OrdersDetails.ToString(), new DynamicParameters(new
            {
                OrdersId,
            }));
            const string OrdersLog =
@"insert into OrdersLog(OrdersId,UserName,OrdersState)
        values(@OrdersId,@UserName,@OrdersState)";
            Factory.DBHelper.ExecSQL(SQLConString, OrdersLog.ToString(), new DynamicParameters(new
            {
                OrdersId,
                UserName,
                OrdersState
            }));

            int OrdersInfoSum =  GetPayOrdersSum(OrdersId);
            string sql = @"UPDATE Flower set FlowerStock=FlowerStock-" + OrdersInfoSum + "  WHERE OrderId=@OrdersId ";
            Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                OrdersId,
            }));
            return true;
        }

        /// <summary>
        /// 订单发货
        /// </summary>
        /// <param name="OrdersId"></param>
        /// <param name="OrdersState"></param>
        /// <param name="LogisticsCompanyNumber"></param>
        /// <param name="LogisticsNumber"></param>
        /// <returns></returns>
        public bool OrderDelivery(string OrdersId, int OrdersState, string LogisticsCompanyNumber, string LogisticsNumber, string UserName)
        {
            const string orders =
@"UPDATE orders set OrdersState=@OrdersState,LogisticsCompanyNumber=@LogisticsCompanyNumber,
LogisticsNumber=@LogisticsNumber,OrderDelivery=getdate()  where OrderId=@OrdersId ";
            Factory.DBHelper.ExecSQL(SQLConString, orders.ToString(), new DynamicParameters(new
            {
                OrdersId,
                OrdersState,
                LogisticsCompanyNumber,
                LogisticsNumber
            }));
            const string OrdersLog =
@"insert into OrdersLog(OrdersId,UserName,OrdersState)
        values(@OrdersId,@UserName,@OrdersState)";
            Factory.DBHelper.ExecSQL(SQLConString, OrdersLog.ToString(), new DynamicParameters(new
            {
                OrdersId,
                UserName,
                OrdersState
            }));
            return true;
        }

        /// <summary>
        /// 修改订单状态
        /// </summary>
        /// <param name="OrdersId"></param>
        /// <param name="OrderState"></param>
        /// <returns></returns>
        public bool SetOrderState(string OrdersId,string OrderState) 
        {
            const string sql =
@"UPDATE  ORDERS SET OrdersState=@OrderState  WHERE OrderId=@OrdersId";
            return  Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                OrderState,
                OrdersId,
            }));
        }

        /// <summary>
        /// 记录订单日志
        /// </summary>
        /// <param name="OrdersLog"></param>
        /// <returns></returns>
        public bool InsertOrderLog(Model.OrdersLog OrdersLog) 
        {
            const string sql =
@"INSERT INTO OrdersLog(OrdersId,UserName,OrdersState,Remark) VALUES(@OrdersId,@UserName,@OrdersState,@Remark)";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
               OrdersLog.OrdersId,
               OrdersLog.UserName,
               OrdersLog.OrdersState,
               OrdersLog.Remark,
            }));
        }

        /// <summary>
        /// 事务下订单
        /// </summary>
        /// <returns></returns>
        public bool InsertOrders(Model.Orders orders, List<Model.OrdersDetails> ordersDetailList, Model.OrdersLog ordersLog)
        {
            const string sql =
@"insert into Orders (OrderId,SellingPrice,CostPrice,profit,UsersId,ConsigneeName,ConsigneePhone,ConsigneAaddress,OrdersState,GoodsSum) 
 values(@OrderId,@SellingPrice,@CostPrice,@profit,@UsersId,@ConsigneeName,@ConsigneePhone,@ConsigneAaddress,@OrdersState,@GoodsSum)";
            Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                orders.OrderId,
                orders.SellingPrice,
                orders.CostPrice,
                orders.profit,
                orders.UsersId,
                orders.ConsigneeName,
                orders.ConsigneePhone,
                orders.ConsigneAaddress,
                orders.OrdersState,
                orders.GoodsSum,
            }));

            const string OrdersLog =
@"insert into OrdersLog(OrdersId,UserName,OrdersState,Remark)  values(@OrdersId,@UserName,@OrdersState,@Remark)";
            Factory.DBHelper.ExecSQL(SQLConString, OrdersLog.ToString(), new DynamicParameters(new
            {
                ordersLog.OrdersId,
                ordersLog.UserName,
                ordersLog.OrdersState,
                ordersLog.Remark,
               
            }));
            foreach (Model.OrdersDetails item in ordersDetailList)
            {
                const string OrdersDetail = @"insert into OrdersDetails(OrderId,FlowerWatchName,FlowerWatchPhoto,FlowerNumber,
                                         SellingPrice,CostPrice,profit,SellingNum) 
                                         values(@OrderId,@FlowerWatchName,@FlowerWatchPhoto,@FlowerNumber,@SellingPrice,
                                         @CostPrice,@profit,@SellingNum)";
                Factory.DBHelper.ExecSQL(SQLConString, OrdersDetail.ToString(), new DynamicParameters(new
                {
                    item.OrderId,
                    item.FlowerWatchName,
                    item.FlowerWatchPhoto,
                    item.FlowerNumber,
                    item.SellingPrice,
                    item.CostPrice,
                    item.profit,
                    item.SellingNum,

                }));
            
            }
            return true;
        }



        /// <summary>
        /// 事务批量执行SQL语句
        /// </summary>
        /// <param name="SQLList">SQL集合</param>
        /// <param name="SqlParameterList">参数集合</param>
        /// <returns></returns>
        public static bool TransactionInsertOrders(Hashtable htb)
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["SQLConString"]))
            {
                conn.Open();
                //声明事务
                SqlTransaction tr = conn.BeginTransaction();
                SqlCommand comm = new SqlCommand();
                comm.Connection = conn;
                //指定给SqlCommand事务
                comm.Transaction = tr;
                try
                {
                    //遍历Hashtable数据，每次遍历执行SqlCommand
                    foreach (DictionaryEntry de in htb)
                    {
                        string cmdText = de.Key.ToString();
                        SqlParameter[] pars = (SqlParameter[])de.Value;

                        //指定执行语句
                        comm.CommandText = cmdText;

                        //有参数则进行添加
                        if (pars != null)
                        {
                            foreach (SqlParameter par in pars)
                            {
                                comm.Parameters.Add(par);
                            }
                        }
                        //执行
                        comm.ExecuteNonQuery();
                        //使用后清空参数，为下次使用
                        comm.Parameters.Clear();
                    }
                    //不出意外事务提前，返回True
                    tr.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    //出意外事务回滚，返回Fasle
                    tr.Rollback();
                    return false;
                }
            }
        }

        /// 执行存储过程
        /// </summary>
        /// <param name="ProcName">存储过程名字</param>
        /// <param name="cmdParms">参数对象</param>
        /// <returns></returns>
        public static SqlCommand ExecuteProc(string ProcName, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["SQLConString"]))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        connection.Open();
                        cmd.Connection = connection;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = ProcName;
                        cmd.Parameters.AddRange(cmdParms);
                        cmd.ExecuteNonQuery();
                        connection.Close();
                        return cmd;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }



    }

}
