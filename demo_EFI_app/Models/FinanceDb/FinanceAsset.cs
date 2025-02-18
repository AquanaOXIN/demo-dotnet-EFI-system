// Models/FinanceDb/FinanceAsset.cs
using System.ComponentModel.DataAnnotations;

namespace demo_EFI_app.Models.FinanceDb
{
    public class FinanceAsset
    {
        [Key]
        public int AssetID { get; set; }
        public int RequestID { get; set; }
        public required string PurchaseOrderNumber { get; set; }
        public decimal ActualCost { get; set; }
        public DateTime PurchaseDate { get; set; }
        public required string VendorName { get; set; }
    }
}