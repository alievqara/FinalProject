using FinalProject.DAL;
using FinalProject.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

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
        [Authorize(Roles = "SuperUser,Satici,Direktor,Menecer")]

        public async Task<IActionResult> Index()
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);

            var product = await _appDBC.Products.Where(p => !p.IsDeleted).OrderByDescending(x => x.Id).ToListAsync();

            return View(product);
        }
        [Authorize(Roles = "SuperUser,Anbardar")]

        public async Task<IActionResult> ListSale()
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);

            var product = await _appDBC.Products.Where(p => !p.IsDeleted && !p.Stock_Sale).OrderByDescending(x => x.Id).ToListAsync();

            return View(product);
        }
        [Authorize(Roles = "SuperUser,Satici,Direktor,Menecer,Anbardar")]

        public async Task<IActionResult> ListStock()
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);

            var product = await _appDBC.Products.Where(p => !p.IsDeleted && p.Stock_Sale).OrderByDescending(x => x.Id).ToListAsync();

            return View(product);
        }
        [Authorize(Roles = "SuperUser,Direktor,Anbardar")]

        public async Task<IActionResult> ListDeffect()
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);

            var product = await _appDBC.Products.Where(p => !p.IsDeleted && p.Stock_Sale && p.Yararsiz).OrderByDescending(x => x.Id).ToListAsync();

            return View(product);
        }



        [Authorize(Roles = "SuperUser,Satici,Direktor,Menecer,Anbardar")]
        public async Task<IActionResult> Detail(int id)
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);

            if (id == null)
            {
                ModelState.AddModelError("", "ID Null");
                return View();
            }



            if ((await _appDBC.Phones.FirstOrDefaultAsync(p => p.Id == id)) != null)
            {
                return RedirectToAction("DetailPhone", "Product", new { id = id });
            }

            if ((await _appDBC.Computers.FirstOrDefaultAsync(p => p.Id == id)) != null)
            {
                return RedirectToAction("DetailComp", "Product", new { id = id });
            }

            if ((await _appDBC.OtherProducts.FirstOrDefaultAsync(p => p.Id == id)) != null)
            {
                return RedirectToAction("DetailOther", "Product", new { id = id });
            }


            return View();
        }


        public async Task<IActionResult> DetailPhone(int id)
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);

            var phone13 = await _appDBC.Phones.FirstOrDefaultAsync(p => p.Id == id);
            return View(phone13);
        }

        public async Task<IActionResult> DetailComp(int id)
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);

            var computer = await _appDBC.Computers.FirstOrDefaultAsync(p => p.Id == id);
            return View(computer);
        }

        public async Task<IActionResult> DetailOther(int id)
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);

            var other = await _appDBC.OtherProducts.FirstOrDefaultAsync(p => p.Id == id);
            return View(other);
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


      

        #region Sale 



        #endregion
    }
}
