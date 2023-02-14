using FinalProject.DAL;
using FinalProject.Entities;
using FinalProject.ViewModels.Search;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    public class SaleBasketController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDBC _appDBC;

        public SaleBasketController(UserManager<User> userManager, AppDBC appDBC)
        {
            _userManager = userManager;
            _appDBC = appDBC;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);
            var SaleUSer = User.Identity.Name.ToString();

            var BasketList = await _appDBC.Products.Where(p=>!p.IsDeleted && p.AddSaleBasket && p.UserSale == SaleUSer).ToListAsync();

            return View(BasketList);
        }

        [HttpGet]
        public async Task<IActionResult> AddProductSale()
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);

            return View();
        }

        #region SearchProduct
        [HttpPost]
        public async Task<IActionResult> AddProductSale(SearchAdd_VM Key)
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);

            var value = Key.Value;

            if (value == null)
            {
                return RedirectToAction("Index");
            }

            if (value != null)
            {

                var phone = await _appDBC.Phones.FirstOrDefaultAsync(p => p.Imei.ToString() == value.ToUpper());

                if (phone != null)
                {
                    phone.AddSaleBasket = true;
                    phone.UserSale = User.Identity.Name.ToString();

                }

                var comp = await _appDBC.Computers.FirstOrDefaultAsync(c => c.SN_Seria.ToUpper() == value.ToUpper());

                if (comp != null)
                {
                    comp.AddSaleBasket = true;
                    comp.UserSale = User.Identity.Name.ToString();
                }

                var otherProduct = await _appDBC.OtherProducts.FirstOrDefaultAsync(p => p.Barcode.ToString().ToUpper() == value.ToUpper());

                if (otherProduct != null)
                {
                    otherProduct.AddSaleBasket = true;
                    otherProduct.UserSale = User.Identity.Name.ToString();
                }


                await _appDBC.SaveChangesAsync();
            }
            return RedirectToAction("Index");


        }




        #endregion


        public  async Task<IActionResult> Delete(int id)
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);

            if (id == null)
            {
                return NotFound();
            }

            Product product = await _appDBC.Products.FirstOrDefaultAsync(c => !c.IsDeleted && c.Id == id && c.AddSaleBasket);

            if (product == null)
            {
                return BadRequest();
            }





            product.AddSaleBasket = false;
            await _appDBC.SaveChangesAsync();

            return RedirectToAction("index");
        }

        public async Task<IActionResult> Cancel()
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);
            var user = User.Identity.Name.ToString();

            List<Product> products = await _appDBC.Products.Where(c => !c.IsDeleted && c.UserSale == user && c.AddSaleBasket).ToListAsync();

            if (products == null)
            {
                return NotFound();
            }


            foreach (var item in products)
            {

                item.AddSaleBasket = false;
            await _appDBC.SaveChangesAsync();
            }






            return RedirectToAction("index","dashboard");
        }


        public async Task<IActionResult> Clear()
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);
            var user = User.Identity.Name.ToString();

            List<Product> products = await _appDBC.Products.Where(c => !c.IsDeleted && c.UserSale == user && c.AddSaleBasket).ToListAsync();

            if (products == null)
            {
                return NotFound();
            }


            foreach (var item in products)
            {

                item.AddSaleBasket = false;
                await _appDBC.SaveChangesAsync();
            }






            return RedirectToAction("index");
        }

    }
}
