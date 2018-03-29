using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ERP.Controllers
{
    public class FlowerActiveController : LoginFilter
    {
        //
        // GET: /Flower/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Add()
        {
            return View();
        }

        public ActionResult GetList(int FlowerId)
        {
            int offset = Convert.ToInt32(Request["offset"]);
            int limit = int.Parse(Request["limit"]);
            StringBuilder sb = new StringBuilder();
            if (FlowerId != 0)
            {
                sb.Append("FlowerId=" + FlowerId);
            }
            Business.Sys_FlowerActive Sys_FlowerActive = new Business.Sys_FlowerActive();         
            return Json(new { total = Sys_FlowerActive.GetFlowerActiveListCount(""), rows = Sys_FlowerActive.FlowerActiveList(limit,offset,"") }, JsonRequestBehavior.AllowGet);
        }
     
        [HttpPost]
        public ActionResult Add(Model.FlowerActive Flower) 
        {
            Flower.CreateTime = DateTime.Now;
            Flower.UpdateTime = DateTime.Now;
            Flower.UsersId = Utility.ChangeText.GetUsersId().ToString();
            Business.Sys_FlowerActive Sys_FlowerActive = new Business.Sys_FlowerActive();
            //ViewData["success"] = "添加失败";
            if (Sys_FlowerActive.InsertFlowerActive(Flower))
            {
               // ViewData["success"] = "添加成功";
            }
            Response.Write("<script>parent.layer.closeAll();</script>");
            return View();
        }

        public ActionResult Edit() 
        {
            int id = int.Parse(Request["id"]);
            Business.Sys_FlowerActive Sys_FlowerActive = new Business.Sys_FlowerActive();
            ViewData["GetFlowerList"] = GetdeptSelectItems();
            return View(Sys_FlowerActive.GetFlowerActiveById(id));
        }
        
        [HttpPost]
        public ActionResult Edit(Model.FlowerActive Flower)
        {
            Business.Sys_FlowerActive Sys_FlowerActive = new Business.Sys_FlowerActive();
            Flower.UpdateTime = DateTime.Now;
            Flower.UsersId = Utility.ChangeText.GetUsersId().ToString();
            //ViewData["success"] = "修改失败";          
            if (Sys_FlowerActive.UpdateFlowerActive(Flower))
            {
               // ViewData["success"] = "修改成功";
            }
            Response.Write("<script>parent.layer.closeAll();</script>");
            return Content("");
        }

        public ActionResult Delete() 
        {
            Business.Sys_FlowerActive Sys_FlowerActive = new Business.Sys_FlowerActive();
            int id = int.Parse(Request["id"]) ;      
            if (Sys_FlowerActive.DeleteFlowerActive(id))
            {               
                return Content("True");
            }
            return Content("Fasle");
        }

        /// <summary>
        /// 花卉
        /// </summary>
        /// <returns></returns>
        public ActionResult GetFlowerList()
        {           
            Business.Sys_Flower Sys_Flower = new Business.Sys_Flower();            
            List<Model.Flower> FlowerList=Sys_Flower.GetFlowerList();
            return Json(FlowerList, JsonRequestBehavior.AllowGet);
        }
        public List<SelectListItem> GetdeptSelectItems()
        {
            Business.Sys_Flower Sys_Flower = new Business.Sys_Flower();
            List<Model.Flower> FlowerList = Sys_Flower.GetFlowerList();
            List<SelectListItem> deptSelectItems = new List<SelectListItem>();
            SelectListItem items = new SelectListItem();
            items.Text = "--请选择--";
            items.Value = "";
            items.Selected = true;
            deptSelectItems.Add(items);
            foreach (Model.Flower d in FlowerList)
            {
                SelectListItem item = new SelectListItem();
                item.Text = d.FlowerWatchName;
                item.Value = d.id.ToString();
                item.Selected = false;
                deptSelectItems.Add(item);
            }
            return deptSelectItems;
        }
        
     }      
}