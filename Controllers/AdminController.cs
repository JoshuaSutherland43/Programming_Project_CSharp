using Microsoft.AspNetCore.Mvc;
using PROG_POE.Models;
using System.Collections.Generic;
using System.Linq;
using PROG_POE.DAL;

namespace PROG_POE.Controllers
{
    public class AdminController : Controller
    {
        private readonly ClaimDbContext _context;
        private readonly Data _data;

        // Initializing admin controller
        public AdminController(ClaimDbContext context, Data data)
        {
            _context = context;
            _data = data;
        }


        public async Task<IActionResult> Index(int? lecturerId = null, bool reset = false)
        {
            // If there is no user logged in, redirect to login page
            if (UserHolder.loggedInUser == null)
            {
                return RedirectToAction("Login", "Login");
            }

            // Get the user's role to determine access
            var userRole = UserHolder.loggedInUser.RoleId;

            // Check if the user is one of the allowed staff roles (Admin, Programme Coordinator, Academic Manager, HR)
            var allowedRoles = new[] { "1", "2", "3", "4" };
            if (!allowedRoles.Contains(userRole))
            {
                return View("Unauthorized");  // Redirect to unauthorized page or handle as needed
            }

            // Get all pending claims
            var pendingClaims = await _data.GetAllClaims(); // Await the task to fetch claims

            // If reset is true, show all processed claims, otherwise show claims for a specific lecturer
            List<Claim> processedClaims;

            if (reset)
            {
                processedClaims = await _data.GetAllProcessedClaims();
            }
            else if (lecturerId.HasValue)
            {
                processedClaims = await _data.GetProcessedClaimsByLecturer(lecturerId.Value);
            }
            else
            {
                processedClaims = await _data.GetAllProcessedClaims();
            }

            ViewData["ShowAddDiv"] = (userRole == "1" || userRole == "2" || userRole == "4");  
            ViewData["ShowWarningLabel"] = (userRole != "1" && userRole != "2" && userRole != "4");  
            ViewData["ShowAdminFeatures"] = (userRole == "1");  
            ViewData["ShowCoordinatorFeatures"] = (userRole == "2");  
            ViewData["ShowManagerFeatures"] = (userRole == "3");  
            ViewData["ShowHRFeatures"] = (userRole == "4");  

            // Saving claims into ViewBag for display in the view
            ViewBag.PendingClaims = pendingClaims;
            ViewBag.ProcessedClaims = processedClaims;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateClaim(int claimID, string status)
        {
            // Retrieve the claim
            var claims = await _data.GetAllClaims();  // Await the task to get the list of claims
            var claim = claims.FirstOrDefault(c => c.ClaimID == claimID);
            if (claim == null)
            {
                return NotFound();  
            }

            // Update the status of the claim
            claim.Status = status;
            _data.UpdateClaim(claim.ClaimID, claim.Status); 

            // If the claim is approved, update the INVOICE table
            if (status == "Approved")
            {
                _data.UploadInvoice(claim.UserID, claim.ClaimID);  // Upload invoice when claim is approved
            }

            return RedirectToAction("Index");
        }
    }
}
//==========================================
//Referencing
//==========================================

//BillWagner (2024)
//Lambda expressions - lambda expressions and anonymous functions - C# reference, Lambda expressions - Lambda expressions and anonymous functions - C# reference | Microsoft Learn.
//Available at: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/lambda-expressions
//(Accessed: 18 October 2024). 

//BillWagner (2023)
//?: Operator - the ternary conditional operator - C# reference, C# reference | Microsoft Learn.
//Available at: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/conditional-operator
//(Accessed: 18 October 2024). 