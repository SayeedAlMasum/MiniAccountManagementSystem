//Vouchers/ExportToExcel.cshtml.cs
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniAccountManagementSystem.Models;
using System.Data;

namespace MiniAccountManagementSystem.Pages.Vouchers
{
    [Authorize(Roles = "Admin,Accountant")]
    public class ExportToExcelModel : PageModel
    {
        private readonly VoucherService _service;

        public ExportToExcelModel(VoucherService service)
        {
            _service = service;
        }

        public IActionResult OnGet()
        {
            var vouchers = _service.GetAllVouchers();

            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Vouchers");

            int row = 1;
            ws.Cell(row, 1).Value = "Ref No";
            ws.Cell(row, 2).Value = "Type";
            ws.Cell(row, 3).Value = "Date";
            ws.Cell(row, 4).Value = "Account";
            ws.Cell(row, 5).Value = "Debit";
            ws.Cell(row, 6).Value = "Credit";

            foreach (var v in vouchers)
            {
                foreach (var e in v.Entries)
                {
                    row++;
                    ws.Cell(row, 1).Value = v.ReferenceNo;
                    ws.Cell(row, 2).Value = v.VoucherType;
                    ws.Cell(row, 3).Value = v.Date.ToShortDateString();
                    ws.Cell(row, 4).Value = e.AccountName;
                    ws.Cell(row, 5).Value = e.Debit;
                    ws.Cell(row, 6).Value = e.Credit;
                }
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            return File(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Vouchers.xlsx");
        }
    }
}
