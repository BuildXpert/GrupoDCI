using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Build_Xpert.Model
{
    public class FileProject
    {
        public int ProjectId { get; set; }
        [Key]
        public string FileId { get; set; }
        [ForeignKey("ProjectId")]
        public virtual Project? User { get; set; }
        [ForeignKey("FileId")]
        public virtual MediaFile? File { get; set; }
    }
}
