using Microsoft.AspNetCore.Mvc;
using PROG_POE.Models;
using System.Collections.Generic;
using System.Linq;
using PROG_POE.DAL;


namespace PROG_POE.Controllers
{
    [RoleAuthorizer("2")]
    public class ProgrammeCoordinatorController : Controller
    {
        private readonly ClaimDbContext _context;
        private readonly Data _data;

        public ProgrammeCoordinatorController(ClaimDbContext context, Data data)
        {
            _context = context;
            _data = data;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (UserHolder.loggedInUser == null || UserHolder.loggedInUser.RoleId != "2") 
            {
                ModelState.AddModelError("", "You do not have permission to access this page.");
                return RedirectToAction("Login", "Login");
            }

            var pendingClaims = await _data.GetAllClaims();
            ViewBag.PendingClaims = pendingClaims;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateClaim(int claimID, string status)
        {
            var claim = (await _data.GetAllClaims()).FirstOrDefault(c => c.ClaimID == claimID);
            if (claim == null) return NotFound();

            claim.Status = status;
            _data.UpdateClaim(claim.ClaimID, claim.Status);

            if (status == "Approved")
            {
                _data.UploadInvoice(claim.UserID, claim.ClaimID);
            }

            return RedirectToAction("Index");
        }
    }
}