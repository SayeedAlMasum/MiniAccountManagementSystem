//Vouchers/Index.cshtml.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniAccountManagementSystem.Models;

namespace MiniAccountManagementSystem.Pages.Vouchers
{
    [Authorize(Roles = "Admin,Accountant,Viewer")]
    public class IndexModel : PageModel
    {
        private readonly VoucherService _service;

        public IndexModel(VoucherService service)
        {
            _service = service;
        }

        public List<VoucherWithEntries> Vouchers { get; set; } = new();

        public void OnGet()
        {
            Vouchers = _service.GetAllVouchers();
        }
    }
}
