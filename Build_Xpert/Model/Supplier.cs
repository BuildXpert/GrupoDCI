using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Build_Xpert.Model
{
    public class Supplier
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del proveedor es obligatorio.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El tipo de servicio o maquinaria es obligatorio.")]
        public string ServiceType { get; set; } 

        [Required(ErrorMessage = "El contacto es obligatorio.")]
        public string Contact { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        public string Status { get; set; } = "Activo";

        public ICollection<SupplierPayment> SupplierPayment { get; set; } = new List<SupplierPayment>();
        public ICollection<SupplierOrder> SupplierOrder { get; set; } = new List<SupplierOrder>();
    }
}