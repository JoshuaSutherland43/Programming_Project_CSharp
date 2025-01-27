
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.X509;
using PROG_POE.Controllers;
using PROG_POE.DAL;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using System.Security.Claims;

namespace PROG_POE.Models
{
    public class Data
    {
        //Obtaining Base API address
        private static string apiBaseUrl = "https://localhost:7257";
        private readonly ClaimDbContext _context;
        private readonly HttpClient client;


        // Inject the DbContext in the constructor
        public Data(ClaimDbContext context, HttpClient httpClient)
        {
            _context = context;
            client = httpClient;
        }

        //Sign Up API
        // (Zang, 2022)
        public async Task<string> SignUp(string email, string password, string fName, string surname)
        {
            string encodedEmail = Uri.EscapeDataString(email);
            string encodedPassword = Uri.EscapeDataString(password);
            string encodedFName = Uri.EscapeDataString(fName);
            string encodedSurname = Uri.EscapeDataString(surname);

            var url = $"{apiBaseUrl}/signup?email={encodedEmail}&password={encodedPassword}&firstName={encodedFName}&surname={encodedSurname}";
            HttpResponseMessage response = await client.PostAsync(url, null);
            return await response.Content.ReadAsStringAsync();
        }


        //Login API method
        // (Zang, 2022)
        public async Task<string> LoginUser(string email, string password)
        {
            // Encode parameters to handle special characters to allow the email to transfer smoothly
            string encodedEmail = Uri.EscapeDataString(email);
            string encodedPassword = Uri.EscapeDataString(password);

            
            var url = $"{apiBaseUrl}/login?email={encodedEmail}&password={encodedPassword}";

            HttpResponseMessage response = await client.PostAsync(url, null); 

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new HttpRequestException($"Error: {response.StatusCode}");
            }
        }


        //Calculate Rate automatically
        public double CalculateRate(int hours)
        {
            if (hours > 0 && hours <= 10)
                return 200;
            else if (hours > 10 && hours <= 20)
                return 300;
            else if (hours > 20 && hours <= 30)
                return 400;
            else if (hours > 30 && hours <= 40)
                return 500;
            else if (hours > 40 && hours <= 50)
                return 600;
            else
                return 0;
        }



        // Upload Claim API
        // (Zang, 2022)
        public async Task<string> UploadClaim(DateTime uploadDate, int hours, double rate, string notes, int userId, string filePath, string status)
        {
            var url = $"{apiBaseUrl}/uploadClaim?uploadDate={uploadDate:yyyy-MM-dd}&hours={hours}&rate={rate}&notes={notes}&userId={userId}&filePath={filePath}&status={status}";
            HttpResponseMessage response = await client.PostAsync(url, null);

            
            string responseString = await response.Content.ReadAsStringAsync();

            return responseString;
        }



        // Obtaining User Claims API
        // (Zang, 2022)
        public async Task<List<Claim>> GetUserClaims(int userId)
        {
            var url = $"{apiBaseUrl}/getUserClaims?userId={userId}";
            HttpResponseMessage response = await client.GetAsync(url);
            string responseString = await response.Content.ReadAsStringAsync();

            // Deserialize the response into a List of Claims
            List<Claim> claims = JsonConvert.DeserializeObject<List<Claim>>(responseString);

            return claims;

        }



        //Update Claims API 
        // (Zang, 2022)
        public async Task<string> UpdateClaim(int claimId, string status)
        {
            var url = $"{apiBaseUrl}/updateClaim?claimId={claimId}&status={status}";
            try
            {
                HttpResponseMessage response = await client.PutAsync(url, null);
                response.EnsureSuccessStatusCode();  
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                return $"Request failed: {ex.Message}";
            }
        }



        //Get all Claims API method
        // (Zang, 2022)
        public async Task<List<Claim>> GetAllClaims()
        {
            var url = $"{apiBaseUrl}/getAllClaims";
            HttpResponseMessage response = await client.GetAsync(url);
            string responseString = await response.Content.ReadAsStringAsync();

            List<Claim> claims = JsonConvert.DeserializeObject<List<Claim>>(responseString);

            if (claims != null)
            {
                // Add claims that are not already in the DbContext
                foreach (var claim in claims)
                {
                    if (!_context.CLAIM.AsNoTracking().Any(c => c.ClaimID == claim.ClaimID))  
                    {
                        _context.CLAIM.Add(claim);
                    }
                }

                await _context.SaveChangesAsync(); 
            }

            return claims;
        }



        // Get User Full Name by CLaim ID
        // (Zang, 2022)
        public async Task<string> GetUserFullNameByClaim(int claimId)
        {
            var url = $"{apiBaseUrl}/getUserFullNameByClaim?claimId={claimId}";
            HttpResponseMessage response = await client.GetAsync(url);
            return await response.Content.ReadAsStringAsync();  
        }



