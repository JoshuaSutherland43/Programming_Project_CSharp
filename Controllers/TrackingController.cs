using Microsoft.AspNetCore.Mvc;
using PROG_POE.Models;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PROG_POE.DAL;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PROG_POE.Controllers
{
    [RoleAuthorizer("1")]
    public class TrackingController : Controller
    {
        //(Rudman, G. 2024)
        //Adding connection string and calling classes

        private readonly ClaimDbContext _context;
        private readonly Data _data;  

        // Initializing the controller
        public TrackingController(ClaimDbContext context, Data data)
        {
            _context = context;
            _data = data; 
        }

        public async Task<IActionResult> Index()
        {
          
            if (UserHolder.loggedInUser == null)
            {
                return RedirectToAction("Login", "Login");
            }

            // Fetch data asynchronously
            await FetchData();

            // Returning the claims in the list
            return View(UserHolder.loggedInUser.Claims);
        }

        
        private async Task FetchData()
        {
            int lecturerId = UserHolder.loggedInUser.UserID;

            try
            {
                // Fetch the claims associated with the lecturer
                var claims = await _data.GetUserClaims(lecturerId);

                // Storing the fetched claims in UserHolder
                UserHolder.loggedInUser.Claims = claims;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching claims: {ex.Message}");
               
            }
        }
    }

    // User holder method to save instance of user
    public static class UserHolder
    {
        public static User loggedInUser;  // Save the logged-in user
    }
}

//==========================================
//Referencing
//==========================================

//Rudman, G. (2024)
//BCA2 CLDV Part 2 Workshop, YouTube.
//Available at: https://www.youtube.com/watch?v=I_tiFJ-nlfE&list=LL&index=1&t=13s
//(Accessed: 18 October 2024). 




