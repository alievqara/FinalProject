using FinalProject.DAL;
using FinalProject.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDBC _appDBC;
        private readonly UserManager<User> _userManager;

        public ProductController(AppDBC appDBC, UserManager<User> userManager)
        {
            _appDBC = appDBC;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);

            var product = await _appDBC.Products.Where(p=>!p.IsDeleted).OrderByDescending(x=>x.Id).ToListAsync();

            return View(product);
        }

        public async Task<IActionResult> ListSale()
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);

            var product = await _appDBC.Products.Where(p => !p.IsDeleted && !p.Stock_Sale).OrderByDescending(x => x.Id).ToListAsync();

            return View(product);
        }

        public async Task<IActionResult> ListStock()
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);

            var product = await _appDBC.Products.Where(p => !p.IsDeleted && p.Stock_Sale).OrderByDescending(x => x.Id).ToListAsync();

            return View(product);
        }

        public async Task<IActionResult> ListDeffect()
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);

            var product = await _appDBC.Products.Where(p => !p.IsDeleted && p.Stock_Sale && p.Yararsiz).OrderByDescending(x => x.Id).ToListAsync();

            return View(product);
        }


        public async Task<IActionResult> Detail(int id)
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);

            if (id == null)
            {
                ModelState.AddModelError("", "ID Null");
                return View();
            }

            var product = await _appDBC.Products.FirstOrDefaultAsync(p => p.IsDeleted && p.Id == id);

            if (product == null)
            {
                return NotFound();
            }


            return View(product);
        }

        #region Delete
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);

            if (id == null)
            {
                return NotFound();
            }

            Product product = await _appDBC.Products.FirstOrDefaultAsync(p => !p.IsDeleted && p.Id == id);

            if (product == null)
            {
                return BadRequest();
            }





            product.IsDeleted = true;
            product.DeletedTime = DateTime.UtcNow.AddHours(4);
            product.DeletorName = User.Identity.Name.ToString();


            await _appDBC.SaveChangesAsync();

            return RedirectToAction("index");
        }

        #endregion
    }
}
