using com.tweetapp.domain.Base;
using com.tweetapp.domain.DAOEntities;
using com.tweetapp.domain.Models;
using com.tweetapp.infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
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
        Task<User> GetUserByUserName(string username);
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUserById(string id);
        Task<bool> CheckEmailAndUserNameExists(string userName,string email);
        Task<IEnumerable<User>> GetSearchedUser(string userName);
    }
    public class UserRepository : IUserRepository
    {
 
        private readonly IDbClient _dbClient;
        public UserRepository(IDbClient dbClient)
        {
            
            _dbClient = dbClient;
        }

        public async Task<bool> ResetPassword(User user)
        {
            try
            {
                var filter = Builders<User>.Filter.Eq(s => s.Email, user.Email);
                await _dbClient.GetUserCollection().ReplaceOneAsync(filter, user);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public Task<User> LoginUser(UserLoginDAO user)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Logout(User user)
        {
            user.IsActive = false;
            user.LastSeen = DateTime.Now;
            try
            {
                var filter = Builders<User>.Filter.Eq(s => s.Email, user.Email);
                await _dbClient.GetUserCollection().ReplaceOneAsync(filter, user);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public async Task<bool> RegisterUser(User user)
        {
            try
            {
                await _dbClient.GetUserCollection().InsertOneAsync(user);
                return true;
            }
            catch (Exception)
            {

                throw new Exception("Db connection failed");
            }
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var filter = Builders<User>.Filter.Eq(s => s.Email, email);
            return await _dbClient.GetUserCollection().Find(filter).FirstOrDefaultAsync();
        }

        public async Task<bool> ForgotPassword(User user)
        {
            try
            {
                var filter = Builders<User>.Filter.Eq(s => s.Email,user.Email);
                 await _dbClient.GetUserCollection().ReplaceOneAsync(filter, user);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _dbClient.GetUserCollection().Find(_ => true).ToListAsync();
        }

        public async Task<User> GetUserById(string id)
        {
            var filter = Builders<User>.Filter.Eq(s => s.Id, id);
            return await _dbClient.GetUserCollection().Find(filter).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByUserName(string username)
        {
            var filter = Builders<User>.Filter.Eq(s => s.UserName, username);
            return await _dbClient.GetUserCollection().Find(filter).FirstOrDefaultAsync();
        }

        public async Task<bool> CheckEmailAndUserNameExists(string userName, string email)
        {
            var filter = Builders<User>.Filter.Where(s=>s.Email==email) | Builders<User>.Filter.Where(s => s.UserName == userName);
            return await _dbClient.GetUserCollection().Find(filter).CountDocumentsAsync()==0;
        }

        public async Task<IEnumerable<User>> GetSearchedUser(string userName)
        {
         
            var searchedUsers = await _dbClient.GetUserCollection().Find(_=>true).ToListAsync();
            return searchedUsers.Where(s => s.UserName.ToLower().Contains(userName.ToLower())).ToList();
        }
    }
}
