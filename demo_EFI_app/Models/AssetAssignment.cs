// Models/AssetAssignment.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace demo_EFI_app.Models
{
    public class AssetAssignment
    {
        [Key]
        public int AssignmentID { get; set; }
        public int AssetID { get; set; }
        public int EmployeeID { get; set; }
        public int DepartmentID { get; set; }
        public required string LocationDescription { get; set; }
        public DateTime AssignmentDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string? AssignmentNotes { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        // Navigation property
        public required Asset Asset { get; set; }
    }
}