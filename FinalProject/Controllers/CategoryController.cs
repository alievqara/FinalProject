using FinalProject.DAL;
using FinalProject.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    [Authorize(Roles ="SuperUser,Anbardar")]
    public class CategoryController : Controller
    {

        private readonly AppDBC _appDBC;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<User> _userManager;

        public CategoryController(AppDBC appDBC, IWebHostEnvironment env, UserManager<User> userManager)
        {
            _appDBC = appDBC;
            _env = env;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);
            IEnumerable<Category> category = await _appDBC.Categories.Where(c => !c.IsDeleted).ToListAsync();

            return View(category);
        }


        #region Create

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.CategoryList = await _appDBC.Categories.Where(c=>!c.IsDeleted && c.isMain).ToListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.CategoryList = await _appDBC.Categories.Where(c => !c.IsDeleted && c.isMain).ToListAsync();


            if (!ModelState.IsValid)
            {
                return View(category);
            }

            if (category == null)
            {
                ModelState.AddModelError("Name", "Category Adi Qeyd Etmeyiniz Mecburidir.");
                return View();
            }

            if (await _appDBC.Categories.AnyAsync(c => !c.IsDeleted && c.Name.ToLower() == category.Name.ToLower().Trim()))
            {
                ModelState.AddModelError("Name", "Already exist.");
                return View();
            }

            if (await _appDBC.Categories.AnyAsync(c => !c.IsDeleted && c.Id == category.Id))
            {
                ModelState.AddModelError("Name", "Already exist.");
                return View();
            }




            category.Name.Trim();
            category.IsDeleted = false;
            category.CreatedTime = DateTime.UtcNow.AddHours(+4);
            category.CreatorName = User.Identity.Name.ToString();

            await _appDBC.Categories.AddAsync(category);
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

            Category category = await _appDBC.Categories
                .Include(c => c.phoneList)
                .Include(c => c.Computers)
                .Include(c => c.OtherProducts)
                .FirstOrDefaultAsync(c => !c.IsDeleted && c.Id == id);

            if (category == null)
            {
                return BadRequest();
            }

            if (category != null)
            {

                if (category.phoneList.Count > 0 || category.Computers.Count > 0 || category.OtherProducts.Count > 0)
                {

                    return RedirectToAction("Index");
                }



            }





            category.IsDeleted = true;
            category.DeletedTime = DateTime.UtcNow.AddHours(4);
            category.DeletorName = User.Identity.Name.ToString();


            await _appDBC.SaveChangesAsync();

            return RedirectToAction("index");
        }

        #endregion

        #region Update

        [HttpGet]
        public async Task<IActionResult> Update(Category category, int? id)
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.CategoryList = await _appDBC.Categories.Where(c => !c.IsDeleted && c.isMain).ToListAsync();



            if (id == null)
            {
                return View();
            }
            return View(await _appDBC.Categories.FirstOrDefaultAsync(c => c.IsDeleted == false && c.Id == id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Category category)
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.CategoryList = await _appDBC.Categories.Where(c => !c.IsDeleted && c.isMain).ToListAsync();


            if (!ModelState.IsValid)
            {
                return View(category);
            }

            Category dbCategory = await _appDBC.Categories.FirstOrDefaultAsync(c => !c.IsDeleted && c.Id == id);

            if (dbCategory == null) return NotFound();

            if (category == null)
            {
                ModelState.AddModelError("Name", "Category Adi Qeyd Etmeyiniz Mecburidir.");
                return View();
            }

            if (await _appDBC.Categories.AnyAsync(c => c.Name.ToLower() == category.Name.ToLower().Trim()))
            {

                if (!(dbCategory.Name.ToLower() == category.Name.ToLower().Trim()))
                {
                    ModelState.AddModelError("Name", "Already Exist.");
                    return View();

                }
            }


            dbCategory.Name = category.Name.Trim();
            dbCategory.EditedTime = DateTime.UtcNow.AddHours(+4);
            dbCategory.EditorName = User.Identity.Name.ToString();
            dbCategory.isMain = category.isMain;
            dbCategory.CategoryId= category.Id;

            await _appDBC.SaveChangesAsync();

            return RedirectToAction("index");
        }

        #endregion






        #region Product in Category


        public async Task<IActionResult> PhoneList(int? id)
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);
            IEnumerable<Phone> phoneList = await _appDBC.Phones.Where(p => !p.IsDeleted && p.CategoryID == id).ToListAsync();
            return View(phoneList);
        }

        public async Task<IActionResult> CompList(int? id)
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);
            IEnumerable<Computer> computers = await _appDBC.Computers.Where(c => !c.IsDeleted && c.CategoryID == id).ToListAsync();
            return View(computers);
        }

        public async Task<IActionResult> OtherList(int? id)
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);
            IEnumerable<OtherProduct> otherProducts = await _appDBC.OtherProducts.Where(o => !o.IsDeleted && o.CategoryID == id).ToListAsync();
            return View(otherProducts);
        }

        #endregion



    }
}
