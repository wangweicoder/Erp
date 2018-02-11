using ERP.Filter;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ThoughtWorks.QRCode.Codec;

namespace ERP.Controllers
{
    
    public class FlowerArrangementController : LoginFilter
    {
        //
        // GET: /FlowerArrangement/
        // [SysAuthAttribute]
        public ActionResult Index()
        {          
            return View();
        }

        /// <summary>
        /// 分页获得数据信息
        /// </summary>
        /// <param name="limit">页码大小</param>
        /// <param name="offset">从第几条数据开始</param>
        /// <param name="UserName">用户名</param>
        /// <param name="Role">角色ID</param>
        /// <returns></returns>
        public JsonResult GetList(int limit, int offset, string arrangement, int belongUsersId)
        {
            Business.Sys_FlowerArrangement Sys_FlowerArrangement = new Business.Sys_FlowerArrangement();
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(arrangement))
            {
                sb.Append(" and arrangement  like '%" + arrangement + "%'");
               
            }
            if (belongUsersId.ToString()!="0")
            {
                sb.Append(" and belongUsersId  = '" + belongUsersId + "'");
            }
            return Json(new { total = Sys_FlowerArrangement.GetListCount(sb.ToString()), rows = Sys_FlowerArrangement.GetList(limit, offset, sb.ToString()) }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Add()
        {
            ViewData["GetOwnedCompanyList"] = GetOwnedCompanyList();
            ViewData["GetShopList"] = GetShopList();
            return View();
        }


        [HttpPost]
        public ActionResult Add(Model.FlowerArrangement FlowerArrangement)
        {
            //ViewData["GetOwnedCompanyList"] = GetOwnedCompanyList();
            //ViewData["GetShopList"] = GetShopList();
            if (Request.Files["attach_path"] != null && Request.Files["attach_path"].ToString() != "")
            {
                HttpPostedFileBase file = Request.Files["attach_path"];
                FlowerArrangement.Photo = Utility.ChangeText.SaveUploadPicture(file, "attach");
            }
            Business.Sys_FlowerArrangement Sys_FlowerArrangement = new Business.Sys_FlowerArrangement();
            int id = Sys_FlowerArrangement.Add(FlowerArrangement);
            Sys_FlowerArrangement.UpdateImgORCodePath(CreateORCode(id), id);            
            return Content("");
        }

        public ActionResult Edit() 
        {
            Business.Sys_FlowerArrangement Sys_FlowerArrangement = new Business.Sys_FlowerArrangement();
            Model.FlowerArrangement FlowerArrangement = Sys_FlowerArrangement.GetModel(Request["id"]);
            ViewData["GetOwnedCompanyList"] = GetOwnedCompanyList(FlowerArrangement.belongUsersId); ViewData["GetShopList"] = GetShopList(FlowerArrangement.id);
            return View(FlowerArrangement);
        }

        [HttpPost]
        public ActionResult Edit(Model.FlowerArrangement FlowerArrangement)
        {
            //ViewData["GetOwnedCompanyList"] = GetOwnedCompanyList(FlowerArrangement.belongUsersId); ViewData["GetShopList"] = GetShopList(FlowerArrangement.id);
            Business.Sys_FlowerArrangement Sys_FlowerArrangement = new Business.Sys_FlowerArrangement();
            if (Request.Files["attach_path"] != null && Request.Files["attach_path"].ToString() != "")
            {
                HttpPostedFileBase file = Request.Files["attach_path"];
                FlowerArrangement.Photo = Utility.ChangeText.SaveUploadPicture(file, "attach");
            }
            Sys_FlowerArrangement.Edit(FlowerArrangement);
            if (string.IsNullOrEmpty(FlowerArrangement.ImgORCodePath))
               Sys_FlowerArrangement.UpdateImgORCodePath(CreateORCode(FlowerArrangement.id), FlowerArrangement.id);
            //Response.Write("<script>parent.layer.closeAll();</script>");
            return Content("");
        }

        public ActionResult Delete(string id) 
        {
            Business.Sys_FlowerArrangement Sys_FlowerArrangement = new Business.Sys_FlowerArrangement();            
            Model.FlowerArrangement FlowerArrangement = Sys_FlowerArrangement.GetModel(id);
            string path = Server.MapPath("~") + FlowerArrangement.ImgORCodePath;
            //删除二维码图片
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            Sys_FlowerArrangement.Delete(id);
            return Content("");
        }
        /// <summary>
        /// 获得公司json数据
        /// </summary>
        /// <author>wangwei</author>
        /// <returns></returns>
        public JsonResult GetCompanyList()
        {
            Business.Sys_UserAdmin Sys_UserAdmin = new Business.Sys_UserAdmin();
            List<Model.UserAdmin> UserAdminList = Sys_UserAdmin.GetAdminInfoList("Customer");
            List<SelectListItem> hourList = new List<SelectListItem>();           
            foreach (var item in UserAdminList)
            {                
                hourList.Add(new SelectListItem { Text = item.OwnedCompany, Value = item.ID.ToString() });
            }
            return Json(hourList,JsonRequestBehavior.AllowGet);
        }
        public List<SelectListItem> GetOwnedCompanyList(int  UsersId=0)
        {
            Business.Sys_UserAdmin Sys_UserAdmin = new Business.Sys_UserAdmin();
            List<Model.UserAdmin> UserAdminList = Sys_UserAdmin.GetAdminInfoList("Customer");
            List<SelectListItem> hourList = new List<SelectListItem>();          
            foreach (var item in UserAdminList)
            {
                if (UsersId!=0)
                {
                    if (item.ID==UsersId)
                    {
                        hourList.Add(new SelectListItem { Text = item.OwnedCompany, Value = item.ID.ToString(),Selected=true });
                        continue;
                    }
                }
                hourList.Add(new SelectListItem { Text = item.OwnedCompany, Value = item.ID.ToString() });
            }
            return hourList;
        }

        public List<SelectListItem> GetShopList(int ShopId = 0)
        {
            Business.Sys_Flower Sys_Flower = new Business.Sys_Flower();
            List<Model.Flower> FlowerList= Sys_Flower.GetFlowerList();
            List<SelectListItem> hourList = new List<SelectListItem>();
            foreach (var item in FlowerList)
            {
                if (ShopId != 0)
                {
                    if (item.id == ShopId)
                    {
                        hourList.Add(new SelectListItem { Text = item.FlowerWatchName+"("+item.FlowerWatchType+")", Value = item.id.ToString(), Selected = true });
                        continue;
                    }
                }
                hourList.Add(new SelectListItem { Text = item.FlowerWatchName + "(" + item.FlowerWatchType + ")", Value = item.id.ToString() });
                
            }
            return hourList;
        }

        // <summary>
        /// 扫码页面中的上传图片
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload()
        {
            try
            {
              
                string FlowerArrangementId = Request["FlowerArrangementId"];
                HttpPostedFileBase files = Request.Files["file"];
                Utility.Log.WriteTextLog("报错", "", "", "", files == null ? "true" : "fasle");
                if (files == null) return Json("Faild", JsonRequestBehavior.AllowGet);

                
                Business.Sys_FlowerArrangement Sys_FlowerArrangement = new Business.Sys_FlowerArrangement();
                Model.FlowerArrangement FlowerArrangement = Sys_FlowerArrangement.GetModel(FlowerArrangementId);

                string FilePath = Utility.ChangeText.SaveUploadPicture(files, "img");

               // Sys_FlowerArrangement.UpdateUploadImg(FilePath, int.Parse(FlowerArrangementId));
                Business.Sys_FlowerChange Sys_FlowerChange = new Business.Sys_FlowerChange();
                Model.FlowerChange FlowerChange = new Model.FlowerChange();
                //增加一条更换记录
                FlowerChange.WorkUsersId = Utility.ChangeText.GetUsersId();
                FlowerChange.WorkUsersRealName = Utility.ChangeText.GetRealName();
                FlowerChange.OwnedUsersId = FlowerArrangement.belongUsersId;
                FlowerChange.OwnedCompany = FlowerArrangement.OwnedCompany;

                FlowerChange.FlowerTreatmentType = "更换花卉";
                FlowerChange.UsersId = Utility.ChangeText.GetUsersId();
                FlowerChange.Photo = FilePath;
                FlowerChange.Number = Utility.ChangeText.OrderIdCreate();
                FlowerChange.State = "未更换";


                FlowerChange.FlowerType = FlowerArrangement.FlowerType;
                FlowerChange.PlacingPosition = FlowerArrangement.arrangement;
                FlowerChange.time = DateTime.Now;
                FlowerChange.Sum = 1;
                Sys_FlowerChange.InsertFlowerChange(FlowerChange);
                Model.Wx_SendMsg Wx_SendMsg = new Model.Wx_SendMsg()
                {
                    template_id = "MU4CvSNXPYTMjhGJdWuWNvpc5Ls2VPAmcaST4lWrTaM",
                    touser = Utility.ChangeText.GetOpenId(),
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
                return Json(new { result = "OK", msg = "更换花卉成功" }, "text/html", JsonRequestBehavior.AllowGet);
            }
            catch (Exception  ex)
            {
                Utility.Log.WriteTextLog("报错","","","",ex.ToString());
                return null;
            }
          
        }
        /// <summary>
        /// 获取文件大小
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private string GetFileSize(long bytes)
        {
            long kblength = 1024;
            long mbLength = 1024 * 1024;
            if (bytes < kblength)
                return bytes.ToString() + "B";
            if (bytes < mbLength)
                return decimal.Round(decimal.Divide(bytes, kblength), 2).ToString() + "KB";
            else
                return decimal.Round(decimal.Divide(bytes, mbLength), 2).ToString() + "MB";
        }


        public string CreateORCode(int ArrangementId) 
        {
            string url = "http://www.thuay.com/MMain/GetArrangementInfo?way=Arrangement&ArrangementId=" + ArrangementId;
            Bitmap bt;
            string enCodeString = url;
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            bt = qrCodeEncoder.Encode(enCodeString, Encoding.UTF8);

            string filename = DateTime.Now.ToString("yyyymmddhhmmss");
            string path = Server.MapPath("/Upload/Attach/") + filename + ".jpg";
            bt.Save(path);
            return "/Upload/Attach/" + filename + ".jpg";
        }

        public ActionResult AddByExcel() 
        {
            return View();
        }
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <author>wangwei</author>
        /// <returns></returns>
        public void DownloadExcel(string ids, int limit, int offset, string arrangement, int belongUsersId)
        {
            var tableHeaderTexts = new string[] { "摆放位置", "规格(M)", "单价", "数量", "合计", "所属公司", "对应花卉名称", "对应花卉品种", "备注" };
            List<string> list=tableHeaderTexts.ToList();
            Business.Sys_FlowerArrangement Sys_FlowerArrangement = new Business.Sys_FlowerArrangement();
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(ids))
            {
                sb.Append(" and f1.Id  in(" + ids + ")");
            }
            if (!string.IsNullOrEmpty(arrangement))
            {
                sb.Append(" and arrangement  like '%" + arrangement + "%'");

            }
            if (belongUsersId.ToString() != "0")
            {
                sb.Append(" and belongUsersId  = '" + belongUsersId + "'");
            }
            List<Model.FlowerArrangement> flist= Sys_FlowerArrangement.GetList(limit, offset, sb.ToString());
            DataTable dt = new DataTable();
            dt = Utility.ExtensionMethods.ToDataTable(flist);
            #region 删除不需要显示的列
            dt.Columns.Remove("id");
            dt.Columns.Remove("Photo");
            dt.Columns.Remove("belongUsersId");
            dt.Columns.Remove("FlowerType");
            dt.Columns.Remove("Weekly");
            dt.Columns.Remove("ShopId");
            dt.Columns.Remove("ImgORCodePath");
            dt.Columns.Remove("XiXin");
            dt.Columns.Remove("YangHuFangFa");
            dt.Columns.Remove("FlowerSalesPrice");
            #endregion
            Utility.Excel.ExplorerExcel(dt,list);
        }
        /// <summary>
        /// 导出二维码
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="arrangement"></param>
        /// <param name="belongUsersId"></param>
        public ActionResult DownloadOrCode(string ids, int limit, int offset, string arrangement, int belongUsersId)
        {
            Business.Sys_FlowerArrangement Sys_FlowerArrangement = new Business.Sys_FlowerArrangement();
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(ids))
            {
                sb.Append(" and f1.Id  in(" + ids + ")");
            }
            if (!string.IsNullOrEmpty(arrangement))
            {
                sb.Append(" and arrangement  like '%" + arrangement + "%'");

            }
            if (belongUsersId.ToString() != "0")
            {
                sb.Append(" and belongUsersId  = '" + belongUsersId + "'");
            }
            List<Model.FlowerArrangement> flist = Sys_FlowerArrangement.GetList(limit, offset, sb.ToString());
            try
            {
                Task task = new Task(()=> CreateImg(flist));
                task.Start();
                while (!task.IsCompleted)
                {
                    task.Wait();
                }
                string weburl=System.Configuration.ConfigurationManager.AppSettings["WebImgUrl"];
                string localurl = System.Configuration.ConfigurationManager.AppSettings["localurl"];
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    string dir = Server.MapPath("/Upload/OrCodepic");                   
                    DirectoryInfo theFolder = new DirectoryInfo(dir);
                    //获得文件列表并按时间倒序
                    var fileInfo = theFolder.GetFiles().OrderByDescending(x => x.CreationTime);
                    foreach (FileInfo NextFile in fileInfo)  //遍历文件
                    {
                        DownloadOneFileByURLWithWebClient(NextFile.Name, weburl, localurl);
                    }
                }
                return Json(new {code="1",msg=localurl },JsonRequestBehavior.AllowGet);
                //return File(filePath, "text/plain", "demo.zip"); //客户端保存的名字
            }
            catch (Exception ex)
            {
                return Json(new { code = "0", msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        
        /// <summary>
        /// 保存图片文件
        /// </summary>
        /// <param name="flist"></param>
        private void CreateImg(List<Model.FlowerArrangement> flist)
        {
            for (int i = 0; i < flist.Count; i++)
            {
                string path = flist[i].ImgORCodePath;
                int index = path.IndexOf(".");
                string exc = path.Substring(index, path.Length - index);
                string filename = flist[i].OwnedCompany + flist[i].arrangement + flist[i].FlowerWatchName + exc;//获得图片的真实名字
                string dir = Server.MapPath("/Upload/OrCodepic");
                string targetPath = dir + "/" + filename;
                if (!System.IO.Directory.Exists(dir))
                { // 目录不存在，建立目录  
                    System.IO.Directory.CreateDirectory(dir);
                }
                string abpath = Server.MapPath("~") + path;
                System.IO.File.Copy(abpath, targetPath, true);
            }

        }
        
        /// <summary>
        /// 下载到指定路径
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="url"></param>
        /// <param name="localPath"></param> 
        public  static  void  DownloadOneFileByURLWithWebClient(string  fileName,  string  url,  string  localPath)
        {
            System.Net.WebClient wc  =   new  System.Net.WebClient();   
             if (System.IO.File.Exists(localPath  +  fileName))
             {
                 System.IO.File.Delete(localPath  +  fileName);
             }
             if  (Directory.Exists(localPath)  ==   false )  
             { 
                 Directory.CreateDirectory(localPath);
             }
             wc.DownloadFile(url  +  fileName, localPath  +  fileName);
         }

         //public string zip() 
         //{
         //    //每次下载的时候都会把文件放到这个文件夹下然后前台通过这个地址进行下载，所以我每次都会删除上次导出时候创建的文件夹，然后在重新创建，这样做的效果就是这个文件夹里永远只有一个最新的文件；
         //    //这里放的是jpeg格式的图片，因为我从服务器上获取的不是jpeg的文件，是dicm的文件，我需要把这个dicm文件转换成jpeg转换成之后，我需要临时存放到这个文件夹里面以备后面压缩。
         //    string weburl = "d:\\webtupian";
         //    if (Directory.Exists(weburl))
         //    {
         //        DirectoryInfo direct = new DirectoryInfo(weburl);
         //        direct.Delete(true);
         //    }
         //    //这个是压缩包zip存放的地方
         //    string lurl = HttpContext.Server.MapPath("../linshi");
         //    if (Directory.Exists(lurl))
         //    {
         //        DirectoryInfo direct1 = new DirectoryInfo(lurl);
         //        direct1.Delete(true);
         //    }
         //    string modality = "";
         //    if (request.Params["modality"] != null && request.Params["modality"].ToString() != "")
         //    {
         //        modality = request.Params["modality"].ToString();
         //    }
         //    //获取特定的数据集信息
         //    DataSet ds = dall.getimgdaochu(where, modality);
         //    StringBuilder html = new StringBuilder();

         //    string zipedFile = string.Empty;
         //    List<Byte[]> lstFI = new List<Byte[]>();
         //    List<string> listimage = new List<string>();
         //    byte[] img = null;
         //    if (ds != null && ds.Tables[0].Rows.Count > 0)
         //    {
         //        int i = 1;
         //        foreach (DataRow row in ds.Tables[0].Rows)
         //        {
         //            context.Response.ClearContent();
         //            string dcm_path = row["REFERENCE_FILE"].ToString();//这是dicm文件的地址
         //            string share_ip = SYS_APP_CONFIG.GetAppConfigValue("dcm_share_ip");//这是服务器的ip地址
         //            zipedFile = row["PATIENT_ID"].ToString();//这是数据的编号。
         //            string tuname = row["PATIENT_NAME"].ToString();//这是当前这个数据人的名字
         //            img = DicomUtility.GetJpegFromDcm(dcm_path, System.Drawing.Imaging.ImageFormat.Jpeg);//这是通过dicm文件的地址和ip什么的，把dicm文件转换成图片格式的byte字节。
         //            Image imgg = BytToImg(img);//这是通过byte字节获取jpeg的图片
         //            zipname = tupian_url + tuname + i.ToString() + "张";//压缩zip的名字
         //            url = "d:\\webtupian";
         //            if (!Directory.Exists(url))
         //            {
         //                Directory.CreateDirectory("d:\\webtupian");//创建新路径
         //            }
         //            string name = tuname + i.ToString();
         //            imgg.Save("d:\\webtupian" + "/" + name + ".jpeg");//图片保存
         //            listimage.Add("d:\\webtupian" + "/" + name + ".jpeg");//把当前图片保存的路径保存起来，用于后面的zip通过路径压缩文件。
         //            i++;
         //            imgg.Dispose();
         //        }
         //        //压缩成zip，需要下载ionic的dll然后引用。
         //        //zipname这个是zip的名字
         //        using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile(zipname, Encoding.Default))
         //        {
         //            foreach (string fileToZip in listimage)
         //            {
         //                using (FileStream fs = new FileStream(fileToZip, FileMode.Open, FileAccess.ReadWrite))//根据路径把文件转换成文件流
         //                {
         //                    byte[] buffer = new byte[fs.Length];//把文件流转换成byte字节
         //                    fs.Read(buffer, 0, buffer.Length);
         //                    string fileName = fileToZip.Substring(fileToZip.LastIndexOf("\\") + 1);
         //                    zip.AddEntry(fileName, buffer);
         //                }
         //            }
         //            if (!Directory.Exists(HttpContext.Current.Server.MapPath("../linshi")))
         //            {
         //                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("../linshi"));//创建新路径
         //            }
         //            zip.Save(HttpContext.Current.Server.MapPath("../linshi/" + zipname + ".zip"));
         //        }
         //        //为了防止乱码，把数据封装了一下，到前台用unescape()解析就可以了。
         //        return Microsoft.JScript.GlobalObject.escape(zipname + "1");//返回zip的名字和拼接的字符串1,1是代表有数据可以导出
         //    }
         //    else
         //    {
         //        return "当前该人员没有图片可供导出。2";
         //    }
         
         //}
        /// <summary>
        /// 导入Excel文件
        /// </summary>
        /// <param name="files"></param>
        /// <author>wangwei</author>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddByExcel(HttpPostedFileBase files) 
        {
            try
            {
                files = Request.Files["file"];
                string path= Utility.ChangeText.SaveUploadPicture(files, "xls");
                Business.Sys_FlowerArrangement Sys_FlowerArrangement = new Business.Sys_FlowerArrangement();
                DataTable dt = new DataTable();
                string templetpath = Server.MapPath("~") + path;
                dt = Utility.Excel.ExcelToTable(templetpath);
                if (dt.Rows.Count <= 0)
                {
                    Response.Write("<script>parent.layer.alert('导入失败!');</script>");
                    return View();
                }
                dt.Rows.RemoveAt(0);
                //公司
                Business.Sys_UserAdmin Sys_UserAdmin = new Business.Sys_UserAdmin();
                List<Model.UserAdmin> UserAdminList = new List<Model.UserAdmin>();
                UserAdminList = Sys_UserAdmin.GetAdminInfoList("Customer");
                //花卉
                List<Model.Flower> FlowerList =new Business.Sys_Flower().GetFlowerList();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Model.FlowerArrangement model = new Model.FlowerArrangement();
                    model.arrangement = dt.Rows[i][0].ToString();
                    model.Specifications = dt.Rows[i][1].ToString();
                    model.UnitPrice = Convert.ToDecimal(dt.Rows[i][2]);
                    model.Count = Convert.ToInt32(dt.Rows[i][3].ToString());
                    model.Total = Convert.ToDecimal(dt.Rows[i][4].ToString());
                    if (UserAdminList.Count(m => m.OwnedCompany == dt.Rows[i][5].ToString()) > 0)
                    {
                        model.belongUsersId = UserAdminList.Where(m => m.OwnedCompany == dt.Rows[i][5].ToString()).ToList()[0].ID;
                    }
                    if (FlowerList.Count(n => n.FlowerWatchName == dt.Rows[i][6].ToString()) > 0)
                    {
                        model.ShopId = FlowerList.Where(n => n.FlowerWatchName.Contains(dt.Rows[i][6].ToString())).ToList()[0].id;
                    }                   
                    model.Remark = dt.Rows[i][8].ToString();
                   int id=Sys_FlowerArrangement.Add(model);
                   Sys_FlowerArrangement.UpdateImgORCodePath(CreateORCode(id),id);
                }
                //删除上传的导入文件
                if (System.IO.File.Exists(templetpath))
                {
                    System.IO.File.Delete(templetpath);
                }
                Response.Write("<script>parent.layer.alert('导入成功!');parent.layer.closeAll();</script>");               
                return View();
            }
            catch (Exception ex)
            {
                Utility.Log.WriteTextLog("导入Excel","异常",ex.ToString(),"","");
                Response.Write("<script>parent.layer.alert('表格格式或数据有误!');</script>");
                return View();
            }
           
        }

        /// <summary>
        /// 下载模板
        /// </summary>
        /// <returns></returns>
        public ActionResult LownExcel() 
        {
            string filePath = Server.MapPath("~/Excel/demo.xlt");//路径
            return File(filePath, "text/plain", "demo.xlt"); //客户端保存的名字
        }
    }
}