using ClothesShop.Models;
using ClothesShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Dynamic;
using System.Globalization;

namespace KuShop.Controllers
{
    public class BuyingController : Controller
    {
        //สร้าง Field สำหรับใช้งาน DBContext ที่กำหนด
        private readonly ClothingShopContext _db;

        //สร้าง Constructor สำหรับตัว Controller เมื่อเริ่มต้นให้ใช้ Obj ของ DBContext
        public BuyingController(ClothingShopContext db)
        { _db = db; }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("StfId") == null)
            {

                TempData["ErrorMessage"] = "Kun mai me sit na naka";
                return RedirectToAction("Login", "Home");
            }

            ViewData["currentNav"] = "Product/Buying";
            //if (HttpContext.Session.GetString("StfId") == null)
            //{
            //    TempData["Errormessage"] = "คุณไม่มีสิทธิใช้งาน";
            //    return RedirectToAction("Shop", "Home");
            //}

            //DateOnly thedate = DateOnly.FromDateTime(DateTime.Now.Date);
            //var pd = from b in _db.Buyings
            //         join s in _db.Suppliers on b.SupId equals s.SupId into join_b_s
            //         from b_s in join_b_s.DefaultIfEmpty()
            //         where b.BuyDate == thedate
            //         select new BuyingVM
            //         {
            //             BuyId = b.BuyId,
            //             SupId = b.SupId,
            //             SupName = b_s.SupName,
            //             BuyDate = b.BuyDate,
            //             StfId = b.StfId,
            //             BuyDocId = b.BuyDocId,
            //             Saleman = b.Saleman,
            //             BuyQty = b.BuyQty,
            //             BuyMoney = b.BuyMoney,
            //             BuyRemark = b.BuyRemark
            //         };
            //return View(pd);

            var theMonth = DateTime.Now.Month;
            var theYear = DateTime.Now.Year;
            DateOnly sDate = new DateOnly(theYear, theMonth, 1);
            //วันสุดท้ายคือวันที 1 ของเดือนหน้า ลบไป1วัน
            DateOnly eDate = new DateOnly(theYear, theMonth, 1).AddMonths(1).AddDays(-1);
            var pd = from b in _db.Buyings
                     join s in _db.Suppliers on b.SupId equals s.SupId into join_b_s
                     from b_s in join_b_s.DefaultIfEmpty()
                     where b.BuyDate >= sDate && b.BuyDate <= eDate

                     select new BuyingVM
                     {
                         BuyId = b.BuyId,
                         SupId = b.SupId,
                         SupName = b_s.SupName,
                         BuyDate = b.BuyDate,
                         StfId = b.StfId,
                         BuyDocId = b.BuyDocId,
                         Saleman = b.Saleman,
                         BuyQty = b.BuyQty,
                         BuyMoney = b.BuyMoney,
                         BuyRemark = b.BuyRemark
                     };
            return View(pd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(DateOnly sDate, DateOnly eDate)
        {
            if (HttpContext.Session.GetString("StfId") == null)
            {

                TempData["ErrorMessage"] = "Kun mai me sit na naka";
                return RedirectToAction("Login", "Home");
            }

            //if (HttpContext.Session.GetString("StfId") == null)
            //{
            //    TempData["Errormessage"] z= "คุณไม่มีสิทธิใช้งาน";
            //    return RedirectToAction("Shop", "Home");
            //}

            var pd = from b in _db.Buyings
                     join s in _db.Suppliers on b.SupId equals s.SupId into join_b_s
                     from b_s in join_b_s.DefaultIfEmpty()
                     where b.BuyDate >= sDate && b.BuyDate <= eDate
                     select new BuyingVM
                     {
                         BuyId = b.BuyId,
                         SupId = b.SupId,
                         SupName = b_s.SupName,
                         BuyDate = b.BuyDate,
                         StfId = b.StfId,
                         BuyDocId = b.BuyDocId,
                         Saleman = b.Saleman,
                         BuyQty = b.BuyQty,
                         BuyMoney = b.BuyMoney,
                         BuyRemark = b.BuyRemark
                     };
            return View(pd);
        }
        //public IActionResult Index(DateOnly thedate)
        //{
        //    //if (HttpContext.Session.GetString("StfId") == null)
        //    //{
        //    //    TempData["Errormessage"] = "คุณไม่มีสิทธิใช้งาน";
        //    //    return RedirectToAction("Shop", "Home");
        //    //}
        //    var pd = from b in _db.Buyings
        //             join s in _db.Suppliers on b.SupId equals s.SupId into join_b_s
        //             from b_s in join_b_s.DefaultIfEmpty()
        //             where b.BuyDate == thedate
        //             select new BuyingVM
        //             {
        //                 BuyId = b.BuyId,
        //                 SupId = b.SupId,
        //                 SupName = b_s.SupName,
        //                 BuyDate = b.BuyDate,
        //                 StfId = b.StfId,
        //                 BuyDocId = b.BuyDocId,
        //                 Saleman = b.Saleman,
        //                 BuyQty = b.BuyQty,
        //                 BuyMoney = b.BuyMoney,
        //                 BuyRemark = b.BuyRemark
        //             };
        //    return View(pd);
        //}

        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("StfId") == null)
            {

                TempData["ErrorMessage"] = "Kun mai me sit na naka";
                return RedirectToAction("Login", "Home");
            }

