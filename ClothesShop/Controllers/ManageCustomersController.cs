using System.Net.NetworkInformation;
using ClothesShop.Models;
using Microsoft.AspNetCore.Mvc;
using ClothesShop.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Threading;

namespace ClothesShop.Controllers
{
    public class ManageCustomersController : Controller
    {
        private readonly ClothingShopContext _db;
        public ManageCustomersController(ClothingShopContext db)
        {
            _db = db;
        }


        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("StfId") == null)
            {

                TempData["ErrorMessage"] = "Kun mai me sit na naka";
                return RedirectToAction("Login", "Home");
            }
            ViewData["currentNav"] = "ManageCustomers";

            //var pd = from p in _db.Products
            //select p;
            var CustomerVM = from c in _db.Customers

                             select new CustomerVM
                             {
                                 CusId = c.CusId,  //รหัวสินค้า
                                 CusName = c.CusName, //
                                 CusLogin = c.CusLogin,  //
                                 CusPass = c.CusPass, //
                                 CusEmail = c.CusEmail,//
                                 CusAdd = c.CusAdd,//
                                 StartDate = c.StartDate, //
                                 LastLogin = c.LastLogin, //
                             };
            if (CustomerVM == null) return NotFound();
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            return View(CustomerVM);
        }

        public IActionResult Create()
        {

            if (HttpContext.Session.GetString("StfId") == null)
            {

                TempData["ErrorMessage"] = "Kun mai me sit na naka";
                return RedirectToAction("Login", "Home");
            }

            ViewData["currentNav"] = "ManageCustomers/Create";
            



            return View();

        }

     

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Customer obj)
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

                    // Check if CusLogin already exists
                    var existingCustomer = _db.Customers.FirstOrDefault(c => c.CusLogin == obj.CusLogin);
                    if (existingCustomer != null)
                    {
                        // CusLogin already exists, return error message or redirect as appropriate
                        ViewBag.ErrorMessage = "Username ID already exists";
                        return View(obj);
                    }
                    // Get the last Customer Id from the database
                    string lastCustomerId = _db.Customers.Select(c => c.CusId).OrderByDescending(c => c).FirstOrDefault();

                    // Extract the numeric part of the last Customer Id
                    string numericPart = lastCustomerId.Substring(1); // Assuming format CXXX

                    // Convert the numeric part to integer and increment
                    int nextId = int.Parse(numericPart) + 1;

                    // Generate the next Customer Id
                    string nextCustomerId = "C" + nextId.ToString("000");

                    // Assign the generated Customer Id to the new Customer object
                    obj.CusId = nextCustomerId;
                    //obj.CusId = "C004";

                    // Add the new Customer to the database
                    _db.Customers.Add(obj);
                    _db.SaveChanges();

                    TempData["SuccessMessage"] = "Successfully Create!";
                    // Redirect to appropriate action after successful registration
                    return RedirectToAction("Edit", "ManageCustomers", new { id = nextCustomerId });
                    //return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(obj);
            }

            ViewBag.ErrorMessage = "การบันทึกผิดพลาด";
            return View(obj);

        }


        public IActionResult Edit(string id)
        {
            if (HttpContext.Session.GetString("StfId") == null)
            {

                TempData["ErrorMessage"] = "Kun mai me sit na naka";
                return RedirectToAction("Login", "Home");
            }

            ViewData["currentNav"] = "ManageCustomers";

            if (id == null)
            {
                TempData["ErrorMessage"] = "ระบุ id";
                return RedirectToAction("Index");
            }
            //ค้นหา Record ของ Product.pdId จาก id ที่ส่งมา
            var obj = _db.Customers.Find(id);
            if (obj == null)
            {
                TempData["ErrorMessage"] = "ไม่พบข้อมูล";
                return RedirectToAction("Index");
            }

            var fileName = id.ToString() + ".png";
            var imgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\imgcus");
            var filePath = Path.Combine(imgPath, fileName);
            if (System.IO.File.Exists(filePath))
            {
                ViewBag.ImgFile = "/imgcus/" + id + ".png";
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
        public IActionResult Edit(Customer obj)
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
                    _db.Customers.Update(obj);
                    _db.SaveChanges();
                    //return RedirectToAction("Index");
                    TempData["SuccessMessage"] = "Save Success!";
                    return RedirectToAction("Edit", "ManageCustomers", new { id = obj.CusId });

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
            //return View(obj);
            return RedirectToAction("Edit", "ManageCustomers", new { id = obj.CusId });
        }

        public IActionResult ImgUpload(IFormFile imgfiles, string theid)
        {

            if (HttpContext.Session.GetString("StfId") == null)
            {

                TempData["ErrorMessage"] = "Kun mai me sit na naka";
                return RedirectToAction("Login", "Home");
            }

            var FileName = theid;
            //var FileExtension = Path.GetExtension(imgfiles.FileName);
            var FileExtension = ".png";
            var SaveFileName = FileName + FileExtension;
            var SavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\imgcus");
            var SaveFilePath = Path.Combine(SavePath, SaveFileName);

            using (FileStream fs = System.IO.File.Create(SaveFilePath))
            {
                imgfiles.CopyTo(fs);
                fs.Flush();
            }
            TempData["SuccessMessage"] = "Upload Image Success!";
            return RedirectToAction("Edit", new { id = theid });
        }

        public IActionResult ImgDelete(string id)
        {
            if (HttpContext.Session.GetString("StfId") == null)
            {

                TempData["ErrorMessage"] = "Kun mai me sit na naka";
                return RedirectToAction("Login", "Home");
            }

            var fileName = id.ToString() + ".png";
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\imgcus");
            var filePath = Path.Combine(imagePath, fileName);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            TempData["SuccessMessage"] = "Delete Image Success!";
            return RedirectToAction("Edit", new { id = id });
        }

        public IActionResult Delete(string id)
        {

            if (HttpContext.Session.GetString("StfId") == null)
            {

                TempData["ErrorMessage"] = "Kun mai me sit na naka";
                return RedirectToAction("Login", "Home");
            }

            ViewData["currentNav"] = "ManageCustomers";

            if (id == null)
            {
                TempData["ErrorMessage"] = "ระบุ ID";
                return RedirectToAction("Index");
            }
            var obj = _db.Customers.Find(id);

            if (obj == null)
            {
                TempData["ErrorMessage"] = "ไม่พบข้อมูล";
                return RedirectToAction("Index");
            }

            var fileName = id.ToString() + ".png";
            var imgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\imgcus");
            var filePath = Path.Combine(imgPath, fileName);
            if (System.IO.File.Exists(filePath))
            {
                ViewBag.ImgFile = "/imgcus/" + id + ".png";
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
        public IActionResult DeletePost(string CusId)
        {
            if (HttpContext.Session.GetString("StfId") == null)
            {

                TempData["ErrorMessage"] = "Kun mai me sit na naka";
                return RedirectToAction("Login", "Home");
            }

            try
            {
                var obj = _db.Customers.Find(CusId);
                if (ModelState.IsValid)
                {
                    _db.Customers.Remove(obj);
                    _db.SaveChanges();
                    TempData["SuccessMessage"] = " Delete SuccessMessage";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "ลบข้อมูลไมสำเร็จ";
                    return RedirectToAction("Index");

                }

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "ลบข้อมูลไมสำเร็จ";
                return RedirectToAction("Index");
            }
        }





    }
}
