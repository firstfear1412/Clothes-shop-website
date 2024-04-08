using ClothesShop.Models;
using ClothesShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Dynamic;

namespace ClothesShop.Controllers
{
    public class OrderController : Controller
    {
        public readonly ClothingShopContext _db;
        public OrderController(ClothingShopContext db) { _db = db; }

        public IActionResult OrderToDay()
        {
            DateOnly todayDate = DateOnly.FromDateTime(DateTime.Now.Date);
            return RedirectToAction("Index", new { date = todayDate });
        }

        public IActionResult Index(DateOnly? date)
        {
            IQueryable<OrderDtl> ordersQuery = _db.Carts
                .Join(_db.Customers, b => b.CusId, c => c.CusId, (b, c) => new OrderDtl
                {
                    CartDate = b.CartDate,
                    CartId = b.CartId,
                    CartMoney = b.CartMoney,
                    CartPay = b.CartPay,
                    CartQty = b.CartQty,
                    CusName = c.CusName,
                })
                .OrderByDescending(o => o.CartDate);

            if (date.HasValue)
            {
                ordersQuery = ordersQuery.Where(o => o.CartDate == date);
            }

            var orders = ordersQuery.ToList();

            return View(orders);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(DateOnly filterDate)
        {
            return RedirectToAction("Index", new { date = filterDate });
        }
        //public IActionResult index()
        //{
        //    ViewData["currentNav"] = "Order";

        //    if (HttpContext.Session.GetString("StfId") == null)
        //    {

        //        TempData["ErrorMessage"] = "Kun mai me sit na naka";
        //        return RedirectToAction("Login", "Home");
        //    }

        //    DateOnly thedate = DateOnly.FromDateTime(DateTime.Now.Date);
        //    var pd = from b in _db.Carts
        //             join c in _db.Customers on b.CusId equals c.CusId
        //             orderby b.CartDate
        //             //สำหรับแสดงเฉพาะตะกร้าที่ Confirm เท่านั้น
        //             where b.CartCf.Equals("Y")
        //             select new OrderDtl
        //             {
        //                 CartDate = b.CartDate,
        //                 CartId = b.CartId,
        //                 CartMoney = b.CartMoney,
        //                 CartPay = b.CartPay,
        //                 CartQty = b.CartQty,
        //                 CusName = c.CusName,
        //                 CartSend = b.CartSend,
        //                 CartVoid = b.CartVoid,


        //             };
        //    return View(pd);
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult index(DateOnly thedate)
        //{
        //    if (HttpContext.Session.GetString("StfId") == null)
        //    {

        //        TempData["ErrorMessage"] = "Kun mai me sit na naka";
        //        return RedirectToAction("Login", "Home");
        //    }


        //    ViewData["currentNav"] = "Order";


        //    var pd = from b in _db.Carts
        //             join c in _db.Customers on b.CusId equals c.CusId
        //             where b.CartDate == thedate
        //             //สำหรับแสดงเฉพาะตะกร้าที่ Confirm เท่านั้น
        //             where b.CartCf.Equals("Y")
        //             select new OrderDtl
        //             {
        //                 CartDate = b.CartDate,
        //                 CartId = b.CartId,
        //                 CartMoney = b.CartMoney,
        //                 CartPay = b.CartPay,
        //                 CartQty = b.CartQty,
        //                 CusName = c.CusName,  
        //                 CartSend = b.CartSend,
        //                 CartVoid = b.CartVoid,

        //             };
        //    return View(pd);
        //}
        public IActionResult Show(string CartId)
        {

            if (HttpContext.Session.GetString("StfId") == null)
            {

                TempData["ErrorMessage"] = "Kun mai me sit na naka";
                return RedirectToAction("Login", "Home");
            }

            ViewData["currentNav"] = "Order";

            if (CartId == null)
            {
                TempData["ErrorMessage"] = "ต้องระบุเลขที่";
                return RedirectToAction("Index");
            }
            //ได้ข้อมูล Buying เป็นส่วน Master
            var buying = from b in _db.Carts
                         where b.CartId == CartId
                         select b;
            if (buying.ToList().Count == 0)
            {
                TempData["ErrorMessage"] = "ไม่พบเอกสาร";
                return RedirectToAction("Index");
            }

            foreach (var s in buying)
            {
                //เพื่อส่งชื่อ Supplier ไปแสดงผลที่ View
                ViewBag.SupName = _db.Customers.FirstOrDefault(sp => sp.CusId == s.CusId).CusName;
            }

            //เลือกข้อมูลของตะกร้าที่ระบุ 
            //สร้าง ViewModel ชื่อ CidVM เพื่อแสดงชื่อสินค้าของ CartDtl (ส่วน Detail)
            var buydtl = from bd in _db.CartDtls
                         join p in _db.Products on bd.PdId equals p.PdId into join_bd_p
                         from bd_p in join_bd_p.DefaultIfEmpty()
                         join s in _db.Sizes on bd_p.SizeId equals s.SizeId
                         join c in _db.Colors on bd_p.ColorId equals c.ColorId
                         where bd.CartId == CartId
                         select new Order
                         {
                             PdName = bd_p.PdName,
                             PdId = bd_p.PdId,
                             BdtlQty = bd.CdtlQty,
                             CartId = bd.CartId,
                             BdtlMoney = bd.CdtlMoney,
                             BdtlPrice =   bd.CdtlPrice,
                             ColorName = c.ColorName,
                             SizeName  = s.SizeName


                         };
            ViewBag.theid = CartId;
            //สร้าง dynamic model
            //เพื่อส่งข้อมูลให้ View แบบสองตารางพร้อมกัน
            dynamic DyModel = new ExpandoObject();
            //แบ่งเป็นส่วน Master รับข้อมูลจาก Obj cart
            DyModel.Master = buying;
            //ส่วน Detail รับข้อมูลจาก Obj cartdtl
            DyModel.Detail = buydtl;
            //ส่ง Dynamic Model ไปที่ View
            return View(DyModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdatePay(string CartId, string CartPay)
        {

            if (HttpContext.Session.GetString("StfId") == null)
            {

                TempData["ErrorMessage"] = "Kun mai me sit na naka";
                return RedirectToAction("Login", "Home");
            }

            var cart = _db.Carts.Find(CartId);
    
            if (cart != null)
            {
                // อัปเดตค่า CartPay
                if (CartPay != null)
                {    
                        cart.CartPay = CartPay;
                   
                }

                // บันทึกการเปลี่ยนแปลงลงในฐานข้อมูล
                TempData["SuccessMessage"] = "Save Success!";
                _db.SaveChanges();
            }

            return RedirectToAction("Show", new { CartId = CartId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateVoid(string CartId, string CartVoid)
        {

            if (HttpContext.Session.GetString("StfId") == null)
            {

                TempData["ErrorMessage"] = "Kun mai me sit na naka";
                return RedirectToAction("Login", "Home");
            }

            var cart = _db.Carts.Find(CartId);

            if (cart != null)
            {
                // อัปเดตค่า CartPay
                if (CartVoid != null)
                {
                    cart.CartVoid = CartVoid;

                }

                // บันทึกการเปลี่ยนแปลงลงในฐานข้อมูล
                TempData["SuccessMessage"] = "Save Success!";
                _db.SaveChanges();
            }

            return RedirectToAction("Show", new { CartId = CartId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateSend(string CartId, string CartSend)
        {

            if (HttpContext.Session.GetString("StfId") == null)
            {

                TempData["ErrorMessage"] = "Kun mai me sit na naka";
                return RedirectToAction("Login", "Home");
            }

            var cart = _db.Carts.Find(CartId);

            if (cart != null)
            {
                // อัปเดตค่า CartPay
                if (CartSend != null)
                {
                    cart.CartSend = CartSend;

                }

                // บันทึกการเปลี่ยนแปลงลงในฐานข้อมูล
                TempData["SuccessMessage"] = "Save Success!";
                _db.SaveChanges();
            }

            return RedirectToAction("Show", new { CartId = CartId });
        }

    }
}
