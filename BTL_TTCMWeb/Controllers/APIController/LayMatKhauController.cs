using BTL_TTCMWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BTL_TTCMWeb.Controllers.APIController
{
    public class LayMatKhauController : ApiController
    {
        HAWContextEntities db = new HAWContextEntities();
        [HttpPut]
        public bool UpdatePass(string user_name,string user_password)
        {
            try
            {
                tbl_user tbl_Order = db.tbl_user.FirstOrDefault(n => n.user_name == user_name);
                if (tbl_Order != null)
                {
                    return false;
                }
                tbl_Order.user_password = user_password;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }   
        }
    }
}
