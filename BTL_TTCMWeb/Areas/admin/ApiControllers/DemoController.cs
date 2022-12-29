using BTL_TTCMWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BTL_TTCMWeb.Areas.admin.ApiControllers
{
    public class DemoController : ApiController
    {
        HAWContextEntities db = new HAWContextEntities();
        DemoDataContext bd = new DemoDataContext();
        //Employee
        [Route("LayTaiKhoan")]
        [HttpGet]
        public List<tbl_admin> Getalladmin()
        {
            return db.tbl_admin.ToList();
        }
        [Route("getadmin/{admin_id}")]
        [HttpGet]
        public tbl_admin Getadmin(int admin_id)
        {
            return db.tbl_admin.FirstOrDefault(n => n.admin_id == admin_id);
        }
        [HttpPost]
        public bool InsertProduce(int admin_id, string admin_name, string admin_password, string admin_phone, string admin_email, Boolean admin_isEmployee)
        {
            try
            {
                tbl_admin tbl_Admin = new tbl_admin();
                tbl_Admin.admin_id = admin_id;
                tbl_Admin.admin_name = admin_name;
                tbl_Admin.admin_password = admin_password;
                tbl_Admin.admin_phone = admin_phone;
                tbl_Admin.admin_email = admin_email;
                tbl_Admin.admin_isEmployee = admin_isEmployee;
                db.tbl_admin.Add(tbl_Admin);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        //sua
        [HttpPut]
        public bool Updateemployee(int admin_id, string admin_name, string admin_password, string admin_phone, string admin_email, Boolean admin_isEmployee)
        {
            try
            {
                tbl_admin tbl_Admin = db.tbl_admin.FirstOrDefault(n => n.admin_id == admin_id);
                if (tbl_Admin == null) { return false; }
                tbl_Admin.admin_id = admin_id;
                tbl_Admin.admin_name = admin_name;
                tbl_Admin.admin_password = admin_password;
                tbl_Admin.admin_phone = admin_phone;
                tbl_Admin.admin_email = admin_email;
                tbl_Admin.admin_isEmployee = admin_isEmployee;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        //xoa
        [Route("deleteemployee/{admin_id}")]
        [HttpDelete]
        public bool Deleteemployee(int admin_id)
        {
            try
            {
                tbl_admin tbl_Admin = db.tbl_admin.FirstOrDefault(n => n.admin_id == admin_id);
                if (tbl_Admin == null) { return false; }
                db.tbl_admin.Remove(tbl_Admin);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        //Category
        [Route("LayDanhMuc")]
        [HttpGet]
        public List<tbl_category> GetallCategory()
        {
            return bd.tbl_categories.ToList();
        }
        [Route("LayDanhMuc/{category_id}")]
        [HttpGet]
        public tbl_category GetCategory(int category_id)
        {
            return bd.tbl_categories.FirstOrDefault(n => n.category_id == category_id);
        }
        [HttpPost]
        public bool InsertCategory(int category_id, string category_name, string category_parent)
        {
            try
            {
                tbl_category tbl_category = new tbl_category();
                tbl_category.category_id = category_id;
                tbl_category.category_name = category_name;
                tbl_category.category_parent = category_parent;
                bd.tbl_categories.InsertOnSubmit(tbl_category);
                bd.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        //sua
        [HttpPut]
        public bool UpdateCategory(int category_id, string category_name, string category_parent)
        {
            try
            {
                tbl_category tbl_category = bd.tbl_categories.FirstOrDefault(n => n.category_id == category_id);
                if (tbl_category == null) { return false; }
                tbl_category.category_id = category_id;
                tbl_category.category_name = category_name;
                tbl_category.category_parent = category_parent;
                bd.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        //User
        [Route("tttt")]
        [HttpGet]
        public List<Demo> GetAllUser()
        {
            return bd.Demos.ToList();
        }
        [Route("Ha/{user_name}")]
        [HttpGet]
        public Demo GetHa(string user_name)
        {
            return bd.Demos.FirstOrDefault(x=>x.user_name== user_name);
        }

        [Route("tttt/{quser_id}")]
        [HttpGet]
        public List<Demo2> GetUserByID(int quser_id)
        {
            return bd.Demo2s.Where(x=>x.user_id==quser_id).ToList();
        }
        [Route("Thongke/{user_id}")]
        [HttpGet]
        public Demo1 ThongKeByID(int user_id)
        {
            return bd.Demo1s.FirstOrDefault(x => x.user_id == user_id);
        }
        [Route("LayCTHD/{order_id}")]
        [HttpGet]
        public List<Demo3> LayCTHDByID(int order_id)
        {
            return bd.Demo3s.Where(x => x.oder_id == order_id).ToList();
        }


    }
}
