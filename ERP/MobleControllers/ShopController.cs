﻿using System;
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
        public ActionResult PayCartOrders()
        {
            #region 生成订单

            string ids = Request["ids"].TrimEnd(',');
            Business.Sys_Flower Sys_Flower = new Business.Sys_Flower();
            Business.Sys_OrdersManaage Sys_OrdersManaage = new Business.Sys_OrdersManaage();
            string[] idarry = ids.Split(new char[','],StringSplitOptions.RemoveEmptyEntries);
            string FlowerNum = Request["FlowerNums"];
            string address = Request["province"] + Request["city"] + Request["area"] + Request["ConsigneAaddress"];
            string ConsigneeName = Request["ConsigneeName"];
            string ConsigneePhone = Request["ConsigneePhone"];
            Model.Orders Orders = new Model.Orders();
            Orders.UsersId = Utility.ChangeText.GetUsersId();
            Orders.CreateTime = DateTime.Now; Orders.OrdersState = 1;
            Orders.OrderId = Utility.ChangeText.OrderIdCreate();
            Orders.GoodsSum = int.Parse(FlowerNum);            
            Orders.ConsigneeName = ConsigneeName;
            Orders.ConsigneePhone = ConsigneePhone;
            Orders.ConsigneAaddress = address;
            for (int j = 0; j < idarry.Length; j++)
            {
                Model.Flower Flower = Sys_Flower.GetFlower(idarry[j]);                
                Orders.SellingPrice += Flower.FlowerSalesPrice * int.Parse(FlowerNum);
                Orders.CostPrice += Flower.FlowerCostPrice * int.Parse(FlowerNum);
                List<Model.OrdersDetails> OrdersDetailsList = new List<Model.OrdersDetails>();
                for (int i = 0; i < int.Parse(FlowerNum); i++)
                {
                    Model.OrdersDetails OrdersDetails = new Model.OrdersDetails();
                    OrdersDetails.OrderId = Orders.OrderId;
                    OrdersDetails.FlowerNumber = Flower.FlowerNumber;
                    OrdersDetails.FlowerWatchName = Flower.FlowerWatchName;
                    OrdersDetails.FlowerWatchPhoto = Flower.FlowerWatchPhoto;
                    OrdersDetails.SellingPrice = Flower.FlowerSalesPrice;
                    OrdersDetails.SellingNum = i;
                    OrdersDetails.CostPrice = Flower.FlowerCostPrice;
                    OrdersDetailsList.Add(OrdersDetails);
                }
                Model.OrdersLog OrdersLog = new Model.OrdersLog();
                OrdersLog.OrdersId = Orders.OrderId;
                OrdersLog.OrdersState = 1;
                OrdersLog.UserName = Utility.ChangeText.GetUserName();
                OrdersLog.Remark = "";
                OrdersLog.Time = DateTime.Now;
            }
            //Sys_OrdersManaage.InsertOrders(Orders, OrdersDetailsList, OrdersLog);
            #endregion

            ViewData["OrdersId"] = Orders.OrderId;
            ViewData["PayTotal"] = 100;//Flower.FlowerSalesPrice * int.Parse(FlowerNum);
            //return Redirect("/WxPay/Index?OrdersId=" + Orders.OrderId + "&PayTotal=" + Flower.FlowerSalesPrice * int.Parse(OrdersNum));
            return View();
        }
        [HttpGet]
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
                OrdersDetails.SellingNum = i;
                OrdersDetails.CostPrice = Flower.FlowerCostPrice;
                OrdersDetailsList.Add(OrdersDetails);
            }
            Model.OrdersLog OrdersLog = new Model.OrdersLog();
            OrdersLog.OrdersId = Orders.OrderId;
            OrdersLog.OrdersState = 1;
            OrdersLog.UserName = Utility.ChangeText.GetUserName();
            OrdersLog.Remark = "";
            OrdersLog.Time = DateTime.Now;

            Business.Sys_OrdersManaage Sys_OrdersManaage = new Business.Sys_OrdersManaage();
            Sys_OrdersManaage.InsertOrders(Orders, OrdersDetailsList, OrdersLog);
            #endregion

            ViewData["OrdersId"] = Orders.OrderId;
            ViewData["PayTotal"] = Flower.FlowerSalesPrice * int.Parse(FlowerNum);
            //return Redirect("/WxPay/Index?OrdersId=" + Orders.OrderId + "&PayTotal=" + Flower.FlowerSalesPrice * int.Parse(OrdersNum));
            return View();
        }
        public ActionResult PayOrdersNow() 
        {
            return Redirect("/WxPay/Index?OrdersId=" + Request["OrdersId"] + "&PayTotal=" + Request["PayTotal"]);
        }

        public ActionResult AddInfo() 
        {
            string ConsigneAaddress = Request["DetailedAddress"];
            string ConsigneeName = Request["ConsigneeName"];
            string ConsigneePhone = Request["ConsigneePhone"];
            string OrdersId = Request["OrdersId"];

            Business.Sys_OrdersManaage Sys_OrdersManaage = new Business.Sys_OrdersManaage();
            Sys_OrdersManaage.AddInfo(OrdersId, ConsigneAaddress, ConsigneeName, ConsigneePhone);
            return null;
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