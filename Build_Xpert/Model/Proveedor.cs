using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Build_Xpert.Model
{
    public class Proveedor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del proveedor es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El tipo de servicio o maquinaria es obligatorio.")]
        public string TipoServicio { get; set; } 

        [Required(ErrorMessage = "El contacto es obligatorio.")]
        public string Contacto { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        public string Estado { get; set; } = "Activo";

        public ICollection<PagoProveedor> Pagos { get; set; } = new List<PagoProveedor>();
        public ICollection<PedidoProveedor> Pedidos { get; set; } = new List<PedidoProveedor>();
    }
}