using FinalProject.DAL;
using FinalProject.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
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

        public async Task<IActionResult> List()
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);
            IEnumerable<Phone> phones = await _appDBC.Phones.Where(p => !p.IsDeleted && p.Stock_Sale).ToListAsync();

            return View(phones);
        }

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

            return RedirectToAction("list");
        }

        #endregion
    }
}
