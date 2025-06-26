//Voucher.cs
namespace MiniAccountManagementSystem.Models
{
    public class Voucher
    {
        public DateTime Date { get; set; }

        public string ReferenceNo { get; set; } = string.Empty;

        public string VoucherType { get; set; } = string.Empty;

        public List<VoucherEntry> Entries { get; set; } = new();
    }
}
