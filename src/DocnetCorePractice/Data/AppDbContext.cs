using DocnetCorePractice.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace DocnetCorePractice.Data
{
    public class AppDbContext :DbContext,IDbContext
    {
        private readonly IConfiguration _configuration;

        public AppDbContext()
        {

        }

        public AppDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }
        DbSet<UserEntity> UserEntity { get; set; }
        DbSet<CaffeEntity> CaffeEntity { get; set; }
        DbSet<OrderEntity> OrderEntity { get; set; }
        DbSet<OrderItemEntity> OrderItemEntity { get; set; }
        DbSet<RefreshTokens> RefreshTokensEntity { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    _configuration.GetConnectionString("DefaultConnectStrings")
                );
            }
        }
    }
}
