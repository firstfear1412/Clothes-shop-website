using System.Diagnostics;
using ClothesShop.Models;
using Microsoft.AspNetCore.Mvc;
using ClothesShop.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;

namespace ClothesShop.Controllers
{
    public class AdminController : Controller
    {
        private readonly ClothingShopContext _db;
        public AdminController(ClothingShopContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
 


            if (HttpContext.Session.GetString("StfId") == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult login(string userName, string userPass)
        {
            var stf = from s in _db.Staffs
                      where s.StfId.Equals(userName)
                          && s.StfPass.Equals(userPass)
                      select s;
            //ถ้าข้อมูลเท่ากับ 0 ให้บอกว่าหาข้อมูลไม่พบ
            if (stf.ToList().Count() == 0)
            {
                TempData["ErrorMessage"] = "ระบุผู้ใช้หรือรหัสผ่านไม่ถูกต้อง";
                return View();
            }


            //ถ้าหาข้อมูลพบ ให้เก็บค่าเข้า Session
            string StfId;
            string StfName;
            string DutyId;
            foreach (var item in stf)
            {
                //อ่านค่าจาก Object เข้าตัวแปร
                StfId = item.StfId;
                StfName = item.StfName;
                DutyId = item.DutyId;
                //เอาค่าจากตัวแปรเข้า Session
                HttpContext.Session.SetString("StfId", StfId);
                HttpContext.Session.SetString("StfName", StfName);
                HttpContext.Session.SetString("DutyId", DutyId);

                var theRecord = _db.Staffs.Find(StfId);

                theRecord.QuitDate = DateOnly.FromDateTime(DateTime.Now);
                _db.Entry(theRecord).State = EntityState.Modified;
            }
            _db.SaveChanges();
            //ทำการย้ายไปหน้าที่ต้องการ
            //return RedirectToAction("Index");
            return RedirectToAction("Index", "Order");

        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
           
        }



        public IActionResult Show(string id)
        {
            //ViewData["currentNav"] = "ManageEmployee";

            if (HttpContext.Session.GetString("StfId") == null)
            {

                TempData["ErrorMessage"] = "Kun mai me sit na naka";
                return RedirectToAction("Login", "Home");
            }

            var currentStfID = HttpContext.Session.GetString("StfId");
            if (currentStfID != id)
            {
                TempData["ErrorMessage"] = "Kun mai me sit na naka";
                //ViewBag.ErrorMessage = "Kun mai me sit na naka";
                //TempData["ErrorMessage"] = ;
                return RedirectToAction("Show", "Admin", new { id = currentStfID });
            }


            if (id == null)
            {
                TempData["ErrorMessage"] = "ระบุ id";
                return RedirectToAction("Index");
            }
            //ค้นหา Record ของ Product.pdId จาก id ที่ส่งมา
            var obj = _db.Staffs.Find(id);
            if (obj == null)
            {
                TempData["ErrorMessagexxx"] = "ไม่พบข้อมูล";
                return RedirectToAction("Index");
            }

            var fileName = id.ToString() + ".png";
            var imgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\imgEmployee");
            var filePath = Path.Combine(imgPath, fileName);
            if (System.IO.File.Exists(filePath))
            {
                ViewBag.ImgFile = "/imgEmployee/" + id + ".png";
            }
            else
            {
                //ViewBag.ImgFile = "/imgcus/No_image2.png";
                ViewBag.ImgFile = "/image/No_image2.png";


            }

            return View(obj);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Staff obj)
        {
            if (HttpContext.Session.GetString("StfId") == null)
            {

                TempData["ErrorMessage"] = "Kun mai me sit na naka";
                return RedirectToAction("Login", "Home");
            }



            try
            {
                if (ModelState.IsValid)
                {
                    _db.Staffs.Update(obj);
                    _db.SaveChanges();
                    //return RedirectToAction("Index");
                    TempData["SuccessMessage"] = "Save Success!";
                    HttpContext.Session.SetString("StfId", obj.StfId);
                    HttpContext.Session.SetString("StfName", obj.StfName);
                    HttpContext.Session.SetString("DutyId", obj.DutyId);
                    return RedirectToAction("Show", "Admin", new { id = obj.StfId });

                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(obj);
            }
            ViewBag.ErrorMessage = "การแก้ไขผิดพลาด";
            //ViewData["Pdt"] = new SelectList(_db.ProductTypes, "PdtId", "PdtName", obj.PdId);
            //ViewData["Brand"] = new SelectList(_db.Brands, "BrandId", "BrandName", obj.BrandId);
            return View(obj);
        }
        public IActionResult ImgUpload(IFormFile imgfiles, string theid)
        {
            var FileName = theid;
            //var FileExtension = Path.GetExtension(imgfiles.FileName);
            var FileExtension = ".png";
            var SaveFileName = FileName + FileExtension;
            var SavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\imgEmployee");
            var SaveFilePath = Path.Combine(SavePath, SaveFileName);

            using (FileStream fs = System.IO.File.Create(SaveFilePath))
            {
                imgfiles.CopyTo(fs);
                fs.Flush();
            }
            TempData["SuccessMessage"] = "Image Upload Success!";
            return RedirectToAction("Show", "Admin", new { id = theid });
        }

        public IActionResult ImgDelete(string id)
        {
            var fileName = id.ToString() + ".png";
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\imgEmployee");
            var filePath = Path.Combine(imagePath, fileName);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            TempData["SuccessMessage"] = "Delete Upload Success!";
            return RedirectToAction("Show", "Admin" , new { id = id });
        }





    }
}
