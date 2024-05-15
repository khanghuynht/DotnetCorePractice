using DocnetCorePractice.Data;
using DocnetCorePractice.Data.Entity;

namespace DocnetCorePractice.Repository
{
    public interface IRefreshTokensRepository
    {
        int InsertToken(RefreshTokens refreshTokens);
    }
    public class RefreshTokensRepository : IRefreshTokensRepository
    {
        private readonly AppDbContext _context;
        public RefreshTokensRepository(AppDbContext context)
        {
            _context = context;
        }
        public int InsertToken(RefreshTokens refreshTokens)
        {
            try
            {
                _context.Set<RefreshTokens>().Add(refreshTokens);
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
