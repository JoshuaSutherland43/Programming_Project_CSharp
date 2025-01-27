
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PROG_POE.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PROG_POE.Controllers
{
    public class LoginController : Controller
    {
        private readonly Data _data;

        public LoginController(Data data)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string Email, string Password)
        {
            try
            {
                // Call the asynchronous LoginUser API method
                string response = await _data.LoginUser(Email, Password);

                // Deserialize the response to extract the token
                var jsonResponse = JsonConvert.DeserializeObject<dynamic>(response);
                string token = jsonResponse?.token?.ToString();

                if (string.IsNullOrEmpty(token))
                {
                    ModelState.AddModelError("", "Invalid login attempt. No token received.");
                    return View();
                }

                // Obtaining the JWT token and ensuring it is valid
                var handler = new JwtSecurityTokenHandler();
                if (!handler.CanReadToken(token))
                {
                    ModelState.AddModelError("", "Invalid token format.");
                    return View();
                }

                // reading the JWT token and then specifying which variables go where.
                var jwtToken = handler.ReadJwtToken(token);

                // Extract claims from the token
                var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                //If the token is missing some variables an error message will display
                if (string.IsNullOrEmpty(emailClaim) || string.IsNullOrEmpty(roleClaim) || string.IsNullOrEmpty(userIdClaim))
                {
                    ModelState.AddModelError("", "Invalid token. Missing required claims.");
                    return View();
                }

                // Create a User object using claims
                var user = new User
                {
                    Email = emailClaim,
                    RoleId = roleClaim,
                    UserID = int.Parse(userIdClaim) 
                };

                // Store logged-in user information
                UserHolder.loggedInUser = user;

                // Store user info in session
                HttpContext.Session.SetString("UserRole", user.RoleId);
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetInt32("UserID", user.UserID); 

                // Redirect based on user role after login
                return user.RoleId switch
                {
                    "1" => RedirectToAction("Index", "Home"),
                    "2" => RedirectToAction("Index", "ProgrammeCoordinator"),
                    "3" => RedirectToAction("Index", "AcademicManager"),
                    "4" => RedirectToAction("Index", "HR"),
                    _ => RedirectToAction("Login", "Login")
                };
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                return View();
            }
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

