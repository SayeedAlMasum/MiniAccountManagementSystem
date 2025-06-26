//Account/Create.cshtml.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MiniAccountManagementSystem.Data;
using MiniAccountManagementSystem.Models;

[Authorize(Roles = "Admin,Accountant")]
public class CreateModel : PageModel
{
    private readonly ChartOfAccountRepository _repo;

    public CreateModel(ChartOfAccountRepository repo)
    {
        _repo = repo;
    }

    [BindProperty]
    public ChartOfAccount Account { get; set; } = new ChartOfAccount();

    public List<SelectListItem> ParentOptions { get; set; }

    public void OnGet()
    {
        ParentOptions = _repo.GetAccounts().Select(a =>
            new SelectListItem { Value = a.Id.ToString(), Text = a.Name }).ToList();
    }
    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            // Re-populate dropdown on error
            ParentOptions = _repo.GetAccounts().Select(a =>
                new SelectListItem { Value = a.Id.ToString(), Text = a.Name }).ToList();

            return Page();
        }

        _repo.Save(Account, "Insert");
        return RedirectToPage("Index");
    }

}
