using _1851010095_NguyenHongPhat_ASPNET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _1851010095_NguyenHongPhat_ASPNET.Controllers
{
    //Liên kết csdl => chuột phải Model => Add new item => Linq to SQL => mở server connect to SQL
    //Chọn các bảng thả vào Linq to SQL
    public class ProductController : Controller
    {
        NWDataContext da = new NWDataContext();
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        //Hien thi ds san pham
        //Chuột phải vào ListProducts => Add View => Loại View là List => Model là Product => OK
        public ActionResult ListProducts()
        {
            List<Product> ds = da.Products.Select(s => s).ToList();
            return View(ds);
        }

        //Hien thi chi tiet san pham
        //Chuột phải vào Details => Add View => Loại View là Details => Model là Product => OK
        public ActionResult Details(int id)
        {
            Product p = da.Products.Where(s => s.ProductID == id).FirstOrDefault();
            return View(p);
        }

        //Them 1 SP moi
        //Tao View
        //Chuột phải vào Create => Add View => Loại View là Create => Model là Product => OK
        public ActionResult Create()
        {
            ViewData["LSP"] = new SelectList(da.Categories, "CategoryID", "CategoryName");
            ViewData["NCC"] = new SelectList(da.Suppliers, "SupplierID", "CompanyName");
            return View();
        }

        //Xu ly them 1 SP
        [HttpPost]
        public ActionResult Create(FormCollection collection, Product p)
        {
            var ncc = int.Parse(collection["NCC"]);
            var lsp = int.Parse(collection["LSP"]);
            var tenSP = collection["ProductName"];
            if (String.IsNullOrEmpty(tenSP))
            {
                ViewData["Loi"] = "Ten San Pham khong duoc trong !!!";
            }
            else
            {
                //Xu ly them
                p.CategoryID = lsp;
                p.SupplierID = ncc;
                da.Products.InsertOnSubmit(p);
                da.SubmitChanges();
                return RedirectToAction("ListProducts");
            }
            return this.Create();
        }

        //Chuột phải vào Delete => Add View => Loại View là Delete => Model là Product => OK
        public ActionResult Delete(int id)
        {
            Product p = da.Products.Where(s => s.ProductID == id).FirstOrDefault();
            return View(p);
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            Product p = da.Products.Where(s => s.ProductID == id).FirstOrDefault();
            da.Products.DeleteOnSubmit(p);
            da.SubmitChanges();
            return RedirectToAction("ListProducts");
        }

        //Chuột phải vào Edit => Add View => Loại View là Edit => Model là Product => OK
        public ActionResult Edit(int id)
        {
            ViewData["LSP"] = new SelectList(da.Categories, "CategoryID", "CategoryName");
            ViewData["NCC"] = new SelectList(da.Suppliers, "SupplierID", "CompanyName");
            Product p = da.Products.Where(s => s.ProductID == id).FirstOrDefault();
            return View(p);
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            Product p = da.Products.Where(s => s.ProductID == id).FirstOrDefault();
            p.ProductName = collection["ProductName"];
            p.UnitPrice = decimal.Parse(collection["UnitPrice"]);
            p.CategoryID = int.Parse(collection["LSP"]);
            p.SupplierID = int.Parse(collection["NCC"]);
            p.QuantityPerUnit = collection["QuantityPerUnit"];

            UpdateModel(p);
            da.SubmitChanges();

            return RedirectToAction("ListProducts");
        }
    }
}