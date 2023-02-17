using FinalProject.DAL;
using FinalProject.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FinalProject.Controllers
{
    [Authorize(Roles = "SuperUser,Anbardar")]

    public class PhoneController : Controller
    {
        private readonly AppDBC _appDBC;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<User> _userManager;

        public PhoneController(AppDBC appDBC, IWebHostEnvironment env,UserManager<User> userManager)
        {
            _appDBC = appDBC;
            _env = env;
            _userManager = userManager;
        }
        #region List
        public async Task<IActionResult> Index()
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);
            IEnumerable<Phone> phones = await _appDBC.Phones.Where(p => !p.IsDeleted && p.Stock_Sale).OrderByDescending(x => x.Id).ToListAsync();

            return View(phones);
        }

        #endregion

        #region Create

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.CategoryList = await _appDBC.Categories.Where(c => !c.IsDeleted).ToListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Phone phone)
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.CategoryList = await _appDBC.Categories.Where(c => !c.IsDeleted).ToListAsync();



            if (!ModelState.IsValid)
            {
                return View(phone);
            }

            if (phone == null)
            {
                return View(phone);
            }

            if (await _appDBC.Phones.AnyAsync(p => !p.IsDeleted && p.Imei == phone.Imei))
            {
                ModelState.AddModelError("Name", "Already Exist.");
                return View(phone);
            }

            if (await _appDBC.Phones.AnyAsync(p => !p.IsDeleted && p.Id == phone.Id))
            {
                return BadRequest();
            }







            phone.Brand_Name.Trim();
            phone.Model_Name.Trim();
            phone.Detail.Trim();
            phone.IsDeleted = false;
            phone.CreatedTime = DateTime.UtcNow.AddHours(+4);
            phone.CreatorName = User.Identity.Name.ToString();
            phone.Stock_Sale = true;
            phone.Yararsiz = false;


            await _appDBC.Phones.AddAsync(phone);
            await _appDBC.SaveChangesAsync();

            return RedirectToAction("index");
        }

        #endregion

        #region Update

        public async Task<IActionResult> Update(Phone phone, int? id)
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.CategoryList = await _appDBC.Categories.Where(c => !c.IsDeleted).ToListAsync();



            if (id == null)
            {
                return View();
            }
            return View(await _appDBC.Phones.FirstOrDefaultAsync(p => p.IsDeleted == false && p.Id == id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Phone phone)
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.CategoryList = await _appDBC.Categories.Where(c => !c.IsDeleted).ToListAsync();


            if (!ModelState.IsValid)
            {
                return View(phone);
            }

            Phone dbPhone = await _appDBC.Phones.FirstOrDefaultAsync(p => !p.IsDeleted && p.Id == id);

            if (dbPhone == null) return NotFound();

            if (phone == null)
            {
                ModelState.AddModelError("Name", "Category Adi Qeyd Etmeyiniz Mecburidir.");
                return View();
            }

            if (await _appDBC.Phones.AnyAsync(c => c.Imei == phone.Imei))
            {

                if (!(dbPhone.Imei == phone.Imei))
                {
                    ModelState.AddModelError("Name", "Already Exist.");
                    return View();

                }
            }


            dbPhone.Brand_Name = phone.Brand_Name.Trim();
            dbPhone.Model_Name = phone.Model_Name.Trim();
            dbPhone.EditedTime = DateTime.UtcNow.AddHours(+4);
            dbPhone.EditorName = User.Identity.Name.ToString();
            dbPhone.Ram = phone.Ram;
            dbPhone.Color = phone.Color;
            dbPhone.Detail = phone.Detail;
            dbPhone.Storage = phone.Storage;
            dbPhone.Yararsiz = phone.Yararsiz;
            dbPhone.Stock_Sale = phone.Stock_Sale;
            dbPhone.Imei = phone.Imei;
            dbPhone.Price = phone.Price;
            dbPhone.CategoryID = phone.CategoryID;

            await _appDBC.SaveChangesAsync();

            return RedirectToAction("index");
        }


        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);

            if (id == null)
            {
                return NotFound();
            }

            Phone phone = await _appDBC.Phones.FirstOrDefaultAsync(p => !p.IsDeleted && p.Id == id);

            if (phone == null)
            {
                return BadRequest();
            }





            phone.IsDeleted = true;
            phone.DeletedTime = DateTime.UtcNow.AddHours(4);
            phone.DeletorName = User.Identity.Name.ToString();


            await _appDBC.SaveChangesAsync();

            return RedirectToAction("index");
        }

        #endregion

    }
}
