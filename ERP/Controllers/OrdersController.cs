using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ERP.Controllers
{
    public class OrdersController : LoginFilter
    {
        //
        // GET: /Orders/
        public ActionResult Index()
        {
            return View();
        }

        ///<summary>
        ///分页获得数据信息
        ///</summary>
        ///<param name="limit">页码大小</param>
        ///<param name="offset">从第几条数据开始</param>
        ///<param name="UserName">用户名</param>
        ///<param name="Role">角色ID</param>
        ///<returns></returns>
        public JsonResult GetOrdersList(int limit, int offset, string OrdersId)
        {
            Business.Sys_OrdersManaage Sys_OrdersManaage = new Business.Sys_OrdersManaage();
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(OrdersId))
            {
                sb.Append(" and OrderId='" + OrdersId + "'");
            }
            return Json(new { total = Sys_OrdersManaage.GetOrdersCount(sb.ToString()), rows = Sys_OrdersManaage.GetOrdersList(limit, offset, sb.ToString()) }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult SetOrderState()
        {
            ViewData["OrdersId"] = Request["OrdersId"];
            return View();
        }


        public ActionResult SetOrderStateInfo()
        {
            string OrderState = Request["OrdersState"];
            string OrdersId = Request["OrdersId"];
            Business.Sys_OrdersManaage Sys_OrdersManaage = new Business.Sys_OrdersManaage();
            Sys_OrdersManaage.SetOrderState(OrdersId, OrderState);
            Model.OrdersLog OrdersLog = new Model.OrdersLog()
            {
                OrdersId = OrdersId,
                OrdersState = Convert.ToInt32(OrderState),
                Time = DateTime.Now,
                UserName = Utility.ChangeText.GetUserName(),
            };
            Sys_OrdersManaage.InsertOrderLog(OrdersLog);
            return Content("1");
        }


        public ActionResult fahuo() 
        {
            ViewData["fahuoOrdersId"] = Request["OrdersId"];
            return View();
        }


        public ActionResult SetFaHuo()
        {
            Business.Sys_OrdersManaage Sys_OrdersManaage = new Business.Sys_OrdersManaage();
            if (Sys_OrdersManaage.OrderDelivery(Request["OrdersId"], 3, Request["kuaidigongsi"], Request["kuaidibianhao"], Utility.ChangeText.GetUserName()))
            {
               return Content("1");
            }
            return Content("0");
        }


        public ActionResult OrdersInfoMation() 
        {
            string OrdersId = Request["OrdersId"];
            Business.Sys_OrdersManaage Sys_OrdersManaage = new Business.Sys_OrdersManaage();
            List<Model.Orders> OrdersList = new List<Model.Orders>();
            OrdersList.Add(Sys_OrdersManaage.GetOrdersInfoByOrdersId(OrdersId));
            ViewData["OrdersInfo"] = OrdersList;

            ViewData["OrdersLog"] = Sys_OrdersManaage.GetOrdersLogByOrdersId(OrdersId);

        

            return View();
        }

        public JsonResult GetOrdersDetailsList(int limit, int offset)
        {
            string OrdersId = Request["OrdersId"];
            Business.Sys_OrdersManaage Sys_OrdersManaage = new Business.Sys_OrdersManaage();
            List<Model.OrdersDetails> OrdersDetailsList = Sys_OrdersManaage.GetOrdersDetailsByOrdersId(OrdersId);
            return Json(new { total = OrdersDetailsList.Count(), rows = OrdersDetailsList }, JsonRequestBehavior.AllowGet);
        }
    }
}