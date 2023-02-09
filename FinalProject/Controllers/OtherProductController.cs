using FinalProject.DAL;
using FinalProject.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    public class OtherProductController : Controller
    {
        private readonly AppDBC _appDBC;
        private readonly UserManager<User> _userManager;

        public OtherProductController(AppDBC appDBC, UserManager<User> userManager)
        {
            _appDBC = appDBC;
            _userManager = userManager;
        }


        #region List
        public async Task<IActionResult> Index()
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);
            IEnumerable<OtherProduct> products = await _appDBC.OtherProducts.Where(p => !p.IsDeleted && p.Stock_Sale).OrderByDescending(x => x.Id).ToListAsync();
            return View(products);
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
        public async Task<IActionResult> Create(OtherProduct product)
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.CategoryList = await _appDBC.Categories.Where(c => !c.IsDeleted).ToListAsync();



            if (!ModelState.IsValid)
            {
                return View(product);
            }

            if (product == null)
            {
                return View(product);
            }

            if (await _appDBC.OtherProducts.AnyAsync(p => !p.IsDeleted && p.Barcode == product.Barcode))
            {
                ModelState.AddModelError("Name", "Already Exist.");
                return View(product);
            }

            if (await _appDBC.OtherProducts.AnyAsync(p => !p.IsDeleted && p.Id == product.Id))
            {
                return BadRequest();
            }







            product.Brand_Name.Trim();
            product.Model_Name.Trim();
            product.Detail.Trim();
            product.IsDeleted = false;
            product.CreatedTime = DateTime.UtcNow.AddHours(+4);
            product.CreatorName = User.Identity.Name.ToString();
            product.Stock_Sale = true;
            product.Yararsiz = false;


            await _appDBC.OtherProducts.AddAsync(product);
            await _appDBC.SaveChangesAsync();

            return RedirectToAction("index");
        }

        #endregion

        #region Update

        public async Task<IActionResult> Update(int? id)
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.CategoryList = await _appDBC.Categories.Where(c => !c.IsDeleted).ToListAsync();



            if (id == null)
            {
                return View();
            }
            return View(await _appDBC.OtherProducts.FirstOrDefaultAsync(p => p.IsDeleted == false && p.Id == id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, OtherProduct product)
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.CategoryList = await _appDBC.Categories.Where(c => !c.IsDeleted).ToListAsync();


            if (!ModelState.IsValid)
            {
                return View(product);
            }

            OtherProduct dbProduct = await _appDBC.OtherProducts.FirstOrDefaultAsync(c => !c.IsDeleted && c.Id == id);

            if (dbProduct == null) return NotFound();

            if (product == null)
            {
                ModelState.AddModelError("Name", "Category Adi Qeyd Etmeyiniz Mecburidir.");
                return View();
            }

            if (await _appDBC.OtherProducts.AnyAsync(c => product.Barcode == product.Barcode))
            {

                if (!(dbProduct.Barcode == product.Barcode))
                {
                    ModelState.AddModelError("Name", "Already Exist.");
                    return View();

                }
            }


            dbProduct.Brand_Name = product.Brand_Name.Trim();
            dbProduct.Model_Name = product.Model_Name.Trim();
            dbProduct.EditedTime = DateTime.UtcNow.AddHours(+4);
            dbProduct.EditorName = User.Identity.Name.ToString();
            dbProduct.Detail = product.Detail;
            dbProduct.Yararsiz = product.Yararsiz;
            dbProduct.Stock_Sale = product.Stock_Sale;
            dbProduct.Barcode = product.Barcode;
            dbProduct.Price = product.Price;
            dbProduct.CategoryID = product.CategoryID;

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

            OtherProduct product = await _appDBC.OtherProducts.FirstOrDefaultAsync(c => !c.IsDeleted && c.Id == id);

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
