using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Build_Xpert.Model
{
    public class ProjectTask
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El título de la tarea es obligatorio.")]
        public string Title { get; set; }

        public bool IsCompleted { get; set; } = false;

        [ForeignKey("Project")]
        public int ProjectId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now; 

        [MaxLength(500)]
        public string Description { get; set; } 


        public virtual Project Project { get; set; }
    }
}
