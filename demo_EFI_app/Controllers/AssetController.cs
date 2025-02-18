using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using demo_EFI_app.Data;
using demo_EFI_app.Models;
using demo_EFI_app.Models.ViewModels;
using demo_EFI_app.Models.EmployeeDb;
using demo_EFI_app.Models.FinanceDb;
using System.Text;

namespace demo_EFI_app.Controllers
{
    public class AssetController : Controller
    {
        private readonly InventoryDbContext _inventoryContext;
        private readonly EmployeeDbContext _employeeContext;
        private readonly FinanceDbContext _financeContext;

        public AssetController(
            InventoryDbContext inventoryContext,
            EmployeeDbContext employeeContext,
            FinanceDbContext financeContext)
        {
            _inventoryContext = inventoryContext;
            _employeeContext = employeeContext;
            _financeContext = financeContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Search(int? assetId)
        {
            if (!assetId.HasValue)
            {
                return View("Index");
            }

            // Get current assignment
            var currentAssignment = await _inventoryContext.AssetAssignments
                .Include(aa => aa.Asset)
                .Where(aa => aa.AssetID == assetId && aa.ReturnDate == null)
                .FirstOrDefaultAsync();

            // Get assignment history
            var assignmentHistory = await _inventoryContext.AssetAssignments
                .Include(aa => aa.Asset)
                .Where(aa => aa.AssetID == assetId)
                .OrderByDescending(aa => aa.AssignmentDate)
                .ToListAsync();

            // Get employee names
            var employeeIds = assignmentHistory.Select(a => a.EmployeeID).Distinct().ToList();
            var employees = await _employeeContext.Employees
                .Where(e => employeeIds.Contains(e.EmployeeID))
                .ToDictionaryAsync(e => e.EmployeeID, e => $"{e.FirstName} {e.LastName}");

            var viewModel = new AssetSearchViewModel
            {
                CurrentAssignment = currentAssignment,
                AssignmentHistory = assignmentHistory.Select(ah => new AssetAssignmentHistoryViewModel
                {
                    AssignmentDate = ah.AssignmentDate,
                    ReturnDate = ah.ReturnDate,
                    LocationDescription = ah.LocationDescription,
                    EmployeeID = ah.EmployeeID,
                    EmployeeName = employees.TryGetValue(ah.EmployeeID, out var name) ? name : "Unknown",
                    AssignmentNotes = ah.AssignmentNotes
                }).ToList()
            };

            return View("Index", viewModel);
        }

        public async Task<IActionResult> List()
        {
            var assets = await _inventoryContext.Assets
                .Select(a => new AssetViewModel
                {
                    AssetID = a.AssetID,
                    SerialNumber = a.SerialNumber,
                    ModelNumber = a.ModelNumber,
                    Manufacturer = a.Manufacturer,
                    StatusName = a.StatusID.ToString(),
                    Notes = a.Notes ?? "",
                    CreatedDate = a.CreatedDate
                })
                .ToListAsync();

            return View(assets);
        }

        public async Task<IActionResult> ListAssignments()
        {
            // Get all current assignments
            var currentAssignments = await _inventoryContext.AssetAssignments
                .Include(aa => aa.Asset)
                .Where(aa => aa.ReturnDate == null)
                .ToListAsync();

            // Get all employees
            var employees = await _employeeContext.Employees.ToListAsync();

            // Get finance information
            var financeAssets = await _financeContext.Assets.ToListAsync();

            // Join the data in memory
            var assignmentDetails = currentAssignments.Select(aa => {
                var employee = employees.FirstOrDefault(e => e.EmployeeID == aa.EmployeeID);
                var financeInfo = financeAssets.FirstOrDefault(fa => fa.AssetID == aa.AssetID);

                return new AssetAssignmentViewModel
                {
                    AssetID = aa.AssetID,
                    SerialNumber = aa.Asset.SerialNumber,
                    LocationDescription = aa.LocationDescription,
                    EmployeeID = aa.EmployeeID,
                    AssigneeName = employee != null ? $"{employee.FirstName} {employee.LastName}" : "Unknown",
                    AssignmentDate = aa.AssignmentDate,
                    PurchaseDate = financeInfo?.PurchaseDate ?? aa.CreatedDate,
                    PurchasePrice = financeInfo?.ActualCost ?? 0
                };
            }).ToList();

            return View(assignmentDetails);
        }

        [HttpPost]
        public async Task<IActionResult> ExportAssets()
        {
            var assets = await _inventoryContext.Assets.ToListAsync();
            
            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("Asset ID,Serial Number,Model Number,Manufacturer,Status,Notes,Created Date");
            
            foreach (var asset in assets)
            {
                csvBuilder.AppendLine($"{asset.AssetID},{asset.SerialNumber},{asset.ModelNumber}," +
                    $"{asset.Manufacturer},{asset.StatusID},{asset.Notes},{asset.CreatedDate}");
            }

            byte[] buffer = Encoding.UTF8.GetBytes(csvBuilder.ToString());
            return File(buffer, "text/csv", $"assets_report_{DateTime.Now:yyyyMMdd}.csv");
        }

        [HttpPost]
        public async Task<IActionResult> ExportAssignments()
        {
            var assignmentDetails = await ListAssignments();
            var assignments = (assignmentDetails as ViewResult)?.Model as List<AssetAssignmentViewModel>;
            
            if (assignments == null)
                return BadRequest();

            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("Asset ID,Serial Number,Location,Employee Name,Employee ID,Assignment Date,Purchase Date,Purchase Price");
            
            foreach (var item in assignments)
            {
                csvBuilder.AppendLine($"{item.AssetID},{item.SerialNumber}," +
                    $"{item.LocationDescription},{item.AssigneeName},{item.EmployeeID}," +
                    $"{item.AssignmentDate},{item.PurchaseDate},{item.PurchasePrice}");
            }

            byte[] buffer = Encoding.UTF8.GetBytes(csvBuilder.ToString());
            return File(buffer, "text/csv", $"asset_assignments_report_{DateTime.Now:yyyyMMdd}.csv");
        }
    }
}