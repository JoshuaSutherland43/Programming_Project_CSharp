using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PROG_POE.Models
{
    public class Claim
    {
        [Key]
        [Column("CLAIM_ID")] 
        public int ClaimID { get; set; }

        [Column("UPLOAD_DATE")] 
        public DateTime UploadDate { get; set; }

        [Column("HOURS")] 
        public int Hours { get; set; }

        [Column("RATE")]  
        public double Rate { get; set; }

        [Column("NOTES")]  
        public string Notes { get; set; }

        [Column("USER_ID")] 
        public int UserID { get; set; }

        [Column("FILE_PATH")]  
        public string FilePath { get; set; }

        [Column("STATUS")] 
        public string Status { get; set; } = "Pending";

        [Column("APPROVED_DATE")]  
        public DateTime? ApprovedDate { get; set; }

    }
}