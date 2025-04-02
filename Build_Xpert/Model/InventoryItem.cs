using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Build_Xpert.Model
{
    public class InventoryItem
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Categoria { get; set; }

        public int Stock { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }

        public int? ProjectId { get; set; }
        public Project Project { get; set; }

        public int ProveedorId { get; set; }
        public Proveedor Proveedor { get; set; }
    }
}
