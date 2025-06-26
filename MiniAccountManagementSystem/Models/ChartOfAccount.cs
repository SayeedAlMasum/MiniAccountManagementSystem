//ChartOfAccount.cs
using System.ComponentModel.DataAnnotations;

namespace MiniAccountManagementSystem.Models
{
    public class ChartOfAccount
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Account name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        public int? ParentId { get; set; }

        // This is for display purposes in views like the Index table
        public string? ParentName { get; set; }
    }
}
