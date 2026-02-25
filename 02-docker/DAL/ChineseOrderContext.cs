using Microsoft.EntityFrameworkCore;
using server.Models;

namespace server.DAL
{
    public class ChineseOrderContext:DbContext
    {
        
            public DbSet<Gift> Gifts { get; set; }
            public DbSet<Donor> Doners { get; set; }
            public DbSet<GiftDescription> GiftDescriptions { get; set; }
            public DbSet<User> Users { get; set; }
            public DbSet<Purchase> Purchases { get; set; }
            public DbSet<Winner> Winners { get; set; }
    



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Gift>()
                .Property(g => g.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

      
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasDefaultValue(UserRole.Client);

        }



        public ChineseOrderContext(DbContextOptions<ChineseOrderContext> options) : base(options)
            {

            }

            
      
        
   }
}
