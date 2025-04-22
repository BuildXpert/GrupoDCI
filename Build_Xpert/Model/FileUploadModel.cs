
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace Build_Xpert.Model
{

    public class FileUploadModel
    {
        [Required]
        public IBrowserFile File { get; set; }

        public string? ProjectId { get; set; }

        public string? UserId { get; set; }

        public bool IsPrivate { get; set; } = true;
    }

}
