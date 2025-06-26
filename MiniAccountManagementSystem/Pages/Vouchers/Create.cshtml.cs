//Vouchers/Create.cshtml.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MiniAccountManagementSystem.Data;
using MiniAccountManagementSystem.Models;

[Authorize(Roles = "Admin,Accountant")]
public class CreateVoucherModel : PageModel
{
    private readonly VoucherService _service;
    private readonly ChartOfAccountRepository _accountRepo;

    public CreateVoucherModel(VoucherService service, ChartOfAccountRepository accountRepo)
    {
        _service = service;
        _accountRepo = accountRepo;
    }

    [BindProperty]
    public VoucherModel Voucher { get; set; } = new();

    public List<SelectListItem> AccountOptions { get; set; }

    public void OnGet()
    {
        AccountOptions = _accountRepo.GetAccounts()
            .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name })
            .ToList();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            OnGet(); // Reload accounts
            return Page();
        }

        _service.SaveVoucher(Voucher);
        return RedirectToPage("Index");
    }
}
