//Users.cshtml.cs
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MiniAccountManagementSystem.Pages.Admin
{
    public class UsersModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UsersModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public List<UserWithRoles> Users { get; set; }

        public async Task OnGetAsync()
        {
            Users = new List<UserWithRoles>();
            foreach (var user in _userManager.Users.ToList())
            {
                var roles = await _userManager.GetRolesAsync(user);
                Users.Add(new UserWithRoles
                {
                    User = user,
                    Roles = string.Join(", ", roles)
                });
            }
        }

        public class UserWithRoles
        {
            public IdentityUser User { get; set; }
            public string Roles { get; set; }
        }
    }
}
