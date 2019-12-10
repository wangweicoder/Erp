using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace ERP.Controllers
{
    public class FlowerChangeController : LoginFilter
    {
        //
        // GET: /FlowerChange/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetList(int limit, int offset, string FlowerNumber, string ChangeNumer, string State)
        {
            Business.Sys_FlowerChange Sys_FlowerChange = new Business.Sys_FlowerChange();
            StringBuilder sb = new StringBuilder();
            //sb.Append(" and OwnedUsersId='" + Utility.ChangeText.GetUsersId() + "' ");
            if (!string.IsNullOrEmpty(FlowerNumber))
            {
                sb.Append(" and FlowerNumber='" + FlowerNumber + "'");
            }
            if (!string.IsNullOrEmpty(ChangeNumer))
            {
                sb.Append(" and Number='" + ChangeNumer + "'");
            }
            if (!string.IsNullOrEmpty(State))
            {
                  sb.Append(" and State='" + State + "'");
            }
            return Json(new { total = Sys_FlowerChange.GetFlowerChangeListCount(sb.ToString()), rows = Sys_FlowerChange.GetList(limit, offset, sb.ToString()) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MIndex()
        {
            Business.Sys_FlowerChange Sys_FlowerChange = new Business.Sys_FlowerChange();
            StringBuilder sb = new StringBuilder();
            Business.Sys_UserAdmin Sys_UserAdmin = new Business.Sys_UserAdmin();
            Model.UserAdmin UserAdmin = Sys_UserAdmin.GetUserAdminByUserId(Utility.ChangeText.GetUsersId());
            if (UserAdmin.RoleCode != "Customer")
            {
                sb.Append(" and UsersId='" + Utility.ChangeText.GetUsersId() + "'");
            }
            else if (Utility.ChangeText.GetUserName() != "admin")
            {
                sb.Append(" and OwnedUsersId='" + Utility.ChangeText.GetUsersId() + "'");
            }
        
            return View(Sys_FlowerChange.GetList(0,1, sb.ToString()));
        }
        public ActionResult Delete()
        {
            Business.Sys_FlowerChange Sys_Flower = new Business.Sys_FlowerChange();
            try
            {
                string ids = Request["ids"];
                string strwhere = " and id in(" + ids + ")";
                List<Model.FlowerChange> list = Sys_Flower.GetFlowerChange(strwhere);
                foreach (var item in list)
                {
                    if (Sys_Flower.DeleteFlowerWatch(item.id.ToString()))
                    {
                        DeleteFlowerPhoto(item.Photo);
                        DeleteFlowerPhoto(item.ChangePhoto);
                    }
                }
                return Content("True");
            }
            catch (Exception ex) {
                return Content("Fasle");
            }          
           
        }        
        public ActionResult Edit()
        {
            string id = Request["id"];
            Business.Sys_FlowerChange Sys_Flower = new Business.Sys_FlowerChange();
            string strwhere = " and id="+id+"";
            List<Model.FlowerChange> list = Sys_Flower.GetFlowerChange(strwhere);
            Model.FlowerChange model = list.Count > 0 ? list[0] : new Model.FlowerChange();
            return View(model);
        }
        public ActionResult Upload()
        {
            try
            {
                string FlowerChangeId = Request["FlowerChangeId"];
                HttpPostedFileBase files = Request.Files["file"];
                Business.Sys_FlowerChange Sys_FlowerChange = new Business.Sys_FlowerChange();
                string strwhere = " and id="+FlowerChangeId+"";
                List<Model.FlowerChange> list = Sys_FlowerChange.GetFlowerChange(strwhere);
                Model.FlowerChange FlowerChange=list.Count > 0 ? list[0] : new Model.FlowerChange();
                if (FlowerChange.ChangePhoto!=null)
                DeleteFlowerPhoto(FlowerChange.ChangePhoto);
                FlowerChange.ChangePhoto = Utility.ChangeText.SaveUploadPicture(files, "changeaf");
                Sys_FlowerChange.AddFlowerPhotoInfo(FlowerChange.Number, FlowerChange.ChangePhoto);                             
                return Json(new { result = "OK", msg = "更换花卉成功" }, "text/html", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) {
                Utility.Log.WriteTextLog("报错", "FlowerChange", "upload", "后台提交更换后的图片", "");
                return null;
            }           
        }
        public ActionResult GetMobleListMore()
        {
            Business.Sys_FlowerChange Sys_FlowerChange = new Business.Sys_FlowerChange();
            StringBuilder sb = new StringBuilder();
            Business.Sys_UserAdmin Sys_UserAdmin = new Business.Sys_UserAdmin();
            Model.UserAdmin UserAdmin = Sys_UserAdmin.GetUserAdminByUserId(Utility.ChangeText.GetUsersId());
            if (UserAdmin.RoleCode != "Customer")
            {
                sb.Append(" and UsersId='" + Utility.ChangeText.GetUsersId() + "'");
            }
            else  if (Utility.ChangeText.GetUserName() != "admin")
            {
                sb.Append(" and OwnedUsersId='" + Utility.ChangeText.GetUsersId() + "'");
            }
         
            int page =int.Parse(Request["page"]);
            if (page>1)
            {
                page = (page - 1) * 10 + 1;
            }
            List<Model.FlowerChange> List=  Sys_FlowerChange.GetList(0, Convert.ToInt32(page), sb.ToString()); 
            return Content(JsonConvert.SerializeObject(List));
        }
	}
}