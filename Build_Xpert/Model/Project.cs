using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Build_Xpert.Model
{
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del proyecto es obligatorio.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "La descripción del proyecto es obligatoria.")]
        public string Description { get; set; }

        public string Status { get; set; } = "Pendiente"; // Puede ser: Entregado, Construccion, Venta, Remodelacion
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string ClientId { get; set; }

        // Relación con los usuarios para determinar su rol
        [ForeignKey("ClientId")]
        public ApplicationUser Client { get; set; }

        [ForeignKey("AdminId")]
        public ApplicationUser? Admin { get; set; }

        public string? AdminId { get; set; }

        // Relación con las fases del Proyecto
        public ICollection<ProjectPhase> ProjectPhase { get; set; }

        [Required(ErrorMessage = "El tipo de construcción es obligatorio.")]
        public string ConstructionType { get; set; }  // Prefabricados o Block

        [Required(ErrorMessage = "La provincia es obligatoria.")]
        public string Province { get; set; }  // Provincia

        [Required(ErrorMessage = "El cantón es obligatorio.")]
        public string Canton { get; set; }  // Cantón

        [Range(0, double.MaxValue, ErrorMessage = "El tamaño de construcción debe ser un número positivo.")]
        public double ConstructionSize { get; set; }  // Tamaño de construcción en m²

        [Range(0, double.MaxValue, ErrorMessage = "El tamaño del terreno debe ser un número positivo.")]
        public double LandSize { get; set; }  // Tamaño del terreno en m²

        [Range(0, 20, ErrorMessage = "El número de habitaciones debe ser entre 0 y 20.")]
        public int Bedrooms { get; set; }  // Número de habitaciones

        [Range(0, 20, ErrorMessage = "El número de baños debe ser entre 0 y 20.")]
        public int Bathrooms { get; set; }  // Número de baños

        [Range(0, double.MaxValue, ErrorMessage = "El precio debe ser un número positivo.")]
        public decimal Price { get; set; }  // Precio de la propiedad

        [Range(0, 10, ErrorMessage = "El número de plantas debe ser entre 0 y 10.")]
        public int Floors { get; set; }  // Número de plantas

        [Required(ErrorMessage = "Debe especificar si tiene cochera.")]
        public bool HasGarage { get; set; }  // ¿Tiene cochera?

        [Range(0, 10, ErrorMessage = "El número de carros debe ser entre 0 y 10.")]
        public int GarageCapacity { get; set; }  // Capacidad de la cochera

        [Required(ErrorMessage = "Debe especificar si es en condominio.")]
        public bool IsCondominium { get; set; }  // ¿Es en condominio?
        [NotMapped]
        public virtual List<MediaFile> MediaFiles { get; set; } = new List<MediaFile>(); // Lista de archivos multimedia asociados al proyecto
    }
}