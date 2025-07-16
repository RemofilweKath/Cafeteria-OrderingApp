using System.ComponentModel.DataAnnotations;

namespace CafeteriaOrderingApp.Models
{
    public class Deposit
    {
        [Required]
        public string EmployeeNumber { get; set; }

        [Required]
        [Range(0.01, Double.MaxValue, ErrorMessage = "Amount must be positive.")]
        public decimal Amount { get; set; }
    }
}
