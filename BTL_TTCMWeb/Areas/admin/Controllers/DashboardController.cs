using BTL_TTCMWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTL_TTCMWeb.Areas.admin.Controllers
{
    public class DashboardController : Controller
    {
        private readonly HAWContextEntities db = new HAWContextEntities();
        // GET: admin/Dasboard
        public ActionResult Index()
        {            
            return View();
        }
        public ActionResult listOrderNew()
        {
            var listOrderNew = db.tbl_Order.Where(x => x.tbl_state.state_name == "Mới");
            return View(listOrderNew);
        }
        public ActionResult ThongKe()
        {
            return View();
        }
    }
}