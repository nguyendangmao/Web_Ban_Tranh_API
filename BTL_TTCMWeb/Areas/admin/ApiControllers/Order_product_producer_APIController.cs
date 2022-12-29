using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BTL_TTCMWeb.Models;
namespace BTL_TTCMWeb.Areas.admin.ApiControllers
{
    public class Order_product_producer_APIController : ApiController
    {
        HAWContextEntities db = new HAWContextEntities();
        DemoDataContext bd = new DemoDataContext();
        //Phần hóa đơn
        [Route("XoaCTHoaDon/{order_detail_id}")]
        [HttpDelete]
        public bool DeleteCTOrder(int order_detail_id)
        {
            try
            {
                tbl_orderDetail tbl_OrderDetail = db.tbl_orderDetail.FirstOrDefault(n => Convert.ToString(n.order_detail_id).Contains(Convert.ToString(order_detail_id)));
                if (tbl_OrderDetail == null)
                {
                    return false;
                }
                db.tbl_orderDetail.Remove(tbl_OrderDetail);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        [Route("asd/{user_id}")]
        [HttpGet]
        public Demo4 Abc(int user_id)
        {
            return bd.Demo4s.FirstOrDefault(x => x.user_id== user_id);
        }
        [Route("Layorder")]
        [HttpGet]
        public List<order1> Getallorder()
        {
            return bd.order1s.ToList();
        }
        [Route("ctOrder/{order_id}")]
        [HttpGet]
        public List<chitietOrder> ctoder(int order_id)
        {
            return bd.chitietOrders.Where(n => n.order_id == order_id).ToList();
        }
        [Route("laycategory")]
        public List<tbl_category> getcategory()
        {
            return bd.tbl_categories.ToList();
        }
        [Route("layproducerr")]
        public List<tbl_producer> getproducer()
        {
            return bd.tbl_producers.ToList();
        }
        [Route("Layorder/{order_id}")]
        [HttpGet]
        public order1 Getorder(int order_id)
        {
            return bd.order1s.FirstOrDefault(n => n.order_id == order_id);
        }
        //sua
        [HttpPut]
        public bool Updateorder(int order_id, int user_id, string order_receiver_note, float order_total_price, int order_state)
        {
            try
            {

                tbl_Order tbl_Order = db.tbl_Order.FirstOrDefault(n => n.order_id == order_id);
                if (tbl_Order == null)
                {
                    return false;
                }
                tbl_Order.order_id = order_id;
                tbl_Order.user_id = user_id;
                tbl_Order.order_receiver_note = order_receiver_note;
                tbl_Order.order_total_price = order_total_price;
                tbl_Order.order_state = order_state;
                db.SaveChanges();
                return true;

            }
            catch
            {
                return false;
            }
        }
        [HttpPut]
        public bool Trahang(int id, int soluong)
        {
            try
            {
                tbl_productColor tbl_Product = db.tbl_productColor.FirstOrDefault(n => n.id == id);
                if (tbl_Product == null)
                {
                    return false;
                }
                tbl_Product.amount += soluong;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        [Route("XoaHoaDon/{order_id}")]
        [HttpDelete]
        public bool DeleteOrder(int order_id)
        {
            try
            {
                tbl_Order tbl_Order = db.tbl_Order.FirstOrDefault(n => n.order_id == order_id);
                if (tbl_Order == null)
                {
                    return false;
                }
                db.tbl_Order.Remove(tbl_Order);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        //begin product
        [Route("Layproduct")]
        [HttpGet]
        public List<product> Getallproduct()
        {
            return bd.products.ToList();
        }
        //lay 1 product
        [Route("Layproduct/{product_id}")]
        [HttpGet]
        public product Getproduct(int product_id)
        {
            return bd.products.FirstOrDefault(n => n.product_id == product_id);
        }
        //them
        [HttpPost]
        public bool Insertproduct(int product_id, string product_name, string product_alias, string product_description, string product_content,
            string product_img, string product_sub_img, 
             int state, int product_producer, int category_id)
        {
            try
            {


                tbl_product tbl_Product = new tbl_product();
                tbl_Product.product_id = product_id;
                tbl_Product.product_name = product_name;
                tbl_Product.product_alias = product_alias;
                tbl_Product.product_description = product_description;
                tbl_Product.product_content = product_content;
                tbl_Product.product_img = product_img;
                tbl_Product.product_sub_img = product_sub_img;
                tbl_Product.product_CreatedAt = DateTime.Now;
                tbl_Product.state = state;            
                tbl_Product.product_producer = product_producer;
                tbl_Product.category_id = category_id;

                db.tbl_product.Add(tbl_Product);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //edit

        [HttpPut]
        public bool UpdateSP(int product_id, string product_name, string product_alias, string product_description, string product_content,
                    string product_img, int state, int product_producer, int category_id)
        {
            try
            {
                tbl_product tbl_Product = db.tbl_product.FirstOrDefault(x => x.product_id == product_id);
                if (tbl_Product == null)
                {
                    return false;
                }
                tbl_Product.product_id = product_id;
                tbl_Product.product_name = product_name;
                tbl_Product.product_alias = product_alias;
                tbl_Product.product_description = product_description;
                tbl_Product.product_content = product_content;
                tbl_Product.product_img = product_img;            
                tbl_Product.product_UpdatedAt = DateTime.Now;           
                tbl_Product.state = state;            
                tbl_Product.product_producer = product_producer;
                tbl_Product.category_id = category_id;

                db.SaveChanges();
                return true;

            }
            catch
            {
                return false;
            }
        }
        ///delete

        [HttpDelete]
        public bool DeleteSP(int product_id)
        {
            try
            {
                tbl_product tbl_Product = db.tbl_product.FirstOrDefault(x => x.product_id == product_id);
                if (tbl_Product == null)
                {
                    return false;
                }
                db.tbl_product.Remove(tbl_Product);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        //end product
        //begin producer
        [Route("Layproducer")]
        [HttpGet]
        public List< producer> Getallproducer()
        {
            return bd.producers.ToList();
        }
        //get 1 product
        [Route("Layproducer/{producer_id}")]
        [HttpGet]
        public producer Getproduce(int producer_id)
        {
            return bd.producers.FirstOrDefault(n => n.producer_id == producer_id);
        }    
        //them
        [HttpPost]
        public bool InsertProduce(int producer_id, string producer_name, string producer_address, int state)
        {
            try
            {
                tbl_producer producer = new tbl_producer();
                producer.producer_id = producer_id;
                producer.producer_name = producer_name;
                producer.producer_address = producer_address;
                producer.state = state;
                bd.tbl_producers.InsertOnSubmit(producer);
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
        public bool UpdateProduce(int producer_id, string producer_name, string producer_address, int state)
        {
            try
            {
                tbl_producer producer = bd.tbl_producers.FirstOrDefault(n => n.producer_id == producer_id);
                if (producer == null) { return false; }
                producer.producer_id = producer_id;
                producer.producer_name = producer_name;
                producer.producer_address = producer_address;
                producer.state = state;
                bd.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        //xoa
        [Route("deleteproducer/{producer_id}")]
        [HttpDelete]
        public bool DeleteProduce(int producer_id)
        {
            try
            {
                tbl_producer tbl_Producer = bd.tbl_producers.FirstOrDefault(n => n.producer_id == producer_id);
                if (tbl_Producer == null) { return false; }
                bd.tbl_producers.DeleteOnSubmit(tbl_Producer);
                bd.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        [Route("laystate")]
        public List<tbl_state> getstate()
        {
            return bd.tbl_states.ToList();
        }
        //Product color
        [Route("Layproductcolor")]
        [HttpGet]
        public List<productcolor> Getallproductcolor()
        {
            return bd.productcolors.ToList();
        }
        [Route("laycolor")]
        public List<tbl_color> getColor()
        {
            return bd.tbl_colors.ToList();
        }

        //lay 1 product
        [Route("xyzz/{id}")]
        [HttpGet]
        public productcolor Getproductcolo(int id)
        {
            return bd.productcolors.FirstOrDefault(n => n.id == id);
        }
        //them
        [HttpPost]
        public bool Insertproductcolor(int id, int color_id, int product_id, string size, int amount, float product_price, float product_sale, float product_discount)
        {
            try
            {
                tbl_productColor tbl_ProductColor = new tbl_productColor();
                tbl_ProductColor.id = id;
                tbl_ProductColor.color_id = color_id;
                tbl_ProductColor.product_id = product_id;
                tbl_ProductColor.size = size;
                tbl_ProductColor.amount = amount;
                tbl_ProductColor.product_price = product_price;
                tbl_ProductColor.product_sale = product_sale;
                tbl_ProductColor.product_discount = product_discount;
                db.tbl_productColor.Add(tbl_ProductColor);
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
        public bool Updateproductcolor(int id, int color_id, int product_id, string size, int amount, float product_price, float product_sale, float product_discount)
        {
            try
            {
                tbl_productColor tbl_ProductColor = db.tbl_productColor.FirstOrDefault(x => x.id == id);
                if (tbl_ProductColor == null)
                {
                    return false;
                }
                tbl_ProductColor.id = id;
                tbl_ProductColor.color_id = color_id;
                tbl_ProductColor.product_id = product_id;
                tbl_ProductColor.size = size;
                tbl_ProductColor.amount = amount;
                tbl_ProductColor.product_price = product_price;
                tbl_ProductColor.product_sale = product_sale;
                tbl_ProductColor.product_discount = product_discount;
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
