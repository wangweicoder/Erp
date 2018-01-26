using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERP.MobleControllers
{
    public class BuyCartController : MLoginFilterController
    {
        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 添加购物车
        /// </summary>
        /// <param name="FlowerId"></param>
        /// <param name="Num"></param>
        /// <returns></returns>
        public ActionResult AddToCart(string FlowerId, int Num)
        {
            string userid = Utility.ChangeText.GetUsersId().ToString();
            Business.Sys_FlowerShopCart bus = new Business.Sys_FlowerShopCart();
            Model.FlowerShopCart Cart = bus.GetFlowerShopCart(FlowerId.ToString(),userid);
            if (Cart != null)
            {
                Cart.Num = Num;
                Cart.UpdateTime = DateTime.Now;
                
                bus.UpdateFlowerShopCart(Cart);//原来有这个商品，更新下数量
            }
            else {
                Model.FlowerShopCart model = new Model.FlowerShopCart();
                model.Num = Num;
                model.UsersId =userid;
                model.FlowerId = FlowerId;
                model.Status = 1;
                model.CreateTime = DateTime.Now;
                model.UpdateTime = DateTime.Now;
                bus.InsertFlowerShopCart(model);
            }
            int num=bus.GetFlowerShopCartListCount("");
            return Json(new {code=1,cnum=num},JsonRequestBehavior.AllowGet);
        }
    }
}