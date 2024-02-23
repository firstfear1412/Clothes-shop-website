using ClothesShop.Models;
using Microsoft.AspNetCore.Mvc;
using ClothesShop.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;

namespace ClothesShop.Controllers
{
    public class ProductController : Controller
    {
        public readonly ClothingShopContext _db;
        public ProductController(ClothingShopContext db) { _db = db; }
        public IActionResult Index()
        {
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
                       };
            if (pdvm == null) return NotFound();
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            return View(pdvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(string? stext)
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
        public IActionResult Create()
        {
            ViewData["Pdt"] = new SelectList(_db.ProductTypes,"PdtId","PdtName");
            //ViewData["Brand"] = new SelectList(_db.Brands, "BrandId", "BrandName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product obj)
        {
            try
            {
                if(ModelState.IsValid) { 
                    _db.Products.Add(obj);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                ViewBag.ErrorMessage = "บันทึกข้อมูลไม่สำเร็จ กรุณาตรวจสอบ";
                /* return เพื่อคืนค่า obj ไปที่ฟอร์ม ที่ user ได้กรอกมา */
                return View(obj);
            }
        }
        public IActionResult Edit(string id)
        {
            //ตรวจสอบว่ามี่การส่ง id หรือไม่
            if(id== null)
            {
                TempData["ErrorMessage"] = "ระบุ id";
                return RedirectToAction("Index");
            }
            //ค้นหา Record ของ Product.pdId จาก id ที่ส่งมา
            var obj = _db.Products.Find(id);
            if(obj == null)
            {
                TempData["ErrorMessage"] = "ไม่พบข้อมูล";
                return RedirectToAction("Index");
            }
            //เพื่อให้ สามารถ แก้ไข ประเภทสินค้า และ ยี่ห้อ ผ่าน drop-down List ได้ โดยไม่ต้องป้อนเป็น primary key ของประเภทนั้นๆ
            ViewData["Pdt"] = new SelectList(_db.ProductTypes, "PdtId", "PdtName");
            //ViewData["Brand"] = new SelectList(_db.Brands, "BrandId", "BrandName");
            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product obj)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    _db.Products.Update(obj);
                    _db.SaveChanges();
                    return RedirectToAction("Index");

                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(obj);
            }
            ViewBag.ErrorMessage = "การแก้ไขผิดพลาด";
            ViewData["Pdt"] = new SelectList(_db.ProductTypes, "PdtId", "PdtName", obj.PdId);
            //ViewData["Brand"] = new SelectList(_db.Brands, "BrandId", "BrandName", obj.BrandId);
            return View(obj);
        }
        public IActionResult Delete(string id)
        {
            if(id == null)
            {
                TempData["ErrorMessage"] = "โปรดระบุ id";
                return RedirectToAction("Index");
            }
            var obj = _db.Products.Find(id);
            if(obj == null)
            {
                TempData["ErrorMessage"] = "ไม่พบข้อมูล";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(string PdId)
        {
            try
            {
                var obj = _db.Products.Find(PdId);
                if (ModelState.IsValid)
                {
                    _db.Products.Remove(obj);
                    _db.SaveChanges();
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