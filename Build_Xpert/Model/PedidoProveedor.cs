using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Build_Xpert.Model
{
    public class PedidoProveedor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Relación con Proveedor
        [Required]
        public int ProveedorId { get; set; }

        [ForeignKey("ProveedorId")]
        public Proveedor Proveedor { get; set; }

        // Relación con Proyecto 🔥
        [Required]
        public int? ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        // Datos del pedido
        [Required(ErrorMessage = "El nombre del material es obligatorio.")]
        public string Material { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "La descripción del pedido es obligatoria.")]
        public string Descripcion { get; set; }

        [Required]
        public DateTime FechaPedido { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "El monto del pedido es obligatorio.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "El estado del pedido es obligatorio.")]
        public string Estado { get; set; } = "Pendiente";


    }

}