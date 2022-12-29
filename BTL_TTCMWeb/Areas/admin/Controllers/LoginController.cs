using BTL_TTCMWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTL_TTCMWeb.Areas.admin.Controllers
{
    public class LoginController : Controller
    {
        private HAWContextEntities db = new HAWContextEntities();
        // GET: admin/Login
        public ActionResult Index()
        {
            //get data from cookie
            HttpCookie cookie = Request.Cookies["Employee"];
            if (cookie == null)
                return View();
            var EmployeeId = int.Parse(cookie.Value);
            var model = db.tbl_admin.FirstOrDefault(x => x.admin_id == EmployeeId);
            Session["NameEmployee"] = model.admin_name;
            Session["IsEmployee"] = model.admin_isEmployee;
            //return to dashboard
            return RedirectToAction("ThongKe", "Dashboard");
        }

        [HttpPost]
        public ActionResult Index(string username, string password)
        {
            var model = db.tbl_admin.FirstOrDefault(x => x.admin_email == username && x.admin_password == password);
            if (model != null)
            {
                //Create cookie
                HttpCookie userCookie = new HttpCookie("Employee", model.admin_id.ToString());
                //Expried date
                userCookie.Expires.AddDays(60);
                //Save data at cookie
                HttpContext.Response.SetCookie(userCookie);
                Session["NameEmployee"] = model.admin_name;
                Session["IsEmployee"] = model.admin_isEmployee;
                //return to dashboard
                return RedirectToAction("ThongKe", "Dashboard");
            }
            else
            {
                ModelState.AddModelError("Test", "Tên tài khoản hoặc mật khẩu sai. Vui lòng đăng nhập lại.");
            }

            return View();
        }
        public ActionResult LogOut()
        {
            HttpCookie cookie = Request.Cookies["Employee"];
            cookie.Expires = DateTime.Now.AddDays(-1);
            //HttpContext.Response.SetCookie(cookie);
            Response.Cookies.Add(cookie);
            return RedirectToAction("index", "Login");
        }
        
    }
}