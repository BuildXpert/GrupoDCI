﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Build_Xpert.Model
{
    public class Property
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la propiedad es obligatorio.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "La descripción de la propiedad es obligatoria.")]
        public string Description { get; set; }

        public string Status { get; set; } = "Disponible"; // Puede ser: Disponible, Vendido, En Construcción, En Remodelación
        public DateTime CreatedDate { get; set; } = DateTime.Now;

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
    }
}