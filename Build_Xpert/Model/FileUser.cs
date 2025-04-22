using System.ComponentModel.DataAnnotations.Schema;

namespace Build_Xpert.Model
{
    public class FileUser
    {
        public string UserId { get; set; }
        public string FileId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }
        [ForeignKey("FileId")]
        public virtual MediaFile? File { get; set; }
    }
}
