using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Build_Xpert.Model
{
    public class ProjectPhaseTasks
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskId { get; set; }

        [Required(ErrorMessage = "El título de la tarea es obligatorio.")]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; } 

        public bool IsCompleted { get; set; } = false;

        [ForeignKey("PhaseId")]
        public int PhaseId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now; 



        public virtual ProjectPhase? ProjectPhase { get; set; }
    }
}
