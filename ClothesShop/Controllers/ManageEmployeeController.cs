using ClothesShop.Models;
using ClothesShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClothesShop.Controllers
{
    public class ManageEmployeeController : Controller
    {
        private readonly ClothingShopContext _db;
        public ManageEmployeeController(ClothingShopContext db)
        {
            _db = db;
        }


        public IActionResult Index()
        {
            ViewData["currentNav"] = "ManageEmployee";

            //var pd = from p in _db.Products
            //select p;
            var EmployeeVM = from ad in _db.Staffs

                             select new EmployeeVM
                             {
                                 StfId = ad.StfId,
                                 StfName = ad.StfName,
                                 StfPass = ad.StfPass,
                                 DutyId = ad.DutyId,
                                 StartDate = ad.StartDate,
                                 QuitDate = ad.QuitDate,

                             };
            if (EmployeeVM == null) return NotFound();
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            return View(EmployeeVM);
        }

        public IActionResult Create()
        {
            ViewData["currentNav"] = "ManageEmployee/Create";

    
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Staff obj)
        {
          
            var existingCustomer = _db.Staffs.FirstOrDefault(c => c.StfId == obj.StfId);
            if (existingCustomer != null)
            {
                // CusLogin already exists, return error message or redirect as appropriate
                ViewBag.ErrorMessage = "Employee ID already exists";
                return View(obj);
            }

            _db.Staffs.Add(obj);
            _db.SaveChanges();

            TempData["SuccessMessage"] = "ลงทะเบียนสำเร็จ!";
            // Redirect to appropriate action after successful registration
            //return RedirectToAction("Edit", "ManageCustomers", new { id = nextCustomerId });


            return RedirectToAction("Edit", "ManageEmployee", new { id = obj.StfId });
            //return View();

        }

        public IActionResult Edit(string id)
        {
            ViewData["currentNav"] = "ManageEmployee";

            if (id == null)
            {
                TempData["ErrorMessage"] = "ระบุ id";
                return RedirectToAction("Index");
            }
            //ค้นหา Record ของ Product.pdId จาก id ที่ส่งมา
            var obj = _db.Staffs.Find(id);
            if (obj == null)
            {
                TempData["ErrorMessage"] = "ไม่พบข้อมูล";
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
            // Check if StfId already exists
            //var existingStaffs = _db.Staffs.FirstOrDefault(c => c.StfId == obj.StfId);
            //if (existingStaffs != null)
            //{
            //    // CusLogin already exists, return error message or redirect as appropriate
            //    ViewBag.ErrorMessage = "Employee ID already exists";
            //    //return View(obj);
            //    return RedirectToAction("Edit", "ManageEmployee", new { id = obj.StfId });
            //}

            try
            {
                if (ModelState.IsValid)
                {
                    _db.Staffs.Update(obj);
                    _db.SaveChanges();
                    //return RedirectToAction("Index");
                    return RedirectToAction("Edit", "ManageEmployee", new { id = obj.StfId });

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
            return RedirectToAction("Edit", new { id = theid });
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
            return RedirectToAction("Edit", new { id = id });
        }



        public IActionResult Delete(string id)
        {

            ViewData["currentNav"] = "ManageCustomers";

            if (id == null)
            {
                TempData["ErrorMessage"] = "ระบุ ID";
                return RedirectToAction("Index");
            }
            var obj = _db.Staffs.Find(id);

            if (obj == null)
            {
                TempData["ErrorMessage"] = "ไม่พบข้อมูล";
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
        public IActionResult DeletePost(string StfId)
        {
            try
            {
                var obj = _db.Staffs.Find(StfId);
                if (ModelState.IsValid)
                {
                    _db.Staffs.Remove(obj);
                    _db.SaveChanges();
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
