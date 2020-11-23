using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Advanced.Models;
using Microsoft.AspNetCore.Identity;

namespace API_Advanced.Data
{
    public class AppIdentityDbContextSeed
    {
        /**
        Seed Data
        **/
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {

            if (!userManager.Users.Any())
            {
                //Create Roles
                var roles = new List<string>{
                    Role.Admin, Role.Editor, Role.User
                };
                foreach (var role in roles)
                {
                    IdentityRole currentRole = new IdentityRole
                    {
                        Name = role
                    };
                    await roleManager.CreateAsync(currentRole);
                }

                //Add Editors
                var editors = new List<string>{
                    "William", "Steve", "Tom"
                };
                foreach (var editor in editors)
                {
                    var currentUser = new AppUser
                    {
                        DisplayName = editor,
                        Email = editor + "@email.com",
                        UserName = editor + "123",
                    };
                    await userManager.CreateAsync(currentUser, "Pa$$w0rd123@");
                    await userManager.AddToRoleAsync(currentUser, Role.Editor);
                }

                //Add Users
                var users = new List<string>{
                    "Sam", "Tim"
                };
                foreach (var user in users)
                {
                    var currentUser = new AppUser
                    {
                        DisplayName = user,
                        Email = user + "@email.com",
                        UserName = user + "123",
                    };
                    await userManager.CreateAsync(currentUser, "Pa$$w0rd123@");
                    await userManager.AddToRoleAsync(currentUser, Role.User);
                }

                //Admin
                var admin = new AppUser
                {
                    DisplayName = "eric",
                    Email = "eric" + "@email.com",
                    UserName = "eric" + "123",
                };
                await userManager.CreateAsync(admin, "Pa$$w0rd123@");
                await userManager.AddToRoleAsync(admin, Role.Admin);
            }
        }

    }
}