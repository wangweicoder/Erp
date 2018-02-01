using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ERP.MobleControllers
{
    public class ShopController : Controller
    {
        //
        // GET: /Shop/
        public ActionResult Index()
        {
            Business.Sys_Flower Sys_Flower = new Business.Sys_Flower();

            return View(Sys_Flower.GetFlowerList());
        }

        string userid = Utility.ChangeText.GetUsersId().ToString();

        public ActionResult Details() 
        {
            string id = Request["id"];
            Business.Sys_Flower Sys_Flower = new Business.Sys_Flower();

            return View(Sys_Flower.GetFlower(id));
        }

        [HttpGet]
        public ActionResult PayOrders(string goodnum, string FlowerNums)
        {
            var cache = HttpRuntime.Cache.Get(userid+"ids")as Model.Orders;
            if (cache == null)
            {
                #region 生成订单
                string ids = Request["ids"].TrimEnd(',');
                Business.Sys_Flower Sys_Flower = new Business.Sys_Flower();
                Business.Sys_OrdersManaage Sys_OrdersManaage = new Business.Sys_OrdersManaage();
                string[] idarry = ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string FlowerNum = Request["FlowerNums"];
                string[] FlowerNumarry = FlowerNum.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string address = Request["province"] + Request["city"] + Request["area"] + Request["ConsigneAaddress"];
                string ConsigneeName = "";
                string ConsigneePhone = "";
                //唯一的订单
                Model.Orders Orders = new Model.Orders();
                Orders.UsersId = int.Parse(userid);
                Orders.CreateTime = DateTime.Now;
                Orders.OrdersState = 1;
                Orders.OrderId = Utility.ChangeText.OrderIdCreate();
                Orders.GoodsSum = int.Parse(Request["goodnum"]);// 总件数
                Orders.ConsigneeName = ConsigneeName;
                Orders.ConsigneePhone = ConsigneePhone;
                Orders.ConsigneAaddress = address;
                //详情列表
                List<Model.OrdersDetails> OrdersDetailsList = new List<Model.OrdersDetails>();
                for (int j = 0; j < idarry.Length; j++)
                {
                    //每一种花
                    Model.Flower Flower = Sys_Flower.GetFlower(idarry[j]);
                    Orders.SellingPrice += Flower.FlowerSalesPrice * int.Parse(FlowerNumarry[j]);
                    Orders.CostPrice += Flower.FlowerCostPrice * int.Parse(FlowerNumarry[j]);
                    //每种花对应的数量
                    for (int i = 0; i < int.Parse(FlowerNumarry[j]); i++)
                    {
                        Model.OrdersDetails OrdersDetails = new Model.OrdersDetails();
                        OrdersDetails.OrderId = Orders.OrderId;
                        OrdersDetails.FlowerNumber = Flower.FlowerNumber;
                        OrdersDetails.FlowerWatchName = Flower.FlowerWatchName;
                        OrdersDetails.FlowerWatchPhoto = Flower.FlowerWatchPhoto;
                        OrdersDetails.SellingPrice = Flower.FlowerSalesPrice;
                        OrdersDetails.SellingNum = 1;
                        OrdersDetails.CostPrice = Flower.FlowerCostPrice;
                        OrdersDetailsList.Add(OrdersDetails);
                    }

                }
                Model.OrdersLog OrdersLog = new Model.OrdersLog();
                OrdersLog.OrdersId = Orders.OrderId;
                OrdersLog.OrdersState = 1;
                OrdersLog.UserName = Utility.ChangeText.GetUserName();
                OrdersLog.Remark = "购物车中下单";
                OrdersLog.Time = DateTime.Now;
                HttpRuntime.Cache.Add(userid + "ids", Orders, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(5), System.Web.Caching.CacheItemPriority.Normal, null);
                HttpRuntime.Cache.Add(userid + "OrdersDetailsList", OrdersDetailsList, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(5), System.Web.Caching.CacheItemPriority.Normal, null);
                HttpRuntime.Cache.Add(userid + "OrdersLog", OrdersLog, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(5), System.Web.Caching.CacheItemPriority.Normal, null);

                #endregion
                //Sys_OrdersManaage.TransactionAddOrders(Orders, OrdersDetailsList, OrdersLog);
                ViewData["OrdersId"] = Orders.OrderId;
                ViewData["PayTotal"] = Orders.SellingPrice;
                ViewData["msg"] = " 提示：请在30分钟内完成在线支付，逾期将视为订单无效";
            }
            else {
                ViewData["OrdersId"] = cache.OrderId;
                ViewData["PayTotal"] = cache.SellingPrice;
                ViewData["msg"] = " 提示：您有订单未支付，请在30分钟内完成在线支付，逾期将视为订单无效";
            }
            //return Redirect("/WxPay/Index?OrdersId=" + Orders.OrderId + "&PayTotal=" + Flower.FlowerSalesPrice * int.Parse(OrdersNum));
            return View();
        }
        [HttpPost]
        public ActionResult PayOrders(string id)
        {
            #region 生成订单
            //string id = Request["id"];
            Business.Sys_Flower Sys_Flower = new Business.Sys_Flower();
            Model.Flower Flower = Sys_Flower.GetFlower(id);
            string FlowerNum = Request["FlowerNum"];
            string address = Request["province"] + Request["city"] + Request["area"] + Request["ConsigneAaddress"];
            string ConsigneeName = Request["ConsigneeName"];
            string ConsigneePhone = Request["ConsigneePhone"];
            Model.Orders Orders = new Model.Orders();
            Orders.UsersId = Utility.ChangeText.GetUsersId();
            Orders.CreateTime = DateTime.Now;
            Orders.SellingPrice = Flower.FlowerSalesPrice * int.Parse(FlowerNum);
            Orders.OrdersState = 1;
            Orders.OrderId = Utility.ChangeText.OrderIdCreate();
            Orders.GoodsSum = int.Parse(FlowerNum);
            Orders.CostPrice = Flower.FlowerCostPrice * int.Parse(FlowerNum);
            Orders.ConsigneeName = ConsigneeName;
            Orders.ConsigneePhone = ConsigneePhone;
            Orders.ConsigneAaddress = address;
            List<Model.OrdersDetails> OrdersDetailsList = new List<Model.OrdersDetails>();
            for (int i = 0; i < int.Parse(FlowerNum); i++)
            {
                Model.OrdersDetails OrdersDetails = new Model.OrdersDetails();
                OrdersDetails.OrderId = Orders.OrderId;
                OrdersDetails.FlowerNumber = Flower.FlowerNumber;
                OrdersDetails.FlowerWatchName = Flower.FlowerWatchName;
                OrdersDetails.FlowerWatchPhoto = Flower.FlowerWatchPhoto;
                OrdersDetails.SellingPrice = Flower.FlowerSalesPrice;
                OrdersDetails.SellingNum = 1;//从原来的FlowerNum修改成1
                OrdersDetails.CostPrice = Flower.FlowerCostPrice;
                OrdersDetailsList.Add(OrdersDetails);
            }
            Model.OrdersLog OrdersLog = new Model.OrdersLog();
            OrdersLog.OrdersId = Orders.OrderId;
            OrdersLog.OrdersState = 1;
            OrdersLog.UserName = Utility.ChangeText.GetUserName();
            OrdersLog.Remark = "商品详情页中下单";
            OrdersLog.Time = DateTime.Now;

            Business.Sys_OrdersManaage Sys_OrdersManaage = new Business.Sys_OrdersManaage();
            Sys_OrdersManaage.InsertOrders(Orders, OrdersDetailsList, OrdersLog);
            #endregion

            //ViewData["OrdersId"] = Orders.OrderId;
            //ViewData["PayTotal"] = Orders.SellingPrice;
            return Redirect("/WxPay/Index?OrdersId=" + Orders.OrderId + "&PayTotal=" + Flower.FlowerSalesPrice * int.Parse(FlowerNum));
            //return View();
        }
        public ActionResult PayOrdersNow() 
        {
            Model.Orders Orders = HttpRuntime.Cache.Get(userid + "ids") as Model.Orders;
            if (Orders != null)
            {                
                 var OrdersDetailsList = HttpRuntime.Cache.Get(userid + "OrdersDetailsList") as List<Model.OrdersDetails>;
                 Model.OrdersLog OrdersLog = HttpRuntime.Cache.Get(userid + "OrdersLog") as Model.OrdersLog;
                 Business.Sys_OrdersManaage Sys_OrdersManaage = new Business.Sys_OrdersManaage();
                 Sys_OrdersManaage.TransactionAddOrders(Orders, OrdersDetailsList, OrdersLog);
                 AddInfo();
                 return Redirect("/WxPay/Index?OrdersId=" + Request["OrdersId"] + "&PayTotal=" + Request["PayTotal"]);
             }
             else {
                 return Content("");
             }
        }

        public void AddInfo()
        {
            string address = Request["province"] + Request["city"] + Request["area"];
            string ConsigneAaddress = address+Request["DetailedAddress"];
            string ConsigneeName = Request["ConsigneeName"];
            string ConsigneePhone = Request["ConsigneePhone"];
            string OrdersId = Request["OrdersId"];

            Business.Sys_OrdersManaage Sys_OrdersManaage = new Business.Sys_OrdersManaage();
            Sys_OrdersManaage.AddInfo(OrdersId, ConsigneAaddress, ConsigneeName, ConsigneePhone);
            
        }

        /// <summary>
        /// 购物车
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCartList()
        {
            Business.Sys_FlowerShopCart Sys_OrdersManaage = new Business.Sys_FlowerShopCart();
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(userid))
            {
                sb.Append(" and UsersId='" + userid + "'");
            }
           List<Model.FlowerCartVM>list= Sys_OrdersManaage.FlowerShopCartList(sb.ToString());
           Model.CartLine model = new Model.CartLine();
           model.Products = list;
           return View(model);
        }
        
	}
}