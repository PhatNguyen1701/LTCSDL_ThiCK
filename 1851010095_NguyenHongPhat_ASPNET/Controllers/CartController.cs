using _1851010095_NguyenHongPhat_ASPNET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Transactions;

namespace _1851010095_NguyenHongPhat_ASPNET.Controllers
{
    public class CartController : Controller
    {
        private NWDataContext da = new NWDataContext();

        //Lấy ds sp
        public List<Cart> GetListCarts()
        {
            List<Cart> carts = Session["Cart"] as List<Cart>;
            if (carts == null)// chưa có sp trong giỏ hàng
            {
                carts = new List<Cart>();
                Session["Cart"] = carts;
            }
            return carts;
        }

        //Thêm sp vào gio hàng
        public ActionResult AddCart(int id)
        {
            //Kiem tra sp đã có chưa
            //Chưa có => thêm mới
            //Có rồi => tăng sl lên 1
            List<Cart> carts = GetListCarts();//lấy ds sp
            Cart c = carts.Find(s => s.ProductID == id);
            if (c == null) //chưa có sp
            {
                //Tao mới sp và thêm
                c = new Cart(id);
                carts.Add(c);
            }
            else
            {
                //chưa có => tăng số lượng
                c.Quantity++;
            }
            return RedirectToAction("ListCarts");
        }

        //Hiển thị giỏ hàng
        //Chuột phải vào ListCarts => Add View => Loại View là List => Model là Cart => OK
        public ActionResult ListCarts()
        {
            List<Cart> carts = GetListCarts();

            if (carts.Count == 0)// giỏ hàng chưa có sp
            {
                return RedirectToAction("ListProducts", "Product"); //DSSP
            }
            ViewBag.CountProduct = Count();
            ViewBag.Total = Total();

            return View(carts);
        }

        public ActionResult Delete(int id)
        {
            List<Cart> carts = GetListCarts();
            Cart c = carts.Find(s => s.ProductID == id);

            if (c != null)
            {
                carts.RemoveAll(s => s.ProductID == id);
                return RedirectToAction("ListCarts");
            }
            if (carts.Count == 0)
            {
                return RedirectToAction("ListProducts", "Product");
            }

            return RedirectToAction("ListCarts");
        }

        private int Count()
        {
            int n = 0;
            List<Cart> carts = Session["Cart"] as List<Cart>;
            if (carts != null)
            {
                n = carts.Sum(s => s.Quantity);
            }
            return n;
        }
        //Tính tổng đơn hàng
        private decimal Total()
        {
            decimal total = 0;
            List<Cart> carts = Session["Cart"] as List<Cart>;
            if (carts != null)
            {
                total = carts.Sum(s => s.Total);
            }
            return total;
        }

        //Xử lý thanh toán hàng
        //Chuột phải vào OrderProduct => Add View => Loại View là List => Model là Order_Detail => OK
        public ActionResult OrderProduct(FormCollection fCollection)
        {

            using (TransactionScope tranScope = new TransactionScope())
            {
                try
                {
                    Order order = new Order();
                    order.OrderDate = DateTime.Now;
                    da.Orders.InsertOnSubmit(order);
                    da.SubmitChanges();
                    //order = dt.Orders.OrderByDescending(s => s.OrderID).Take(1).SingleOrDefault();
                    List<Cart> carts = GetListCarts();//lấy giỏ hàng
                    foreach (var item in carts)
                    {
                        Order_Detail d = new Models.Order_Detail();
                        d.OrderID = order.OrderID;
                        d.ProductID = item.ProductID;
                        d.Quantity = short.Parse(item.Quantity.ToString());
                        d.UnitPrice = item.UnitPrice;
                        d.Discount = 0;

                        da.Order_Details.InsertOnSubmit(d);
                    }
                    da.SubmitChanges();
                    tranScope.Complete();
                    Session["Cart"] = null;
                }
                catch (Exception)
                {
                    tranScope.Dispose();
                    return RedirectToAction("ListCarts");

                }
            }
            return RedirectToAction("OrderDetailList", "Cart");
        }

        //Hiển thị chi tiết hóa đơn
        //Chuột phải vào OrderDetailList => Add View => Loại View là List => Model là Order_Detail => OK
        public ActionResult OrderDetailList()
        {
            var p = da.Order_Details.OrderByDescending(s => s.OrderID).Select(s => s).ToList();
            return View(p);
        }


    }
}