using System.Diagnostics;
using ClothesShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using ClothesShop.ViewModels;
using System.Drawing;
using System.Security.Policy;


namespace ClothesShop.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        private readonly ClothingShopContext _db;
        public HomeController(ClothingShopContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            ViewData["NoContainerClass"] = true;
            var recommend = from p in _db.Products

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

                       where p.PdId.Equals("P013-02-02") || p.PdId.Equals("P012-02-02") || p.PdId.Equals("P010-02-03")


                       select new PdVM
                       {

                           PdId = p.PdId,  //รหัวสินค้า
                           ColorName = p_color.ColorName, //สี
                           SizeName = p_size.SizeName!, //ขนาด
                           PdDtls = p.PdDtls,
                           TargetName = p_target.TargetName, //กลุ่มลูกค้า ชาย หญิง เด็ก
                           PdName = p.PdName, //ชื่อสินค้า
                           PdtName = p_pt.PdtName, //ประเภทสินค้า 
                           PdPrice = p.PdPrice, //ราคา
                           PdCost = p.PdCost, //ต้นทุน
                           PdStk = p.PdStk, //คงเหลือ
                           StatusName = p_status.StatusName, //สถาานะ

                       };



            if (recommend == null) return NotFound();
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            return View(recommend);
            //แก้ให้ลิงค์แสดงหน้าร้าน
            //return RedirectToAction("Shop");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string? userName, string userPass)
        {
            var cus = from c in _db.Customers
                      where c.CusLogin.Equals(userName) && c.CusPass.Equals(userPass)
                      select c;

            if (cus.ToList().Count() == 0)
            {
                TempData["ErrorMessage"] = "ผู้ใช้หรือรหัสผ่านไม่ถูกต้อง";
                return RedirectToAction("Login");

            }

            string CusId;
            string CusName;

            foreach (var item in cus)
            {
                CusId = item.CusId;
                CusName = item.CusName;

                HttpContext.Session.SetString("CusId", CusId);
                HttpContext.Session.SetString("CusName", CusName);

                var theRecord = _db.Customers.Find(CusId);

                theRecord.LastLogin = DateOnly.FromDateTime(DateTime.Now);
                _db.Entry(theRecord).State = EntityState.Modified;

            }
            _db.SaveChanges();
            //return RedirectToAction("Index");
            // *** ทำการตรวจสอบตะกร้าเดิม ที่ยังไม่ได้ยืนยัน ***
            return RedirectToAction("Check", "Cart");

        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Shop(string? stext)
        {
            if (stext == null)
            {
                return RedirectToAction("Index");
            }
            var pdvm = from p in _db.Products
                           //from p in _db.Products.Take(4)


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
                       where p.PdName.Contains(stext) ||
                            p_pt.PdtName.Contains(stext)

                       select new PdFilterVM

                       {

                           PdId = p.PdId,  //รหัวสินค้า
                           ColorId = p.ColorId,
                           ColorName = p_color.ColorName, //สี
                           SizeId = p_size.SizeId,
                           SizeName = p_size.SizeName, //ขนาด
                           TargetName = p_target.TargetName, //กลุ่มลูกค้า ชาย หญิง เด็ก
                           PdName = p.PdName, //ชื่อสินค้า
                           PdtName = p_pt.PdtName, //ประเภทสินค้า 
                           PdPrice = p.PdPrice, //ราคา
                           PdCost = p.PdCost, //ต้นทุน
                           PdStk = p.PdStk, //คงเหลือ
                           StatusName = p_status.StatusName, //สถาานะ

                       };
            var FilterCategory = from t in _db.ProductTypes
                                 select new
                                 {
                                     PdtId = t.PdtId,
                                     PdtName = t.PdtName,
                                 };
            var FilterColor = from c in _db.Colors
                              select new
                              {
                                  colorId = c.ColorId,
                                  colorName = c.ColorName,
                              };
            var FilterSize = from s in _db.Sizes
                             select new
                             {
                                 SizeId = s.SizeId,
                                 SizeName = s.SizeName,
                             };

            if (pdvm == null) return NotFound();
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            ViewData["FilterCategory"] = FilterCategory.ToList();
            ViewData["FilterColor"] = FilterColor.ToList();
            ViewData["FilterSize"] = FilterSize.ToList();
            
            ViewBag.stext = stext;
            return View(pdvm);
        }

        public IActionResult Shop()
        {
            var pdvm = from p in _db.Products
                           //from p in _db.Products.Take(4)


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



                       select new PdFilterVM

                       {

                           PdId = p.PdId,  //รหัวสินค้า
                           ColorId = p.ColorId,
                           ColorName = p_color.ColorName, //สี
                           SizeId = p_size.SizeId,
                           SizeName = p_size.SizeName, //ขนาด
                           TargetName = p_target.TargetName, //กลุ่มลูกค้า ชาย หญิง เด็ก
                           PdName = p.PdName, //ชื่อสินค้า
                           PdtName = p_pt.PdtName, //ประเภทสินค้า 
                           PdPrice = p.PdPrice, //ราคา
                           PdCost = p.PdCost, //ต้นทุน
                           PdStk = p.PdStk, //คงเหลือ
                           StatusName = p_status.StatusName, //สถาานะ

                       };
            var FilterCategory = from t in _db.ProductTypes
                                 select new
                                 {
                                     PdtId = t.PdtId,PdtName = t.PdtName,
                                 };
			var FilterColor = from c in _db.Colors
                        select new
                        {
                            colorId = c.ColorId, colorName = c.ColorName,
                        };
            var FilterSize = from s in _db.Sizes
                       select new{
				            SizeId = s.SizeId, 
                            SizeName = s.SizeName,
                        };

			if (pdvm == null) return NotFound();
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
			ViewData["FilterCategory"] = FilterCategory.ToList();
			ViewData["FilterColor"] = FilterColor.ToList();
            ViewData["FilterSize"] = FilterSize.ToList();
            return View(pdvm);
        }
        public IActionResult men()
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
					   
                       where p_target.TargetName.Equals("ผู้ชาย") && p_status.StatusName.Equals("วางจำหน่าย")


					   select new PdFilterVM

					   {

						   PdId = p.PdId,  //รหัวสินค้า
						   ColorId = p.ColorId,
						   ColorName = p_color.ColorName, //สี
						   SizeId = p_size.SizeId,
						   SizeName = p_size.SizeName, //ขนาด
						   TargetName = p_target.TargetName, //กลุ่มลูกค้า ชาย หญิง เด็ก
						   PdName = p.PdName, //ชื่อสินค้า
						   PdtName = p_pt.PdtName, //ประเภทสินค้า 
						   PdPrice = p.PdPrice, //ราคา
						   PdCost = p.PdCost, //ต้นทุน
						   PdStk = p.PdStk, //คงเหลือ
						   StatusName = p_status.StatusName, //สถาานะ

					   };
			var FilterCategory = from t in _db.ProductTypes
								 select new
								 {
									 PdtId = t.PdtId,
									 PdtName = t.PdtName,
								 };
			var FilterColor = from c in _db.Colors
							  select new
							  {
								  colorId = c.ColorId,
								  colorName = c.ColorName,
							  };
			var FilterSize = from s in _db.Sizes
							 select new
							 {
								 SizeId = s.SizeId,
								 SizeName = s.SizeName,
							 };

			if (pdvm == null) return NotFound();
			ViewBag.ErrorMessage = TempData["ErrorMessage"];
			ViewData["FilterCategory"] = FilterCategory.ToList();
			ViewData["FilterColor"] = FilterColor.ToList();
			ViewData["FilterSize"] = FilterSize.ToList();
			return View(pdvm);
        }
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult men(string? stext)
		{
			if (stext == null)
			{
				return RedirectToAction("men");
			}
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

					   where p_target.TargetName.Equals("ผู้ชาย") && p_status.StatusName.Equals("วางจำหน่าย")


					   select new PdFilterVM

					   {

						   PdId = p.PdId,  //รหัวสินค้า
						   ColorId = p.ColorId,
						   ColorName = p_color.ColorName, //สี
						   SizeId = p_size.SizeId,
						   SizeName = p_size.SizeName, //ขนาด
						   TargetName = p_target.TargetName, //กลุ่มลูกค้า ชาย หญิง เด็ก
						   PdName = p.PdName, //ชื่อสินค้า
						   PdtName = p_pt.PdtName, //ประเภทสินค้า 
						   PdPrice = p.PdPrice, //ราคา
						   PdCost = p.PdCost, //ต้นทุน
						   PdStk = p.PdStk, //คงเหลือ
						   StatusName = p_status.StatusName, //สถาานะ

					   };
			var FilterCategory = from t in _db.ProductTypes
								 select new
								 {
									 PdtId = t.PdtId,
									 PdtName = t.PdtName,
								 };
			var FilterColor = from c in _db.Colors
							  select new
							  {
								  colorId = c.ColorId,
								  colorName = c.ColorName,
							  };
			var FilterSize = from s in _db.Sizes
							 select new
							 {
								 SizeId = s.SizeId,
								 SizeName = s.SizeName,
							 };
			ViewBag.ErrorMessage = TempData["ErrorMessage"];
			ViewData["FilterCategory"] = FilterCategory.ToList();
			ViewData["FilterColor"] = FilterColor.ToList();
			ViewData["FilterSize"] = FilterSize.ToList();
			if (pdvm == null) return NotFound();
			ViewBag.stext = stext;
			return View(pdvm);
		}

		public IActionResult women()
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

					   where p_target.TargetName.Equals("ผู้หญิง") && p_status.StatusName.Equals("วางจำหน่าย")


					   select new PdFilterVM

					   {

						   PdId = p.PdId,  //รหัวสินค้า
						   ColorId = p.ColorId,
						   ColorName = p_color.ColorName, //สี
						   SizeId = p_size.SizeId,
						   SizeName = p_size.SizeName, //ขนาด
						   TargetName = p_target.TargetName, //กลุ่มลูกค้า ชาย หญิง เด็ก
						   PdName = p.PdName, //ชื่อสินค้า
						   PdtName = p_pt.PdtName, //ประเภทสินค้า 
						   PdPrice = p.PdPrice, //ราคา
						   PdCost = p.PdCost, //ต้นทุน
						   PdStk = p.PdStk, //คงเหลือ
						   StatusName = p_status.StatusName, //สถาานะ

					   };
			var FilterCategory = from t in _db.ProductTypes
								 select new
								 {
									 PdtId = t.PdtId,
									 PdtName = t.PdtName,
								 };
			var FilterColor = from c in _db.Colors
							  select new
							  {
								  colorId = c.ColorId,
								  colorName = c.ColorName,
							  };
			var FilterSize = from s in _db.Sizes
							 select new
							 {
								 SizeId = s.SizeId,
								 SizeName = s.SizeName,
							 };

			if (pdvm == null) return NotFound();
			ViewBag.ErrorMessage = TempData["ErrorMessage"];
			ViewData["FilterCategory"] = FilterCategory.ToList();
			ViewData["FilterColor"] = FilterColor.ToList();
			ViewData["FilterSize"] = FilterSize.ToList();
			return View(pdvm);
        }

        public IActionResult kids()
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

					   where p_target.TargetName.Equals("เด็ก") && p_status.StatusName.Equals("วางจำหน่าย")


					   select new PdFilterVM

					   {

						   PdId = p.PdId,  //รหัวสินค้า
						   ColorId = p.ColorId,
						   ColorName = p_color.ColorName, //สี
						   SizeId = p_size.SizeId,
						   SizeName = p_size.SizeName, //ขนาด
						   TargetName = p_target.TargetName, //กลุ่มลูกค้า ชาย หญิง เด็ก
						   PdName = p.PdName, //ชื่อสินค้า
						   PdtName = p_pt.PdtName, //ประเภทสินค้า 
						   PdPrice = p.PdPrice, //ราคา
						   PdCost = p.PdCost, //ต้นทุน
						   PdStk = p.PdStk, //คงเหลือ
						   StatusName = p_status.StatusName, //สถาานะ

					   };
			var FilterCategory = from t in _db.ProductTypes
								 select new
								 {
									 PdtId = t.PdtId,
									 PdtName = t.PdtName,
								 };
			var FilterColor = from c in _db.Colors
							  select new
							  {
								  colorId = c.ColorId,
								  colorName = c.ColorName,
							  };
			var FilterSize = from s in _db.Sizes
							 select new
							 {
								 SizeId = s.SizeId,
								 SizeName = s.SizeName,
							 };

			if (pdvm == null) return NotFound();
			ViewBag.ErrorMessage = TempData["ErrorMessage"];
			ViewData["FilterCategory"] = FilterCategory.ToList();
			ViewData["FilterColor"] = FilterColor.ToList();
			ViewData["FilterSize"] = FilterSize.ToList();
			return View(pdvm);
        }

        //------------------------------------- register ------------------------------------
        public IActionResult Register()
        {

            return View();

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(Customer obj)
        {
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

                    TempData["SuccessMessage"] = "Successfully registered!";
                    // Redirect to appropriate action after successful registration
                    return RedirectToAction("Login", "Home");
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


    }

}
    