using FinalProject.DAL;
using FinalProject.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace FinalProject.Controllers
{
    [Authorize(Roles = "SuperUser,Hr,Satici,Direktor,Menecer,Anbardar,Idareci")]

    public class DashboardController : Controller
    {
        private readonly AppDBC _appDBC;
        private readonly UserManager<User> _userManager;


        public DashboardController(AppDBC appDBC, UserManager<User> userManager)
        {
            _appDBC = appDBC;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.User = user;


            return View();
        }

    }
}
