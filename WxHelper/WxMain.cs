using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections;
using System.Security.Cryptography;

namespace WxHelper
{
    /// <summary>
    /// 微信统一的方法入口
    /// </summary>
    public class WxMain
    {

        public class Getaccess_tokenClass
        {
            public string access_token { get; set; }
            public int expires_in { get; set; }

            public string openid { set; get; }
        }



        public class Getjsapi_ticketClass
        {
            public int errcode { get; set; }
            public string errmsg { get; set; }
            public string ticket { get; set; }
            public int expires_in { get; set; }
        }

        /// <summary>
        /// 获得微信access_token
        /// </summary>
        /// <returns></returns>
        public static string Getaccess_token()
        {
            try
            {
                if (Utility.CacheHelper.GetCache("access_token") != null)
                {
                    return Utility.CacheHelper.GetCache("access_token").ToString();
                }
                string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + WxConfigs.WxAppId + "&secret=" + WxConfigs.appsecret + "";
                string msg = Utility.PostData.GetData(url);
             
                string access_token = JsonConvert.DeserializeObject<Getaccess_tokenClass>(msg).access_token;
                Utility.CacheHelper.SetCache("access_token", access_token);
                return access_token;
            }
            catch (Exception  ex)
            {
                Utility.Log.WriteTextLog("抓取token", "", Utility.CacheHelper.GetCache("access_token")==null?"1":"2", "", ex.Message);
                return "";
            }
          
        }


        /// <summary>
        /// 获得用户openid
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public static string Getopenid(string Code)
        {
            string url = " https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + WxConfigs.WxAppId + "&secret=" + WxConfigs.appsecret + "&code=" + Code + "&grant_type=authorization_code ";
            string openid = JsonConvert.DeserializeObject<Getaccess_tokenClass>(Utility.PostData.GetData(url)).openid;
            return openid;
        }

        public static string GetTicket()
        {
            string apiurl = string.Empty;
            string detail = string.Empty;
            #region 通过 appid + appsecert 获取公众号的 access_token（不是用户的 access_token）
            if (Utility.CacheHelper.GetCache("jsapi_access_token") == null)
            {
                apiurl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + WxConfigs.WxAppId + "&secret=" + WxConfigs.appsecret;
                #region 通过传入的参数得到url请求
                detail = Utility.PostData.GetData(apiurl);
                //有可能出错
                //Utility.Log.WriteTextLog("获得ticket", "WxMain/GetTicket", detail, "", "");
                WX_JSSDK WX_JSSDK = JsonConvert.DeserializeObject<WX_JSSDK>(detail);
                if (WX_JSSDK.access_token != null)//为空时出错
                {
                    Utility.CacheHelper.SetCache("jsapi_access_token", WX_JSSDK.access_token);
                }
                #endregion
            }
                

            #endregion

            #region 获取ticket
            apiurl = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + Getaccess_token() + "&type=jsapi";
            detail = Utility.PostData.GetData(apiurl);
            WX_JSSDK WX_JSSDKS = JsonConvert.DeserializeObject<WX_JSSDK>(detail);
            Utility.CacheHelper.SetCache("jsapi_ticket", WX_JSSDKS.ticket);
            #endregion
            return WX_JSSDKS.ticket;

        }


        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <returns></returns>
        public static string SendMsg(string msg)
        {
            try
            {
                string api = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + Getaccess_token() + "";
                string msgg = Utility.PostData.Post(api, msg);
              
                return "";
            }
            catch (Exception  ex)
            {
                Utility.Log.WriteTextLog("发送模板消息异常", "", "", "", ex.ToString());
                return "";
            }
         
        }




        /// <summary>
        /// 获得随机字符串(当前作用于微信)
        /// </summary>
        /// <returns></returns>
        public static string getNoncestr()
        {
            Random random = new Random();
            return Utility.ChangeText.md5(random.Next(1000).ToString());
        }

        /// <summary>
        /// 获得时间戳(当前作用于微信)
        /// </summary>
        /// <returns></returns>
        public static string getTimestamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /// <summary>
        /// 按ASCII 码从小到大排序（字典序）(当前作用于微信部分)
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public static string getSignMsg(Dictionary<string, string> para)
        {

            StringBuilder sb = new StringBuilder();
            SortedList<string, string> SortedList = new SortedList<string, string>();
            foreach (KeyValuePair<string, string> pair in para)
            {
                SortedList.Add(pair.Key, pair.Value);
            }

            ArrayList Arraylist = new ArrayList();
            List<string> msgList = new List<string>();
            foreach (string key in para.Keys)
            {
                msgList.Add(key + "=" + para[key]);
            }
            msgList.Sort(string.CompareOrdinal);
            string result = "";
            foreach (string val in msgList)
            {
                result += val + "&";
            }
            //去掉末尾&
            return result.TrimEnd('&');
        }
        public partial class WX_JSSDK
        {
            public string access_token { get; set; }
            public string ticket { get; set; }
        }

    }
}
