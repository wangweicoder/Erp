using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ERP.MobleControllers
{
    public class BuyCartController : MLoginFilterController
    {
        string userid = Utility.ChangeText.GetUsersId().ToString();
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
            Business.Sys_FlowerShopCart bus = new Business.Sys_FlowerShopCart();
            Model.FlowerShopCart Cart = bus.GetFlowerShopCart(FlowerId.ToString(),userid);
            if (Cart != null)
            {
                Cart.Num += Num;
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
            int num = bus.GetFlowerList().Where(m=>m.UsersId==userid).ToList().Sum(m=>m.Num);
            return Json(new {code=1,cnum=num},JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetTotal() 
        {
            Business.Sys_FlowerShopCart bus = new Business.Sys_FlowerShopCart();
            List<Model.FlowerShopCart> list = bus.GetFlowerList().Where(m => m.UsersId == userid).ToList();
            int num = list.Sum(p => p.Num);
            return Json(new { code = 1, cnum = num }, JsonRequestBehavior.AllowGet);
        }
               
        /// <summary>    
        /// 点击数量+号或点击数量-号或自己输入一个值
        /// </summary>
        /// <param name="id"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult IncreaseOrDecreaseOne(string id, int quantity)
        {
            Business.Sys_FlowerShopCart bus = new Business.Sys_FlowerShopCart();
            Model.FlowerShopCart Cart = bus.GetFlowerShopCartById(id);
            if (Cart != null)
            {
                Cart.Num = quantity;
                Cart.UpdateTime = DateTime.Now;

                bus.UpdateFlowerShopCart(Cart);//原来有这个商品，更新下数量
            }
            return Json(new
            {
                msg = true
            });            
        }
                
        /// <summary>
        /// 从购物车移除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult RemoveFromCart(string id)
        {
            Business.Sys_FlowerShopCart bus = new Business.Sys_FlowerShopCart();
            Model.FlowerShopCart Cart = bus.GetFlowerShopCartById(id);
            if (Cart != null)
            {               
                bus.DeleteFlowerShopCart(Cart.Id.ToString());//
            }
            return Json(new { code = 1 }, JsonRequestBehavior.AllowGet);
        }
    }
}