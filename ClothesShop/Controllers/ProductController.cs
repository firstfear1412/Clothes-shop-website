using ClothesShop.Models;
using Microsoft.AspNetCore.Mvc;
using ClothesShop.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Drawing;
using System;
using System.Linq;

namespace ClothesShop.Controllers
{
    public class ProductController : Controller
    {
        public readonly ClothingShopContext _db;
        public ProductController(ClothingShopContext db) { _db = db; }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("StfId") == null)
            {

                TempData["ErrorMessage"] = "Kun mai me sit na naka";
                return RedirectToAction("Login", "Home");
            }


            ViewData["currentNav"] = "Product";
            //var pd = from p in _db.Products
            //select p;
            var pdvm = from p in _db.Products
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

                       join Sup in _db.Suppliers on p.SupId equals Sup.SupId into join_p_Sup
                       from p_Sup in join_p_Sup.DefaultIfEmpty()


                           //join b in _db.Brands on p.BrandId equals b.BrandId into join_p_b
                           //from p_b in join_p_b.DefaultIfEmpty()


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
                           SupName = p_Sup.SupName
                       };
            if (pdvm == null) return NotFound();
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            return View(pdvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(string? stext)
        {
            if (HttpContext.Session.GetString("StfId") == null)
            {

                TempData["ErrorMessage"] = "Kun mai me sit na naka";
                return RedirectToAction("Login", "Home");
            }

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

                       join Sup in _db.Suppliers on p.SupId equals Sup.SupId into join_p_Sup
                       from p_Sup in join_p_Sup.DefaultIfEmpty()

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
                           SupName = p_Sup.SupName
                       };
            if (pdvm == null) return NotFound();
            ViewBag.stext = stext;
            return View(pdvm);
        }
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("StfId") == null)
            {

                TempData["ErrorMessage"] = "Kun mai me sit na naka";
                return RedirectToAction("Login", "Home");
            }

            ViewData["Pdt"] = new SelectList(_db.ProductTypes, "PdtId", "PdtName");
            ViewData["Suppliers"] = new SelectList(_db.Suppliers, "SupId", "SupName");
            ViewData["Size"] = new SelectList(_db.Sizes, "SizeId", "SizeName");
            ViewData["Color"] = new SelectList(_db.Colors, "ColorId", "ColorName");
            ViewData["Target"] = new SelectList(_db.Targets, "TargetId", "TargetName");
            ViewData["Status"] = new SelectList(_db.Statuses, "StatusId", "StatusName");

            //ViewData["currentNavPD_Create"] = "Product/Create";
            ViewData["currentNav"] = "Product/Create";

            return View();
            //return RedirectToAction("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IFormFile? formFile, Product obj)
        {
            if (HttpContext.Session.GetString("StfId") == null)
            {

                TempData["ErrorMessage"] = "Kun mai me sit na naka";
                return RedirectToAction("Login", "Home");
            }

            ViewData["Pdt"] = new SelectList(_db.ProductTypes, "PdtId", "PdtName");
            ViewData["Suppliers"] = new SelectList(_db.Suppliers, "SupId", "SupName");
            ViewData["Size"] = new SelectList(_db.Sizes, "SizeId", "SizeName");
            ViewData["Color"] = new SelectList(_db.Colors, "ColorId", "ColorName");
            ViewData["Target"] = new SelectList(_db.Targets, "TargetId", "TargetName");
            ViewData["Status"] = new SelectList(_db.Statuses, "StatusId", "StatusName");

            try
            {
                if (ModelState.IsValid)
                {

                    _db.Products.Add(obj);
                    _db.SaveChanges();
                    var theid = obj.PdId;

                    if (formFile != null && formFile.Length > 0)
                    {
                        var FileName = theid;
                        var FileExtension = ".png";
                        var SaveFileName = FileName + FileExtension;
                        var SavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\imgpd");
                        var SaveFilePath = Path.Combine(SavePath, SaveFileName);

                        using (FileStream fs = System.IO.File.Create(SaveFilePath))
                        {
                            formFile.CopyTo(fs);
                            fs.Flush();
                        }
                    }
                    TempData["SuccessMessage"] = "Successfully!";

                    return RedirectToAction("Edit", new { id = theid });

                    //return RedirectToAction("Index");
                }
                //return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Failed to Save";
                //ViewBag.ErrorMessage = ex.Message;
                /* return เพื่อคืนค่า obj ไปที่ฟอร์ม ที่ user ได้กรอกมา */
                return View(obj);
            }
            ViewBag.ErrorMessage = "บันทึกข้อมูลไม่สำเร็จ กรุณาตรวจสอบ";
            /* return เพื่อคืนค่า obj ไปที่ฟอร์ม ที่ user ได้กรอกมา */
            return View(obj);
        }


        public IActionResult Edit(string id)
        {
            if (HttpContext.Session.GetString("StfId") == null)
            {

                TempData["ErrorMessage"] = "Kun mai me sit na naka";
                return RedirectToAction("Login", "Home");
            }

            ViewData["currentNav"] = "Product";

            ViewData["Pdt"] = new SelectList(_db.ProductTypes, "PdtId", "PdtName");
            ViewData["Suppliers"] = new SelectList(_db.Suppliers, "SupId", "SupName");
            ViewData["Size"] = new SelectList(_db.Sizes, "SizeId", "SizeName");
            ViewData["Color"] = new SelectList(_db.Colors, "ColorId", "ColorName");
            ViewData["Target"] = new SelectList(_db.Targets, "TargetId", "TargetName");
            ViewData["Status"] = new SelectList(_db.Statuses, "StatusId", "StatusName");
            //ตรวจสอบว่ามี่การส่ง id หรือไม่
            if (id == null)
            {
                TempData["ErrorMessage"] = "ระบุ id";
                return RedirectToAction("Index");
            }
            //ค้นหา Record ของ Product.pdId จาก id ที่ส่งมา
            var obj = _db.Products.Find(id);
            if (obj == null)
            {
                TempData["ErrorMessage"] = "ไม่พบข้อมูล";
                return RedirectToAction("Index");
            }
            //เพื่อให้ สามารถ แก้ไข ประเภทสินค้า และ ยี่ห้อ ผ่าน drop-down List ได้ โดยไม่ต้องป้อนเป็น primary key ของประเภทนั้นๆ
            ViewData["Pdt"] = new SelectList(_db.ProductTypes, "PdtId", "PdtName");
            //ViewData["Brand"] = new SelectList(_db.Brands, "BrandId", "BrandName");


            var fileName = id.ToString() + ".png";
            var imgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\imgpd");
            var filePath = Path.Combine(imgPath, fileName);
            if (System.IO.File.Exists(filePath))
            {
                ViewBag.ImgFile = "/imgpd/" + id + ".png";
            }
            else
            {
                ViewBag.ImgFile = "/imgpd/No_image2.png";
            }


            return View(obj);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(IFormFile? formFile, Product obj)
        {
            if (HttpContext.Session.GetString("StfId") == null)
            {

                TempData["ErrorMessage"] = "Kun mai me sit na naka";
                return RedirectToAction("Login", "Home");
            }

            ViewBag.formFile = formFile;
            ViewData["formFile"] = formFile;
            ViewData["currentNav"] = "Product";

            ViewData["Pdt"] = new SelectList(_db.ProductTypes, "PdtId", "PdtName");
            ViewData["Suppliers"] = new SelectList(_db.Suppliers, "SupId", "SupName");
            ViewData["Size"] = new SelectList(_db.Sizes, "SizeId", "SizeName");
            ViewData["Color"] = new SelectList(_db.Colors, "ColorId", "ColorName");
            ViewData["Target"] = new SelectList(_db.Targets, "TargetId", "TargetName");
            ViewData["Status"] = new SelectList(_db.Statuses, "StatusId", "StatusName");

            try
            {
                if (ModelState.IsValid)
                {
                    var theid = obj.PdId;
                    _db.Products.Update(obj);
                    _db.SaveChanges();

                    if (formFile != null && formFile.Length > 0)
                    {
                        var FileName = theid;
                        var FileExtension = ".png";
                        var SaveFileName = FileName + FileExtension;
                        var SavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\imgpd");
                        var SaveFilePath = Path.Combine(SavePath, SaveFileName);

                        using (FileStream fs = System.IO.File.Create(SaveFilePath))
                        {
                            formFile.CopyTo(fs);
                            fs.Flush();
                        }
                    }

                    TempData["SuccessMessage"] = "Successfully!";
                    return RedirectToAction("Edit", new { id = theid });

                    //return RedirectToAction("Index");

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


        public IActionResult Delete(string id)
        {
            if (HttpContext.Session.GetString("StfId") == null)
            {

                TempData["ErrorMessage"] = "Kun mai me sit na naka";
                return RedirectToAction("Login", "Home");
            }

            ViewData["Pdt"] = new SelectList(_db.ProductTypes, "PdtId", "PdtName");
            ViewData["Suppliers"] = new SelectList(_db.Suppliers, "SupId", "SupName");
            ViewData["Size"] = new SelectList(_db.Sizes, "SizeId", "SizeName");
            ViewData["Color"] = new SelectList(_db.Colors, "ColorId", "ColorName");
            ViewData["Target"] = new SelectList(_db.Targets, "TargetId", "TargetName");
            ViewData["Status"] = new SelectList(_db.Statuses, "StatusId", "StatusName");

            ViewData["currentNav"] = "Product";

            if (id == null)
            {
                TempData["ErrorMessage"] = "โปรดระบุ id";
                return RedirectToAction("Index");
            }
            var obj = _db.Products.Find(id);
            if (obj == null)
            {
                TempData["ErrorMessage"] = "ไม่พบข้อมูล";
                return RedirectToAction("Index");
            }




            var fileName = id.ToString() + ".png";
            var imgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\imgpd");
            var filePath = Path.Combine(imgPath, fileName);
            if (System.IO.File.Exists(filePath))
            {
                ViewBag.ImgFile = "/imgpd/" + id + ".png";
            }
            else
            {
                ViewBag.ImgFile = "/imgpd/No_image2.png";
            }





            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(string PdId)
        {

            if (HttpContext.Session.GetString("StfId") == null)
            {

                TempData["ErrorMessage"] = "Kun mai me sit na naka";
                return RedirectToAction("Login", "Home");
            }


            try
            {
                var obj = _db.Products.Find(PdId);
                if (ModelState.IsValid)
                {
                    _db.Products.Remove(obj);
                    _db.SaveChanges();
                    TempData["SuccessMessage"] = "Delete SuccessMessage";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "ลบข้อมูลไม่สำเร็จ";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "ลบข้อมูลไม่สำเร็จ : " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        //-----------------------------------------

        [HttpGet]
        [Route("/api/products")]
        public IActionResult GetProducts()

        {



            var pdvm = from p in _db.Products

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

            return Json(pdvm);

        }

        public IActionResult show(string id)
        {

            if (id == null)
            {
                TempData["ErrorMessage"] = "ระบุ id";
                //return RedirectToAction("Index");
                return RedirectToAction("Index", "Home");
            }
            //ค้นหา Record ของ Product.pdId จาก id ที่ส่งมา
            var obj = _db.Products.Find(id);
            if (obj == null)
            {
                TempData["ErrorMessage"] = "ไม่พบข้อมูล";
                //return RedirectToAction("Index");
                return RedirectToAction("Index", "Home");
            }

            //เพื่อให้ สามารถ แก้ไข ประเภทสินค้า และ size ผ่าน drop-down List ได้ โดยไม่ต้องป้อนเป็น primary key ของประเภทนั้นๆ
            ViewData["Pdt"] = new SelectList(_db.ProductTypes, "PdtId", "PdtName");
            string[] parts = id.Split('-');
            //string PrefixId = parts[0] + "-" + parts[1];
            string PrefixId = parts[0];

			var productFilter = from p in _db.Products
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
								where p.PdId.StartsWith(PrefixId) 
                                        //&& p_status.StatusName.Equals("วางจำหน่าย")
								select new PdFilterVM
								{
									PdId = p.PdId,  //รหัวสินค้า
									ColorId = p.ColorId,
									ColorName = p_color.ColorName, //สี
									SizeId = p.SizeId,
									SizeName = p_size.SizeName, //ขนาด
									TargetId = p.TargetId,
									TargetName = p_target.TargetName, //กลุ่มลูกค้า ชาย หญิง เด็ก
									PdName = p.PdName, //ชื่อสินค้า
									PdtId = p.PdtId,
									PdtName = p_pt.PdtName, //ประเภทสินค้า 
									PdPrice = p.PdPrice, //ราคา
									PdCost = p.PdCost, //ต้นทุน
									PdStk = p.PdStk, //คงเหลือ
									StatusName = p_status.StatusName, //สถาานะ
								};

			var items = productFilter.ToList(); // แปลงข้อมูลที่ได้จาก LINQ query เป็น List<DtlsVM>
            ViewData["item"] = items;
            //ViewData["colors"] = colors.Distinct(); ;

			return View(obj);
        }
        [HttpPost]
        public IActionResult GetFilteredProducts(int[] typeIds, int[] sizeIds, int[] colorIds, string targetName)
        {
            var productFilter = from p in _db.Products
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

								//where (typeIds == null || typeIds.Length == 0 || typeIds.Contains(Convert.ToInt32(p.PdtId)))
                                where (typeIds == null || typeIds.Length == 0 || (p_pt != null && typeIds.Contains(p_pt.PdtId)))
									&& (sizeIds == null || sizeIds.Length == 0 || sizeIds.Contains(Convert.ToInt32(p.SizeId)))
									&& (colorIds == null || colorIds.Length == 0 || colorIds.Contains(Convert.ToInt32(p.ColorId)))
									&& (targetName == null || targetName.Length == 0 || targetName.Contains(p_target.TargetName))
									&& p_status.StatusName.Equals("วางจำหน่าย")
								select new PdFilterVM
                                {
                                    PdId = p.PdId,  //รหัวสินค้า
                                    ColorId = p.ColorId,
                                    ColorName = p_color.ColorName, //สี
                                    SizeId = p.SizeId,
                                    SizeName = p_size.SizeName, //ขนาด
                                    TargetId = p.TargetId,
                                    TargetName = p_target.TargetName, //กลุ่มลูกค้า ชาย หญิง เด็ก
                                    PdName = p.PdName, //ชื่อสินค้า
                                    PdtId = p.PdtId,
                                    PdtName = p_pt.PdtName, //ประเภทสินค้า 
                                    PdPrice = p.PdPrice, //ราคา
                                    PdCost = p.PdCost, //ต้นทุน
                                    PdStk = p.PdStk, //คงเหลือ
                                    StatusName = p_status.StatusName, //สถาานะ
                                };

            Console.WriteLine("Products:");
			foreach (var product in productFilter)
			{
				Console.WriteLine($"Product Id: {product.PdId}");
			}

			Console.WriteLine("Size Ids:");
			foreach (var sizeId in sizeIds)
			{
				Console.WriteLine($"Size Id: {sizeId}");
			}

			return PartialView("_ProductList", productFilter);
        }

		[HttpPost]
		public ActionResult GetStock(string PrefixPdId, string colorId, string sizeId)
		{
			// ดึงข้อมูลจำนวนสินค้าคงเหลือจากฐานข้อมูล
			byte colorByte = byte.Parse(colorId);
			byte sizeByte = byte.Parse(sizeId);
			// โดยใช้ colorId และ sizeId ที่ได้รับจาก client
			// ส่งข้อมูลจำนวนสินค้าคงเหลือกลับไปยัง client
			var stockRemain =  (from p in _db.Products
								    where p.PdId.StartsWith(PrefixPdId) && p.ColorId == colorByte && p.SizeId == sizeByte
								select p.PdStk).FirstOrDefault();
			//return View(stockRemain);
			return Content(stockRemain.ToString());
		}


	}
}


//[
//    {
//        "pdId": "P001-01-01",
//        "colorName": "ขาว",
//        "sizeName": "XS   ",
//        "pdName": "เสื้อยืด",
//        "pdPrice": 490,
//        "pdCost": 100,
//        "pdStk": 5,
//        "pdtName": "ส่วนบน",
//        "statusId": null,
//        "statusName": null,
//        "targetId": null,
//        "targetName": "ผู้ชาย",
//        "supId": null
//    },
//    {
//    "pdId": "P001-02-01",
//        "colorName": "ดำ",
//        "sizeName": "XS   ",
//        "pdName": "เสื้อยืด",
//        "pdPrice": 490,
//        "pdCost": 100,
//        "pdStk": 8,
//        "pdtName": "ส่วนบน",
//        "statusId": null,
//        "statusName": null,
//        "targetId": null,
//        "targetName": "ผู้ชาย",
//        "supId": null
//    }
//]