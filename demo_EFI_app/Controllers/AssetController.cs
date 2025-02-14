// Controllers/AssetController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using demo_EFI_app.Data;
using demo_EFI_app.Models;

namespace demo_EFI_app.Controllers
{
    public class AssetController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AssetController(ApplicationDbContext context)
        {
            _context = context;
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

            var assetAssignment = await _context.AssetAssignments
                .Include(aa => aa.Asset)
                .Where(aa => aa.AssetID == assetId && aa.ReturnDate == null)
                .FirstOrDefaultAsync();

            return View("Index", assetAssignment);
        }
    }
}