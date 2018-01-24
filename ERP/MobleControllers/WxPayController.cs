using ERP.WxPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Xml;

namespace ERP.MobleControllers
{
    public class WxPayController : Controller
    {
        public static string wxJsApiParam { get; set; } //H5调起JS API参数
        //
        // GET: /WxPay/
        public ActionResult Index()
        {
            //JSAPI支付预处理
            try
            {
                string openid = Utility.ChangeText.GetOpenId();
                string total_fee = Request["PayTotal"].ToString();
                decimal Paytotal_fee = Convert.ToDecimal(total_fee) * 100;
                string OrdersId = Request["OrdersId"];
                ViewData["OrdersId"] = OrdersId;
                //检测是否给当前页面传递了相关参数

                //若传递了相关参数，则调统一下单接口，获得后续相关接口的入口参数
                JsApiPay jsApiPay = new JsApiPay();
                jsApiPay.openid = openid;
                jsApiPay.total_fee = Convert.ToInt32(Paytotal_fee);

                WxPayData unifiedOrderResult = jsApiPay.GetUnifiedOrderResult(OrdersId);
                wxJsApiParam = jsApiPay.GetJsApiParameters();//获取H5调起JS API参数   
                ViewData["WxPayInfo"] = wxJsApiParam;
                ERP.WxPay.Log.Debug(this.GetType().ToString(), "wxJsApiParam : " + wxJsApiParam);
            }
            catch (Exception ex)
            {
                Utility.Log.WriteTextLog("微信支付", "", "", "", ex.ToString());
            }
            return View();
        }


        public ActionResult PaySuccess()
        {
            try
            {
                string xml = Utility.PostData.PostInput();
                Utility.Log.WriteTextLog("微信支付回掉", "", "", "", xml);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);
                string out_trade_no = xmlDoc.SelectSingleNode("/xml/out_trade_no").InnerText;
                Business.Sys_OrdersManaage Sys_OrdersManaage = new Business.Sys_OrdersManaage();
                Sys_OrdersManaage.OrdersPay(out_trade_no, xmlDoc.SelectSingleNode("/xml/transaction_id").InnerText, "", 2);    
            }
            catch (Exception ex)
            {
                Utility.Log.WriteTextLog("微信支付回掉", "", "", "", ex.ToString());
            }
            return Content("<xml><return_code><![CDATA[SUCCESS]]></return_code><return_msg><![CDATA[OK]]></return_msg></xml>");
        }


        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class xml
        {

            private string appidField;

            private string attachField;

            private string bank_typeField;

            private string cash_feeField;

            private string fee_typeField;

            private string is_subscribeField;

            private string mch_idField;

            private string nonce_strField;

            private string openidField;

            private string out_trade_noField;

            private string result_codeField;

            private string return_codeField;

            private string signField;

            private string time_endField;

            private byte total_feeField;

            private string trade_typeField;

            private string transaction_idField;

            /// <remarks/>
            public string appid
            {
                get
                {
                    return this.appidField;
                }
                set
                {
                    this.appidField = value;
                }
            }

            /// <remarks/>
            public string attach
            {
                get
                {
                    return this.attachField;
                }
                set
                {
                    this.attachField = value;
                }
            }

            /// <remarks/>
            public string bank_type
            {
                get
                {
                    return this.bank_typeField;
                }
                set
                {
                    this.bank_typeField = value;
                }
            }

            /// <remarks/>
            public string cash_fee
            {
                get
                {
                    return this.cash_feeField;
                }
                set
                {
                    this.cash_feeField = value;
                }
            }

            /// <remarks/>
            public string fee_type
            {
                get
                {
                    return this.fee_typeField;
                }
                set
                {
                    this.fee_typeField = value;
                }
            }

            /// <remarks/>
            public string is_subscribe
            {
                get
                {
                    return this.is_subscribeField;
                }
                set
                {
                    this.is_subscribeField = value;
                }
            }

            /// <remarks/>
            public string mch_id
            {
                get
                {
                    return this.mch_idField;
                }
                set
                {
                    this.mch_idField = value;
                }
            }

            /// <remarks/>
            public string nonce_str
            {
                get
                {
                    return this.nonce_strField;
                }
                set
                {
                    this.nonce_strField = value;
                }
            }

            /// <remarks/>
            public string openid
            {
                get
                {
                    return this.openidField;
                }
                set
                {
                    this.openidField = value;
                }
            }

            /// <remarks/>
            public string out_trade_no
            {
                get
                {
                    return this.out_trade_noField;
                }
                set
                {
                    this.out_trade_noField = value;
                }
            }

            /// <remarks/>
            public string result_code
            {
                get
                {
                    return this.result_codeField;
                }
                set
                {
                    this.result_codeField = value;
                }
            }

            /// <remarks/>
            public string return_code
            {
                get
                {
                    return this.return_codeField;
                }
                set
                {
                    this.return_codeField = value;
                }
            }

            /// <remarks/>
            public string sign
            {
                get
                {
                    return this.signField;
                }
                set
                {
                    this.signField = value;
                }
            }

            /// <remarks/>
            public string time_end
            {
                get
                {
                    return this.time_endField;
                }
                set
                {
                    this.time_endField = value;
                }
            }

            /// <remarks/>
            public byte total_fee
            {
                get
                {
                    return this.total_feeField;
                }
                set
                {
                    this.total_feeField = value;
                }
            }

            /// <remarks/>
            public string trade_type
            {
                get
                {
                    return this.trade_typeField;
                }
                set
                {
                    this.trade_typeField = value;
                }
            }

            /// <remarks/>
            public string transaction_id
            {
                get
                {
                    return this.transaction_idField;
                }
                set
                {
                    this.transaction_idField = value;
                }
            }
        }


    }
}