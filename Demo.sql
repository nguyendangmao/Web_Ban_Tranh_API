create view Demo as
select tbl_user.*,question from tbl_user left join tbl_question on tbl_user.question_id=tbl_question.question_id
create view Demo1 as
select tbl_user.*,SoLanMua from (select user_id,count(order_id) as SoLanMua from tbl_Order group by user_id) as T right join tbl_user on T.user_id=tbl_user.user_id 
create view Demo2 as
select tbl_Order.*,user_name,state_name from tbl_Order right join tbl_user on tbl_Order.user_id=tbl_user.user_id 
inner join tbl_state on tbl_state.state_id=tbl_Order.order_state

create view Demo3 as
 select tbl_orderDetail.*,user_id,tbl_product.product_name,product_img,(quantity*price) as TongTien from tbl_orderDetail inner join tbl_productColor on tbl_productColor.color_id=tbl_orderDetail.productColor_id and tbl_productColor.product_price=tbl_orderDetail.price
 inner join tbl_product on tbl_product.product_id=tbl_productColor.product_id
 inner join Demo2 on Demo2.order_id=tbl_orderDetail.oder_id

 create view BanTheoNam as
select year(h.date) as Nam,month(h.date) as Thang,count (h.order_id) as TongHD ,sum (quantity) as SoSP,sum(h.order_total_price) as TongTien from tbl_orderDetail c join Demo2 h on c.oder_id=h.order_id
where state_name =N'Đã giao hàng'
group by YEAR(h.date),month(h.date)
drop view BanTheoNam
create view TopSPbanchay as
select
year(c.date) as Nam,month(c.date) as Thang ,sum (h.quantity) as SoSP,sum(h.price) as TongTien,product_name,size,color_name,product_img from Demo2 c join tbl_orderDetail h on c.order_id=h.oder_id
join tbl_productColor tbl on tbl.id = h.productColor_id
join tbl_product tb on tbl.product_id=tb.product_id
join tbl_color cl on cl.color_id=tbl.color_id
where state_name =N'Đã giao hàng'
 group by YEAR(c.date),month(c.date),product_name,size,color_name,product_img
 order by sum (h.quantity) desc,sum(c.order_total_price) desc offset 0 rows
 drop view TopSPbanchay
 create view  product as select
p.product_id,
p.product_name,
p.product_alias,
p.product_description,
p.product_content,
p.product_img,
p.product_sub_img,
p.product_isHot,
p.product_isNew,
p.product_isSale,
p.product_CreatedAt,
p.product_UpdatedAt,
p.product_code,
p.product_package,
p.product_delivery,
p.product_delivery_time,
p.product_payment,
p.product_payment_type,
p.state,
s.state_name,
p.title_seo,
p.des_seo,
p.friendly_url,
p.keyword,
p.product_material,
p.product_shape,
p.product_producer,
pr.producer_name,
c.category_id,
c.category_name

from tbl_product as p 
join tbl_category  as c on p.category_id= c.category_id
join tbl_producer as pr on p.product_producer=pr.producer_id
join tbl_state as s on p.state=s.state_id
create view order1 as
select  o.order_id,u.user_id,u.user_name,o.order_receiver_note,o.order_total_price,s.state_id,s.state_name,
from tbl_Order as o join  tbl_state as s on o.order_state =s.state_id join tbl_user as u on u.user_id=o.user_id
create view producer as
select p.producer_id,p.producer_name,p.producer_address,p.state,s.state_name from tbl_producer p join tbl_state s on p.state=s.state_id
create view chitietOrder as
select
tbl_order.*,
tbl_color.color_name,
tbl_state.state_name,
tbl_product.product_name,
tbl_orderDetail.quantity,
tbl_orderDetail.price,
tbl_productColor.size,
tbl_product.product_img,
id
from tbl_orderDetail join tbl_Order on tbl_orderDetail.oder_id =tbl_Order.order_id
join tbl_productColor on tbl_orderDetail.productColor_id= tbl_productColor.id
join tbl_color on tbl_productColor.color_id=tbl_color.color_id
join tbl_product on tbl_productColor.product_id=tbl_product.product_id
join tbl_state on tbl_Order.order_state=tbl_state.state_id
drop view chitietOrder
create view Demo4 as 
select tbl_user.user_id,tbl_user.user_name from tbl_user
create view productcolor as select p.id,p.color_id,c.color_name,p.product_id,pr.product_name, p.size,p.amount,p.product_price,p.product_sale,p.product_discount
from tbl_productColor p join tbl_color c on p.color_id=c.color_id join tbl_product pr on p.product_id=pr.product_id

