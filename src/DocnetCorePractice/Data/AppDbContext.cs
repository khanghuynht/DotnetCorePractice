using DocnetCorePractice.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace DocnetCorePractice.Data
{
    public class AppDbContext :DbContext,IDbContext
    {

        public AppDbContext()
        {
            
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        DbSet<UserEntity> UserEntity { get; set; }
        DbSet<CaffeEntity> CaffeEntity { get; set; }
        DbSet<OrderEntity> OrderEntity { get; set; }
        DbSet<OrderItemEntity> OrderItemEntity { get; set; }
        DbSet<RefreshTokens> RefreshTokensEntity { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnectStrings"));
        }
    }
}
