//Index.cshtml.cs
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniAccountManagementSystem.Data;
using MiniAccountManagementSystem.Models;
using System.Collections.Generic;

namespace MiniAccountManagementSystem.Pages.Accounts
{
    public class IndexModel : PageModel
    {
        private readonly ChartOfAccountRepository _repo;

        public IndexModel(ChartOfAccountRepository repo)
        {
            _repo = repo;
        }

        public List<ChartOfAccount> Accounts { get; set; }

        public void OnGet()
        {
            Accounts = _repo.GetAccounts();
        }
    }
}
