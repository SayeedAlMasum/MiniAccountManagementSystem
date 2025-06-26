//VoucherWithEntires.cs
namespace MiniAccountManagementSystem.Models
{
    public class VoucherWithEntries
    {
        public int Id { get; set; }
        public string ReferenceNo { get; set; } = string.Empty;
        public string VoucherType { get; set; } = string.Empty;
        public DateTime Date { get; set; }

        public List<VoucherEntryDisplay> Entries { get; set; } = new();
    }
}
