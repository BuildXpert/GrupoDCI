using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Build_Xpert.Model
{
    public class InventoryItemUsage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InventoryUsageId { get; set; }

        [Required]
        public int InventoryItemId { get; set; }

        [Required]
        public int ProjectId { get; set; }

        [Required]
        public int QuantityUsed { get; set; }

        [Required]
        public Boolean IsService { get; set; }
        
        [Required]
        public Decimal TotalSpent { get; set; }

        [ForeignKey(nameof(InventoryItemId))]
        public virtual InventoryItem InventoryItem { get; set; }

        [ForeignKey(nameof(ProjectId))]
        public virtual Project Project { get; set; }
    }
}
