using System.ComponentModel.DataAnnotations;

namespace CafeteriaOrderingApp.Models
{
    public class Restaurant
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string LocationDescription { get; set; }
        public string ContactNumber { get; set; }

        public virtual ICollection<MenuItem> MenuItems { get; set; } = [];
    }
}
