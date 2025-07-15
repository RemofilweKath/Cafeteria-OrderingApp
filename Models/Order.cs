using System.ComponentModel.DataAnnotations.Schema;

namespace CafeteriaOrderingApp.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public DateTime OrderDate { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }

        public OrderStatus Status { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; } = [];

        public enum OrderStatus
        {
            Pending,
            Preparing,
            Delivering,
            Delivered,
            Cancelled
        }
    }
}
