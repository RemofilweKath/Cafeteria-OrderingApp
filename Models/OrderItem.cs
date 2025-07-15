using System.ComponentModel.DataAnnotations.Schema;

namespace CafeteriaOrderingApp.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal UnitPriceAtTimeOfOrder { get; set; }

        public virtual Order Order { get; set; }
        public virtual MenuItem MenuItem { get; set; }
    }
}
