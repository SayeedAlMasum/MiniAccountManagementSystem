//Delete.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniAccountManagementSystem.Data;
using MiniAccountManagementSystem.Models;

public class DeleteModel : PageModel
{
    private readonly ChartOfAccountRepository _repo;

    public DeleteModel(ChartOfAccountRepository repo)
    {
        _repo = repo;
    }

    [BindProperty]
    public ChartOfAccount Account { get; set; }

    public string ParentName { get; set; }

    public void OnGet(int id)
    {
        Account = _repo.GetById(id);
        if (Account?.ParentId != null)
        {
            var parent = _repo.GetById(Account.ParentId.Value);
            ParentName = parent?.Name;
        }
    }

    public IActionResult OnPost()
    {
        if (Account != null)
        {
            _repo.Save(Account, "Delete");
        }
        return RedirectToPage("Index");
    }
}
