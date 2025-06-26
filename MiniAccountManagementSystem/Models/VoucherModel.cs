//VoucherModel.cs
namespace MiniAccountManagementSystem.Models
{
    public class VoucherModel
    {
        public DateTime Date { get; set; } = DateTime.Today;
        public string ReferenceNo { get; set; } = string.Empty;
        public string VoucherType { get; set; } = "Journal";
        public List<VoucherEntryModel> Entries { get; set; } = new();
    }
}
