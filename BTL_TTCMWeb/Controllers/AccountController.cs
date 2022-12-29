using BTL_TTCMWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTL_TTCMWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly HAWContextEntities db = new HAWContextEntities();
        // GET: Login
        public ActionResult Login()
        {
            //kiểm tra cookie người dùng
            HttpCookie cookie = Request.Cookies["hawuser_id"];
            if (cookie != null)
            {
                var userid = int.Parse(cookie.Value);
                var userLoggedIn = db.tbl_user.SingleOrDefault(x => x.user_id == userid);
                Session["username"] = userLoggedIn.user_name;
            }
            if (Session["username"] != null)
            {
                return RedirectToAction("Index", "User");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Login(tbl_user user)
        {
            var userLoggedIn = db.tbl_user.SingleOrDefault(x => x.user_name == user.user_name && x.user_password == user.user_password);

            if (userLoggedIn != null)
            {
                //create cookie
                HttpCookie userCookie = new HttpCookie("hawuser_id", userLoggedIn.user_id.ToString());
                //Expried date
                userCookie.Expires.AddDays(60);
                //Save data at cookie
                HttpContext.Response.SetCookie(userCookie);

                ViewBag.message = "loggedin";
                ViewBag.triedOnce = "yes";
                Session["username"] = user.user_name;

                //check clear cart
                HttpCookie cookie = Request.Cookies["cart"];
                if (cookie != null)
                {
                    var cartSession = cookie.Value;
                    var model = db.tbl_cart.FirstOrDefault(x => x.session_id == new Guid(cartSession));
                    if (model.account_id != null && model.account_id != userLoggedIn.user_id)
                    {
                        //cart của user khác => clear cookie cart
                        cookie.Expires = DateTime.Now.AddDays(-1);
                        Response.Cookies.Add(cookie);

                        //tạo TempData xóa cookie của cart
                        TempData["ClearCookieCart"] = true;
                    }
                }

                return RedirectToAction("Index", "User");
            }
            else
            {
                ViewBag.triedOnce = "yes";
                return View();
            }
        }

        public ActionResult Logout()
        {
            //clear session
            Session.Remove("username");

            //clear cookie người dùng hiện tại
            HttpCookie cookie = Request.Cookies["hawuser_id"];
            cookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(cookie);

            //clear cookie giỏ hàng hiện tại
            HttpCookie cookieCart = Request.Cookies["cart"];
            if (cookieCart != null)
            {
                cookieCart.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookieCart);
            }

            //return ra trang chủ
            return RedirectToAction("Index", "User");
        }
        public ActionResult LayMatKhau()
        {
            return View();  
        }
    }
}