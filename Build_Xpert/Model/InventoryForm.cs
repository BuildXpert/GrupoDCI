using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Build_Xpert.Model
{
    public class InventoryForm
    {
        public int InventoryItemId { get; set; }
        public int Quantity { get; set; }
        public Dictionary<string, object> SelectedAttributes { get; set; } = new();
    }
}