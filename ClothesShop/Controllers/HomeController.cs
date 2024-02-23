using System.Diagnostics;
using ClothesShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using ClothesShop.ViewModels;
using System.Drawing;


namespace ClothesShop.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        private readonly ClothingShopContext _db;
        public HomeController(ClothingShopContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            //return View();
            //แก้ให้ลิงค์แสดงหน้าร้าน
            return RedirectToAction("Shop");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string? userName, string userPass)
        {
            var cus = from c in _db.Customers
                      where c.CusLogin.Equals(userName) && c.CusPass.Equals(userPass)
                      select c;

            var stf = from s in _db.Staffs
                      where s.StfId.Equals(userName) && s.StfPass.Equals(userPass)
                      select s;


            if (cus.ToList().Count() == 0 && stf.ToList().Count() == 0)
            {
                TempData["ErrorMessage"] = "ผู้ใช้หรือรหัสผ่านไม่ถูกต้อง";
                return RedirectToAction("Index");

            }

            if (cus.ToList().Count() != 0)
            {
                string CusId;
                string CusName;

                foreach (var item in cus)
                {
                    CusId = item.CusId;
                    CusName = item.CusName;

                    HttpContext.Session.SetString("CusId", CusId);
                    HttpContext.Session.SetString("CusName", CusName);

                    var theRecord = _db.Customers.Find(CusId);

                    theRecord.LastLogin = DateOnly.FromDateTime(DateTime.Now);
                    _db.Entry(theRecord).State = EntityState.Modified;

                }
                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            else if (stf.ToList().Count() != 0)
            {
                string StfId;
                string StfName;

                foreach (var item in stf)
                {
                    StfId = item.StfId;
                    StfName = item.StfName;

                    HttpContext.Session.SetString("StfId", StfId);
                    HttpContext.Session.SetString("StfName", StfName);

                    var theRecord = _db.Staffs.Find(StfId);

                    theRecord.StartDate = DateOnly.FromDateTime(DateTime.Now);
                    _db.Entry(theRecord).State = EntityState.Modified;

                }
                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");

        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Shop(string? stext)
        {
            if (stext == null)
            {
                return RedirectToAction("Index");
            }
            var pdvm = from p in _db.Products
                       join pt in _db.ProductTypes on p.PdtId equals pt.PdtId into join_p_pt
                       from p_pt in join_p_pt.DefaultIfEmpty()
                           //join b in _db.Brands on p.BrandId equals b.BrandId into join_p_b
                           //from p_b in join_p_b.DefaultIfEmpty()

                       join color in _db.Colors on p.ColorId equals color.ColorId into join_p_color
                       from p_color in join_p_color.DefaultIfEmpty()

                       join size in _db.Sizes on p.SizeId equals size.SizeId into join_p_size
                       from p_size in join_p_size.DefaultIfEmpty()

                       join target in _db.Targets on p.TargetId equals target.TargetId into join_p_target
                       from p_target in join_p_target.DefaultIfEmpty()

                       join status in _db.Statuses on p.StatusId equals status.StatusId into join_p_status
                       from p_status in join_p_status.DefaultIfEmpty()

                       where p.PdName.Contains(stext) ||
                            p_pt.PdtName.Contains(stext)

                       select new PdVM
                       {
                           PdId = p.PdId,  //รหัวสินค้า
                           ColorName = p_color.ColorName, //สี
                           SizeName = p_size.SizeName, //ขนาด
                           TargetName = p_target.TargetName, //กลุ่มลูกค้า ชาย หญิง เด็ก
                           PdName = p.PdName, //ชื่อสินค้า
                           PdtName = p_pt.PdtName, //ประเภทสินค้า 
                           PdPrice = p.PdPrice, //ราคา
                           PdCost = p.PdCost, //ต้นทุน
                           PdStk = p.PdStk, //คงเหลือ
                           StatusName = p_status.StatusName, //สถาานะ
                       };
            if (pdvm == null) return NotFound();
            ViewBag.stext = stext;
            return View(pdvm);
        }

        public IActionResult Shop()
        {
            var pdvm = from p in _db.Products.Take(4)

                       join pt in _db.ProductTypes on p.PdtId equals pt.PdtId into join_p_pt

                       from p_pt in join_p_pt.DefaultIfEmpty()

                       join color in _db.Colors on p.ColorId equals color.ColorId into join_p_color
                       from p_color in join_p_color.DefaultIfEmpty()

                       join size in _db.Sizes on p.SizeId equals size.SizeId into join_p_size
                       from p_size in join_p_size.DefaultIfEmpty()

                       join target in _db.Targets on p.TargetId equals target.TargetId into join_p_target
                       from p_target in join_p_target.DefaultIfEmpty()

                       join status in _db.Statuses on p.StatusId equals status.StatusId into join_p_status
                       from p_status in join_p_status.DefaultIfEmpty()



                       select new PdVM

                       {

                           PdId = p.PdId,  //รหัวสินค้า
                           ColorName = p_color.ColorName, //สี
                           SizeName = p_size.SizeName, //ขนาด
                           TargetName = p_target.TargetName, //กลุ่มลูกค้า ชาย หญิง เด็ก
                           PdName = p.PdName, //ชื่อสินค้า
                           PdtName = p_pt.PdtName, //ประเภทสินค้า 
                           PdPrice = p.PdPrice, //ราคา
                           PdCost = p.PdCost, //ต้นทุน
                           PdStk = p.PdStk, //คงเหลือ
                           StatusName = p_status.StatusName, //สถาานะ

                       };



            if (pdvm == null) return NotFound();
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            return View(pdvm);
        }
        
    }
}