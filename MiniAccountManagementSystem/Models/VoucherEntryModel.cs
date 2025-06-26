// Models/VoucherEntryModel.cs
namespace MiniAccountManagementSystem.Models
{
    public class VoucherEntryModel
    {
        public int AccountId { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
    }
}
