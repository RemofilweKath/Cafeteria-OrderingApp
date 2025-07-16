using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CafeteriaOrderingApp.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string EmployeeNumber { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }

        private DateTime _lastDepositMonth = DateTime.Now;

        public decimal MonthlyDepositBalance { get; set; } = 0.0m;
        public DateTime LastDepositMonth
        {
            get => _lastDepositMonth;
            set => _lastDepositMonth = new DateTime(value.Year, value.Month, 1);
        }
    }
}
