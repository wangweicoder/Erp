using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ERP.MobleControllers
{
    public class MOrdersController : MLoginFilterController
    {
        //
        // GET: /MOrders/
        public ActionResult MIndex()
        {
           List<Model.Orders> model = new List<Model.Orders>();          
           string UsersId = Utility.ChangeText.GetUsersId().ToString();
           model = OrdersList(0, 1, UsersId,"");           
           return View(model);
        }
        /// <summary>
        /// 订单
        /// </summary>
        /// <returns></returns>
        public List<Model.Orders> OrdersList(int limit, int offset, string UsersId,string OrderState) 
        {
            Business.Sys_OrdersManaage Sys_OrdersManaage = new Business.Sys_OrdersManaage();
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(UsersId))
            {
                sb.Append(" and UsersId='" + UsersId + "'");
            }
            if (!string.IsNullOrEmpty(OrderState))
            {
                if(OrderState =="1")
                sb.Append(" and OrdersState='" + OrderState + "'");
                else if(OrderState != "1")
                sb.Append(" and OrdersState!='1'");
            }
            return  Sys_OrdersManaage.GetOrdersList(limit, offset, sb.ToString());
   
        }
        /// <summary>
        /// 更多订单
        /// </summary>
        /// <returns></returns>
        public ActionResult GetMobleListMore()
        {
            Business.Sys_OrdersManaage Sys_OrdersManaage = new Business.Sys_OrdersManaage();
            StringBuilder sb = new StringBuilder();
            string UsersId = Utility.ChangeText.GetUsersId().ToString();
            if (!string.IsNullOrEmpty(UsersId))
            {
                sb.Append(" and UsersId='" + UsersId + "'");
            }
            string OrderState = Request["OrderState"];
            if (!string.IsNullOrEmpty(OrderState))
            {
                if (OrderState == "1")
                    sb.Append(" and OrdersState='" + OrderState + "'");
                else if (OrderState != "1")
                    sb.Append(" and OrdersState!='1'");
            }
            int page = int.Parse(Request["page"]);
            if (page > 1)
            {
                page = (page - 1) * 10 + 1;
            }
            List<Model.Orders> List = Sys_OrdersManaage.GetOrdersList(0, page, sb.ToString()); ;
            return Json(List, JsonRequestBehavior.AllowGet);
            //return Content(JsonConvert.SerializeObject(List));
        }
        public ActionResult OrderDetails() 
        {
            string OrdersId = Request["OrdersId"];
            Business.Sys_OrdersManaage Sys_OrdersManaage = new Business.Sys_OrdersManaage();
            List<Model.Orders> OrdersList = new List<Model.Orders>();
            OrdersList.Add(Sys_OrdersManaage.GetOrdersInfoByOrdersId(OrdersId));
            ViewData["OrdersInfo"] = OrdersList;
            ViewData["OrdersLog"] = Sys_OrdersManaage.GetOrdersLogByOrdersId(OrdersId);
            return View();
        }
        /// <summary>
        /// 获得订单详情
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public JsonResult GetOrdersDetailsList()
        {
            string OrdersId = Request["OrdersId"];
            Business.Sys_OrdersManaage Sys_OrdersManaage = new Business.Sys_OrdersManaage();
            List<Model.OrdersDetails> OrdersDetailsList = Sys_OrdersManaage.GetOrdersDetailsByOrdersId(OrdersId);
            return Json(OrdersDetailsList,JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 创建订单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateOrders() 
        {
            string GoodsId = Request["GoodsId"];
            Business.Sys_Flower Sys_Flower = new Business.Sys_Flower();
            Model.Flower Flower = Sys_Flower.GetFlowerByFlowerNumber(GoodsId);
            //判断库存是否足够商品
            if (Flower.FlowerStock<Convert.ToInt32(Request["FlowerStock"]))
            {
                ViewData["success"] = "抱歉,库存不足";
                return View();
            }
            Model.Orders Orders = new Model.Orders();
            Orders.OrderId = Utility.ChangeText.GenerateOutTradeNo();
            Orders.OrdersState = 1;
            Orders.SellingPrice=Flower.FlowerSalesPrice*Convert.ToInt32(Request["FlowerStock"]);
            Orders.CostPrice=Flower.FlowerCostPrice*Convert.ToInt32(Request["FlowerStock"]);
            Orders.ConsigneAaddress=Request["ConsigneAaddress"];
            Orders.ConsigneeName=Request["ConsigneeName"];
            Orders.GoodsSum=Convert.ToInt32(Request["FlowerStock"]);
            Orders.ConsigneePhone=Request["ConsigneePhone"];
            List<Model.OrdersDetails> OrdersDetailsList=new List<Model.OrdersDetails> ();
            for (int i = 0; i < Convert.ToInt32(Request["FlowerStock"]); i++)
			{
			   Model.OrdersDetails OrdersDetails=new Model.OrdersDetails ();
               OrdersDetails.OrderId=Orders.OrderId;
               OrdersDetails.SellingPrice=Flower.FlowerSalesPrice;
               OrdersDetails.FlowerNumber=Flower.FlowerNumber;
               OrdersDetails.CostPrice = Flower.FlowerCostPrice;
               OrdersDetails.FlowerWatchPhoto = Flower.FlowerWatchPhoto;
               OrdersDetails.SellingNum = 1;
               OrdersDetails.FlowerWatchName = Flower.FlowerWatchName;
               OrdersDetailsList.Add(OrdersDetails);
			}
            Model.OrdersLog OrdersLog = new Model.OrdersLog();
            OrdersLog.OrdersId = Orders.OrderId;
            OrdersLog.OrdersState = 1;
            OrdersLog.UserName = Utility.ChangeText.GetUserName();
            Business.Sys_OrdersManaage Sys_OrdersManaage = new Business.Sys_OrdersManaage();
            if (!Sys_OrdersManaage.InsertOrders(Orders, OrdersDetailsList, OrdersLog))
            {
                ViewData["success"] = "下单失败,请联系管理员";
                return View();
            }
            return RedirectToAction("","");
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <returns></returns>
        public ActionResult ClaceOrders() 
        {
            Business.Sys_OrdersManaage Sys_OrdersManaage = new Business.Sys_OrdersManaage();
            if (Sys_OrdersManaage.ClaceOrders(Request["OrdersId"],Utility.ChangeText.GetUserName(),5))
            {
                return Content("1");
            }
            return Content("2");
        }

        /// <summary>
        /// 支付订单
        /// </summary>
        /// <returns></returns>
        public ActionResult PayOrders()
        {
            string OrdersId = Request["OrdersId"];
            Business.Sys_OrdersManaage Sys_OrdersManaage = new Business.Sys_OrdersManaage();
            Sys_OrdersManaage.GetOrdersInfoByOrdersId(OrdersId);
            return View();
        }
	}
}