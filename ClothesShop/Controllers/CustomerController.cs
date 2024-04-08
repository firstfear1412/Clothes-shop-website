using System;
using ClothesShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClothesShop.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ClothingShopContext _db;
        public CustomerController(ClothingShopContext db) {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
		public IActionResult Show(string id)
		{
			if (id == null)
			{
				TempData["ErrorMessage"] = "ต้องระบุค่า id";
				return RedirectToAction("Index");
			}
            if (HttpContext.Session.GetString("CusId") == null)
            {
                TempData["ErrorMessage"] = "กรุณาเข้าสู่ระบบ";
                return RedirectToAction("Index");
            }
            if (HttpContext.Session.GetString("CusId") != id)
            {
                TempData["ErrorMessage"] = "คุณไม่มีสิทธิ์ในการเข้าถึงข้อมูล";
                return RedirectToAction("Show", new { id = HttpContext.Session.GetString("CusId") });
            }
            var obj = _db.Customers.Find(id);
			if (obj == null)
			{
				TempData["ErrorMessage"] = "หา id ไม่พบ";
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
				ViewBag.ImgFile = "/image/No_image2.png";
			}
			return View(obj);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(Customer obj)
		{


			try
			{

				if (ModelState.IsValid)
				{
					_db.Customers.Update(obj);
					_db.SaveChanges();
					HttpContext.Session.SetString("CusName", obj.CusName);
                    //return RedirectToAction("Index");
                    TempData["SuccessMessage"] = "Save Success!";
					return RedirectToAction("Show", "Customer", new { id = obj.CusId });

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
			return RedirectToAction("Show", "Customer", new { id = obj.CusId });
		}

		public IActionResult ImgDelete(string id)
		{
			var fileName = id.ToString() + ".png";
			var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\imgcus");
			var filePath = Path.Combine(imagePath, fileName);
			if (System.IO.File.Exists(filePath))
			{
				System.IO.File.Delete(filePath);
			}
			TempData["SuccessMessage"] = "Delete Image Success!";
			return RedirectToAction("Show", new { id = id });
		}

		public IActionResult ImgUpload(IFormFile imgfiles, string theid)
		{
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
            return RedirectToAction("Show", new { id = theid });
		}


	}

}