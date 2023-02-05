using Constans;
using FinalProject.Entities;
using Microsoft.AspNetCore.Identity;

namespace Helpers
{
    public static class DbInitializer
    {
        public async static Task SeedAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {

            foreach (var role in Enum.GetValues(typeof(UserRoles)))
            {
                if (!await roleManager.RoleExistsAsync(role.ToString()))
                {
                    await roleManager.CreateAsync(new IdentityRole
                    {
                        Name = role.ToString(),
                    });

                }

            }

            if ((await userManager.FindByNameAsync("SuperUser")) == null)
            {

                var SuperUser = new User
                {
                    Surname = "Aliev",
                    Name = "Qara",
                    FatherName = "Kamandar",
                    Wage = 13000,
                    UserName = "alievqara",
                    Email = "alievqara@erp.az",
                    CreatorID = "13"

                };

                await userManager.CreateAsync(SuperUser, "User*1313");
                await userManager.AddToRoleAsync(SuperUser, UserRoles.SuperUser.ToString());
            }



        }
    }
}
