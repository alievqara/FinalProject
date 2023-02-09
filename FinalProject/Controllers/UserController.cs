using EduHome.Extention;
using FinalProject.DAL;
using FinalProject.Entities;
using FinalProject.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using ViewModels.Account;

namespace FinalProject.Controllers
{
    [Authorize]
    [Authorize(Roles ="SuperUser")]
    public class UserController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _env;
        private readonly AppDBC _appDBC;

        public UserController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, IWebHostEnvironment env, AppDBC appDBC)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _env = env;
            _appDBC = appDBC;
        }




        #region CreateUser



        [Authorize(Roles = "SuperUser,Hr,Direktor")]
        [HttpGet]
        public async Task<IActionResult> CreateNewUser()
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.User = user;
            //ViewBag.Role = await _roleManager.Roles.ToListAsync();
            return View();
        }

        [Authorize(Roles = "SuperUser,Hr")]
        [HttpPost]
        public async Task<IActionResult> CreateNewUser(AccountCreateVM model)
        {

            if (!ModelState.IsValid) return View(model);

            var user = new User
            {
                Name = model.Name,
                Surname = model.Surname,
                FatherName = model.FatherName,
                Wage = model.Wage,
                Email = model.Email,
                UserName = model.Username,
                Image = model.ImageFile.CreateImage(_env, "assets", "images", "faces"),
                CreatorID = User.Identity.Name.ToString(),


            };


            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            var role = await _userManager.AddToRoleAsync(user, model.RoleName.Trim().ToUpper());
            if (!role.Succeeded)
            {
                ModelState.AddModelError(string.Empty, errorMessage: "Vezife Duzgun Secilmemisdir");
            }

            return RedirectToAction("index", "dashboard");

        }

        #endregion


        #region UserList

        [HttpGet]
        [Authorize(Roles = "SuperUser,Hr,Direktor")]
        public async Task<IActionResult> UserList()
        {
            var Users = await _userManager.Users
                .ToListAsync();


            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);
            return View(Users);
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.User = user;

            return View();
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePass pass,string id)
        {

            ViewBag.User = await _userManager.FindByNameAsync(User.Identity.Name);

            var updatedUser = await _userManager.FindByNameAsync(id);

            if (!ModelState.IsValid) return View();

            pass.Password.GetHashCode();

            updatedUser.PasswordHash = pass.Password;

            var result = await _userManager.UpdateAsync(updatedUser);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(pass);
        }



        #endregion

    }
}
