using System.Globalization;
using ClothesShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;
using ClothesShop.ViewModels;
namespace ClothesShop.Controllers
{
    public class CartController : Controller
    {
        private readonly ClothingShopContext _db;
        public CartController(ClothingShopContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddDtl(string pdid)
        {
            //ตรวจสอบ Login??
            if (HttpContext.Session.GetString("CusId") == null)
            {
                TempData["ErrorMessage"] = "Login ก่อนซื้อสินค้า";
                return RedirectToAction("Login", "Home"); //ยังไม่ได้สร้าง ต้องไปสร้างก่อน
            }
            //ตรวจสอบว่ามี pdid ส่งมา
            if (pdid == null)
            {
                TempData["ErrorMessage"] = "ต้องระบุรหัสสินค้า";
                return RedirectToAction("Index", "Home");
            }
            //ตรวจสอบว่ามีตะกร้า?? ถ้าไม่มีก็สร้างตะกร้า
            if (HttpContext.Session.GetString("CartId") == null)
            {
                return RedirectToAction("Add", new { pdid = pdid }); //ยังไม่ได้สร้าง ต้องไปสร้างก่อน
            }
            //ถ้ามีแล้วบันทึกข้อมูลสินค้าลงตะกร้า
            //หาข้อมูลสินค้า - ราคาขาย
            var pd = _db.Products.Find(pdid);
            string CartId = HttpContext.Session.GetString("CartId");
            var cdtl = from cd in _db.CartDtls
                       where cd.CartId.Equals(CartId)
                       && cd.PdId.Equals(pdid)
                       select cd;
            if (cdtl.Count() == 0)
            {
                CartDtl obj = new CartDtl();
                obj.CartId = CartId;
                obj.PdId = pdid;
                obj.CdtlQty = 1;
                obj.CdtlPrice = pd.PdPrice;
                obj.CdtlMoney = pd.PdPrice * 1;
                _db.Entry(obj).State = EntityState.Added;
            }
            else
            {
                foreach (var obj in cdtl)
                {
                    obj.CdtlQty = obj.CdtlQty + 1;
                    obj.CdtlMoney = pd.PdPrice * (obj.CdtlQty);
                    _db.Entry(obj).State = EntityState.Modified;
                }
            }
            _db.SaveChanges();
            //เมื่อ Detail เปลี่ยน Master ก็เปลี่ยน
            //หายอดรวมของ Detail
            var CartMoney = _db.CartDtls.Where(a => a.CartId == CartId).Sum(a => a.CdtlMoney);
            var CartQty = _db.CartDtls.Where(a => a.CartId == CartId).Sum(a => a.CdtlQty);
            //Update db
            var cart = _db.Carts.Find(CartId);
            cart.CartQty = CartQty;
            cart.CartMoney = CartMoney;
            _db.SaveChanges();

            //Update Session เพื่อแสดงผล
            HttpContext.Session.SetString("CartQty", CartQty.ToString());
            HttpContext.Session.SetString("CartMoney", CartMoney.ToString());

            return RedirectToAction("Index", "Home");

        }
        public IActionResult Add(string pdid)
        {
            //Gen CartId
            string theId;
            int rowCount = 0;
            int i = 0;
            string today;
            string CusId = HttpContext.Session.GetString("CusId");

            CultureInfo us = new CultureInfo("en-US");
            do
            {
                //สร้าง id จากปีเดิอนปัจจุบันต่อด้วยลำดับ รูปแบบ 000x
                i++;
                today = DateTime.Now.ToString("'CT'yyyyMMdd");
                theId = string.Concat(today, i.ToString("0000"));
                //ทำการตรวจสอบว่ามี CartId นี้แล้วหรือยัง
                var cart = from ct in _db.Carts
                           where ct.CartId.Equals(theId)
                           select ct;
                rowCount = cart.Count();
            } while (rowCount != 0);
            //ทำการบันทึกลง Table Cart
            try
            {
                //สร้าง Obj Cart และกำหนด Field ต่างๆ
                Cart obj = new Cart();
                obj.CartId = theId;
                obj.CusId = CusId;
                obj.CartDate = DateOnly.FromDateTime(DateTime.Now.Date);
                //obj.CartDate = DateTime.Now.Date;

                obj.CartQty = 0;
                obj.CartMoney = 0;
                //กำหนดสถานะทำงานเป็น Add และสั่งบันทึก
                _db.Entry(obj).State = EntityState.Added;
                _db.SaveChanges();

                //บันทึกลง Session Cart
                HttpContext.Session.SetString("CartId", theId);
                HttpContext.Session.SetString("CartQty", "0");
                HttpContext.Session.SetString("CartMoney", "0");

                return RedirectToAction("AddDtl", new { pdid = pdid });
            }catch (Exception ex) {
                TempData["ErrorMessage"] = "การบันทึกผิดพลาด";
                return RedirectToAction("Index", "Home");
            }


        }
        public IActionResult Show(string cartid)
        {
            //Master
            //ตรวจสอบว่าเป็นตะกร้าของลูกค้าที่ใช้งานอยู่หรือไม่
            //ได้ข้อมูล Cart เป็นส่วน Master
            string cusid = HttpContext.Session.GetString("CusId");
            var cart = from ct in _db.Carts
                       where ct.CartId == cartid &&
                           ct.CusId == cusid
                       select ct;
            if (cart == null)
            {
                TempData["ErrorMessage"] = "ไม่พบตะกร้าที่ระบุ";
                return RedirectToAction("Index", "Home");
            }

            //Detail เลือกข้อมูลของตะกร้า+สร้าง ViewModel CtdVM แสดงชื่อสินค้า
            var cartdtl = from ctd in _db.CartDtls
                          join p in _db.Products on ctd.PdId equals p.PdId
                              into join_ctd_p
                          from ctd_p in join_ctd_p.DefaultIfEmpty()
                          where ctd.CartId == cartid
                          select new CtdVM
                          {
                              CartId = ctd.CartId,
                              PdId = ctd.PdId,
                              PdName = ctd_p.PdName,
                              CdtlMoney = ctd.CdtlMoney,
                              CdtlPrice = ctd.CdtlPrice,
                              CdtlQty = ctd.CdtlQty
                          };
            //สร้าง Dynamic model เพื่อส่งข้อมูลให้ View เป็นสองตารางพร้อมกัน
            dynamic DyModel = new ExpandoObject();
            //ระบุส่วน Master รับข้อมูลจาก Obj cart
            DyModel.Master = cart;
            //ระบุส่วน Detail รับข้อมูลจาก Obj cartdtl
            DyModel.Detail = cartdtl;
            //ส่ง Dynamic Model ไปที่ View
            return View(DyModel);

        }
    }
}
