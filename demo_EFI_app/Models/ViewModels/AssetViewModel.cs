// Models/ViewModels/AssetViewModel.cs
namespace demo_EFI_app.Models.ViewModels
{
    public class AssetViewModel
    {
        public int AssetID { get; set; }
        public required string SerialNumber { get; set; }
        public required string ModelNumber { get; set; }
        public required string Manufacturer { get; set; }
        public required string StatusName { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}