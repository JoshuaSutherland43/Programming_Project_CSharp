using Microsoft.AspNetCore.Mvc;
using PROG_POE.Models;
using System.Collections.Generic;
using System.Linq;
using PROG_POE.DAL;

namespace PROG_POE.Controllers
{
    [RoleAuthorizer("3")]
    public class AcademicManagerController : Controller
    {
        private readonly ClaimDbContext _context;
        private readonly Data _data;

        public AcademicManagerController(ClaimDbContext context, Data data)
        {
            _context = context;
            _data = data;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? lecturerId = null)
        {
            if (UserHolder.loggedInUser == null || UserHolder.loggedInUser.RoleId != "3") 
            {
                ModelState.AddModelError("", "You do not have permission to access this page.");
                return RedirectToAction("Login", "Login");
            }

            var processedClaims = lecturerId.HasValue
                ? await _data.GetProcessedClaimsByLecturer(lecturerId.Value)
                : await _data.GetAllProcessedClaims();

            ViewBag.ProcessedClaims = processedClaims;
            return View();
        }
    }
}