        // Get Lecturer Full Name by Lecturer ID
        // (Zang, 2022)
        public async Task<string> GetLecturerFullNameFromLecturer(int userId)
        {
            var url = $"{apiBaseUrl}/getLecturerFullName?lecturerId={userId}";
            HttpResponseMessage response = await client.GetAsync(url);
            return await response.Content.ReadAsStringAsync();

        }



        // Amount earned for claim API method
        // (Zang, 2022)
        public async Task<string> GetAmountEarned(int claimId)
        {
            var url = $"{apiBaseUrl}/getAmountEarned?claimId={claimId}";
            HttpResponseMessage response = await client.GetAsync(url);
            return await response.Content.ReadAsStringAsync();

        }



        // Get Total Amount Earned by Lecturer Method
        // (Zang, 2022)
        public async Task<string> GetTotalAmountEarnedByLecturer(int userId)
        {
            var url = $"{apiBaseUrl}/getTotalAmountEarned?userId={userId}";
            HttpResponseMessage response = await client.GetAsync(url);
            return await response.Content.ReadAsStringAsync();

        }



        // Upload Invoice API Method
        // (Zang, 2022)
        public async Task<string> UploadInvoice(int userId, int claimId)
        {
            
                var url = $"{apiBaseUrl}/uploadInvoice?lecturerId={userId}&claimId={claimId}";
                 HttpResponseMessage response = await client.PostAsync(url, null);

                string responseString = await response.Content.ReadAsStringAsync();

                return responseString;

        }



        // Get All Invoices Method API
        // (Zang, 2022)
        public async Task<List<Invoice>> GetAllInvoices()
        {
            var url = $"{apiBaseUrl}/getAllInvoices";
            HttpResponseMessage response = await client.GetAsync(url);

            string responseString = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"API Response: {responseString}");  

            List<Invoice> invoices = JsonConvert.DeserializeObject<List<Invoice>>(responseString);

            if (invoices == null || !invoices.Any())
            {
                Console.WriteLine("No invoices found or deserialization failed.");
            }

            return invoices;
        }




        // Get All Claims from Invoices
        // (Zang, 2022)
        public async Task<string> GetAllClaimsFromInvoice()
        {

            var url = $"{apiBaseUrl}/getAllClaimsFromInvoice";
            HttpResponseMessage response = await client.GetAsync(url);
            return await response.Content.ReadAsStringAsync();

        }




        // Get All Processed Claims
        // (Zang, 2022)
        public async Task<List<Claim>> GetAllProcessedClaims()
        {

            var url = $"{apiBaseUrl}/getAllProcessedClaims";
            HttpResponseMessage response = await client.GetAsync(url);
            string responseString = await response.Content.ReadAsStringAsync();

            //Deserializing claim from response string
            List<Claim> claims = JsonConvert.DeserializeObject<List<Claim>>(responseString);

            return claims;

        }




        // Get Processed Claims by Lecturer
        // (Zang, 2022)
        public async Task<List<Claim>> GetProcessedClaimsByLecturer(int userId)
        {

            var url = $"{apiBaseUrl}/getProcessedClaimsByLecturer/{userId}";
            HttpResponseMessage response = await client.GetAsync(url);
            string responseString = await response.Content.ReadAsStringAsync();

            // Deserializing API and adding to list claim
            List<Claim> claims = JsonConvert.DeserializeObject<List<Claim>>(responseString);

            if (claims != null)
            {
                foreach (var claim in claims)
                {
                    _context.CLAIM.Add(claim);
                }

                await _context.SaveChangesAsync();
            }

            return claims; 

        }




        // Get Approved Claims by Lecturer
        // (Zang, 2022)
        public async Task<List<Claim>> GetApprovedClaimsByLecturer(int userId)
        {
            var url = $"{apiBaseUrl}/getApprovedClaimsByLecturer/{userId}";
            HttpResponseMessage response = await client.GetAsync(url);
            string responseString = await response.Content.ReadAsStringAsync();

            // Deserializing the Json Response string and adding it to list
            List<Claim> claims = JsonConvert.DeserializeObject<List<Claim>>(responseString);

            return claims;

        }
    }
}

//=====================================================
// Referencing
//=====================================================

//Rudman, G. (2024)
//BCA2 CLDV Part 2 Workshop, YouTube.
//Available at: https://www.youtube.com/watch?v=I_tiFJ-nlfE&list=LL&index=1&t=13s
//(Accessed: 18 October 2024). 


//A.Zang (2022)
//Connecting a web API with an ASP.NET core MVC application, Telerik Blogs.
//Available at: https://www.telerik.com/blogs/connecting-web-api-aspnet-core-mvc-application
//(Accessed: 22 November 2024). 