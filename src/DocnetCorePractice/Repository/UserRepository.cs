using DocnetCorePractice.Data;
using DocnetCorePractice.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace DocnetCorePractice.Repository
{
    public interface IUserRepository
    {
        List<UserEntity>? GetAllUser();
        int AddUser(UserEntity entity);
        int DeleteUser(string id);
    }
    public class UserRepository : IUserRepository
    {
        public UserRepository() { }
        public int AddUser(UserEntity entity)
        {
            using (var context = new AppDbContext())
            {
                context.Set<UserEntity>().Add(entity);
                return context.SaveChanges();
            }
        }

        public int DeleteUser(string id)
        {
            throw new NotImplementedException();
        }

        public List<UserEntity>? GetAllUser()
        {
            using (var context = new AppDbContext())
            {
                var user = context.Set<UserEntity>().AsNoTracking().ToList();
                return user;
            }
        }
    }
}
