using Microsoft.AspNetCore.Mvc;
using ClothesShop.Models;
using ClothesShop.ViewModels;
using System.Dynamic;
using System;
using Microsoft.VisualBasic;
using System.Composition;

namespace ClothesShop.Controllers
{
    public class ReportController : Controller
    {
        public readonly ClothingShopContext _db;
        public ReportController(ClothingShopContext db) { _db = db; }

        public IActionResult SalesDailyProducts()
        {
            if (HttpContext.Session.GetString("StfId") == null)
            {

                TempData["ErrorMessage"] = "Kun mai me sit na naka";
                return RedirectToAction("Login", "Home");
            }

            ViewData["currentNav"] = "Report/Product";

            DateOnly thedate = DateOnly.FromDateTime(DateTime.Now.Date);
            var rep = from cd in _db.CartDtls

                      join p in _db.Products on cd.PdId equals p.PdId into join_cd_p
                      from cd_p in join_cd_p.DefaultIfEmpty()

                      join c in _db.Carts on cd.CartId equals c.CartId into join_cd
                      from c_cd in join_cd
                      where c_cd.CartDate == thedate
                      group cd by new { cd.PdId, cd_p.PdName } into g
                      select new ReportPDVM
                      {
                          PdId = g.Key.PdId,
                          PdName = g.Key.PdName,
                          CdtlMoney = g.Sum(x => x.CdtlMoney),
                          CdtlQty = g.Sum(x => x.CdtlQty)
                      };
            ViewBag.theDate = thedate.ToString("yyyy-MM-dd");
            return View(rep);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SalesDailyProducts(DateOnly thedate)
        {
            if (HttpContext.Session.GetString("StfId") == null)
            {

                TempData["ErrorMessage"] = "Kun mai me sit na naka";
                return RedirectToAction("Login", "Home");
            }

            ViewData["currentNav"] = "Report/Product";
            var rep = from cd in _db.CartDtls

                      join p in _db.Products on cd.PdId equals p.PdId into join_cd_p
                      from cd_p in join_cd_p.DefaultIfEmpty()
                      join c in _db.Carts on cd.CartId equals c.CartId into join_cd
                      from c_cd in join_cd
                      where c_cd.CartDate == thedate

                      group cd by new { cd.PdId, cd_p.PdName } into g

                      select new ReportPDVM
                      {
                          PdId = g.Key.PdId,
                          PdName = g.Key.PdName,
                          CdtlMoney = g.Sum(x => x.CdtlMoney),
                          CdtlQty = g.Sum(x => x.CdtlQty)
                      };
            ViewBag.theDate = thedate.ToString("yyyy-MM-dd");
            return View(rep);
        }
        public IActionResult SaleMonthlyProducts()
        {

            if (HttpContext.Session.GetString("StfId") == null)
            {

                TempData["ErrorMessage"] = "Kun mai me sit na naka";
                return RedirectToAction("Login", "Home");
            }

            ViewData["currentNav"] = "Report/MonthlyProduct";
            //กำหนดวันแรก และคำนวณหาวันสุดท้ายของเดือนปัจจุบัน
            var theMonth = DateTime.Now.Month;
            var theYear = DateTime.Now.Year;
            //วันแรกคือวันที่ 1 ของเดือน
            DateOnly sDate = new DateOnly(theYear, theMonth, 1);
            //วันสุดท้ายคือวันที 1 ของเดือนหน้า ลบไป1วัน
            DateOnly eDate = new DateOnly(theYear, theMonth, 1).AddMonths(1).AddDays(-1);

            var rep = from cd in _db.CartDtls

                      join p in _db.Products on cd.PdId equals p.PdId into join_cd_p
                      from cd_p in join_cd_p.DefaultIfEmpty()

                      join c in _db.Carts on cd.CartId equals c.CartId into join_cd
                      from c_cd in join_cd
                      where c_cd.CartDate >= sDate && c_cd.CartDate <= eDate
                      group cd by new { cd.PdId, cd_p.PdName } into g
                      select new ReportPDVM
                      {
                          PdId = g.Key.PdId,
                          PdName = g.Key.PdName,
                          CdtlMoney = g.Sum(x => x.CdtlMoney),
                          CdtlQty = g.Sum(x => x.CdtlQty)
                      };
            ViewBag.sDate = sDate.ToString("yyyy-MM-dd");
            ViewBag.eDate = eDate.ToString("yyyy-MM-dd");

            return View(rep);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaleMonthlyProducts(DateOnly sDate, DateOnly eDate)
        {

            if (HttpContext.Session.GetString("StfId") == null)
            {

                TempData["ErrorMessage"] = "Kun mai me sit na naka";
                return RedirectToAction("Login", "Home");
            }

            //กำหนดวันแรก และคำนวณหาวันสุดท้ายของเดือนปัจจุบัน
            //var theMonth = DateTime.Now.Month;
            //var theYear = DateTime.Now.Year;
            //วันแรกคือวันที่ 1 ของเดือน
            //DateOnly sDate = new DateOnly(theYear, theMonth, 1);
            //วันสุดท้ายคือวันที 1 ของเดือนหน้า ลบไป1วัน
            //DateOnly eDate = new DateOnly(theYear, theMonth, 1).AddMonths(1).AddDays(-1);
            var rep = from cd in _db.CartDtls

                      join p in _db.Products on cd.PdId equals p.PdId into join_cd_p
                      from cd_p in join_cd_p.DefaultIfEmpty()

                      join c in _db.Carts on cd.CartId equals c.CartId into join_cd
                      from c_cd in join_cd
                      where c_cd.CartDate >= sDate && c_cd.CartDate <= eDate
                      group cd by new { cd.PdId, cd_p.PdName } into g

                      select new ReportPDVM
                      {
                          PdId = g.Key.PdId,
                          PdName = g.Key.PdName,
                          CdtlMoney = g.Sum(x => x.CdtlMoney),
                          CdtlQty = g.Sum(x => x.CdtlQty)
                      };
            ViewBag.sDate = sDate.ToString("yyyy-MM-dd");
            ViewBag.eDate = eDate.ToString("yyyy-MM-dd");
            return View(rep);
        }
        public IActionResult SaleDailyCustomer()
        {

            if (HttpContext.Session.GetString("StfId") == null)
            {

                TempData["ErrorMessage"] = "Kun mai me sit na naka";
                return RedirectToAction("Login", "Home");
            }

            ViewData["currentNav"] = "Report/Purchase";
            DateOnly thedate = DateOnly.FromDateTime(DateTime.Now.Date);


            var rep = from cd in _db.CartDtls
                      join p in _db.Carts on cd.CartId equals p.CartId into join_cd_p
                      from cd_p in join_cd_p.DefaultIfEmpty()
                      join c in _db.Customers on cd_p.CusId equals c.CusId into join_cd
                      from c_cd in join_cd.DefaultIfEmpty()
                      where cd_p.CartDate == thedate
                      group cd by new { c_cd.CusId, c_cd.CusName } into g
                      select new ReportCusVM
                      {
                          CusId = g.Key.CusId,
                          CusName = g.Key.CusName,
                          CdtlQty = g.Sum(x => x.CdtlQty),
                          CartMoney = g.Sum(x => x.CdtlMoney)
                      };
            ViewBag.theDate = thedate.ToString("yyyy-MM-dd");
            return View(rep);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaleDailyCustomer(DateOnly thedate)
        {

            if (HttpContext.Session.GetString("StfId") == null)
            {

                TempData["ErrorMessage"] = "Kun mai me sit na naka";
                return RedirectToAction("Login", "Home");
            }

            ViewData["currentNav"] = "Report/Purchase";

            var rep = from cd in _db.CartDtls
                      join p in _db.Carts on cd.CartId equals p.CartId into join_cd_p
                      from cd_p in join_cd_p.DefaultIfEmpty()
                      join c in _db.Customers on cd_p.CusId equals c.CusId into join_cd
                      from c_cd in join_cd.DefaultIfEmpty()
                      where cd_p.CartDate == thedate
                      group cd by new { c_cd.CusId, c_cd.CusName } into g
                      select new ReportCusVM
                      {
                          CusId = g.Key.CusId,
                          CusName = g.Key.CusName,
                          CdtlQty = g.Sum(x => x.CdtlQty),
                          CartMoney = g.Sum(x => x.CdtlMoney)
                      };
            ViewBag.theDate = thedate.ToString("yyyy-MM-dd");
            return View(rep);
        }
        public IActionResult SaleMonthlyCustomer()
        {

            if (HttpContext.Session.GetString("StfId") == null)
            {

                TempData["ErrorMessage"] = "Kun mai me sit na naka";
                return RedirectToAction("Login", "Home");
            }

            ViewData["currentNav"] = "Report/MonthlyPurchase";
            
            //กำหนดวันแรก และคำนวณหาวันสุดท้ายของเดือนปัจจุบัน
            var theMonth = DateTime.Now.Month;
            var theYear = DateTime.Now.Year;
            //วันแรกคือวันที่ 1 ของเดือน
            DateOnly sDate = new DateOnly(theYear, theMonth, 1);
            //วันสุดท้ายคือวันที 1 ของเดือนหน้า ลบไป1วัน
            DateOnly eDate = new DateOnly(theYear, theMonth, 1).AddMonths(1).AddDays(-1);

            var rep = from cd in _db.CartDtls
                      join p in _db.Carts on cd.CartId equals p.CartId into join_cd_p
                      from cd_p in join_cd_p.DefaultIfEmpty()
                      join c in _db.Customers on cd_p.CusId equals c.CusId into join_cd
                      from c_cd in join_cd.DefaultIfEmpty()
                      where cd_p.CartDate >= sDate && cd_p.CartDate <= eDate
                      group cd by new { c_cd.CusId, c_cd.CusName } into g
                      select new ReportCusVM
                      {
                          CusId = g.Key.CusId,
                          CusName = g.Key.CusName,
                          CdtlQty = g.Sum(x => x.CdtlQty),
                          CartMoney = g.Sum(x => x.CdtlMoney)
                      };
            ViewBag.sDate = sDate.ToString("yyyy-MM-dd");
            ViewBag.eDate = eDate.ToString("yyyy-MM-dd");

            return View(rep);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaleMonthlyCustomer(DateOnly sDate, DateOnly eDate)
        {
            if (HttpContext.Session.GetString("StfId") == null)
            {

                TempData["ErrorMessage"] = "Kun mai me sit na naka";
                return RedirectToAction("Login", "Home");
            }


            ViewData["currentNav"] = "Report/MonthlyPurchase";
            var rep = from cd in _db.CartDtls
                      join p in _db.Carts on cd.CartId equals p.CartId into join_cd_p
                      from cd_p in join_cd_p.DefaultIfEmpty()
                      join c in _db.Customers on cd_p.CusId equals c.CusId into join_cd
                      from c_cd in join_cd.DefaultIfEmpty()
                      where cd_p.CartDate >= sDate && cd_p.CartDate <= eDate
                      group cd by new { c_cd.CusId, c_cd.CusName } into g
                      select new ReportCusVM
                      {
                          CusId = g.Key.CusId,
                          CusName = g.Key.CusName,
                          CdtlQty = g.Sum(x => x.CdtlQty),
                          CartMoney = g.Sum(x => x.CdtlMoney)
                      };
            ViewBag.sDate = sDate.ToString("yyyy-MM-dd");
            ViewBag.eDate = eDate.ToString("yyyy-MM-dd");

            return View(rep);
        }
    }
}

