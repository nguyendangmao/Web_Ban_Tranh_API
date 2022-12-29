using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTL_TTCMWeb.Areas.admin.Controllers
{
    public class Employee_Category_UserController : Controller
    {
        // GET: admin/Employee_Category_User
        public ActionResult Employess()
        {
            return View();
        }
        public ActionResult Category()
        {
            return View();
        }
        public ActionResult User()
        {
            return View();
        }
    }
}