using FinalProject.DAL;
using FinalProject.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    public class ComputerController : Controller
    {
        private readonly AppDBC _appDBC;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<User> _userManager;

        public ComputerController(AppDBC appDBC, IWebHostEnvironment env, UserManager<User> userManager)
        {
            _appDBC = appDBC;
            _env = env;
            _userManager = userManager;
        }

        #region List
        public async Task<IActionResult> Index()
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);
            IEnumerable<Computer> computers = await _appDBC.Computers.Where(p => !p.IsDeleted && p.Stock_Sale).OrderByDescending(x => x.Id).ToListAsync();

            return View(computers);
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
        public async Task<IActionResult> Create(Computer computer)
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.CategoryList = await _appDBC.Categories.Where(c => !c.IsDeleted).ToListAsync();



            if (!ModelState.IsValid)
            {
                return View(computer);
            }

            if (computer == null)
            {
                return View(computer);
            }

            if (await _appDBC.Computers.AnyAsync(p => !p.IsDeleted && p.SN_Seria == computer.SN_Seria))
            {
                ModelState.AddModelError("Name", "Already Exist.");
                return View(computer);
            }

            if (await _appDBC.Computers.AnyAsync(p => !p.IsDeleted && p.Id == computer.Id))
            {
                return BadRequest();
            }







            computer.Brand_Name.Trim();
            computer.Model_Name.Trim();
            computer.Detail.Trim();
            computer.GPU.Trim();
            computer.CPU.Trim();
            computer.IsDeleted = false;
            computer.CreatedTime = DateTime.UtcNow.AddHours(+4);
            computer.CreatorName = User.Identity.Name.ToString();
            computer.Stock_Sale = true;
            computer.Yararsiz = false;


            await _appDBC.Computers.AddAsync(computer);
            await _appDBC.SaveChangesAsync();

            return RedirectToAction("index");
        }

        #endregion

        #region Update

        public async Task<IActionResult> Update(Computer computer, int? id)
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.CategoryList = await _appDBC.Categories.Where(c => !c.IsDeleted).ToListAsync();



            if (id == null)
            {
                return View();
            }
            return View(await _appDBC.Computers.FirstOrDefaultAsync(p => p.IsDeleted == false && p.Id == id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Computer comp)
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.CategoryList = await _appDBC.Categories.Where(c => !c.IsDeleted).ToListAsync();


            if (!ModelState.IsValid)
            {
                return View(comp);
            }

            Computer dbComp = await _appDBC.Computers.FirstOrDefaultAsync(c => !c.IsDeleted && c.Id == id);

            if (dbComp == null) return NotFound();

            if (comp == null)
            {
                ModelState.AddModelError("Name", "Category Adi Qeyd Etmeyiniz Mecburidir.");
                return View();
            }

            if (await _appDBC.Computers.AnyAsync(c => c.SN_Seria == comp.SN_Seria))
            {

                if (!(dbComp.SN_Seria == comp.SN_Seria))
                {
                    ModelState.AddModelError("Name", "Already Exist.");
                    return View();

                }
            }


            dbComp.Brand_Name = comp.Brand_Name.Trim();
            dbComp.Model_Name = comp.Model_Name.Trim();
            dbComp.EditedTime = DateTime.UtcNow.AddHours(+4);
            dbComp.EditorName = User.Identity.Name.ToString();
            dbComp.Ram = comp.Ram;
            dbComp.CPU = comp.CPU;
            dbComp.Detail = comp.Detail;
            dbComp.Storage = comp.Storage;
            dbComp.Yararsiz = comp.Yararsiz;
            dbComp.Stock_Sale = comp.Stock_Sale;
            dbComp.SN_Seria = comp.SN_Seria;
            dbComp.Price = comp.Price;
            dbComp.CategoryID = comp.CategoryID;
            dbComp.GPU= comp.GPU;

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

            Computer computer = await _appDBC.Computers.FirstOrDefaultAsync(c => !c.IsDeleted && c.Id == id);

            if (computer == null)
            {
                return BadRequest();
            }





            computer.IsDeleted = true;
            computer.DeletedTime = DateTime.UtcNow.AddHours(4);
            computer.DeletorName = User.Identity.Name.ToString();


            await _appDBC.SaveChangesAsync();

            return RedirectToAction("index");
        }

        #endregion
    }
}
