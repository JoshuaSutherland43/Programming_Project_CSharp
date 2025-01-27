using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using PROG_POE.Controllers;

namespace PROG_POE.Models
{
    // Using a role authorizer to make my pages be assigned specifc roles.
    // (Yiyi. 2020)
    public class RoleAuthorizer : Attribute, IAuthorizationFilter
    {

        // declairing a list for my roles
        private readonly string[] _roles;

        public RoleAuthorizer(params string[] roles)
        {
            _roles = roles;
        }

        //Once a user is logged in they will be directed to their page, however if they open a page they are not allowed to open they will be sent back to the login page.
        // (Yiyi. 2020)
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = UserHolder.loggedInUser;

            // Check if the user is not logged in or does not have the required role
            if (user == null || !_roles.Contains(user.RoleId))
            {
                context.Result = user?.RoleId switch
                {
                    // Lecturer
                    "1" => new RedirectToActionResult("Index", "Home", null),    
                    
                    //Program Co
                    "2" => new RedirectToActionResult("Index", "ProgrammeCoordinator", null), 

                    //Academic Manager
                    "3" => new RedirectToActionResult("Index", "AcademicManager", null),     

                    //HR
                    "4" => new RedirectToActionResult("Index", "HR", null),  
                    
                    //If no specified role
                    _ => new RedirectToActionResult("Login", "Login", null)                  
                };
            }
        }
    }
}

//Referencing:
//============================
//
// 1. Yiyi (2020) How to pass information from an authorizationfilter to the action it is authorizing?, Stack Overflow.
// Available at: https://stackoverflow.com/questions/63360051/how-to-pass-information-from-an-authorizationfilter-to-the-action-it-is-authoriz
// (Accessed: 22 November 2024). 
//
//