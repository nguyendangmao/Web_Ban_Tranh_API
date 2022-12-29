using BTL_TTCMWeb.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BTL_TTCMWeb.Controllers
{
    public class UserController : Controller
    {
        private readonly HAWContextEntities db = new HAWContextEntities();
        // GET: Home
        //Truyền dữ liệu vào index(View lên)
        public ActionResult Index(string username)
        {
            //Truyền dữ liệu vào session theo tên
            Session["listCategory"] = db.tbl_category.AsQueryable();
            Session["listColor"] = db.tbl_color.AsQueryable();
            Session["listSize"] = db.tbl_productColor.Select(x => x.size).Distinct().AsQueryable();
            return View();
        }
        //Trả về dữ liệu của bảng contact
        //Hiện view contact
        public ActionResult Contact()
        {
            return View(db.tbl_contact.FirstOrDefault());
        }
        //View Đăng kí
        public ActionResult Register()
        {
            return View();
        }
        //Xử lí khi đăng kí
        [HttpPost]
        public ActionResult Register( tbl_user user)
        {
            if (db.tbl_user.Where(x => x.user_name == user.user_name).FirstOrDefault() == null)
            {
                user.isActive = true;
                db.tbl_user.Add(user);
                db.SaveChanges();
                return RedirectToAction("Login", "Account");
            }
            else
            {
                ModelState.AddModelError("Test", "Vui lòng kiểm tra tài khoản.");
            }
            return View(user);

        }

        public ActionResult GetDataProductPartial(int? page, string category, string color, string size, int? priceFrom, int? priceTo, string search)
        {
            //Lấy sp theo yêu cầu
            var listProduct = db.tbl_product.AsQueryable();
            //Tìm kiếm ở mục tìm kiếm
            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                listProduct = listProduct.Where(p => p.product_name.ToLower().Contains(search));
            }
            //Tìm kiếm theo giá
            if (priceFrom != null &&
                priceTo != null &&
                priceTo >= priceFrom)
                listProduct = listProduct.Where(p => p.tbl_productColor.Min(x => x.product_price) >= priceFrom &&
                                                     p.tbl_productColor.Max(x => x.product_price) <= priceTo);
            var aaa = listProduct.Select(x => new
            {
                name = x.product_name,
                min = x.tbl_productColor.Min(y => y.product_price),
                max = x.tbl_productColor.Max(y => y.product_price)
            }).ToList();
            //Tìm kiếm theo loại danh mục
            if (!string.IsNullOrEmpty(category))
            {
                var arrCategory = category.Split(',');

                listProduct = listProduct.Where(x => arrCategory.Contains(x.tbl_category.category_name));
            }
            //Tìm kiếm theo màu
            if (!string.IsNullOrEmpty(color))
            {
                var arrcolor = color.Split(',').Select(x => x.Trim()).ToList();
                var ProductHasColor = db.tbl_productColor.Where(x => arrcolor.Contains(x.tbl_color.color_name.Trim().ToLower()))
                                                            .Select(x => x.product_id).ToList();
                listProduct = listProduct.Where(x => ProductHasColor.Contains(x.product_id));
            }
            //Tìm kiếm theo kích thước
            if (!string.IsNullOrEmpty(size))
            {
                var arrcolor = size.Split(',').Select(x => x.Trim()).ToList();
                var ProductHasSize = db.tbl_productColor.Where(x => arrcolor.Contains(x.size.Trim().ToLower()))
                                                            .Select(x => x.product_id).ToList();
                listProduct = listProduct.Where(x => ProductHasSize.Contains(x.product_id));
            }
            //Trả về sản phẩm theo phân trang ban đầu
            return PartialView("_GetDataProductPartial", listProduct.OrderBy(x => x.product_id).ToPagedList(page ?? 1, 9));
        }
        //Hiện các sản phẩm ngẫu nhiên
        public List<tbl_product> getSuggestProduct()
        {
            Random randomizer = new Random();
            var list = db.tbl_product.ToList();
            List<tbl_product> items = new List<tbl_product>();
            while (items.Count() < 5)
            {
                var product = list[randomizer.Next(list.Count)];
                items.Add(product);
                items = items.Distinct().ToList();
            }

            return items;
        }
        //Hiện giá của sản phẩm
        public ActionResult getRangePriceAction()
        {
            var result = new[]
            {
                db.tbl_product.Min(x=>x.tbl_productColor.Min(y=>y.product_price)).ToString(),
                db.tbl_product.Max(x=>x.tbl_productColor.Max(y=>y.product_price)).ToString()
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //Hiện chi tiết sản phẩm
        public ActionResult ProductDetail(int? Id)
        {
            
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_product model =  db.tbl_product.FirstOrDefault(x=>x.product_id==Id);
            if (model == null)
            {
                return HttpNotFound();
            }
            Session["listSuggestProduct"] = getSuggestProduct();
            return View(model);
        }
        //Hiện ra các size ảnh hiện có
        public PartialViewResult cbbSize(int productId, int colorId)
        {
            IQueryable<tbl_productColor> model = db.tbl_productColor.Where(x => x.product_id == productId && x.color_id == colorId).AsQueryable();
            return PartialView(model);
        }
        //Hiển thị phần bên phải chi tiêt sản phẩm
        public ActionResult getProductColor(int productId, int colorId, string size)
        {
            tbl_productColor model = db.tbl_productColor.First(x => x.product_id == productId && x.color_id == colorId && x.size == size);
            return Json(new
            {
                //Giá cuối
                price = model.product_discount == null ? model.product_price.ToString() : (model.product_price * (100 - model.product_discount) / 100).ToString(),
                amount = model.amount.ToString(),
                //Id màu sp
                productColorId = model.id.ToString(),
                //Giảm giá
                discount = model.product_discount != null ? "-" + model.product_discount.ToString() + "%" : string.Empty,
                //Giá ban đầu
                priceBefore = model.product_discount == null ? string.Empty : model.product_price.ToString()

            }, JsonRequestBehavior.AllowGet);
        }
        //Add sản phẩm vào giỏ hàng
        public void AddToCart(int productColorId, int quantity)
        {
            HttpCookie cookie = Request.Cookies["cart"];
            var productColor = db.tbl_productColor.First(x => x.id == productColorId);
            if (cookie != null)
            {
                //đã tồn tại cart
                var cartSession = cookie.Value;
                var model = db.tbl_cart.FirstOrDefault(x => x.session_id == new Guid(cartSession));
                if (model.tbl_cartDetail.Any(x => x.productColor_id == productColorId))
                {
                    //đã tồn tại sản phầm => add thêm số lượng
                    var CartDetail = model.tbl_cartDetail.First(x => x.productColor_id == productColorId);
                    CartDetail.quantity += quantity;
                    db.Entry(CartDetail).State = EntityState.Modified;
                    db.SaveChanges();
                    model = db.tbl_cart.FirstOrDefault(x => x.session_id == new Guid(cartSession));
                    model.total_price = model.tbl_cartDetail.Sum(x => x.quantity * (x.tbl_productColor.product_sale ?? x.tbl_productColor.product_price));
                    //kiểm trả cookie userid 
                    HttpCookie cookieUser = Request.Cookies["hawuser_id"];
                    if (cookie != null && model.account_id == null)
                    {
                        var userid = int.Parse(cookieUser.Value);
                        model.account_id = userid;
                    }
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    //chưa add sản phẩm vào cart => thêm mới sản phẩm vào cart
                    tbl_cartDetail cartDetail = new tbl_cartDetail()
                    {
                        cart_id = model.cart_id,
                        price = productColor.product_price,
                        quantity = quantity,
                        productColor_id = productColor.id
                    };
                    db.tbl_cartDetail.Add(cartDetail);
                    db.SaveChanges();
                    model.total_price = model.tbl_cartDetail.Sum(x => x.quantity * (x.tbl_productColor.product_sale ?? x.tbl_productColor.product_price));
                    HttpCookie cookieUser = Request.Cookies["hawuser_id"];
                    if (cookie != null && model.account_id == null)
                    {
                        var userid = int.Parse(cookieUser.Value);
                        model.account_id = userid;
                    }
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else
            {
                //chưa tồn tại cart
                var sessionId = Guid.NewGuid();
                //Create cookie
                HttpCookie userCookie = new HttpCookie("cart", sessionId.ToString());
                //Expried date
                userCookie.Expires.AddDays(60);
                //Save data at cookie
                HttpContext.Response.SetCookie(userCookie);
                tbl_cart cart = new tbl_cart();
                HttpCookie cookieUser = Request.Cookies["hawuser_id"];
                if (cookieUser != null)
                {
                    var userid = int.Parse(cookieUser.Value);
                    cart.session_id = sessionId;
                    cart.total_price = productColor.product_price;
                    cart.account_id = userid;
                }
                else
                {
                    cart.session_id = sessionId;
                    cart.total_price = productColor.product_price;
                }

                cart.tbl_cartDetail.Add(new tbl_cartDetail() { quantity = quantity, price = productColor.product_price, productColor_id = productColorId });
                db.tbl_cart.Add(cart);
                db.SaveChanges();
            }
        }
        //Xóa sản phẩm đã mua bên giỏ hàng
        public ActionResult DeleteToCart(int productColorId)
        {
            try
            {
                HttpCookie cookie = Request.Cookies["cart"];
                var cartSession = cookie.Value;
                var model = db.tbl_cart.FirstOrDefault(x => x.session_id == new Guid(cartSession));
                //Lấy sản phẩm cần xóa
                var item = model.tbl_cartDetail.First(x => x.cart_detail_id == productColorId);
                //Giá sản phẩm xóa
                var priceRemove = item.quantity * item.price;
                //Số lượng sản phẩm xóa
                var quantityRemove = item.quantity;
                //Xóa sản phẩm khỏi giỏ hàng
                model.tbl_cartDetail.Remove(item);
                //Tính lại giá tiền bên giỏ hàng sau khi xóa
                model.total_price = model.tbl_cartDetail.Sum(x => x.quantity * (x.tbl_productColor.product_sale ?? x.tbl_productColor.product_price));
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { price = model.total_price.ToString(), quantity = model.tbl_cartDetail.Select(x => x.quantity).Count(), priceRemove = model.total_price - priceRemove, quantityRemove = model.tbl_cartDetail.Select(x => x.quantity).Count() - quantityRemove }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }

        }
        public ActionResult DanhGiaSanPham(int MaThietBi, string DanhGia)
        {
            tbl_product thietBi = db.tbl_product.SingleOrDefault(n => n.product_id == MaThietBi);
            thietBi.TongSoSao += int.Parse(DanhGia);
            thietBi.TongSoDanhGia++;
            db.SaveChanges();
            return View();
        }
        //clear sau khi xoa gio hang
        public void ClearAfterDelete()
        {
            //Gọi cookies
            HttpCookie cookie = Request.Cookies["cart"];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }
        }
        //View lên và xử lí bên giỏ hàng
        public ActionResult checkout()
        {
            //Lấy các sản phẩm đã thêm vào giỏ(truyền vào session giỏ hàng)
            Session["listSuggestProduct"] = getSuggestProduct();
            HttpCookie cookie = Request.Cookies["cart"];
            if (cookie != null)
            {
                var cartSession = cookie.Value;
                //Tìm các sản phẩm theo session ban đầu
                var model = db.tbl_cart.FirstOrDefault(x => x.session_id == new Guid(cartSession));
                return View(model);
            }
            return View();
        }
        //Phần kiểm tra thanh toán
        public async Task<ActionResult> CheckPayment(string note)
        {
            try
            {
                //Lấy tt khách hàng
                if (Session["username"] != null)
                {
                    //Lấy cookie giỏ hàng theo khách hàng đó
                    var username = Session["username"].ToString();
                    var CurrentUser = await db.tbl_user.FirstAsync(x => x.user_name == username);
                    HttpCookie cookie = Request.Cookies["cart"];
                    // K có dữ liệu mua hàng
                    if (cookie == null)
                    {
                        return Json(new { message = "Vui lòng chọn sản phẩm muốn mua!" }, JsonRequestBehavior.AllowGet);
                    }
                    // Lấy các giá trị trong giỏ hàng 
                    var cartSession = cookie.Value;
                    var cart = db.tbl_cart.FirstOrDefault(x => x.session_id == new Guid(cartSession));
                    //Chưa chọn sản phẩm nào
                    if (cart.total_price == 0)
                    {
                        return Json(new { message = "Vui lòng chọn sản phẩm muốn mua!" }, JsonRequestBehavior.AllowGet);
                    }
                    //create order
                    tbl_Order order = new tbl_Order()
                    {
                        order_receiver_note = note,
                        order_state = db.tbl_state.First(x => x.state_name == "Mới").state_id,
                        order_total_price = cart.total_price,
                        user_id = CurrentUser.user_id,
                        date = DateTime.Now,

                    };
                    foreach (var item in cart.tbl_cartDetail)
                    {
                        order.tbl_orderDetail.Add(new tbl_orderDetail() { price = item.price, productColor_id = item.productColor_id, quantity = item.quantity });
                        //trừ số lượng sản phẩm trong kho
                        var productColor = await db.tbl_productColor.FirstAsync(x => x.id == item.productColor_id);
                        productColor.amount -= item.quantity.Value;
                        db.Entry(productColor).State = EntityState.Modified;
                    }
                    db.tbl_Order.Add(order);

                    //Clear cookie
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(cookie);
                    await db.SaveChangesAsync();
                    return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { message = "Vui lòng đăng nhập trước!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        //View lên thanh toán thành công
        public ActionResult Payment()
        {
            return View();
        }
    }
}