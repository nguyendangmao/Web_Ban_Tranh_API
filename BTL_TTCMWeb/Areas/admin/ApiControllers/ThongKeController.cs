using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BTL_TTCMWeb.Areas.admin.ApiControllers
{
    public class ThongKeController : ApiController
    {
        DemoDataContext bd = new DemoDataContext();
        [Route("Abc/{year}")]
        [HttpGet]
        public List<BanTheoNam> LayTTB(int year)
        {
            return bd.BanTheoNams.Where(x => x.Nam == year).ToList();
        }
        [HttpGet]
        public List<TopSPbanchay> LayTT(int year, int month)
        {
            return bd.TopSPbanchays.Where(x => x.Nam == year && x.Thang == month).Take(4).ToList();
        }
    }
}
