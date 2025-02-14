// Models/Asset.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace demo_EFI_app.Models
{
    public class Asset
    {
        [Key]
        public int AssetID { get; set; }
        public required string SerialNumber { get; set; }
        public required string ModelNumber { get; set; }
        public required string Manufacturer { get; set; }
        public int StatusID { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        // Navigation property
        public ICollection<AssetAssignment> Assignments { get; set; } = new List<AssetAssignment>();
    }
}