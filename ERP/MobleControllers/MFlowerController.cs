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
        #region 养护花卉
        /// <summary>
        /// 养护花卉
        /// </summary>
        /// <returns></returns>
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
                Utility.Log.WriteTextLog("养护花卉", "ConservationFlowers",ex.Message, "养护花卉", "需要把ip加入微信公众开发平台的白名单");
              
            }
            return null;
        }
        /// <summary>
        /// 养护花卉
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// 开始养护花卉
        /// </summary>
        /// <returns></returns>
        public ActionResult StartCurFlowers(string shopid,string ownedUsersId)
        {
            try
            {
                Model.FlowerTreatment FlowerTreatment = new Model.FlowerTreatment();
                Business.Sys_UserAdmin Sys_UserAdmin = new Business.Sys_UserAdmin();
                Business.Sys_FlowerTreatment Sys_FlowerTreatment = new Business.Sys_FlowerTreatment();
                int userid = Utility.ChangeText.GetUsersId();
                FlowerTreatment.FlowerTreatmentType = "开始养护";
                FlowerTreatment.UsersId = userid;
                FlowerTreatment.FlowerNumber = shopid;
                FlowerTreatment.OwnedUsersId = ownedUsersId;
                FlowerTreatment.UserRealName = Utility.ChangeText.GetRealName();
                FlowerTreatment.FlowerTreatmentAddress = "";
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
                else
                {
                    return Content("0");
                }
            }
            catch {
            }
            return Json(new { resule='1'}, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 结束养护
        /// </summary>       
        /// <returns></returns>
        public ActionResult EndCurFlowers(string shopid, string ownedUsersId)
        {
            Model.FlowerTreatment FlowerTreatment = new Model.FlowerTreatment();
            Business.Sys_UserAdmin Sys_UserAdmin = new Business.Sys_UserAdmin();
            Business.Sys_FlowerTreatment Sys_FlowerTreatment = new Business.Sys_FlowerTreatment();
            int userid = Utility.ChangeText.GetUsersId();
            FlowerTreatment=Sys_FlowerTreatment.GetModelbyShopid(shopid, ownedUsersId, userid.ToString());
            if (FlowerTreatment.id > 0)
            {
                FlowerTreatment.endtime = DateTime.Now;
                FlowerTreatment.FlowerTreatmentType = "结束养护";
                if(Sys_FlowerTreatment.UpdateEndtime(FlowerTreatment))
                {
                    return Content("1");
                }
            }
            return Content("0");
        }
        /// <summary>
        /// 查询服务前图片
        /// </summary>       
        /// <returns></returns>
        public ActionResult ServerBefor(string shopid, string ownedUsersId)
        {
            Model.FlowerTreatment FlowerTreatment = new Model.FlowerTreatment();
            Business.Sys_UserAdmin Sys_UserAdmin = new Business.Sys_UserAdmin();
            Business.Sys_FlowerTreatment Sys_FlowerTreatment = new Business.Sys_FlowerTreatment();
            int userid = Utility.ChangeText.GetUsersId();
            FlowerTreatment = Sys_FlowerTreatment.GetModelbyShopid(shopid, ownedUsersId, userid.ToString());
            //只有服务前图片
            if (FlowerTreatment!=null && FlowerTreatment.Photo!=null && FlowerTreatment.ChangePhoto == null)
            {  
                return Json(new { result = "OK", data = FlowerTreatment }, "text/html", JsonRequestBehavior.AllowGet);
            }
            else {
                return Json(new { result = "OK", data = "" }, "text/html", JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 服务后
        /// </summary>
        /// <returns></returns>
        public ActionResult AddServerPhoto()
        {
            Model.FlowerTreatment FlowerTreatment = new Model.FlowerTreatment()
            { id = int.Parse(Request["id"]),CompanyName= Request["ArrangementId"] };
            return View(FlowerTreatment);
        }
        /// <summary>
        /// 差评
        /// </summary>        
        public ActionResult SetFlowerBad()
        {
            ViewData["ArrangementId"] = Request["ArrangementId"];
            return View();
        }
        /// <summary>
        /// 差评提交
        /// </summary>
        [HttpPost]
        public ActionResult SetFlowerBad(Model.FlowerAppraise flowerAppraise)
        {
            if (flowerAppraise.Content != null)
            {
                Business.Sys_FlowerAppraise sys_FlowerAppraise = new Business.Sys_FlowerAppraise();
                flowerAppraise.IsGood = "0";
                flowerAppraise.UsersId = Utility.ChangeText.GetUsersId().ToString();
                flowerAppraise.CreateTime = DateTime.Now;
                //同一登录人，对一个植物，一天只能提交一次
                StringBuilder stb = new StringBuilder();
                int userid = Utility.ChangeText.GetUsersId();
                if (userid != 0)
                {
                    stb.Append(" and UsersId=" + userid + "");
                }
                if (!string.IsNullOrEmpty(flowerAppraise.ArrangementId))
                {
                    stb.Append(" and ArrangementId='" + flowerAppraise.ArrangementId + "'");
                }
                string dt = DateTime.Now.ToShortDateString();
                {
                    stb.Append(" and CreateTime>'" + dt + "'");
                }
                if (sys_FlowerAppraise.GetFlowerAppraiseCount(stb.ToString()).Count == 0)
                {
                    sys_FlowerAppraise.InsertFlowerAppraise(flowerAppraise);
                    Response.Write("<script>parent.layer.closeAll();</script>");
                }
                else
                {
                    ViewData["success"] = "今日已评价";
                }            
            } 
            return View();
        }
        /// <summary>
        /// 好评提交
        /// </summary>       
        public ActionResult SetFlowerGood(string ArrangementId)
        {
            if (!string.IsNullOrEmpty(ArrangementId))
            {
                Business.Sys_FlowerAppraise sys_FlowerAppraise = new Business.Sys_FlowerAppraise();
                Model.FlowerAppraise flowerAppraise = new Model.FlowerAppraise();
                flowerAppraise.IsGood = "1";
                flowerAppraise.ArrangementId = ArrangementId;
                flowerAppraise.CreateTime = DateTime.Now;
                flowerAppraise.UsersId= Utility.ChangeText.GetUsersId().ToString();
                //同一登录人，对一个植物，一天只能提交一次
                StringBuilder stb = new StringBuilder();
                int userid = Utility.ChangeText.GetUsersId();
                if (userid != 0)
                {
                    stb.Append(" and UsersId=" + userid + "");
                }
                if (!string.IsNullOrEmpty(ArrangementId))
                {
                    stb.Append(" and ArrangementId='" + ArrangementId + "'");
                }
                string dt = DateTime.Now.ToShortDateString();
                {
                    stb.Append(" and CreateTime>'" + dt + "'");
                }
                if (sys_FlowerAppraise.GetFlowerAppraiseCount(stb.ToString()).Count == 0)
                {
                    sys_FlowerAppraise.InsertFlowerAppraise(flowerAppraise);
                }
                else {
                    return Json(new { result = "OK" , data="0"}, "text/html", JsonRequestBehavior.AllowGet);
                }
            }           
            return Json(new { result = "OK", data = "1" }, "text/html", JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获得评价json数据
        /// </summary>
        /// <author>wangwei</author>
        /// <returns></returns>
        public JsonResult GetAppraiseCount(string ArrangementId)
        {
            Business.Sys_FlowerAppraise sys_FlowerAppraise = new Business.Sys_FlowerAppraise();
            StringBuilder sb = new StringBuilder();
            var gcount = 0; var bcount = 0;
            if (ArrangementId != "")
            {
               sb.Append(" and ArrangementId=" + ArrangementId);
               List<Model.FlowerAppraise> list= sys_FlowerAppraise.GetFlowerAppraiseCount(sb.ToString());
               gcount = list.Where(x => x.IsGood == "1").Count();
               bcount = list.Where(x => x.IsGood == "0").Count();
            }
            return Json(new { gcount=gcount,bcount=bcount }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 服务后提交
        /// </summary>
        [HttpPost]
        public ActionResult AddServerPhoto(Model.FlowerTreatment FlowerTreatment)
        {
            try
            {
                HttpPostedFileBase file = Request.Files["attach_paths"];
                FlowerTreatment.ChangePhoto = Utility.ChangeText.SaveUploadPicture(file, "Serveraf");
                FlowerTreatment.FlowerTreatmentType = "服务后";
                Business.Sys_FlowerTreatment Sys_FlowerTreatment = new Business.Sys_FlowerTreatment();                
                Utility.Log.WriteTextLog(" 服务后提交图", "ID", Request["id"], "路径", FlowerTreatment.ChangePhoto);
                if (Sys_FlowerTreatment.AddServerPhoto(FlowerTreatment))
                {                   
                    Response.Redirect("/MMain/GetArrangementInfo?ArrangementId="+ FlowerTreatment.CompanyName, true);                   
                    //return RedirectToAction("GetArrangementInfo", "MMain", new { ArrangementId = id });
                }
            }
            catch (Exception ex)
            {
                Utility.Log.WriteTextLog(" 服务后提交图", "", "", "", ex.ToString());
            }

            return View();
        }
        // <summary>
        /// 扫码页面中的上传图片养护
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload()
        {
            try
            {
                string FlowerArrangementId = Request["FlowerArrangementId"];
                string remarks = Request["remarks"];
                HttpPostedFileBase files = Request.Files["file"];
                if (files == null)
                {
                    Utility.Log.WriteTextLog("扫码页面上传图片养护", "FlowerArrangementId", FlowerArrangementId, "files", files == null ? "true" : "fasle");
                    return Json("Faild", JsonRequestBehavior.AllowGet);
                }
                string FilePath = Utility.ChangeText.SaveUploadPicture(files, "img");

                Business.Sys_FlowerArrangement Sys_FlowerArrangement = new Business.Sys_FlowerArrangement();
                Model.FlowerArrangement FlowerArrangement = Sys_FlowerArrangement.GetModel(FlowerArrangementId);
              
                Business.Sys_UserAdmin Sys_UserAdmin = new Business.Sys_UserAdmin();

                Business.Sys_FlowerTreatment Sys_FlowerTreatment = new Business.Sys_FlowerTreatment();              
                int userid = Utility.ChangeText.GetUsersId();
                Model.FlowerTreatment FTreatment = Sys_FlowerTreatment.GetModelbyUsersid(FlowerArrangement.ShopId.ToString(), FlowerArrangement.belongUsersId.ToString(), userid.ToString());
                if (FTreatment == null ||  FTreatment.time < DateTime.Now)//今天还没有提交
                {                 
               
                    Model.FlowerTreatment FlowerTreatment = new Model.FlowerTreatment();
                    FlowerTreatment.FlowerTreatmentType = "服务前";
                    FlowerTreatment.UsersId = userid;
                    FlowerTreatment.FlowerNumber = FlowerArrangement.ShopId.ToString();
                    FlowerTreatment.OwnedUsersId = FlowerArrangement.belongUsersId.ToString();
                    FlowerTreatment.UserRealName = Utility.ChangeText.GetRealName();
                    FlowerTreatment.FlowerTreatmentAddress = FlowerArrangement.OwnedCompany;
                    Model.UserAdmin UserAdmin = Sys_UserAdmin.GetUserAdminByUserId(Convert.ToInt32(FlowerTreatment.OwnedUsersId));
                    FlowerTreatment.OwnedUsersRealName = UserAdmin.RealName;
                    FlowerTreatment.OwnedCompany = UserAdmin.OwnedCompany;
                    FlowerTreatment.LogoPhoto = UserAdmin.LogoPhoto;
                    FlowerTreatment.Photo = FilePath;//提交图片
                    FlowerTreatment.ContentMsg = remarks;//提交内容
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
                    }
                    else
                    {
                        return Content("0");//今天已经上传
                    }
                    if(Session["RoleCode"].ToString() == "Customer" || Session["RoleCode"].ToString()=="Tourist")
                    {
                        Model.Wx_SendMsg Wx_SendMsg = new Model.Wx_SendMsg()
                        {
                            template_id = "MU4CvSNXPYTMjhGJdWuWNvpc5Ls2VPAmcaST4lWrTaM",
                            touser = Utility.ChangeText.GetOpenId(),
                            url = "http://www.thuay.com/MMain/GetArrangementInfo?way=Arrangement&ArrangementId=" + FlowerArrangementId,
                             data = new
                            {
                                first = new { value = "您好!已经有客户(" + FlowerTreatment.OwnedCompany + ")需要服务,请尽快前往。", color = "#173177" },
                                keyword1 = new { value = FlowerTreatment.FlowerNumber, color = "#173177" },
                                keyword2 = new { value = "养护花卉", color = "#173177" },
                                keyword3 = new { value = "养护", color = "#173177" },
                                keyword4 = new { value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), color = "#173177" },
                                remark = new { value = "更换内容:" + FlowerTreatment.ContentMsg + ".点击此消息,进行补录更换后图片。", color = "#173177" },
                            }
                        };
                        WxHelper.WxMain.SendMsg(JsonConvert.SerializeObject(Wx_SendMsg));
                    }
                    return Content("1");
                }
                else
                {
                    return Content("0");//今天已经上传
                }                
            }
            catch (Exception ex)
            {
                Utility.Log.WriteTextLog("扫码页面上传图片养护报错", "FlowerArrangementId", "MFlower", "Upload", ex.ToString());
                return null;
            }

        }
        #endregion

        #region 更换花卉
        /// <summary>
        /// 更换花卉
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// 更换花卉
        /// </summary>
        /// <returns></returns>
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
                FlowerChange.Photo = Utility.ChangeText.SaveUploadPicture(file, "change");

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
        /// <summary>
        /// 保存更换后图片
        /// </summary>
        public ActionResult AddFlowersPhotoInfo() 
        {
            Model.FlowerChange FlowerChange = new Model.FlowerChange() { Number = Request["Number"] };
            return View(FlowerChange);
        }
        /// <summary>
        /// 保存更换后图片
        /// </summary>
        /// <param name="FlowerChange"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddFlowersPhotoInfo(Model.FlowerChange FlowerChange)
        {
            try
            {
             
                HttpPostedFileBase file = Request.Files["attach_paths"];
                FlowerChange.ChangePhoto = Utility.ChangeText.SaveUploadPicture(file, "changeaf");
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
       
        #endregion

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
            else if (Session["RoleCode"].ToString() == "Customer")
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

        public ActionResult GetLocation()
        {           
            return View();
        }
        public ActionResult MapView()
        {
            return View();
        }
    }
}