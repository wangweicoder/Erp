using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ERP.Controllers
{
    public class ProblemsAndSuggestionsController : LoginFilter
    {
        //
        // GET: /ProblemsAndSuggestions/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetList(int limit, int offset, string UsersId, string Stateinfo)
        {
            Business.Sys_ProblemsAndSuggestions Sys_ProblemsAndSuggestions = new Business.Sys_ProblemsAndSuggestions();
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(UsersId))
            {
                sb.Append(" and UsersId='" + UsersId + "'");
            }
            if (!string.IsNullOrEmpty(Stateinfo))
            {
                  sb.Append(" and State='" + Stateinfo + "'");
            }
            return Json(new { total = Sys_ProblemsAndSuggestions.GetProblemsAndSuggestionsListCount(sb.ToString()), rows = Sys_ProblemsAndSuggestions.UserProblemsAndSuggestionsList(limit, offset, sb.ToString()) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateState() 
        {
            ViewData["idText"] = Request["id"];
            return View();
        }

        public ActionResult UpdateStateInfo() 
        {
            Business.Sys_ProblemsAndSuggestions Sys_ProblemsAndSuggestions = new Business.Sys_ProblemsAndSuggestions();
            if (Sys_ProblemsAndSuggestions.UpdateState(Request["State"], Request["id"]))
            {
                  return Content("1");
            }
            return Content("0");
        }
	}
}