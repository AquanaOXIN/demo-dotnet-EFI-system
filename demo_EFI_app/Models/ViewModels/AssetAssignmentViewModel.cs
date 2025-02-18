// Models/ViewModels/AssetAssignmentViewModel.cs
namespace demo_EFI_app.Models.ViewModels
{
    public class AssetAssignmentViewModel
    {
        public int AssetID { get; set; }
        public required string SerialNumber { get; set; }
        public required string LocationDescription { get; set; }
        public required string AssigneeName { get; set; }
        public int EmployeeID { get; set; }
        public DateTime AssignmentDate { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal PurchasePrice { get; set; }
    }
}