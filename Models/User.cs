using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PROG_POE.Models
{
    public class User
    {
        [Key]
        [Column("User_ID")] 
        public int UserID { get; set; }

        [Column("FIRST_NAME")]  
        public string First_Name { get; set; }

        [Column("LAST_NAME")]  
        public string Last_Name { get; set; }

        [Column("EMAIL")]  
        public string Email { get; set; }

        [Column("PASSWORD")]  
        public string Password { get; set; }

        [Column("ROLE_ID")]  
        public string RoleId { get; set; }
    
       public virtual ICollection<Claim>? Claims { get; set; }
    }
}
