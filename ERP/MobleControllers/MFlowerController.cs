using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace ERP.MobleControllers
{
    public class MFlowerController : MLoginFilterController
    {
        //
        // GET: /MFlower/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ConservationFlowers()
        {
            try
            {
                //ViewData["deptSelectItems"] = GetdeptSelectItems(0);

                //access_token来获取jsapi_ticket  
                string ticket = WxHelper.WxMain.GetTicket();
                string timeStamp = WxHelper.WxMain.getTimestamp();
                string nonceStr = WxHelper.WxMain.getNoncestr();
                //设置参数  
                StringBuilder sb = new StringBuilder();
                sb.Append("jsapi_ticket=" + ticket);
                sb.Append("&noncestr=" + nonceStr);
                sb.Append("&timestamp=" + timeStamp);
                sb.Append("&url=" + Request.Url.AbsoluteUri);

                ViewData["signature"] = Utility.ChangeText.SHA1(sb.ToString(), Encoding.UTF8);

                ViewData["timestamp"] = timeStamp;
                ViewData["nonceStr"] = nonceStr;

                return View();
            }
            catch (Exception ex)
            {
                Utility.Log.WriteTextLog("log", "ConservationFlowers",ex.Message, "养护花卉", "需要把ip加入微信公众开发平台的白名单");
              
            }
            return null;
        }

        [HttpPost]
        public ActionResult ConservationFlowers(Model.FlowerTreatment FlowerTreatment)
        {
            try
            {
                string x = string.Empty, y = string.Empty, strReturn = string.Empty;
                string apiurl = "http://api.map.baidu.com/geoconv/v1/?coords=" + Request["longitude"] + "," + Request["latitude"] + "&from=1&to=5&ak=Kl3rqGn6gECfy7mH5rS3fkGkaWYiyVlr";
                string detail = Utility.PostData.GetData(apiurl);
                ERP.MobleControllers.MMainController.BaiDuCoordinates jd = JsonConvert.DeserializeObject<ERP.MobleControllers.MMainController.BaiDuCoordinates>(detail);
                List<ERP.MobleControllers.MMainController.bc_result> result = jd.result;
                foreach (var item in result)
                {
                    x = item.x;
                    y = item.y;
                }
                apiurl = "http://api.map.baidu.com/geocoder/v2/?ak=Kl3rqGn6gECfy7mH5rS3fkGkaWYiyVlr&callback=renderReverse&location=" + y + "," + x + "&output=json&pois=1";
                detail = Utility.PostData.GetData(apiurl);
                detail = detail.Replace("renderReverse&&renderReverse(", "");
                detail = detail.TrimEnd(')');
                ERP.MobleControllers.MMainController.GetAddressNew GetAddress = JsonConvert.DeserializeObject<ERP.MobleControllers.MMainController.GetAddressNew>(detail);
                Utility.Log.WriteTextLog("返回定位", "花卉养护当前地址", GetAddress.result.formatted_address, "detail", detail);
                Business.Sys_UserAdmin Sys_UserAdmin = new Business.Sys_UserAdmin();
                Business.Sys_FlowerTreatment Sys_FlowerTreatment = new Business.Sys_FlowerTreatment();
                int userid = Utility.ChangeText.GetUsersId();
                FlowerTreatment.FlowerTreatmentType = "养护花卉";
                FlowerTreatment.UsersId = userid;
                FlowerTreatment.OwnedUsersId = Request["deptSelectItems"];
                FlowerTreatment.UserRealName = Utility.ChangeText.GetRealName();
                FlowerTreatment.FlowerTreatmentAddress = GetAddress.result.formatted_address;
                Model.UserAdmin UserAdmin = Sys_UserAdmin.GetUserAdminByUserId(Convert.ToInt32(FlowerTreatment.OwnedUsersId));
                FlowerTreatment.OwnedUsersRealName = UserAdmin.RealName;
                FlowerTreatment.OwnedCompany = UserAdmin.OwnedCompany;
                FlowerTreatment.LogoPhoto = UserAdmin.LogoPhoto;
                //同一登录人，同一公司，一天只能提交一次
                StringBuilder stb = new StringBuilder();
                if (userid != 0)
                {
                    stb.Append(" t.UsersId=" + userid + "");
                }
                if (!string.IsNullOrEmpty(FlowerTreatment.OwnedUsersId))
                {
                    stb.Append(" and t.OwnedUsersId='" + FlowerTreatment.OwnedUsersId + "'");
                }
                string dt = DateTime.Now.ToShortDateString();
                { 
                    stb.Append(" and time>'" + dt + "'"); 
                }
                if (Sys_FlowerTreatment.FlowerTreatmentList(stb.ToString()).Count == 0)
                {
                    Sys_FlowerTreatment.InsertFlowerTreatment(FlowerTreatment);
                    return Content("1");
                }
                else {
                    return Content("0");
                }
                
            }
            catch (Exception ex)
            {
                Utility.Log.WriteTextLog("返回定位错误", "ConservationFlowers", "养护花卉", "post方法", ex.ToString());
                return View();
            }
        }

        public ActionResult ChangeFlowers() 
        {
            //如果当前是客户提交申请,则直接记录工作服务对象OwnedCompany，
            //如果是工作人自己发起，则需要选择一个属于自己服务范围的公司进行操作
            Business.Sys_UserAdmin Sys_UserAdmin = new Business.Sys_UserAdmin();
            Model.UserAdmin  UserAdmin=Sys_UserAdmin.GetUserAdminByUserId(Utility.ChangeText.GetUsersId());
          
            if (Utility.ChangeText.GetRoleCode().Trim() == "admin" || Utility.ChangeText.GetRoleCode().Trim() == "Admin" || Utility.ChangeText.GetRoleCode().Trim() == "SuperAdministrator")
            {
            
                ViewData["ShowMsg"] = "1";
                ViewData["deptSelectItems"] = GetdeptSelectItemsByWorkUsersId();
            }
            else  if (!UserAdmin.RoleCode.Contains("Customer"))
            {
                ViewData["ShowMsg"] = "1";
                ViewData["deptSelectItems"] = GetdeptSelectItemsByWorkUsersId();
            }
            return View();
        }

        [HttpPost]
        public ActionResult ChangeFlowers(Model.FlowerChange FlowerChange)
        {
            try
            {
                //如果当前是客户提交申请,则直接记录工作服务对象OwnedCompany，
                //如果是工作人自己发起，则需要选择一个属于自己服务范围的公司进行操作
                Business.Sys_UserAdmin Sys_UserAdmin = new Business.Sys_UserAdmin();
                string openId = "";
                Model.UserAdmin UserAdmin = Sys_UserAdmin.GetUserAdminByUserId(Utility.ChangeText.GetUsersId());
             
                if (!UserAdmin.RoleCode.Contains("Customer"))
                {
                    if (Request["deptSelectItems"].Split('&').Length > 1)
                    {
                        openId = Utility.ChangeText.GetOpenId();
                        FlowerChange.WorkUsersId = Utility.ChangeText.GetUsersId();
                        FlowerChange.WorkUsersRealName = Utility.ChangeText.GetRealName();
                        FlowerChange.OwnedUsersId = int.Parse(Request["deptSelectItems"].Split('&')[0]);
                        FlowerChange.OwnedCompany = Request["deptSelectItems"].Split('&')[1];
                    }
                }
                else
                {
                    //是客户自己本身提交
                    FlowerChange.OwnedUsersId = Utility.ChangeText.GetUsersId();
                    FlowerChange.OwnedCompany = UserAdmin.OwnedCompany;
                    FlowerChange.WorkUsersId = int.Parse(UserAdmin.WorkUsersId);
                    FlowerChange.WorkUsersRealName = UserAdmin.RealName;
                    openId = UserAdmin.OpenId;
                }
                FlowerChange.FlowerTreatmentType = "更换花卉"; 
                FlowerChange.UsersId = Utility.ChangeText.GetUsersId();
                HttpPostedFileBase file = Request.Files["attach_path"];
                FlowerChange.Photo = Utility.ChangeText.SaveUploadPicture(file, "attach");

                Business.Sys_FlowerChange Sys_FlowerChange = new Business.Sys_FlowerChange();
                FlowerChange.Number = Utility.ChangeText.OrderIdCreate();
                FlowerChange.State = "未更换";
                Model.Wx_SendMsg Wx_SendMsg = new Model.Wx_SendMsg()
                {
                    template_id = "MU4CvSNXPYTMjhGJdWuWNvpc5Ls2VPAmcaST4lWrTaM",
                    touser = openId,
                    url = "http://www.thuay.com/MFlower/AddFlowersPhotoInfo?Number=" + FlowerChange.Number,
                    data = new
                    {
                        first = new { value = "您好!已经有客户(" + FlowerChange.OwnedCompany + ")需要服务,请尽快前往。", color = "#173177" },
                        keyword1 = new { value = FlowerChange.Number, color = "#173177" },
                        keyword2 = new { value = "更换花卉", color = "#173177" },
                        keyword3 = new { value = "更换", color = "#173177" },
                        keyword4 = new { value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), color = "#173177" },
                        remark = new { value = "更换内容:" + FlowerChange.Reamrk + ".点击此消息,进行补录更换后图片。", color = "#173177" },
                    }
                };
                WxHelper.WxMain.SendMsg(JsonConvert.SerializeObject(Wx_SendMsg));
                if (Sys_FlowerChange.InsertFlowerChange(FlowerChange))
                {
                    ViewData["success"] = "操作成功";
                    return View();
                }
            }
            catch (Exception ex)
            {

                Utility.Log.WriteTextLog("更换花卉", "ChangeFlowers", "更换花卉", "post", ex.Message);
            }
          
            return View();
        }

        public ActionResult AddFlowersPhotoInfo() 
        {
            Model.FlowerChange FlowerChange = new Model.FlowerChange() { Number = Request["Number"] };
            return View(FlowerChange);
        }

        [HttpPost]
        public ActionResult AddFlowersPhotoInfo(Model.FlowerChange FlowerChange)
        {
            try
            {
             
                HttpPostedFileBase file = Request.Files["attach_paths"];
                FlowerChange.ChangePhoto = Utility.ChangeText.SaveUploadPicture(file, "attach");
                Business.Sys_FlowerChange Sys_FlowerChange = new Business.Sys_FlowerChange();
                Utility.Log.WriteTextLog("补图", "ID", Request["Number"], "路径", FlowerChange.ChangePhoto);
                if (Sys_FlowerChange.AddFlowerPhotoInfo(Request["Number"], FlowerChange.ChangePhoto))
                {
                    ViewData["success"] = "操作成功";
                }
            }
            catch (Exception ex)
            {
                Utility.Log.WriteTextLog("补图", "", "", "", ex.ToString());
            }
        
            return View();
        }
        public List<SelectListItem> GetdeptSelectItemsByWorkUsersId() 
        {
            Business.Sys_UserAdmin Sys_UserAdmin = new Business.Sys_UserAdmin();
            List<Model.UserAdmin> UserAdminList = new List<Model.UserAdmin>();
            if (Utility.ChangeText.GetRoleCode().Trim() == "admin" || Utility.ChangeText.GetRoleCode().Trim() == "Admin" || Utility.ChangeText.GetRoleCode().Trim() == "SuperAdministrator")
            {
                Utility.Log.WriteTextLog("当前登陆人的角色", "", "", "进入正确", Utility.ChangeText.GetRoleCode().ToString());
                UserAdminList = Sys_UserAdmin.GetAdminInfoList("Customer");
            }
            else
            {
                UserAdminList = Sys_UserAdmin.GetUserAdminListByRoleCode("Customer", Utility.ChangeText.GetUsersId());
            }
            //UserAdminList = Sys_UserAdmin.GetUserAdminListByWorkUsersId(Utility.ChangeText.GetUsersId());
            List<SelectListItem> GetdeptSelectItemsByWorkUsersId = new List<SelectListItem>();
            SelectListItem item = new SelectListItem();
            foreach (Model.UserAdmin d in UserAdminList)
            {
                item = new SelectListItem();

                item.Text = d.RealName;
                item.Text = string.IsNullOrEmpty(d.OwnedCompany) == true ? item.Text : item.Text + "(" + d.OwnedCompany + ")";
                item.Value = d.ID.ToString() + "&" + d.OwnedCompany;
                item.Selected = false;
                GetdeptSelectItemsByWorkUsersId.Add(item);
            }
            if (GetdeptSelectItemsByWorkUsersId.Count > 0)
            {
                GetdeptSelectItemsByWorkUsersId[0].Selected = true;
            }
            return GetdeptSelectItemsByWorkUsersId;
        }

        /// <summary>
        /// 获得公司json数据
        /// </summary>
        /// <author>wangwei</author>
        /// <returns></returns>
        public JsonResult GetCompanyList(int week)
        {            
            List<SelectListItem> hourList = new List<SelectListItem>();
            hourList = GetdeptSelectItems(week);
            return Json(hourList, JsonRequestBehavior.AllowGet);
        }


        public List<SelectListItem> GetdeptSelectItems(int week)
        {
            Business.Sys_UserAdmin Sys_UserAdmin = new Business.Sys_UserAdmin();
            List<Model.UserAdmin> UserAdminList = new List<Model.UserAdmin>();
            if (Utility.ChangeText.GetUserName() == "admin")
            {               
                UserAdminList = Sys_UserAdmin.GetAdminInfoListbyweek("Customer",week);
            }
            else if (Session["RoleName"].ToString() == "客户")
            {
                 UserAdminList = Sys_UserAdmin.GetUserAdminUsByRoleCode("Customer", Utility.ChangeText.GetUsersId());
            }
            else {
                UserAdminList = Sys_UserAdmin.GetUserAdminListByWorkUsersId("Customer", Utility.ChangeText.GetUsersId(),week);
            }
         
            List<SelectListItem> deptSelectItems = new List<SelectListItem>();
            SelectListItem item = new SelectListItem();
            foreach (Model.UserAdmin d in UserAdminList)
            {
                item = new SelectListItem();

                item.Text = d.RealName;
                item.Text = string.IsNullOrEmpty(d.OwnedCompany) == true ? item.Text : item.Text + "(" + d.OwnedCompany + ")";
                item.Value = d.ID.ToString();
                item.Selected = false;
                deptSelectItems.Add(item);
            }
            if (deptSelectItems.Count>0)
            {
                deptSelectItems[0].Selected = true;
            }
            return deptSelectItems;
        }
	}
}