using BTL_TTCMWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BTL_TTCMWeb.Areas.admin.ApiControllers
{
    public class DefaultController : ApiController
    {
        
        HAWContextEntities db = new HAWContextEntities();
        DemoDataContext bd = new DemoDataContext();
        //Question
        [Route("LayCauHoi")]
        public List<tbl_question> Getquestion()
        {
            return bd.tbl_questions.ToList();
        }
        //get 1 state
        [Route("getquestion/{question_id}")]
        [HttpGet]
        public tbl_question Getstate(int question_id)
        {
            return bd.tbl_questions.FirstOrDefault(n => n.question_id == question_id);
        }
        [HttpPost]
        public bool Insertquestion(int question_id, string question)
        {
            try
            {
                tbl_question tbl_Question = new tbl_question();
                tbl_Question.question_id = question_id;
                tbl_Question.question = question;
                bd.tbl_questions.InsertOnSubmit(tbl_Question);
                bd.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        [HttpPut]
        public bool Updatequestion(int question_id, string question)
        {
            try
            {
                tbl_question tbl_Question = bd.tbl_questions.FirstOrDefault(n => n.question_id == question_id);
                if (tbl_Question == null) { return false; }
                tbl_Question.question_id = question_id;
                tbl_Question.question = question;
                bd.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        //State
        [Route("Laytrangthai")]
        [HttpGet]
        public List<tbl_state> GetallState()
        {
            return bd.tbl_states.ToList();
        }
        [Route("Laytrangthai/{state_id}")]
        [HttpGet]
        public tbl_state GetallState(int state_id)
        {
            return bd.tbl_states.FirstOrDefault(x=>x.state_id== state_id);
        }
        //them
        [HttpPost]
        public bool InsertColor(int state_id, string state_name)
        {
            try
            {
                tbl_state state = new tbl_state();
                state.state_id = state_id;
                state.state_name = state_name;

                bd.tbl_states.InsertOnSubmit(state);
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
        public bool UpdateState(int state_id, string state_name)
        {
            try
            {
                tbl_state state = bd.tbl_states.FirstOrDefault(n => n.state_id == state_id);
                if (state == null) { return false; }
                state.state_id = state_id;
                state.state_name = state_name;
                bd.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        //Color
        [Route("LayMau")]
        [HttpGet]
        public List<tbl_color> GetallColor()
        {
            return bd.tbl_colors.ToList();
        }
        [Route("LayMau/{color_id}")]
        [HttpGet]
        public tbl_color GetallColor(int color_id)
        {
            return bd.tbl_colors.FirstOrDefault(x => x.color_id == color_id);
        }
        //them
        [HttpPost]
        public bool InsertColor(int color_id, string color_name, string color_img)
        {
            try
            {
                tbl_color color = new tbl_color();
                color.color_id = color_id;
                color.color_name = color_name;
                color.color_img = color_img;

                bd.tbl_colors.InsertOnSubmit(color);
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
        public bool UpdateColor(int color_id, string color_name, string color_img)
        {
            try
            {
                tbl_color color = bd.tbl_colors.FirstOrDefault(n => n.color_id == color_id);
                if (color == null) { return false; }
                color.color_id = color_id;
                color.color_name = color_name;
                color.color_img = color_img;
                bd.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        //Contact
        [Route("LayLienHe")]
        public List<tbl_contact> GetContacts()
        {
            return db.tbl_contact.ToList();
        }
        [Route("LayLienHe/{id}")]
        public tbl_contact GetContacts(int id)
        {
            return db.tbl_contact.Where(x=>x.id==id).FirstOrDefault();
        }
        [Route("XoaLienHe/{id}")]
        [HttpDelete]
        public bool DeleteSP(int id)
        {
            try
            {
                tbl_contact contact = db.tbl_contact.Where(x => x.id == id).FirstOrDefault();
                if (contact == null)
                {
                    return false;
                }
                db.tbl_contact.Remove(contact);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        [HttpPut]
        public bool Updatecontact(int id, string name, string email, string phone, string address, string comment)
        {
            try
            {
                tbl_contact tbl_Contact = db.tbl_contact.FirstOrDefault(n => n.id == id);
                if (tbl_Contact == null) { return false; }
                tbl_Contact.id = id;
                tbl_Contact.name = name;
                tbl_Contact.email = email;
                tbl_Contact.phone = phone;
                tbl_Contact.address = address;
                tbl_Contact.comment = comment;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }
        [HttpPost]
        public bool Insertcontact(int id, string name, string email, string phone, string address, string comment)
        {
            try
            {
                tbl_contact tbl_Contact = new tbl_contact();
                tbl_Contact.id = id;
                tbl_Contact.name = name;
                tbl_Contact.email = email;
                tbl_Contact.phone = phone;
                tbl_Contact.address = address;
                tbl_Contact.comment = comment;
                db.tbl_contact.Add(tbl_Contact);
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
