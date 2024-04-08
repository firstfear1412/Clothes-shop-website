using ClothesShop.Models;
using ClothesShop.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ClothesShop.Controllers
{
    public class ManageSuppliersController : Controller
    {
        private readonly ClothingShopContext _db;
        public ManageSuppliersController(ClothingShopContext db)
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

            ViewData["currentNav"] = "ManageSuppliers";

            //var pd = from p in _db.Products
            //select p;
            var SupplierVM = from sup in _db.Suppliers

                          select new SupplierVM
                          {
                              SupId = sup.SupId,
                              SupName = sup.SupName,
                              SupContact = sup.SupContact,
                              SupTel = sup.SupTel,
                              SupEmail = sup.SupEmail,
                              SupAdd = sup.SupAdd,
                              SupRemark = sup.SupRemark,
                              
                           

                          };

 
            if (SupplierVM == null) return NotFound();
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            return View(SupplierVM);
        }
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("StfId") == null)
            {

                TempData["ErrorMessage"] = "Kun mai me sit na naka";
                return RedirectToAction("Login", "Home");
            }

            ViewData["currentNav"] = "ManageSuppliers/Create";


            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Supplier obj)
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
                    var existingCustomer = _db.Suppliers.FirstOrDefault(c => c.SupId == obj.SupId);
                    if (existingCustomer != null)
                    {
                        // CusLogin already exists, return error message or redirect as appropriate
                        ViewBag.ErrorMessage = "User ID already exists";
                        return View(obj);
                    }
                    // Get the last Customer Id from the database
                    string lastCustomerId = _db.Suppliers.Select(c => c.SupId).OrderByDescending(c => c).FirstOrDefault();

                    // Extract the numeric part of the last Customer Id
                    string numericPart = lastCustomerId.Substring(2); // Assuming format SPXXX

                    // Convert the numeric part to integer and increment
                    int nextId = int.Parse(numericPart) + 1;

                    // Generate the next Customer Id
                    string nextCustomerId = "SP" + nextId.ToString("000");

                    // Assign the generated Customer Id to the new Customer object
                    obj.SupId = nextCustomerId;
                    //obj.CusId = "C004";

                    // Add the new Customer to the database
                    _db.Suppliers.Add(obj);
                    _db.SaveChanges();

                    TempData["SuccessMessage"] = "Successfully Create!";
                    // Redirect to appropriate action after successful registration
                    return RedirectToAction("Edit", "ManageSuppliers", new { id = nextCustomerId });
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

            ViewData["currentNav"] = "ManageSuppliers";

            if (id == null)
            {
                TempData["ErrorMessage"] = "ระบุ id";
                return RedirectToAction("Index");
            }
            //ค้นหา Record ของ Product.pdId จาก id ที่ส่งมา
            var obj = _db.Suppliers.Find(id);
            if (obj == null)
            {
                TempData["ErrorMessage"] = "ไม่พบข้อมูล";
                return RedirectToAction("Index");
            }



            return View(obj);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Supplier obj)
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
                    _db.Suppliers.Update(obj);
                    _db.SaveChanges();
                    //return RedirectToAction("Index");
                    return RedirectToAction("Edit", "ManageSuppliers", new { id = obj.SupId });
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(obj);
            }
            ViewBag.ErrorMessage = "การแก้ไขผิดพลาด";
            return View(obj);
        }

        public IActionResult Delete(string id)
        {
            if (HttpContext.Session.GetString("StfId") == null)
            {

                TempData["ErrorMessage"] = "Kun mai me sit na naka";
                return RedirectToAction("Login", "Home");
            }


            ViewData["currentNav"] = "ManageSuppliers";

            if (id == null)
            {
                TempData["ErrorMessage"] = "ระบุ ID";
                return RedirectToAction("Index");
            }
            var obj = _db.Suppliers.Find(id);

            if (obj == null)
            {
                TempData["ErrorMessage"] = "ไม่พบข้อมูล";
                return RedirectToAction("Index");
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(string SupId)
        {
            if (HttpContext.Session.GetString("StfId") == null)
            {

                TempData["ErrorMessage"] = "Kun mai me sit na naka";
                return RedirectToAction("Login", "Home");
            }

            try
            {
                var obj = _db.Suppliers.Find(SupId);
                if (ModelState.IsValid)
                {
                    _db.Suppliers.Remove(obj);
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


