using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Build_Xpert.Model
{
    public class SupplierPayment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int SupplierId { get; set; }

        [ForeignKey("SupplierId")]
        public Supplier Supplier { get; set; }

        [Required(ErrorMessage = "El monto del pago es obligatorio.")]
        [Range(1, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "La fecha del pago es obligatoria.")]
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
