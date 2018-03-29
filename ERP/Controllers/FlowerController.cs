using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ERP.Controllers
{
    public class FlowerController : LoginFilter
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

        public ActionResult GetList()
        {
            int offset = Convert.ToInt32(Request["offset"]);
            string FlowerWatchName = Request["FlowerWatchName"];
            Business.Sys_Flower Sys_Flower = new Business.Sys_Flower();
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(FlowerWatchName))
            {
                sb.Append(" and FlowerWatchName='" + FlowerWatchName + "'");
            }
            return Json(new { total = Sys_Flower.GetFlowerListCount(sb.ToString()), rows = Sys_Flower.FlowerList(offset, sb.ToString()) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckFlowerWatchName() 
        {
            string FlowerWatchName = Request["FlowerWatchName"]; Business.Sys_Flower Sys_Flower = new Business.Sys_Flower();

            if (Sys_Flower.CheckFlowerWatchName(FlowerWatchName))
            {
                return Content("True");
            }
            return Content("False");
        }

        [HttpPost]
        public ActionResult Add(Model.Flower Flower) 
        {
            HttpPostedFileBase file = Request.Files["attach_path"];
            Flower.FlowerNumber = GetFlowerNum();
            Flower.FlowerWatchPhoto = Utility.ChangeText.SaveUploadPicture(file, "FlowerPhoto");
            Business.Sys_Flower Sys_Flower = new Business.Sys_Flower();
            //ViewData["success"] = "添加失败";
            if (Sys_Flower.InsertFlowerWatch(Flower))
            {
                //ViewData["success"] = "添加成功";
            }
            Response.Write("<script>parent.layer.closeAll();</script>");
            return View();
        }

        public ActionResult Edit() 
        {
            string id = Request["id"];
            Business.Sys_Flower Sys_Flower = new Business.Sys_Flower();
            return View(Sys_Flower.GetFlower(id));
        }
        
        [HttpPost]
        public ActionResult Edit(Model.Flower Flower)
        {
            Business.Sys_Flower Sys_Flower = new Business.Sys_Flower();
            //Model.Flower Flowers = Sys_Flower.GetFlowerByFlowerNumber(Flower.FlowerNumber);

            if (Request.Files["attach_path"] != null)
            {
                HttpPostedFileBase file = Request.Files["attach_path"];               
                if (!string.IsNullOrEmpty(file.FileName))
                {
                    DeleteFlowerPhoto(Flower.FlowerWatchPhoto);
                    Flower.FlowerWatchPhoto = Utility.ChangeText.SaveUploadPicture(file, "FlowerPhoto");
                }         
            }
            //else 
            //{
            //    Flower.FlowerWatchPhoto = Flowers.FlowerWatchPhoto;
            //}
            //ViewData["success"] = "修改失败";
            if (Sys_Flower.UpdateFlowerWatch(Flower))
            {
               // ViewData["success"] = "修改成功";
            }
            Response.Write("<script>parent.layer.closeAll();</script>");
            return View();
        }

        public ActionResult Delete() 
        {
            Business.Sys_Flower Sys_Flower = new Business.Sys_Flower();
            string id = Request["id"];
            Model.Flower Flower = Sys_Flower.GetFlower(id);
            if (Sys_Flower.DeleteFlowerWatch(id))
            {
                DeleteFlowerPhoto(Flower.FlowerWatchPhoto);
                return Content("True");
            }
            return Content("Fasle");
        }
        /// <summary>
        /// 删除花卉图片
        /// </summary>
        /// <param name="photourl"></param>
        private void DeleteFlowerPhoto(string photourl) 
        {
            string path = Server.MapPath("~") + photourl;
            //删除图片
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }
        public ActionResult CheckFlowerNumber() 
        {
            string FlowerNumber = Request["FlowerNumber"]; 
            Business.Sys_Flower Sys_Flower = new Business.Sys_Flower();
            if (Sys_Flower.CheckFlowerNumber(FlowerNumber))
            {
                return Content("True");
            }
            return Content("False");
        }


        public ActionResult FlowerCategory() 
        {
            ViewData["deptSelectItems"] = GetdeptSelectItems();
            return View();
        }

        public ActionResult GetFlowerCategoryList()
        {
            int offset = Convert.ToInt32(Request["offset"]);
            string FlowerCategoryType = Request["SelectItem"];
            Business.Sys_FlowerCategory Sys_FlowerCategory = new Business.Sys_FlowerCategory();
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(FlowerCategoryType))
            {
                sb.Append(" and id='" + FlowerCategoryType + "'");
            }
            return Json(new { total = Sys_FlowerCategory.GetFlowerCategoryListCount(sb.ToString()), rows = Sys_FlowerCategory.FlowerCategoryList(offset, sb.ToString()) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FlowerCategoryAdd() 
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult FlowerCategoryAdd(Model.FlowerCategory FlowerCategory) 
        {
            Business.Sys_FlowerCategory Sys_FlowerCategory = new Business.Sys_FlowerCategory();
            ViewData["success"] = "添加失败";
            if (Sys_FlowerCategory.Add(FlowerCategory.FlowerCategoryType))
            {
                ViewData["success"] = "添加成功";
            }
            Response.Write("<script>parent.layer.closeAll();</script>");
            return View();
        }

        public ActionResult FlowerCategoryEdit()
        {
            Business.Sys_FlowerCategory Sys_FlowerCategory = new Business.Sys_FlowerCategory();
            return View(Sys_FlowerCategory.Get(Request["id"]));
        }

        [HttpPost]
        public ActionResult FlowerCategoryEdit(Model.FlowerCategory FlowerCategory)
        {
            Business.Sys_FlowerCategory Sys_FlowerCategory = new Business.Sys_FlowerCategory();
            ViewData["success"] = "修改失败";
            if (Sys_FlowerCategory.Edit(FlowerCategory.FlowerCategoryType, FlowerCategory.id))
            {
                ViewData["success"] = "修改成功";
            }
            Response.Write("<script>parent.layer.closeAll();</script>");
            return View();
        }
        public ActionResult FlowerCategoryDel() 
        {
            Business.Sys_FlowerCategory Sys_FlowerCategory = new Business.Sys_FlowerCategory();
            if (Sys_FlowerCategory.Del(Request["ID"]))
            {
                 return Content("True");
            }
            return Content("False");
        }

        public ActionResult CheckFlowerCategory() 
        {
            string FlowerCategory = Request["FlowerCategory"]; Business.Sys_FlowerCategory Sys_FlowerCategory = new Business.Sys_FlowerCategory();

            if (Sys_FlowerCategory.Check(FlowerCategory))
            {
                return Content("True");
            }
            return Content("False");
        }
        public List<SelectListItem> GetdeptSelectItems()
        {
            Business.Sys_FlowerCategory Sys_FlowerCategory = new Business.Sys_FlowerCategory();
            List<Model.FlowerCategory> FlowerCategoryList = Sys_FlowerCategory.GetListModel();
            List<SelectListItem> deptSelectItems = new List<SelectListItem>();
            SelectListItem items = new SelectListItem();
            items.Text = "--请选择--";
            items.Value = "";
            items.Selected = true;
            deptSelectItems.Add(items);
            foreach (Model.FlowerCategory d in FlowerCategoryList)
            {
                SelectListItem item = new SelectListItem();
                item.Text = d.FlowerCategoryType;
                item.Value = d.id.ToString();
                item.Selected = false;
                deptSelectItems.Add(item);
            }
            return deptSelectItems;
        }


        public string GetFlowerNum() 
        {
            string num = DateTime.Now.ToString("yyyyMMddHHmmss");
            Random ran = new Random();
            int RandKey = ran.Next(10000, 99999);
            num = num + RandKey.ToString();
            return num;
        }
        /// <summary>
        /// 下载模板
        /// </summary>
        /// <returns></returns>
        public ActionResult DownExcel()
        {
            string filePath = Server.MapPath("~/Excel/Flowerdemo.xlt");//路径
            return File(filePath, "text/plain", "Flowerdemo.xlt"); //客户端保存的名字
        }
        public ActionResult AddByExcel()
        {
            return View();
        }
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
                string path = Utility.ChangeText.SaveUploadPicture(files, "xlt");
                Business.Sys_Flower Sys_Flower = new Business.Sys_Flower();
                DataTable dt = new DataTable();
                string templetpath = Server.MapPath("~") + path;
                dt = Utility.Excel.ExcelToTable(templetpath);
                if (dt.Rows.Count <= 0)
                {
                    Response.Write("<script>parent.layer.alert('导入失败!');</script>");
                    return View();
                }
                dt.Rows.RemoveAt(0);
                List<Model.Flower>list= Sys_Flower.GetFlowerList();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Model.Flower model = new Model.Flower();
                    model.FlowerNumber = GetFlowerNum();
                    var fname = dt.Rows[i][0].ToString();//检查名称重复
                    if (list.Count(m => m.FlowerWatchName.Contains(fname))<1) 
                    {
                        model.FlowerWatchName = fname;
                    }
                    model.FlowerWatchType = dt.Rows[i][1].ToString();
                    model.FlowerStock = Convert.ToInt32(dt.Rows[i][2].ToString());
                    model.FlowerCostPrice = Convert.ToInt32(dt.Rows[i][3].ToString());
                    model.FlowerSalesPrice = Convert.ToDecimal(dt.Rows[i][4].ToString());
                    model.FlowerIntroduction = dt.Rows[i][5].ToString();
                    model.XiXin = dt.Rows[i][6].ToString();
                    model.YangHuFangFa = dt.Rows[i][7].ToString();
                    Sys_Flower.InsertFlowerWatch(model);
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
                Utility.Log.WriteTextLog("导入Excel", "异常", ex.ToString(), "", "");
                Response.Write("<script>parent.layer.alert('表格格式或数据有误!');</script>");
                return View();
            }

        }
	}
}