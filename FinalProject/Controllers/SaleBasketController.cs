using FinalProject.DAL;
using FinalProject.Entities;
using FinalProject.ViewModels.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using ViewModels.Dashboard;

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
        [Authorize(Roles = "SuperUser,Satici,Direktor,Menecer")]

        public async Task<IActionResult> Sale()
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);
            var SaleUSer = User.Identity.Name.ToString();

            var BasketList = await _appDBC.Products.Where(p => !p.IsDeleted && p.AddSaleBasket && p.UserSale == SaleUSer).ToListAsync();

            return View(BasketList);
        }

        [HttpGet]
        public async Task<IActionResult> CancelSaleSearch()
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CancelSaleSearch(Musteri? client)
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);

            List<Musteri> musteriler = await _appDBC.Musteriler.Where(m => !m.IsDeleted && m.Name.ToLower() == client.Name.ToLower() && m.Surname.ToLower() == client.Surname.ToLower() && m.FatherName.ToLower() == client.FatherName.ToLower()).ToListAsync();


            if (musteriler == null)
            {
                return NotFound();
            }

            ViewBag.CancelList = musteriler;

            return View();
        }


        [Authorize(Roles = "SuperUser,Satici,Direktor,Menecer")]

        public async Task<IActionResult> Allsalelist()
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.User = user;

            DateTime date = DateTime.UtcNow.AddHours(+4);

            DashboardIndexVM dashboardIndexVM = new DashboardIndexVM()
            {
                CreditList = await _appDBC.Musteriler
                .Include(m => m.Products)
                .Where(m => !m.IsDeleted && m.Kredit_Nagd && m.CreatedTime.Value.Year == date.Year && m.CreatedTime.Value.Month == date.Month && m.CreatedTime.Value.Day == date.Day).ToListAsync(),
                CashList = await _appDBC.Musteriler
                .Include(m => m.Products)
                .Where(m => !m.IsDeleted && !m.Kredit_Nagd && m.CreatedTime.Value.Year == date.Year && m.CreatedTime.Value.Month == date.Month && m.CreatedTime.Value.Day == date.Day).ToListAsync(),
                Products = await _appDBC.Products.Where(p => !p.IsDeleted).ToListAsync(),

            };


            return View(dashboardIndexVM);
        }

        [Authorize(Roles = "SuperUser,Satici,Direktor,Menecer")]

        public async Task<IActionResult> SaleListMounth()
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.User = user;

            DateTime date = DateTime.UtcNow.AddHours(+4);

            DashboardIndexVM dashboardIndexVM = new DashboardIndexVM()
            {
                CreditList = await _appDBC.Musteriler
                .Include(m => m.Products)
                .Where(m => !m.IsDeleted && m.Kredit_Nagd && m.CreatedTime.Value.Year == date.Year && m.CreatedTime.Value.Month == date.Month).ToListAsync(),
                CashList = await _appDBC.Musteriler
                .Include(m => m.Products)
                .Where(m => !m.IsDeleted && !m.Kredit_Nagd && m.CreatedTime.Value.Year == date.Year && m.CreatedTime.Value.Month == date.Month).ToListAsync(),
                Products = await _appDBC.Products.Where(p => !p.IsDeleted).ToListAsync(),

            };


            return View(dashboardIndexVM);
        }

        [Authorize(Roles = "SuperUser,Satici,Direktor,Menecer")]

        public async Task<IActionResult> MySaleListDay()
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.User = user;

            DateTime date = DateTime.UtcNow.AddHours(+4);

            DashboardIndexVM dashboardIndexVM = new DashboardIndexVM()
            {
                CreditList = await _appDBC.Musteriler
                .Include(m => m.Products)
                .Where(m => !m.IsDeleted && m.Kredit_Nagd && m.CreatedTime.Value.Year == date.Year && m.CreatedTime.Value.Month == date.Month && m.CreatedTime.Value.Day == date.Day && m.Satici == User.Identity.Name).ToListAsync(),
                CashList = await _appDBC.Musteriler
                .Include(m => m.Products)
                .Where(m => !m.IsDeleted && !m.Kredit_Nagd && m.CreatedTime.Value.Year == date.Year && m.CreatedTime.Value.Month == date.Month && m.CreatedTime.Value.Day == date.Day && m.Satici == User.Identity.Name).ToListAsync(),
                Products = await _appDBC.Products.Where(p => !p.IsDeleted).ToListAsync(),

            };


            return View(dashboardIndexVM);
        }

        [Authorize(Roles = "SuperUser,Satici,Direktor,Menecer")]

        public async Task<IActionResult> MySaleListMount()
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.User = user;

            DateTime date = DateTime.UtcNow.AddHours(+4);

            DashboardIndexVM dashboardIndexVM = new DashboardIndexVM()
            {
                CreditList = await _appDBC.Musteriler
                .Include(m => m.Products)
                .Where(m => !m.IsDeleted && m.Kredit_Nagd && m.CreatedTime.Value.Year == date.Year && m.CreatedTime.Value.Month == date.Month && m.Satici == User.Identity.Name).ToListAsync(),
                CashList = await _appDBC.Musteriler
                .Include(m => m.Products)
                .Where(m => !m.IsDeleted && !m.Kredit_Nagd && m.CreatedTime.Value.Year == date.Year && m.CreatedTime.Value.Month == date.Month  && m.Satici == User.Identity.Name).ToListAsync(),
                Products = await _appDBC.Products.Where(p => !p.IsDeleted).ToListAsync(),

            };


            return View(dashboardIndexVM);
        }


        [HttpGet]
        [Authorize(Roles = "SuperUser,Satici,Direktor,Menecer")]
        public async Task<IActionResult> AddProductSale()
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);

            return View();
        }

        #region SearchProduct
        [HttpPost]
        [Authorize(Roles = "SuperUser,Satici,Direktor,Menecer")]

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
            return RedirectToAction("sale");


        }




        #endregion


        public async Task<IActionResult> Delete(int id)
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

            return RedirectToAction("sale");
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






            return RedirectToAction("index", "dashboard");
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


            return RedirectToAction("sale");
        }


        [HttpGet]
        [Authorize(Roles = "SuperUser,Satici,Direktor,Menecer")]

        public async Task<IActionResult> CashSale()
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);

            return View();
        }


        [HttpPost]
        [Authorize(Roles = "SuperUser,Satici,Direktor,Menecer")]

        public async Task<IActionResult> CashSale(Musteri musteri)
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);
            var SaleUSer = User.Identity.Name.ToString();
            var BasketList = await _appDBC.Products.Where(p => !p.IsDeleted && p.AddSaleBasket && p.UserSale == SaleUSer).ToListAsync();

            decimal countPrice = 0;

            foreach (var item in BasketList)
            {
                item.Stock_Sale = false;
                item.EditedTime = DateTime.Now.AddHours(+4);
                countPrice += item.Price;

            }

            Musteri newMusteri = new Musteri()
            {
                Name = musteri.Name,
                Surname = musteri.Surname,
                FatherName = musteri.FatherName,
                Tel = musteri.Tel,
                Satici = User.Identity.Name.ToString(),
                Products = BasketList,
                UmumiDeyer_Borc = countPrice,
                CreatorName = User.Identity.Name.ToString(),
                CreatedTime = DateTime.UtcNow.AddHours(+4),
                IsDeleted = false,
                Kredit_Nagd = false,


            };

            await _appDBC.Musteriler.AddAsync(newMusteri);

            foreach (var item in BasketList)
            {
                item.AddSaleBasket = false;
            }
            await _appDBC.SaveChangesAsync();

            return RedirectToAction("index", "dashboard");
        }


        [HttpGet]
        [Authorize(Roles = "SuperUser,Satici,Direktor,Menecer")]

        public async Task<IActionResult> CreditSale()
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "SuperUser,Satici,Direktor,Menecer")]
        public async Task<IActionResult> CreditSale(Musteri musteri)
        {
            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);
            var SaleUSer = User.Identity.Name.ToString();
            var BasketList = await _appDBC.Products.Where(p => !p.IsDeleted && p.AddSaleBasket && p.UserSale == SaleUSer).ToListAsync();

            decimal countPrice = 0;

            foreach (var item in BasketList)
            {
                item.Stock_Sale = false;
                item.EditedTime = DateTime.Now.AddHours(+4);
                countPrice += item.Price;

            }

            Musteri newMusteri = new Musteri()
            {
                Name = musteri.Name,
                Surname = musteri.Surname,
                FatherName = musteri.FatherName,
                Tel = musteri.Tel,
                HomeTel = musteri.HomeTel,
                ZaminTel = musteri.ZaminTel,
                Is_Yeri = musteri.Is_Yeri,
                Is_Yerinin_Adi = musteri.Is_Yerinin_Adi,
                Is_Staji = musteri.Is_Staji,
                Kredit_Muddeti = musteri.Kredit_Muddeti,
                Kredit_Faizi = musteri.Kredit_Faizi,
                OdemeTarixi = musteri.OdemeTarixi,
                IlkinOdenis = musteri.IlkinOdenis,
                Satici = User.Identity.Name.ToString(),
                Products = BasketList,
                UmumiDeyer_Borc = countPrice,
                CreatorName = User.Identity.Name.ToString(),
                CreatedTime = DateTime.UtcNow.AddHours(+4),
                IsDeleted = false,
                Kredit_Nagd = true,

            };

            await _appDBC.Musteriler.AddAsync(newMusteri);

            foreach (var item in BasketList)
            {
                item.AddSaleBasket = false;
            }
            await _appDBC.SaveChangesAsync();

            return RedirectToAction("index", "dashboard");
        }

    }
}
