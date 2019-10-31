using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ERP.Controllers
{
    public class FlowerTreatmentController : LoginFilter
    {
        //
        // GET: /FlowerTreatment/
        public ActionResult Index()
        {
            ViewData["SelectItem"] = GetOwnedCompanyList();
            ViewData["UsersSelectItems"] = GetUserInfoSelectItems("Customer", "0");//管理员
            return View();
        }
        /// <summary>
        /// 获得公司名称
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetOwnedCompanyList()
        {
            Business.Sys_UserAdmin Sys_UserAdmin = new Business.Sys_UserAdmin();
            List<Model.UserAdmin> UserAdminList = Sys_UserAdmin.GetAdminInfoList("Customer");
            List<SelectListItem> hourList = new List<SelectListItem>();
            SelectListItem item = new SelectListItem();
            item.Text = "-请选择-";
            item.Value = "";
            item.Selected = true;
            hourList.Add(item);
            foreach (Model.UserAdmin d in UserAdminList)
            {
                item = new SelectListItem();
                item.Text = d.OwnedCompany;
                item.Value = d.ID.ToString();
                item.Selected = false;
                hourList.Add(item);
            }
            return hourList;
        }
        /// <summary>
        /// 养护人名称列表
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetFlowerTreatmentName()
        {
            Business.Sys_FlowerTreatment Sys_FlowerTreatment = new Business.Sys_FlowerTreatment();

            List<Model.FlowerTreatment> FlowerTreatment = Sys_FlowerTreatment.GetFlowerTreatmentName();
            List<SelectListItem> deptSelectItems = new List<SelectListItem>();
            SelectListItem item = new SelectListItem();
            item.Text = "-请选择-";
            item.Value = "";
            item.Selected = true;
            deptSelectItems.Add(item);
            foreach (Model.FlowerTreatment d in FlowerTreatment)
            {
                item = new SelectListItem();
                item.Text = d.OwnedUsersRealName;
                item.Value = d.OwnedUsersId.ToString();
                item.Selected = false;
                deptSelectItems.Add(item);
            }
            return deptSelectItems;
        }
        /// <summary>
        /// 养护花卉列表
        /// </summary>
        /// <param name="limit">条数</param>
        /// <param name="offset">偏移量</param>
        /// <param name="UsersSelectItems">养护人</param>
        /// <param name="deptSelectItems">养护客户的名称</param>
        /// <returns></returns>
        public ActionResult GetList(int limit, int offset, string UsersSelectItems, string deptSelectItems,string time)
        {
            Business.Sys_FlowerTreatment Sys_FlowerTreatment = new Business.Sys_FlowerTreatment();
            StringBuilder sb = new StringBuilder();
            //sb.Append(" and OwnedUsersId='"+Utility.ChangeText.GetUsersId()+"' ");
            if (!string.IsNullOrEmpty(UsersSelectItems))
            {
                sb.Append(" and UsersId='" + UsersSelectItems + "'");
            }
            if (!string.IsNullOrEmpty(deptSelectItems))
            {
                sb.Append(" and OwnedUsersId='" + deptSelectItems + "'");
            }
            if(!string.IsNullOrEmpty(time))
            {
                sb.Append(" and substring(Convert(varchar,time,120),1,10)='" + time + "'");
            }
            return Json(new { total = Sys_FlowerTreatment.GetFlowerTreatmentListCount(sb.ToString()), rows = Sys_FlowerTreatment.FlowerTreatmentList(limit, offset, sb.ToString()) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit()
        {
            string id = Request["id"];
            Business.Sys_FlowerTreatment Sys_FlowerTreatment = new Business.Sys_FlowerTreatment();
            ViewData["GetUserInfoSelectItems"] = GetUserInfoSelectItems("Customer", "0");//管理员
            ViewData["GetCustomerInfoSelectItems"] = GetUserInfoSelectItems("Customer", "1");//客户
            return View(Sys_FlowerTreatment.GetModel(id));
        }
        /// <summary>
        /// 手机端养护记录
        /// </summary>
        /// <returns></returns>
        public ActionResult MIndex()
        {
            Business.Sys_FlowerTreatment Sys_FlowerTreatment = new Business.Sys_FlowerTreatment();
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
            return View(Sys_FlowerTreatment.FlowerTreatmentList(10, 1, sb.ToString()));
        }
        /// <summary>
        /// 手机端养护记录
        /// </summary>
        /// <returns></returns>
        public ActionResult TreatRecord(string flowernumber)
        {
            Business.Sys_FlowerTreatment Sys_FlowerTreatment = new Business.Sys_FlowerTreatment();
            StringBuilder sb = new StringBuilder();
            Business.Sys_UserAdmin Sys_UserAdmin = new Business.Sys_UserAdmin();
            Model.UserAdmin UserAdmin = Sys_UserAdmin.GetUserAdminByUserId(Utility.ChangeText.GetUsersId());
            if (UserAdmin.RoleCode != "Customer")
            {
                sb.Append(" and FlowerNumber="+flowernumber);
            }
            else if (Utility.ChangeText.GetUserName() != "admin")
            {
                sb.Append(" and OwnedUsersId='" + Utility.ChangeText.GetUsersId() + "'");
            }
            return View(Sys_FlowerTreatment.MTreatmentList(10, 1, sb.ToString()));
        }

        public ActionResult GetMobleListMore()
        {
            Business.Sys_FlowerTreatment Sys_FlowerTreatment = new Business.Sys_FlowerTreatment();
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
            Utility.Log.WriteTextLog("testsql", "养护", "GetMobleListMore", Request["page"], sb.ToString());
            int page = int.Parse(Request["page"]);
            //(@pagesize*(@pagenumber-1)+1) and (@pagesize*@pagenumber)按第几页
            if (page > 1)
            {
                page = (page - 1) * 10 + 1;//按偏移量
            }
            List<Model.FlowerTreatment> List = Sys_FlowerTreatment.FlowerTreatmentList(10, Convert.ToInt32(page), sb.ToString());
            return Json(List,JsonRequestBehavior.AllowGet);
        }


        public ActionResult EditInfo(Model.FlowerTreatment FlowerTreatment)
        {
            Business.Sys_FlowerTreatment Sys_FlowerTreatment = new Business.Sys_FlowerTreatment();
            Business.Sys_UserAdmin Sys_UserAdmin = new Business.Sys_UserAdmin();
            Model.UserAdmin UserAdmin = Sys_UserAdmin.GetUserAdminByUserId(Convert.ToInt32(FlowerTreatment.OwnedUsersId));
            Model.UserAdmin UserAdmin1 = Sys_UserAdmin.GetUserAdminByUserId(FlowerTreatment.UsersId);
            FlowerTreatment.UserRealName = UserAdmin1.RealName;
            FlowerTreatment.OwnedUsersRealName = UserAdmin.RealName;         
            if (Sys_FlowerTreatment.Update(FlowerTreatment))
            {
                return Content("1");
            }
            return Content("0");
        }

        public ActionResult DeleteInfo()
        {
            string id = Request["id"];
            Business.Sys_FlowerTreatment Sys_FlowerTreatment = new Business.Sys_FlowerTreatment();
            if (Sys_FlowerTreatment.DeleteInfo(id))
            {
                return Content("1");
            }
            return Content("0");
        }
        /// <summary>
        /// 绑定养护人、客户
        /// </summary>
        /// <param name="rolecode"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<SelectListItem> GetUserInfoSelectItems(string rolecode, string type)
        {
            Business.Sys_UserAdmin Sys_UserAdmin = new Business.Sys_UserAdmin();
            List<Model.UserAdmin> list = new List<Model.UserAdmin>();
            List<SelectListItem> deptSelectItems = new List<SelectListItem>();
            SelectListItem item = new SelectListItem();
            if (type == "0")
            {
                item.Text = "-请选择-";
                item.Value = "";
                item.Selected = true;
                deptSelectItems.Add(item);
                list = Sys_UserAdmin.GetUserAdminListByRoleCodeNo(rolecode);//不等于
                foreach (Model.UserAdmin d in list)
                {
                    item = new SelectListItem();
                    item.Text = d.RealName;
                    item.Value = d.ID.ToString();
                    item.Selected = true;
                    deptSelectItems.Add(item);
                }
            }
            else
            {
                list = Sys_UserAdmin.GetAdminInfoList(rolecode); 
                foreach (Model.UserAdmin d in list)
                {
                    item = new SelectListItem();
                    item.Text = d.OwnedCompany;
                    item.Value = d.ID.ToString();
                    item.Selected = true;
                    deptSelectItems.Add(item);
                }
            }
            return deptSelectItems;
        }
    }
}