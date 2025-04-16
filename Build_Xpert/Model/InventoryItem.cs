using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Build_Xpert.Model
{
    public class InventoryItem
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Category { get; set; }

        public int Stock { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int? ProjectId { get; set; }
        public Project Project { get; set; }

        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }
    }
}
