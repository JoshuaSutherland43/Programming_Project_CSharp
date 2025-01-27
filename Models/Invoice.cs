using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PROG_POE.Models
{
    public class Invoice
    {
        [Key]
        [Column("INVOICE_ID")]  
        public int InvoiceID { get; set; }

        [Column("CLAIM_ID")] 
        public int ClaimID { get; set; }

        [Column("USER_ID")]
        public int UserID { get; set; }

        [Column("TOTAL_AMOUNT")]  
        public double TotalAmount { get; set; }

        [Column("INVOICE_DATE")] 
        public DateTime InvoiceDate { get; set; } = DateTime.Now;

        public User user { get; set; } 
        public Claim Claim { get; set; }  
    }

}