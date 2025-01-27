using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using PROG_POE.DAL;
using PROG_POE.Models;
using System.Text;
using IronPdf;
using System.IO;
using System.Security.Claims;

namespace PROG_POE.Controllers
{
    //Stating which user can use this page
    [RoleAuthorizer("4")]
    public class HRController : Controller
    {
        private readonly ClaimDbContext _context;

        private readonly Data _da;

        private readonly IWebHostEnvironment _environment;

        private readonly ILogger<HRController> _logger;

        //Initializing controller
        public HRController(ILogger<HRController> logger, IWebHostEnvironment environment, Data da, ClaimDbContext context)
        {
            _context = context;
            _da = da;
            _logger = logger;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                // Use 'await' to asynchronously fetch all invoices
                List<Invoice> invoices = await _da.GetAllInvoices();
                return View(invoices);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching invoices.");
                TempData["ErrorMessage"] = "Fetching invoices resulted in an error.";
                return RedirectToAction("Index");  
            }
        }

        //Invoice PDF Generator
        [HttpPost]
        public async Task<IActionResult> InvoiceToPDF(int invoiceId)
        {
            try
            {
                // Get all invoices and find the one with the specified invoiceId
                var invoices = await _da.GetAllInvoices();
                var invoice = invoices.FirstOrDefault(i => i.InvoiceID == invoiceId);

                if (invoice == null)
                {
                    _logger.LogWarning("Invoice not found: {InvoiceId}", invoiceId);
                    TempData["ErrorMessage"] = $"Invoice with ID {invoiceId} not found.";
                    return RedirectToAction("Index");
                }

                // Define the file path and content
                string folder = @"C:\Users\lab_services_student\Documents\INVOICES Prog";
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                //(IRONPDF. 2024)
                string fileName = $"Invoice_{invoice.InvoiceID}.pdf";

                // Creating HTML data to write a PDF document
                //(IRONPDF. 2024)
                StringBuilder content = new StringBuilder();
                content.AppendLine("<html><header><h2>Invoice Details:</h2></header><body>");
                content.AppendLine("<hr>");
                content.AppendLine("<br/>");
                content.AppendLine("<p>Invoice ID: " + invoice.InvoiceID + "</p>");
                content.AppendLine("<p>Lecturer ID: " + invoice.UserID + "</p>");
                content.AppendLine("<p>Lecturer Name: " + await _da.GetLecturerFullNameFromLecturer(invoice.UserID) + "</p>");
                content.AppendLine("<br/>");
                content.AppendLine("<h2>Claim History</h2>");
                content.AppendLine("<hr>");
                content.AppendLine("<table style='width:100%; border-collapse: collapse;'>");
                content.AppendLine("<tr style='text-align: center'><th>Claim ID</th><th>Hours Worked</th><th>Rate (R)</th><th>Amount Earned (R)</th></tr>");
                content.AppendLine("<br/>");

                //(IRONPDF. 2024)
                List<Models.Claim> relatedClaims = _da.GetApprovedClaimsByLecturer(invoice.UserID).Result;
                foreach (var claim in relatedClaims)
                {
                    content.AppendLine($"<tr style='text-align: center'><td>{claim.ClaimID}</td><td>{claim.Hours}</td><td>{claim.Rate:F2}</td><td>{_da.GetAmountEarned(claim.ClaimID).Result:F2}</td></tr>");
                }

                //(IRONPDF. 2024)
                content.AppendLine("</table>");
                content.AppendLine("<hr>");
                content.AppendLine("<table style='width:100%; border-collapse: collapse;'>");
                content.AppendLine("<br/>");
                content.AppendLine("<tr><td><strong>Total Amount Earned for Month:</strong></td><td>" + invoice.TotalAmount.ToString("C") + "</td></tr>");
                content.AppendLine("<br/>");
                content.AppendLine("<tr><td><strong>Invoice Date:</strong></td><td>" + invoice.InvoiceDate.ToString("yyyy-MM-dd") + "</td></tr>");
                content.AppendLine("</table>");
                content.AppendLine("</body></html>");

                // Use IronPdf to generate the PDF from the HTML content
                //(IRONPDF. 2024)
                ChromePdfRenderer renderer = new ChromePdfRenderer();
                PdfDocument pdf = renderer.RenderHtmlAsPdf(content.ToString());

                //(IRONPDF. 2024)
                // Save the PDF to the folder
                string pdfFilePath = Path.Combine(folder, fileName);
                pdf.SaveAs(pdfFilePath);

                _logger.LogInformation("Invoice generated and saved successfully for download: {InvoiceId}", invoiceId);

                // Return the PDF file for download
                byte[] fileBytes = System.IO.File.ReadAllBytes(pdfFilePath);
                return File(fileBytes, "application/pdf", fileName);
            }
            // Catch exception if an error occurs when trying to generate pdf
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating invoice PDF.");
                TempData["ErrorMessage"] = "An error occurred while generating the invoice PDF.";
                return RedirectToAction("Index"); 
            }
        }

        [HttpGet]
        public IActionResult OpenFile(string fileName)
        {
            try
            {
                string folder = @"C:\Users\lab_services_student\Documents\INVOICES Prog";
                string fullPath = Path.Combine(folder, fileName);

                if (!System.IO.File.Exists(fullPath))
                {
                    //Opening a file that does not exist/ not found error
                    _logger.LogWarning("File not found: {FilePath}", fullPath);
                    TempData["ErrorMessage"] = $"File '{fileName}' not found.";
                    return RedirectToAction("Index"); 
                }

                // displaying file
                byte[] fileBytes = System.IO.File.ReadAllBytes(fullPath);
                return File(fileBytes, "application/octet-stream", fileName);
            }
            // Error when a file does not display 
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while producing file.");
                TempData["ErrorMessage"] = "An error occured.";
                return RedirectToAction("Index"); 
            }
        }
    }
}
//=====================
//References
//=====================
// 1. IRONPDF (2024) C# text to PDF [code example & tutorial], IronPDF. Available at: https://ironpdf.com/blog/using-ironpdf/csharp-text-to-pdf/ (Accessed: 22 November 2024). 
//
//

