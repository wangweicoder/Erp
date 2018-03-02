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
            string ids = Request["ids"].TrimEnd(',');
            ViewData["ids"] = ids;
            //ViewData["PayTotal"] = Orders.SellingPrice;
            ViewData["msg"] = " 提示：请在30分钟内完成在线支付，逾期将视为订单无效";           
            Business.Sys_FlowerShopCart Sys_OrdersManaage = new Business.Sys_FlowerShopCart();
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(userid))
            {
                sb.Append(" and UsersId='" + userid + "'");
            }
            sb.Append(" and  b.FlowerId in("+ids+")");
            List<Model.FlowerCartVM> list = Sys_OrdersManaage.FlowerShopCartList(sb.ToString());
            Model.CartLine model = new Model.CartLine();
            model.Products = list;
            return View(model);
            //return Redirect("/WxPay/Index?OrdersId=" + Orders.OrderId + "&PayTotal=" + Flower.FlowerSalesPrice * int.Parse(OrdersNum));
            
        }
        [HttpPost]
        public ActionResult PayOrders(string id)
        {
            #region 生成订单
            ////string id = Request["id"];
            //Business.Sys_Flower Sys_Flower = new Business.Sys_Flower();
            //Model.Flower Flower = Sys_Flower.GetFlower(id);
            //string FlowerNum = Request["FlowerNum"];
            //string address = Request["province"] + Request["city"] + Request["area"] + Request["ConsigneAaddress"];
            //string ConsigneeName = Request["ConsigneeName"];
            //string ConsigneePhone = Request["ConsigneePhone"];
            //Model.Orders Orders = new Model.Orders();
            //Orders.UsersId = Utility.ChangeText.GetUsersId();
            //Orders.CreateTime = DateTime.Now;
            //Orders.SellingPrice = Flower.FlowerSalesPrice * int.Parse(FlowerNum);
            //Orders.OrdersState = 1;
            //Orders.OrderId = Utility.ChangeText.OrderIdCreate();
            //Orders.GoodsSum = int.Parse(FlowerNum);
            //Orders.CostPrice = Flower.FlowerCostPrice * int.Parse(FlowerNum);
            //Orders.ConsigneeName = ConsigneeName;
            //Orders.ConsigneePhone = ConsigneePhone;
            //Orders.ConsigneAaddress = address;
            //List<Model.OrdersDetails> OrdersDetailsList = new List<Model.OrdersDetails>();
            //for (int i = 0; i < int.Parse(FlowerNum); i++)
            //{
            //    Model.OrdersDetails OrdersDetails = new Model.OrdersDetails();
            //    OrdersDetails.OrderId = Orders.OrderId;
            //    OrdersDetails.FlowerNumber = Flower.FlowerNumber;
            //    OrdersDetails.FlowerWatchName = Flower.FlowerWatchName;
            //    OrdersDetails.FlowerWatchPhoto = Flower.FlowerWatchPhoto;
            //    OrdersDetails.SellingPrice = Flower.FlowerSalesPrice;
            //    OrdersDetails.SellingNum = 1;//从原来的FlowerNum修改成1
            //    OrdersDetails.CostPrice = Flower.FlowerCostPrice;
            //    OrdersDetailsList.Add(OrdersDetails);
            //}
            //Model.OrdersLog OrdersLog = new Model.OrdersLog();
            //OrdersLog.OrdersId = Orders.OrderId;
            //OrdersLog.OrdersState = 1;
            //OrdersLog.UserName = Utility.ChangeText.GetUserName();
            //OrdersLog.Remark = "商品详情页中下单";
            //OrdersLog.Time = DateTime.Now;

            //Business.Sys_OrdersManaage Sys_OrdersManaage = new Business.Sys_OrdersManaage();
            //Sys_OrdersManaage.InsertOrders(Orders, OrdersDetailsList, OrdersLog);
           #endregion           
            ViewData["ids"] = id;
            ViewData["msg"] = " 提示：请在30分钟内完成在线支付，逾期将视为订单无效"; 
            Business.Sys_Flower Sys_Flower = new Business.Sys_Flower();
            Model.Flower flower= Sys_Flower.GetFlower(id);
            Model.FlowerCartVM fc=new Model.FlowerCartVM();
            fc.Id=0;
            fc.FlowerId=flower.id.ToString();
            fc.Num = int.Parse(Request["FlowerNum"]);
            fc.FlowerSalesPrice=flower.FlowerSalesPrice;
            fc.FlowerWatchPhoto = flower.FlowerWatchPhoto;
            List<Model.FlowerCartVM> list = new List<Model.FlowerCartVM> { fc};
            Model.CartLine model = new Model.CartLine();
            model.Products = list;
            return View(model);            
        }
        public ActionResult PayOrdersNow(string Remark)
        {
            #region 生成订单
            string ids = Request["ids"].TrimEnd(',');
            Business.Sys_Flower Sys_Flower = new Business.Sys_Flower();
            Business.Sys_OrdersManaage Sys_OrdersManaage = new Business.Sys_OrdersManaage();
            string[] idarry = ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string FlowerNum = Request["FlowerNums"];
            string[] FlowerNumarry = FlowerNum.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            
            string ConsigneAaddress =  Request["DetailedAddress"];
            string ConsigneeName = Request["ConsigneeName"];
            string ConsigneePhone = Request["ConsigneePhone"];
            //唯一的订单
            Model.Orders Orders = new Model.Orders();
            Orders.UsersId = int.Parse(userid);
            Orders.CreateTime = DateTime.Now;
            Orders.OrdersState = 1;
            Orders.OrderId = Utility.ChangeText.OrderIdCreate();
            Orders.GoodsSum = int.Parse(Request["goodnum"]);// 总件数
            Orders.ConsigneeName = ConsigneeName;
            Orders.ConsigneePhone = ConsigneePhone;
            Orders.ConsigneAaddress = ConsigneAaddress;
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
            OrdersLog.Remark = Remark;
            OrdersLog.Time = DateTime.Now;

            #endregion
            Sys_OrdersManaage.TransactionAddOrders(Orders, OrdersDetailsList, OrdersLog);
            //
            return Redirect("/WxPay/Index?OrdersId=" + Orders.OrderId + "&PayTotal=" + Orders.SellingPrice);
          
        }

        public void AddInfo()
        {
            string address = Request["province"] + Request["city"] + Request["area"];
            string ConsigneAaddress = address + Request["DetailedAddress"];
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
            List<Model.FlowerCartVM> list = Sys_OrdersManaage.FlowerShopCartList(sb.ToString());
            Model.CartLine model = new Model.CartLine();
            model.Products = list;
            return View(model);
        }

    }
}