            ViewData["currentNav"] = "Product/Buying";

            //GenID เพื่อไปแสดงใน View
            string theId;
            int rowCount;
            int i = 0;
            CultureInfo us = new CultureInfo("en-US");
            do
            {
                //สร้าง id จากปีและเดือนปัจจุบัน และต่อด้วย String 000x
                i++;
                theId = string.Concat(DateTime.Now.ToString("'BUY'yyyyMMdd", us), i.ToString("00"));
                //ทำการตรวจสอบว่ามี id ที่ซ้ำกันหรือไม่ วนจนกว่าจะไม่ซ่ำ
                var buying = from b in _db.Buyings
                             where b.BuyId.Equals(theId)
                             select b;
                rowCount = buying.ToList().Count;
            }
            while (rowCount != 0);
            //ส่งค่า ID ให้ View
            ViewBag.BuyId = theId;
            //อ่านข้อมูลจากตารางลง SelectList แล้วใส่ข้อมูลลงตัว ViewData
            ViewData["Sup"] = new SelectList(_db.Suppliers, "SupId", "SupName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Buying obj)
        {

            if (HttpContext.Session.GetString("StfId") == null)
            {

                TempData["ErrorMessage"] = "Kun mai me sit na naka";
                return RedirectToAction("Login", "Home");
            }

            ViewData["currentNav"] = "Product/Buying";

            try
            {
                if (ModelState.IsValid)
                {
                    _db.Buyings.Add(obj);
                    _db.SaveChanges();
                    return RedirectToAction("Show", "Buying", new { buyid = obj.BuyId });
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "การบันทึกผิดพลาด";
                return View(obj);
            }
            TempData["ErrorMessage"] = "การบันทึกผิดพลาด";
            return View(obj);
        }

        public IActionResult Show(string buyid)
        {

            if (HttpContext.Session.GetString("StfId") == null)
            {

                TempData["ErrorMessage"] = "Kun mai me sit na naka";
                return RedirectToAction("Login", "Home");
            }

            ViewData["currentNav"] = "Product/Buying";
            //ตรวจสอบMaster
            if (buyid == null)
            {
                TempData["ErrorMessage"] = "ต้องระบุเลขที่";
                return RedirectToAction("Index");
            }
            //ได้ข้อมูล Buying เป็นส่วน Master
            var buying = from b in _db.Buyings
                         where b.BuyId == buyid
                         select b;
            if (buying.ToList().Count == 0)
            {
                TempData["ErrorMessage"] = "ไม่พบเอกสาร";
                return RedirectToAction("Index");
            }

            foreach (var s in buying)
            {
                //เพื่อส่งชื่อ Supplier ไปแสดงผลที่ View
                ViewBag.SupName = _db.Suppliers.FirstOrDefault(sp => sp.SupId == s.SupId).SupName;
            }

            //เลือกข้อมูลของตะกร้าที่ระบุ 
            //สร้าง ViewModel ชื่อ CidVM เพื่อแสดงชื่อสินค้าของ CartDtl (ส่วน Detail)
            var buydtl = from bd in _db.BuyDtls
                         join p in _db.Products on bd.PdId equals p.PdId into join_bd_p
                         from bd_p in join_bd_p.DefaultIfEmpty()
                         where bd.BuyId == buyid
                         select new BdVM
                         {
                             BuyId = bd.BuyId,
                             PdId = bd.PdId,
                             PdName = bd_p.PdName,
                             BdtlMoney = bd.BdtlMoney,
                             BdtlPrice = bd.BdtlPrice,
                             BdtlQty = bd.BdtlQty
                         };
            ViewBag.theid = buyid;
            //สร้าง dynamic model
            //เพื่อส่งข้อมูลให้ View แบบสองตารางพร้อมกัน
            dynamic DyModel = new ExpandoObject();
            //แบ่งเป็นส่วน Master รับข้อมูลจาก Obj cart
            DyModel.Master = buying;
            //ส่วน Detail รับข้อมูลจาก Obj cartdtl
            DyModel.Detail = buydtl;
            //ส่ง Dynamic Model ไปที่ View
            return View(DyModel);
        }

        public IActionResult Edit(string buyid)
        {
            if (HttpContext.Session.GetString("StfId") == null)
            {

                TempData["ErrorMessage"] = "Kun mai me sit na naka";
                return RedirectToAction("Login", "Home");
            }

            ViewData["currentNav"] = "Product/Buying";
            if (buyid == null)
            {
                TempData["ErrorMessage"] = "ต้องระบุเลขที่";
                return RedirectToAction("Index");
            }

            var obj = _db.Buyings.Find(buyid);
            if (obj == null)
            {
                TempData["ErrorMessage"] = "ไม่พบเอกสาร";
                return RedirectToAction("Index");
            }
            ViewData["SupId"] = new SelectList(_db.Suppliers, "SupId", "SupName", obj.SupId);
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Buying obj)
        {

            if (HttpContext.Session.GetString("StfId") == null)
            {

                TempData["ErrorMessage"] = "Kun mai me sit na naka";
                return RedirectToAction("Login", "Home");
            }

            ViewData["currentNav"] = "Product/Buying";
            try
            {
                if (ModelState.IsValid)
                {
                    _db.Buyings.Update(obj);
                    _db.SaveChanges();
                    return RedirectToAction("Show", "Buying", new { buyid = obj.BuyId });
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "การแก้ไขผิดพลาด";
                return View(obj);
            }
            TempData["ErrorMessage"] = "การแก้ไขผิดพลาด";
            return View(obj);
        }

        public IActionResult Delete(string buyid)
        {
            if (HttpContext.Session.GetString("StfId") == null)
            {

                TempData["ErrorMessage"] = "Kun mai me sit na naka";
                return RedirectToAction("Login", "Home");
            }

            ////////////////////////
            // ลบ Master
            var master = _db.Buyings.Find(buyid);
            _db.Buyings.Remove(master);
            _db.SaveChanges();

            ////////////////////////
            //ลบ Master แล้วต้องลบ Detail ด้วย
            //เลือกข้อมูลเป็น Set ต้องใช้ linq
            var detail = from bd in _db.BuyDtls
                         where bd.BuyId == buyid
                         select bd;
            foreach (var item in detail)
            {
                _db.BuyDtls.Remove(item);
                ////////////
                //Update Stock สินค้า
                var pd = _db.Products.Find(item.PdId);
                //ปรับค่า Stock ปัจจุบัน(ลบ)
                pd.PdStk = pd.PdStk - item.BdtlQty;
            }
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult CreateDtl(string buyid)
        {
            if (HttpContext.Session.GetString("StfId") == null)
            {

                TempData["ErrorMessage"] = "Kun mai me sit na naka";
                return RedirectToAction("Login", "Home");
            }

            if (buyid == null)
            {
                TempData["ErrorMessage"] = "ต้องระบุเลขที่";
                return RedirectToAction("Show", new { buyid = buyid });
            }

            // ดึงข้อมูลสินค้าจากตาราง Products พร้อมกับข้อมูลขนาดและสี
            // ดึงข้อมูลสินค้าจากตาราง Products พร้อมกับข้อมูลขนาดและสี
            var productsWithDetails = _db.Products
                .Select(p => new
                {
                    p.PdId,
                    p.PdName,
                    ColorName = _db.Colors.FirstOrDefault(c => c.ColorId == p.ColorId) != null ? _db.Colors.FirstOrDefault(c => c.ColorId == p.ColorId).ColorName : null,
                    SizeName = _db.Sizes.FirstOrDefault(s => s.SizeId == p.SizeId) != null ? _db.Sizes.FirstOrDefault(s => s.SizeId == p.SizeId).SizeName : null
                })
                .ToList();


            // สร้าง SelectList โดยใช้ชื่อสินค้าพร้อมกับขนาดและสี
            var productList = productsWithDetails.Select(p => new SelectListItem
            {
                Value = p.PdId,
                Text = $"{p.PdName} - Color: {p.ColorName}, Size: {p.SizeName}"
            });

            // ส่ง SelectList ไปยัง View
            ViewData["PdId"] = new SelectList(productList, "Value", "Text");
            ViewBag.BuyId = buyid;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateDtl(BuyDtl obj)
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
                    _db.BuyDtls.Add(obj);
                    _db.SaveChanges();

                    ///////////////
                    //ส่วนปรับยอด Master
                    //
                    //หายอดรวม Detail
                    var buymoney = _db.BuyDtls.Where(a => a.BuyId == obj.BuyId).Sum(b => b.BdtlMoney);
                    var buyqty = _db.BuyDtls.Where(a => a.BuyId == obj.BuyId).Sum(b => b.BdtlQty);
                    //Update Buying
                    var buy = _db.Buyings.Find(obj.BuyId);
                    buy.BuyQty = buyqty;
                    buy.BuyMoney = buymoney;
                    _db.SaveChanges();

                    ////////////
                    //Update Stock สินค้า
                    var pd = _db.Products.Find(obj.PdId);
                    //ปรับค่า Stock ปัจจุบัน(เพิ่ม) และ วันที่ขายวันสุดท้ายจากระบบ
                    pd.PdStk = pd.PdStk + obj.BdtlQty;
                    pd.PdLastBuy = buy.BuyDate;
                    pd.PdCost = obj.BdtlPrice;
                    //_db.Entry(pd).State = EntityState.Modified;
                    _db.SaveChanges();

                    return RedirectToAction("Show", "Buying", new { buyid = obj.BuyId });
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "การบันทึกผิดพลาด";
                return RedirectToAction("Show", "Buying", new { buyid = obj.BuyId });
            }
            TempData["ErrorMessage"] = "การบันทึกผิดพลาด";
            return RedirectToAction("Show", "Buying", new { buyid = obj.BuyId });
        }






        public IActionResult DeleteDtl(string pdid, string buyid)
        {
            if (HttpContext.Session.GetString("StfId") == null)
            {

                TempData["ErrorMessage"] = "Kun mai me sit na naka";
                return RedirectToAction("Login", "Home");
            }

            ////////////////////////
            ///หา Detail
            var obj = _db.BuyDtls.Find(buyid, pdid);

            ////////////
            //Update Stock สินค้า
            var pd = _db.Products.Find(obj.PdId);
            //ปรับค่า Stock ปัจจุบัน(ลบ)
            pd.PdStk = pd.PdStk - obj.BdtlQty;
            //pd.PdLastBuy = buy.BuyDate;
            //pd.PdCost = obj.BdtlPrice;
            _db.SaveChanges();

            /////ลบ BuyDtls
            _db.BuyDtls.Remove(obj);
            _db.SaveChanges();

            ///////////////
            //ส่วนปรับยอด Master
            //
            //หายอดรวม Detail
            var buymoney = _db.BuyDtls.Where(a => a.BuyId == buyid).Sum(b => b.BdtlMoney);
            var buyqty = _db.BuyDtls.Where(a => a.BuyId == buyid).Sum(b => b.BdtlQty);
            //Update Buying
            var buy = _db.Buyings.Find(buyid);
            buy.BuyQty = buyqty;
            buy.BuyMoney = buymoney;
            _db.SaveChanges();

            return RedirectToAction("Show", "Buying", new { buyid = buyid });
        }
    }
}
