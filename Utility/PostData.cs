using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Utility
{
    /// <summary>
    /// Post请求方法类
    /// </summary>
    public  class PostData
    {

        public static string GetData(string url)
        {
            try
            {
                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "GET";
                HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                StreamReader sr = new StreamReader(httpResponse.GetResponseStream(), System.Text.Encoding.GetEncoding("utf-8"));
                string result = sr.ReadToEnd();
                result = result.Replace("\r", "").Replace("\n", "").Replace("\t", "");
                //int status = (int)httpResponse.StatusCode;
                sr.Close();

                return result;
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// 下载Url文件类
        /// </summary>
        /// <param name="downLoadUrl">文件的url路径</param>
        /// <param name="saveFolder">需要保存在本地的路径</param>
        /// <param name="saveFullName">需要保存在本地的文件名</param>
        /// <param name="saveFullName">需要保存在本地的文件类型</param>
        /// <returns></returns>
        public static bool DownloadFile(string downLoadUrl, string saveFolder, string saveName, string FolderType)
        {
            bool flagDown = false;
            System.Net.HttpWebRequest httpWebRequest = null;
            try
            {
                //根据url获取远程文件流
                httpWebRequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(downLoadUrl);

                System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)httpWebRequest.GetResponse();
                System.IO.Stream sr = httpWebResponse.GetResponseStream();

                //创建文件侠
                if (!Directory.Exists(saveFolder))
                {
                    Directory.CreateDirectory(saveFolder);
                }

                //创建本地文件写入流
                System.IO.Stream sw = new System.IO.FileStream(saveFolder + saveName + FolderType, System.IO.FileMode.Create);

                long totalDownloadedByte = 0;
                byte[] by = new byte[1024];
                int osize = sr.Read(by, 0, (int)by.Length);
                while (osize > 0)
                {
                    totalDownloadedByte = osize + totalDownloadedByte;
                    sw.Write(by, 0, osize);
                    osize = sr.Read(by, 0, (int)by.Length);
                }
                System.Threading.Thread.Sleep(100);
                flagDown = true;
                sw.Close();
                sr.Close();
            }
            finally
            {
                if (httpWebRequest != null)
                    httpWebRequest.Abort();
            }
            return flagDown;
        }

        /// <summary>
        /// post提交数据 XML
        /// </summary>
        /// <param name="PostDate">//这是要post的数据</param>
        /// <param name="Uri">提交的url</param>

        public static string PostDataByxml(string Uri, string Parameter)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(Parameter);
                Uri uRI = new Uri(Uri);
                HttpWebRequest req = WebRequest.Create(uRI) as HttpWebRequest;
                req.Method = "POST";
                req.KeepAlive = true;
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = data.Length;
                req.AllowAutoRedirect = true;
                req.ServicePoint.ConnectionLimit = int.MaxValue;
                Stream outStream = req.GetRequestStream();
                outStream.Write(data, 0, data.Length);
                outStream.Close();
                HttpWebResponse res = req.GetResponse() as HttpWebResponse;
                Stream inStream = res.GetResponseStream();
                StreamReader sr = new StreamReader(inStream, Encoding.UTF8);
                string htmlResult = sr.ReadToEnd();
                System.GC.Collect();
                res.Close();
                return htmlResult;//返回值
            }
            catch (Exception ex)
            {
                return "网络错误：" + ex.Message.ToString();
            }
        }
       
        /// <summary>
        /// HTTP协议发起POST请求 方法体必须为JSON
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Post(string url, string data)
        {
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Method = "POST";
                webRequest.ContentType = "application/json; charset=utf-8";
                webRequest.Accept = "application/json; charset=utf-8";
                webRequest.Timeout = 200000;
                var requestByte = Encoding.UTF8.GetBytes(data);
                using (var requestStream = webRequest.GetRequestStream())
                {
                    requestStream.Write(requestByte, 0, requestByte.Length);
                }
                using (var webResponse = webRequest.GetResponse())
                {
                    var responseStream = webResponse.GetResponseStream();
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        var responseText = reader.ReadToEnd();
                        return responseText;
                    }
                }
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string PostInput()
        {
            try
            {
                Stream s = HttpContext.Current.Request.InputStream;
                int count = 0;
                byte[] buffer = new byte[1024];
                StringBuilder builder = new StringBuilder();
                while ((count = s.Read(buffer, 0, 1024)) > 0)
                {
                    builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
                }
                s.Flush();
                s.Close();
                s.Dispose();
                return builder.ToString();
            }
            catch (Exception ex)
            {
                return "101";
            }
        }
    }
}
