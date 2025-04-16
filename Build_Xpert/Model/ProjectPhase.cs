using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Build_Xpert.Model
{
    public class ProjectPhase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PhaseId { get; set; }

        [Required(ErrorMessage = "El nombre de la fase es obligatorio.")]
        public string PhaseName { get; set; }

        [ForeignKey("Project")]
        public int ProjectId { get; set; }

        public ICollection<ProjectPhaseTasks> ProjectPhaseTasks { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;



        public virtual Project Project { get; set; }
    }
}
