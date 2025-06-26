//ManageRoles.cshtml.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MiniAccountManagementSystem.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class ManageRolesModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ManageRolesModel(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public List<IdentityUser> Users { get; set; } = new();
        public Dictionary<string, IList<string>> UserRoles { get; set; } = new();
        public List<string> AllRoles { get; set; } = new();

        public async Task OnGetAsync()
        {
            Users = _userManager.Users.ToList();
            AllRoles = _roleManager.Roles.Select(r => r.Name).ToList();

            foreach (var user in Users)
            {
                UserRoles[user.Id] = await _userManager.GetRolesAsync(user);
            }
        }

        public async Task<IActionResult> OnPostAssignRoleAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null && await _roleManager.RoleExistsAsync(role))
            {
                await _userManager.AddToRoleAsync(user, role);
            }
            return RedirectToPage();
        }
    }
}
