using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CafeteriaOrderingApp.Models
{
    public class MenuItem
    {
        public int Id { get; set; }

        [Required]
        public int RestaurantId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        public virtual Restaurant Restaurant { get; set; }
    }
}
