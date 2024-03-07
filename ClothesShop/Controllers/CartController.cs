using System.Globalization;
using ClothesShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;
using ClothesShop.ViewModels;
using Microsoft.AspNetCore.Http;
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
        public IActionResult AddDtl(string pdid,int qty)
        {
			Console.WriteLine("AddDetail Function");
			Console.WriteLine($"Product Id: {pdid}");
			Console.WriteLine($"Quantity: {qty}");
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
                return RedirectToAction("Add", new { pdid = pdid ,qty = qty}); //ยังไม่ได้สร้าง ต้องไปสร้างก่อน
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
                obj.CdtlQty = qty;
                //obj.CdtlQty = 1;
                obj.CdtlPrice = pd.PdPrice;
                obj.CdtlMoney = pd.PdPrice * qty;
                _db.Entry(obj).State = EntityState.Added;
            }
            else
            {
                foreach (var obj in cdtl)
                {
                    obj.CdtlQty = obj.CdtlQty + qty;
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
        public IActionResult Add(string pdid, int qty)
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

                return RedirectToAction("AddDtl", new { pdid = pdid , qty = qty});
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


                          join color in _db.Colors on ctd_p.ColorId equals color.ColorId
                              into join_ctd_color
                          from ctd_color in join_ctd_color.DefaultIfEmpty()

                          join size in _db.Sizes on ctd_p.SizeId equals size.SizeId
                              into join_ctd_size
                          from ctd_size in join_ctd_size.DefaultIfEmpty()

                          where ctd.CartId == cartid
                          select new CtdVM
                          {
                              CartId = ctd.CartId,
                              PdId = ctd.PdId,
                              PdName = ctd_p.PdName,
                              ColorName = ctd_color.ColorName,
                              SizeName = ctd_size.SizeName,
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
        public IActionResult Check()
        {
            string cusid = HttpContext.Session.GetString("CusId");
            var cart = from ct in _db.Carts.Take(1)
                       where ct.CusId.Equals(cusid) && ct.CartCf != "Y"
                       select ct;
            int rowCount = cart.Count();
            if(rowCount > 0)
            {
                Cart obj = new Cart();
                foreach(var item in cart)
                {
                    obj = item;
                }
                HttpContext.Session.SetString("CartId", obj.CartId);
                HttpContext.Session.SetString("CartQty", obj.CartQty.ToString());
                HttpContext.Session.SetString("CartMoney",obj.CartMoney.ToString());
            }
            return RedirectToAction("Shop","Home");

        }
        public IActionResult Delete(string cartid) {
            var detail = from ctd in _db.CartDtls
                         where ctd.CartId.Equals(cartid)
                         select ctd;
            foreach(var item in detail)
            {
                _db.CartDtls.Remove(item);
            }
            _db.SaveChanges();
            //ลบส่วน master
            var master = _db.Carts.Find(cartid);
            if(master == null)
            {
                TempData["ErrorMessage"] = "ไม่พบตะกร้า";
                return RedirectToAction("Show","Cart",new {cartid = cartid});
            }
            _db.Carts.Remove(master);
            _db.SaveChanges();

            HttpContext.Session.Remove("CartId");
            HttpContext.Session.Remove("CartQty");
            HttpContext.Session.Remove("CartMoney");

            TempData["SuccessMessage"] = "ยกเลิกคำสั่งซื้อแล้ว";
            return RedirectToAction("Shop", "Home");
        }
        public IActionResult DeleteDtl(string pdid,string cartid)
        {
            var obj = _db.CartDtls.Find(cartid, pdid);
            if(obj == null)
            {
                TempData["ErrorMessage"] = "ไม่พบข้อมูล";
            }
            _db.CartDtls.Remove(obj);
            _db.SaveChanges();

            var cartmoney = _db.CartDtls.Where(a => a.CartId == cartid).Sum(a => a.CdtlMoney);
            var cartqty = _db.CartDtls.Where(a => a.CartId == cartid).Sum(a => a.CdtlQty);
            if(cartqty == 0)
            {
                var master = _db.Carts.Find(cartid);
                _db.Carts.Remove(master);
                _db.SaveChanges();

                HttpContext.Session.Remove("CartId");
                HttpContext.Session.Remove("CartQty");
                HttpContext.Session.Remove("CartMoney");

                TempData["SuccessMessage"] = "ยกเลิกคำสั่งซื้อแล้ว";
                return RedirectToAction("Shop", "Home");

            }
            else
            {
                var cart = _db.Carts.Find(cartid);
                cart.CartQty = cartqty;
                cart.CartMoney = cartmoney;
                _db.SaveChanges();

                HttpContext.Session.SetString("CartMoney", cartmoney.ToString());
                HttpContext.Session.SetString("CartQty", cartqty.ToString());
                return RedirectToAction("Show","Cart",new {cartid = cartid});
            }
        }
        public IActionResult Confirm(string cartid)
        {
            var cartdtl = from ctd in _db.CartDtls
                          where ctd.CartId.Equals(cartid)
                          select ctd;
            int rowCount = cartdtl.Count();
            if(rowCount == 0)
            {
                TempData["ErrorMessage"] = "การยืนยันผิดพลาด";
                return RedirectToAction("show", "Cart", new { cartid = cartid });
            }
            foreach(var detail in cartdtl)
            {
                Product pd = _db.Products.Find(detail.PdId);
                pd.PdStk = pd.PdStk - detail.CdtlQty;
                pd.PdLastSale = DateOnly.FromDateTime(DateTime.Now.Date);
                _db.Entry(pd).State = EntityState.Modified;

            }
            _db.SaveChanges();

            //update cart for confirm
            var master = _db.Carts.Find(cartid);
            master.CartCf = "Y";
            _db.Entry(master).State = EntityState.Modified;
            _db.SaveChanges();

            //del session cart
            HttpContext.Session.Remove("CartId");
            HttpContext.Session.Remove("CartQty");
            HttpContext.Session.Remove("CartMoney");

            TempData["SuccessMessage"] = "ยืนยันคำสั่งซื้อแล้ว";
            return RedirectToAction("Shop", "Home");
        }
        public IActionResult List(string cusid)
        {
            var cart = from c in _db.Carts
                       where c.CusId.Equals(cusid)
                       orderby c.CartId descending
                       select c;
            return View(cart);
        }
    }
}
