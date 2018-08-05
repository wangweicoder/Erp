
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Utility
{
    /// <summary>
    /// 字符串处理类
    /// </summary>
    public class ChangeText
    {
        /// <summary>
        /// md5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string md5(string str)
        {
            if (string.IsNullOrEmpty(str)) return "";

            StringBuilder sBuilder = new StringBuilder();
            using (MD5 md5 = MD5.Create())
            {
                byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
                for (int i = 0; i < s.Length; i++)
                {
                    sBuilder.Append(s[i].ToString("X2"));
                }
            }
            return sBuilder.ToString().ToLower();
        }

        /// <summary>  
        /// SHA1 加密，返回大写字符串  
        /// </summary>  
        /// <param name="content">需要加密字符串</param>  
        /// <param name="encode">指定加密编码</param>  
        /// <returns>返回40位大写字符串</returns>  
        public static string SHA1(string content, Encoding encode)
        {
            try
            {
                SHA1 sha1 = new SHA1CryptoServiceProvider();
                byte[] bytes_in = encode.GetBytes(content);
                byte[] bytes_out = sha1.ComputeHash(bytes_in);
                sha1.Dispose();
                string result = BitConverter.ToString(bytes_out);
                result = result.Replace("-", "");
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("SHA1加密出错：" + ex.Message);
            }
        }
        /// <summary>
        /// 编号生成
        /// </summary>
        /// <returns></returns>
        public static string OrderIdCreate()
        {
            string ordersid = DateTime.Now.ToString("yyMMddHHmmssfff");
            System.Random Random = new System.Random();
            int Result = Random.Next(0, 999);
            return ordersid + Result;
        }

        /// <summary>
        /// 订单号生成
        /// </summary>
        /// <returns></returns>
        public static string GenerateOutTradeNo()
        {
            var ran = new Random();
            return string.Format("{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmssms"), ran.Next(999));
        }
        public static int GetUsersId()
        {
            //若读取不到账户信息
            if (HttpContext.Current.Session["UsersId"] != null)
            {
                return Convert.ToInt32(HttpContext.Current.Session["UsersId"]);
            }
            return 0;
        }

        public static string GetUserName()
        {
            //若读取不到账户信息
            if (HttpContext.Current.Session["UserName"] != null)
            {
                return HttpContext.Current.Session["UserName"].ToString();
            }
            return "";
        }

        public static string GetRealName()
        {
            //若读取不到账户信息
            if (HttpContext.Current.Session["RealName"] != null)
            {
                return HttpContext.Current.Session["RealName"].ToString();
            }
            return "";
        }
        public static string GetRoleCode()
        {
            //若读取不到账户信息
            if (HttpContext.Current.Session["RoleCode"] != null)
            {
                return HttpContext.Current.Session["RoleCode"].ToString();
            }
            return "";
        }

        public static string GetOpenId()
        {
            //若读取不到账户信息
            if (HttpContext.Current.Session["OpenId"] != null)
            {
                return HttpContext.Current.Session["OpenId"].ToString();
            }
            return "";
        }
        /// <summary>
        /// 保存图片(base64转图片)
        /// </summary>        
        /// <returns></returns>
        public static string SaveUploadFile(string base64, string folder)
        {
            if (base64 == null)
            {
                return "";
            }
            //

            System.Drawing.Image originalImg = null;   //原图


            byte[] bytes = Convert.FromBase64String(base64);
            MemoryStream memStream = new MemoryStream(bytes);
            BinaryFormatter binFormatter = new BinaryFormatter();
            Image img = (Image)binFormatter.Deserialize(memStream);
            originalImg = img;

            #region 大图
            var LargeFilePath = AppDomain.CurrentDomain.BaseDirectory + folder + "/Large";
            string LargelocalPath = Path.Combine(LargeFilePath, "jpg");
            string dire = Path.GetDirectoryName(LargelocalPath);
            if (!Directory.Exists(dire + "\\"))
            {
                Directory.CreateDirectory(dire + "\\");
            }
            string newname = Guid.NewGuid().ToString() + ".jpg";

            originalImg.Save(dire + "\\" + newname);
            //保存缩略图                   

            #endregion

            //string filename = Path.GetFileName(file.FileName);//文件名
            //string newname = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            //file.SaveAs(dire + "\\" + newname);
            return folder + "/Large/" + newname;
        }
        /// <summary>
        /// 保存文件 type img attach
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string SaveUploadPicture(HttpPostedFileBase file, string uploadType = "img")
        {
            try
            {
                if (uploadType == "img")
                {
                    string type = file.ContentType;
                    if (type.IndexOf("jpeg") > -1 || type.IndexOf("gif") > -1 || type.IndexOf("png") > -1)
                    {
                        string folder = "/Upload/Images";
                        return SaveUploadFile(file, folder, null);
                    }
                }
                else if (uploadType == "LogoPhoto")
                {
                    string type = file.ContentType;
                    if (type.IndexOf("jpeg") > -1 || type.IndexOf("gif") > -1 || type.IndexOf("png") > -1)
                    {
                        string folder = "/Upload/" + uploadType;
                        return SaveUploadFile(file, folder, null);
                    }
                }
                 else if (uploadType == "LogoPhoto")
                {
                    string type = file.ContentType;
                    if (type.IndexOf("jpeg") > -1 || type.IndexOf("gif") > -1 || type.IndexOf("png") > -1)
                    {
                        string folder = "/Upload/" + uploadType;
                        return SaveUploadFile(file, folder, null);
                    }
                }
                else if (uploadType == "AdvPic")
                {
                    string type = file.ContentType;
                    if (type.IndexOf("jpeg") > -1 || type.IndexOf("gif") > -1 || type.IndexOf("png") > -1)
                    {
                        string folder = "/Upload/" + uploadType;
                        return SaveUploadFile(file, folder, uploadType);
                    }
                }
                else if (uploadType == "xls")
                {
                    string type = file.ContentType;
                    if (type != null && type != "")
                    {
                        string folder = "/Excel";
                        return SaveUploadFileExcel(file, folder, "FlowerArrangement");
                    }
                }
                else if (uploadType == "xlt")
                {
                    string type = file.ContentType;
                    if (type != null && type != "")
                    {
                        string folder = "/Excel";
                        return SaveUploadFileExcel(file, folder, "Flower");
                    }
                }
                else
                {
                    string type = file.ContentType;
                    if (type != null && type != "")
                    {
                        string folder = "/Upload/Attach";
                        return SaveUploadFile(file, folder, null);
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="file"></param>
        /// <param name="folder"></param>
        /// <param name="httpurl"></param>
        /// <returns></returns>
        public static string SaveUploadFile(HttpPostedFileBase file, string folder, string type)
        {
            if (file == null || file.ContentLength == 0)
            {
                return "";
            }
            //           
            //System.IO.Stream myStream = null;
            //System.Drawing.Image originalImg = null;   //原图
            //System.Drawing.Image thumbImg = null;      //缩放图  
            //myStream = file.InputStream;//上传文件的Stream

            //originalImg = System.Drawing.Image.FromStream(myStream);//从数据流创建图片           


            #region 大图
            string LargeFilePath = AppDomain.CurrentDomain.BaseDirectory + folder + "/Large";
            string LargelocalPath = Path.Combine(LargeFilePath, file.FileName);
            string dire = Path.GetDirectoryName(LargelocalPath);
            if (!Directory.Exists(dire + "\\"))
            {
                Directory.CreateDirectory(dire + "\\");
            }
            string newname = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string filepath = dire + "\\" + newname;
            string newfilepath = dire + "\\" + "ThumbNail" + newname;
            file.SaveAs(filepath);
            int maxWidth = 400;  //最大宽度
            int maxHeight = 532;  //最大高度 
            if (type=="FlowerPhoto")
            {
                maxWidth = 600;
                maxHeight = 732;
            }
            RemoveRotateFlip(filepath, newfilepath,maxWidth,maxHeight);
            #endregion
            //            
            //string filename = Path.GetFileName(file.FileName);//文件名
            //string newname = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            //file.SaveAs(dire + "\\" + newname);
            return folder + "/Large/" + "ThumbNail" + newname;
        }
        /// <summary>  
        /// 移除图片的翻转旋转设置  
        /// </summary>  
        /// <param name="srcPathName">原文件名</param>  
        /// <param name="newPathName">新文件名</param>  
        public static void RemoveRotateFlip(string srcPathName, string newPathName, int maxWidth = 400, int maxHeight = 532)
        {
            Image image = new Bitmap(srcPathName);//初始化图片对象             
            foreach (PropertyItem p in image.PropertyItems)
            {
                if (p.Id == 0x112)
                {
                    RotateFlipType rft = p.Value[0] == 6 ? RotateFlipType.Rotate90FlipNone
                            : p.Value[0] == 3 ? RotateFlipType.Rotate180FlipNone
                            : p.Value[0] == 8 ? RotateFlipType.Rotate270FlipNone
                            : p.Value[0] == 1 ? RotateFlipType.RotateNoneFlipNone
                            : RotateFlipType.RotateNoneFlipNone;
                    p.Value[0] = 0;  //旋转属性值设置为不旋转  
                    image.SetPropertyItem(p); //回拷进图片流  
                    image.RotateFlip(rft);
                }

            }
                        
            System.Drawing.Image thumbImg = null;      //缩放图  
            thumbImg = PubClass.GetThumbNailImage(image, maxWidth, maxHeight);  //按宽、高缩放                
            thumbImg.Save(newPathName, ImageFormat.Jpeg); //重新保存为正常的图片 
            thumbImg.Dispose();
            image.Dispose(); //释放图片对象资源  
            PubClass.FileDel(srcPathName);//这里是否删除，根据业务需要定  
        }
        /// <summary>
        /// 保存excel
        /// </summary>
        /// <param name="file"></param>
        /// <param name="folder"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string SaveUploadFileExcel(HttpPostedFileBase file, string folder, string fileName)
        {
            if (file == null || file.ContentLength == 0)
            {
                return "";
            }
            var serverPath = AppDomain.CurrentDomain.BaseDirectory + folder;

            string localPath = Path.Combine(serverPath, file.FileName);
            string dire = Path.GetDirectoryName(localPath);
            if (!Directory.Exists(dire + "\\"))
            {
                Directory.CreateDirectory(dire + "\\");
            }

            string filename = Path.GetFileName(file.FileName);//文件名
            string newname = fileName + Path.GetExtension(file.FileName);
            file.SaveAs(dire + "\\" + newname);
            return folder + "/" + newname;
        }
    }
}
