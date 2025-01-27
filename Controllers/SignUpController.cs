using Microsoft.AspNetCore.Mvc;
using PROG_POE.DAL;
using PROG_POE.Models;

namespace PROG_POE.Controllers
{
    public class SignUpController : Controller
    {
        private readonly ClaimDbContext _context;
        private readonly Data _data;

        // Initializing admin controller
        public SignUpController(ClaimDbContext context, Data data)
        {
            _context = context;
            _data = data;
        }

        [HttpGet]
        public IActionResult SignUp( )
        {
            return View();
        }

        [HttpPost]
        //(Rudman, G. 2024)
        public async Task<IActionResult> SignUp(string FirstName, string Surname, string Email, string Password, string ConfirmPassword)
        {
            if (Password != ConfirmPassword)
            {
                // Return error if passwords don't match
                ModelState.AddModelError("", "Passwords do not match.");
                return View();
            }

            // Call the method to sign up and save the user in the database
            await _data.SignUp(Email, Password, FirstName, Surname); 


            // Redirect to login
            return RedirectToAction("Login", "Login");
        }
    }
}

//==========================================
//Referencing
//==========================================

//Rudman, G. (2024)
//BCA2 CLDV Part 2 Workshop, YouTube.
//Available at: https://www.youtube.com/watch?v=I_tiFJ-nlfE&list=LL&index=1&t=13s
//(Accessed: 18 October 2024). 

