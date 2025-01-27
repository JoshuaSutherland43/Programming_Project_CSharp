using Microsoft.AspNetCore.Mvc;
using PROG_POE.Models;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Claim = PROG_POE.Models.Claim; // Ensure this is included

namespace PROG_POE.Controllers
{
    //Specifying Users role
    [RoleAuthorizer("1")]
    public class HomeController : Controller
    {
        private readonly Data _data;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<HomeController> _logger;

        // Initializing controller
        public HomeController(Data data,ILogger<HomeController> logger, IWebHostEnvironment environment)
        {
            _data = data;
            _logger = logger;
            _environment = environment;
        }


        // The correct method to get all claims asynchronously
        public async Task<IActionResult> Index()
        {
            try
            {
               
                List<Claim> claims = await _data.GetAllClaims(); 

                
                return View(claims);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching claims");
                return View("Error"); 
            }
        }

        //Calling upload method and sending data to data class
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file, int hours, string notes, string status)
        {
            if (!HttpContext.Session.TryGetValue("UserID", out var userIdBytes))
            {
                return RedirectToAction("Login", "Login"); 
            }


            if (file != null && file.Length > 0)
            {
                //Specifying documents allowed
                var allowedExtensions = new[] { ".pdf", ".docx", ".doc" };
                var fileExtension = Path.GetExtension(file.FileName).ToLower();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    TempData["ErrorMessage"] = "Invalid file type. Only PDF and Word Docs allowed";
                    return RedirectToAction("Index");
                }

                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(_environment.WebRootPath, "uploads", fileName);

                //disallowing the user to submit more than 1 of the same document.
                if (System.IO.File.Exists(path))
                {
                    TempData["ErrorMessage"] = "This file has already been uploaded.";
                    return RedirectToAction("Index");
                }

                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    var fileBytes = memoryStream.ToArray();
                    var encryptedBytes = EncryptionHelper.Encrypt(fileBytes);
                    System.IO.File.WriteAllBytes(path, encryptedBytes);
                }

                // Calculate rate and call the upload method
                double rate = _data.CalculateRate(hours);
                string responseString = await _data.UploadClaim(
                    uploadDate: DateTime.Now,
                    hours: hours,
                    rate: rate,
                    notes: notes,
                    userId: UserHolder.loggedInUser.UserID, 
                    filePath: fileName,
                    status: "Pending"
                );
            }

            return RedirectToAction("Index");
        }

        // Method to open and decrypt a file
        public IActionResult OpenFile(string fileName)
        {
            var path = Path.Combine(_environment.WebRootPath, "uploads", fileName);
            var encryptedBytes = System.IO.File.ReadAllBytes(path);
            var decryptedBytes = EncryptionHelper.Decrypt(encryptedBytes);
            return File(decryptedBytes, "application/octet-stream", fileName);
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

//Rudman, G. (2024)
//BCA2 CLDV Part 2 Workshop, YouTube.
//Available at: https://www.youtube.com/watch?v=I_tiFJ-nlfE&list=LL&index=1&t=13s
//(Accessed: 18 October 2024). 
