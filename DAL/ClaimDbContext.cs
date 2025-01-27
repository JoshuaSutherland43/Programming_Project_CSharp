using Microsoft.EntityFrameworkCore;
using PROG_POE.Models;

namespace PROG_POE.DAL
{
    public class ClaimDbContext : DbContext
    {
        public ClaimDbContext(DbContextOptions<ClaimDbContext> options)
            : base(options)
        {
        }

        public DbSet<Claim> CLAIM { get; set; }
        public DbSet<Invoice> HR { get; set; }
    }

}
