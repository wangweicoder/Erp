using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace ERP.MobleControllers
{
    public class MMainController : MLoginFilterController
    {

        public ActionResult GetArrangementInfo()
        {
            try
            {

                string id = Request["ArrangementId"];
                Business.Sys_FlowerArrangement Sys_FlowerArrangement = new Business.Sys_FlowerArrangement();
                Model.FlowerArrangement  FlowerArrangement=Sys_FlowerArrangement.GetModel(id);
                if (FlowerArrangement.belongUsersId != 0)
                {
                 DateTime dt = Sys_FlowerArrangement.GetFlowerTreatmentModel(FlowerArrangement.belongUsersId.ToString()).time;
                 ViewBag.Treattime = dt;
                 ViewBag.PlanTreatTime = dt.AddDays(7);
                }
                if (Session["RoleCode"] != null && Session["RoleCode"].ToString() == "Tourist")
                {
                    ViewData["IsTourist"] = 1;
                }
                
                //当操作人不是对应绑定客户与超级管理员时,判断是否为养护人员,如果为养护人员则判断是否有权限操作此更换花卉
                if (Session["RoleCode"] != null)
                {
                    if (Session["RoleCode"].ToString() != "Customer" && Session["RoleCode"].ToString() != "SuperAdministrator")
                    {
                        if (Session["RoleCode"].ToString() == "	yanghu")
                        {
                            Business.Sys_UserAdmin Sys_UserAdmin = new Business.Sys_UserAdmin();
                            List<Model.UserAdmin> UserAdminList = new List<Model.UserAdmin>();

                            UserAdminList = Sys_UserAdmin.GetUserAdminListByRoleCode("Customer", Utility.ChangeText.GetUsersId());
                            //判断所属客户权限中是否包含此客户
                            UserAdminList = UserAdminList.Where(x => x.ID == FlowerArrangement.belongUsersId).ToList();
                            if (UserAdminList.Count() > 0)
                            {
                                ViewData["IsAllower"] = 1;
                            }
                            else
                            {
                                ViewData["IsAllower"] = 0;
                            }
                        }
                        else
                        {
                            ViewData["IsAllower"] = 0;
                        }
                    }
                    else {
                        ViewData["IsAllower"] = 1;
                    }
                }
         
                return View(FlowerArrangement);
            }
            catch (Exception ex)
            {
                Utility.Log.WriteTextLog("扫码页面", "MMain",ex.Message, "GetArrangementInfo", ex.ToString());
                 return null;
            }
           
        }
        public ActionResult GetArrangementInfos()
        {
            try
            {

                string id = Request["ArrangementId"];
                Business.Sys_FlowerArrangement Sys_FlowerArrangement = new Business.Sys_FlowerArrangement();
                Model.FlowerArrangement FlowerArrangement = Sys_FlowerArrangement.GetModel(id);
                if (FlowerArrangement.belongUsersId != 0)
                {
                    DateTime dt = Sys_FlowerArrangement.GetFlowerTreatmentModel(FlowerArrangement.belongUsersId.ToString()).time;
                    ViewBag.Treattime = dt;
                    ViewBag.PlanTreatTime = dt.AddDays(7);
                }
                if (Session["RoleCode"] != null && Session["RoleCode"].ToString() == "Tourist")
                {
                    ViewData["IsTourist"] = 1;
                }
                ViewData["IsAllower"] = 1;
                //当操作人不是对应绑定客户与超级管理员时,判断是否为养护人员,如果为养护人员则判断是否有权限操作此更换花卉
                if (Session["RoleCode"] != null)
                {
                    if (Session["RoleCode"].ToString() != "Customer" && Session["RoleCode"].ToString() != "SuperAdministrator")
                    {
                        if (Session["RoleCode"].ToString() == "	yanghu")
                        {
                            Business.Sys_UserAdmin Sys_UserAdmin = new Business.Sys_UserAdmin();
                            List<Model.UserAdmin> UserAdminList = new List<Model.UserAdmin>();

                            UserAdminList = Sys_UserAdmin.GetUserAdminListByRoleCode("Customer", Utility.ChangeText.GetUsersId());
                            //判断所属客户权限中是否包含此客户
                            UserAdminList = UserAdminList.Where(x => x.ID == FlowerArrangement.belongUsersId).ToList();
                            if (UserAdminList.Count() > 0)
                            {
                                ViewData["IsAllower"] = 1;
                            }
                            else
                            {
                                ViewData["IsAllower"] = 0;
                            }
                        }
                        else
                        {
                            ViewData["IsAllower"] = 0;
                        }
                    }
                    else
                    {
                        ViewData["IsAllower"] = 1;
                    }
                }

                return View(FlowerArrangement);
            }
            catch (Exception ex)
            {
                Utility.Log.WriteTextLog("扫码页面", "MMain", ex.Message, "GetArrangementInfo", ex.ToString());
                return null;
            }

        }
        // GET: /MMain/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddClockAttendanceInfo()
        {
            Business.Sys_UserAdmin Sys_UserAdmin = new Business.Sys_UserAdmin();

            Model.ClockAttendance ClockAttendance = new Model.ClockAttendance()
            {
                UsersId = Utility.ChangeText.GetUsersId(),
                Address = Request["Address"],
                RealName = Utility.ChangeText.GetRealName(),
                CheckAddress = Sys_UserAdmin.GetUserAdminByUserId(Utility.ChangeText.GetUsersId()).CheckAddress
            };
            Business.Sys_ClockAttendance Sys_ClockAttendance = new Business.Sys_ClockAttendance();
            if (Sys_ClockAttendance.InsertClockAttendance(ClockAttendance))
            {
                return Content("1");
            } return Content("2");
        }

        public ActionResult ClockAttendance()
        {
            #region 获取定位所需要的参数
            //jssdk  JS接口安全域名填写，  
            try
            {
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
            }
            catch(Exception ex){
                Utility.Log.WriteTextLog("考勤打卡", "MMain/ClockAttendance", ex.Message, "", "");
            }
            #endregion
            return View();
        }

        public ActionResult GetLocation()
        {
            string lat = Request["lat"];
            string lon = Request["lon"];

            string x = string.Empty, y = string.Empty, strReturn = string.Empty;

            string apiurl = "http://api.map.baidu.com/geoconv/v1/?coords=" + lon + "," + lat + "&from=1&to=5&ak=Kl3rqGn6gECfy7mH5rS3fkGkaWYiyVlr";
            string detail = Utility.PostData.GetData(apiurl);
            BaiDuCoordinates jd = JsonConvert.DeserializeObject<BaiDuCoordinates>(detail);
            List<bc_result> result = jd.result;
            foreach (var item in result)
            {
                x = item.x;
                y = item.y;
            }
            apiurl = "http://api.map.baidu.com/geocoder/v2/?ak=Kl3rqGn6gECfy7mH5rS3fkGkaWYiyVlr&callback=renderReverse&location=" + y + "," + x + "&output=json&pois=1";
            detail = Utility.PostData.GetData(apiurl);
            detail = detail.Replace("renderReverse&&renderReverse(", "");
            detail = detail.TrimEnd(')');
            GetAddressNew GetAddress = JsonConvert.DeserializeObject<GetAddressNew>(detail);
            return Content(GetAddress.result.formatted_address);           
        }
        #region 百度坐标转换
        public partial class BaiDuCoordinates
        {
            public string status { get; set; }
            public List<bc_result> result { get; set; }
        }
        public partial class bc_result
        {
            public string x { get; set; }
            public string y { get; set; }
        }
        #endregion
        public ActionResult ProblemsAndSuggestions()
        {
            return View();
        }

        public ActionResult ShowMoreInfo()
        {
            return View();
        }
        public ActionResult CustomRequirements()
        {
            ViewData["deptSelectItems"] = GetdeptSelectItems();
            return View();
        }

        [HttpPost]
        public ActionResult CustomRequirements(Model.FlowerDemand FlowerDemand)
        {
            ViewData["deptSelectItems"] = GetdeptSelectItems();
            FlowerDemand.FlowerCategoryType = Request["deptSelectItems"];
            Business.Sys_FlowerDemand Sys_FlowerDemand = new Business.Sys_FlowerDemand();
            if (Sys_FlowerDemand.InsertFlowerDemand(FlowerDemand))
            {
                ViewData["success"] = "添加成功";
            } return View();
        }
        [HttpPost]
        public ActionResult ProblemsAndSuggestions(Model.ProblemsAndSuggestions ProblemsAndSuggestions)
        {

            Business.Sys_ProblemsAndSuggestions Sys_ProblemsAndSuggestions = new Business.Sys_ProblemsAndSuggestions();
            HttpPostedFileBase file = Request.Files["main_img"];
            if (file != null)
            {
                ProblemsAndSuggestions.PhotoList = Utility.ChangeText.SaveUploadPicture(file, "attach");
            }
            ProblemsAndSuggestions.State = 1;
            ProblemsAndSuggestions.RealName = Utility.ChangeText.GetRealName();
            ProblemsAndSuggestions.UsersId = Utility.ChangeText.GetUsersId();
            if (Sys_ProblemsAndSuggestions.InsertProblemsAndSuggestions(ProblemsAndSuggestions))
            {
                ViewData["success"] = "添加成功";
            }
            return View();
        }

        public class GetAddressNew
        {
            public int status { get; set; }
            public Result result { get; set; }
        }

        public class Result
        {
            public Location location { get; set; }
            public string formatted_address { get; set; }
            public string business { get; set; }
            public Addresscomponent addressComponent { get; set; }
            public Pois[] pois { get; set; }
            public object[] roads { get; set; }
            public Poiregion[] poiRegions { get; set; }
            public string sematic_description { get; set; }
            public int cityCode { get; set; }
        }

        public class Location
        {
            public float lng { get; set; }
            public float lat { get; set; }
        }

        public class Addresscomponent
        {
            public string country { get; set; }
            public int country_code { get; set; }
            public string province { get; set; }
            public string city { get; set; }
            public string district { get; set; }
            public string adcode { get; set; }
            public string street { get; set; }
            public string street_number { get; set; }
            public string direction { get; set; }
            public string distance { get; set; }
        }

        public class Pois
        {
            public string addr { get; set; }
            public string cp { get; set; }
            public string direction { get; set; }
            public string distance { get; set; }
            public string name { get; set; }
            public string poiType { get; set; }
            public Point point { get; set; }
            public string tag { get; set; }
            public string tel { get; set; }
            public string uid { get; set; }
            public string zip { get; set; }
            public Parent_Poi parent_poi { get; set; }
        }

        public class Point
        {
            public float x { get; set; }
            public float y { get; set; }
        }

        public class Parent_Poi
        {
            public string name { get; set; }
            public string tag { get; set; }
            public string addr { get; set; }
            public Point1 point { get; set; }
            public string direction { get; set; }
            public string distance { get; set; }
            public string uid { get; set; }
        }

        public class Point1
        {
            public float x { get; set; }
            public float y { get; set; }
        }

        public class Poiregion
        {
            public string direction_desc { get; set; }
            public string name { get; set; }
            public string tag { get; set; }
        }

        public List<SelectListItem> GetdeptSelectItems()
        {
            Business.Sys_FlowerCategory Sys_FlowerCategory = new Business.Sys_FlowerCategory();
            List<Model.FlowerCategory> FlowerCategoryList = Sys_FlowerCategory.GetListModel();
            List<SelectListItem> deptSelectItems = new List<SelectListItem>();
            int i = 0;
            foreach (Model.FlowerCategory d in FlowerCategoryList)
            {
                SelectListItem item = new SelectListItem();
                item.Text = d.FlowerCategoryType;
                item.Value = d.id.ToString();
                item.Selected = false;
                if (i == 0)
                {
                    item.Selected = true;
                }
                deptSelectItems.Add(item);
                i++;
            }
            return deptSelectItems;
        }
    }
}