using com.tweetapp.domain.Base;
using com.tweetapp.domain.DAOEntities;
using com.tweetapp.domain.Models;
using com.tweetapp.infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace com.tweetapp.infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task<bool> RegisterUser(User user);
        Task<User> LoginUser(UserLoginDAO user);
        Task<bool> Logout(User user);
        Task<bool> ResetPassword(User user);
        Task<bool> ForgotPassword(User user);
        Task<User> GetUserByEmail(string email);
        Task<IEnumerable<User>> GetAllUsers();
    }
    public class UserRepository : IUserRepository
    {
        private readonly TwitterDbContext _context;
        public UserRepository(TwitterDbContext context)
        {
            _context = context ;
        }
        public IUnitOfWork UnitOfWork => _context;

        public async Task<bool> ResetPassword(User user)
        {
           _context.Users.Update(user);
            var changedRecordCount = await _context.SaveChangesAsync();
            return changedRecordCount > 0;
        }

        public Task<User> LoginUser(UserLoginDAO user)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Logout(User user)
        {
            user.IsActive = false;
            user.LastSeen = DateTime.Now;
            _context.Users.Update(user);
           var changes= await _context.SaveChangesAsync();
            return changes > 0;
        }


        public async Task<bool> RegisterUser(User user)
        {
            try
            {
              await _context.Users.AddAsync(user);
              var changedRecoundCount = await _context.SaveChangesAsync();
                if (changedRecoundCount > 0)
                {
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {

                throw new  Exception("Db connection failed");
            }
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(s => s.Email == email);
        }

        public async Task<bool> ForgotPassword(User user)
        {
             _context.Users.Update(user);
            var changedRecordCount = await _context.SaveChangesAsync();
            return changedRecordCount > 0;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
          return await  _context.Users.ToListAsync();
        }
    }
}
