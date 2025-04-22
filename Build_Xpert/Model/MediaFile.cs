using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Build_Xpert.Model
{
    public class MediaFile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string MediaId { get; set; }
        public bool IsPrivate { get; set; }
        public string ContainerName { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string TypeId { get; set; }
        public virtual FileType? FileType { get; set; }
        [NotMapped]
        public string? PublicUrl { get; set; }
    }
}
