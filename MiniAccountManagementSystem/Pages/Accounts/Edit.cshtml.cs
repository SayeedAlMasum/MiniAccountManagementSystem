//Edit.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MiniAccountManagementSystem.Data;
using MiniAccountManagementSystem.Models;

public class EditModel : PageModel
{
    private readonly ChartOfAccountRepository _repo;

    public EditModel(ChartOfAccountRepository repo)
    {
        _repo = repo;
    }

    [BindProperty]
    public ChartOfAccount Account { get; set; }

    public List<SelectListItem> ParentOptions { get; set; }

    public IActionResult OnGet(int id)
    {
        Account = _repo.GetById(id);
        if (Account == null)
            return NotFound();

        ParentOptions = _repo.GetAccounts()
            .Where(a => a.Id != id) // prevent circular parent
            .Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Name
            }).ToList();

        return Page();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
            return Page();

        _repo.Save(Account, "Update");
        return RedirectToPage("Index");
    }
}
