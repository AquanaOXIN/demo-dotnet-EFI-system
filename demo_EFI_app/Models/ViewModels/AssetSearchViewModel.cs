// Models/ViewModels/AssetSearchViewModel.cs
namespace demo_EFI_app.Models.ViewModels
{
    public class AssetSearchViewModel
    {
        public AssetAssignment? CurrentAssignment { get; set; }
        public List<AssetAssignmentHistoryViewModel> AssignmentHistory { get; set; } = new();
    }

    public class AssetAssignmentHistoryViewModel
    {
        public DateTime AssignmentDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public required string LocationDescription { get; set; }
        public int EmployeeID { get; set; }
        public required string EmployeeName { get; set; }
        public string? AssignmentNotes { get; set; }
    }
}