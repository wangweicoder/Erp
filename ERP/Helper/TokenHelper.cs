using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP
{
    public class TokenHelper
    {
        private static string secret = ConfigurationManager.AppSettings["TokenKey"];
        /// <summary>
        /// 获取JWT token
        /// </summary>
        /// <param name="token"></param>
        /// <param name="expireTime">过期时间</param>
        /// <returns></returns>
        public static bool GetToken(out JwtResult jwtResult, int expireTime = 20)
        {
            try
            {
                DateTime UTC = DateTime.Now;
                Dictionary<string, object> payload = new Dictionary<string, object>
                {
                    {"iat",ConvertDateTimeInt(UTC) },
                    {"iss","ERP@Oversea" },
                    {"exp",ConvertDateTimeInt(UTC.AddMinutes(expireTime)) }
                };

                IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
                IJsonSerializer serializer = new JsonNetSerializer();
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
                string result = encoder.Encode(payload, secret);
                string token = DESEncrypt.DesEncrypt(result);
                jwtResult = new JwtResult()
                {
                    JwtCode = token,
                    IsSuccess = true,
                    Message = "success"
                };
                return true;
            }
            catch (Exception e)
            {
                Utility.Log.WriteTextLog("JWTGetToken", "JWT.GetToken:", e.Message, "", "");
                jwtResult = new JwtResult()
                {
                    JwtCode = "",
                    IsSuccess = false,
                    Message = e.Message
                };
                return false;
            }

        }


        public static bool VaildateToken(string tokenkey, out TokenInfo json)
        {
            if (!string.IsNullOrEmpty(tokenkey))
            {
                try
                {
                    string token = DESEncrypt.DesDecrypt(tokenkey);
                    byte[] key = Encoding.UTF8.GetBytes(secret);
                    IJsonSerializer serializer = new JsonNetSerializer();
                    IDateTimeProvider provider = new UtcDateTimeProvider();
                    IJwtValidator validator = new JwtValidator(serializer, provider);
                    IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                    IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);
                    string result = decoder.Decode(token, key, true);
                    json = decoder.DecodeToObject<TokenInfo>(token, key, true);
                    if (json != null)
                    {
                        return true;
                    }
                }
                catch (Exception e)
                {
                    // ignored
                }
            }
            json = null;
            return false;
        }
        /// <summary>
        /// 转unix时间戳
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int ConvertDateTimeInt(DateTime date)
        {
            var startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return (int)(date - startTime).TotalSeconds;
        }
    }
    public class TokenInfo
    {
        public int iat { get; set; }
        public string iss { get; set; }
        public string audience { get; set; }
        public int exp { get; set; }
    }
    public class JwtResult
    {
       public string JwtCode { get; set; }
       public bool IsSuccess { get; set; }
       public string Message { get; set; }
    }
}
