using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Utility;

namespace ERP.Controllers
{
    public class LoginAuthController : ApiController
    {

        [HttpGet]
        [Route("api/LoginAuth/GetToken")]
        [AllowAnonymous]
        public IHttpActionResult GetJwtToken(string username, string pw)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(pw))
                return Json(new { result = "参数为空" });
            UserAdmin UserAdmin = new UserAdmin();
            UserAdmin.UserName = username;
            UserAdmin.PassWord = pw;
            Business.Sys_UserAdmin Sys_UserAdmin = new Business.Sys_UserAdmin();
            UserAdmin.PassWord = Utility.ChangeText.md5(UserAdmin.PassWord);
            UserAdmin = Sys_UserAdmin.AdminLogin(UserAdmin);
            if (UserAdmin != null)
            {
                JwtResult result;
                if (TokenHelper.GetToken(out result))
                {
                    return Json(new { result });
                }
            }
            return Json(new { result = "none" });
        }

        // GET api/values/getname?name=

        [ApiAuthorize]
        public string Getname(string name)
        {
            return name;
        }
       

       
    }
}
