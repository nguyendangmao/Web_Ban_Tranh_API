using BTL_TTCMWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTL_TTCMWeb.Controllers
{
    public class ContactController : Controller
    {
        //Lấy ra thông tìn liên hệ
        private readonly HAWContextEntities db = new HAWContextEntities();
        // GET: Contact
        public ActionResult Index()
        {
            return View(db.tbl_contact.FirstOrDefault());
        }
    }
}