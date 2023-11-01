using DAL.Data;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _dbContext;

        public UserRepository(UserDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task CreateDriverInfoAsync(DriverInfo driverInfo)
        {
            await _dbContext.DriversInfo.AddAsync(driverInfo);
        }

        public async Task CreateUserAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
        }

        public void DeleteUser(User user)
        {
            _dbContext.Users.Remove(user);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _dbContext.Users
                .Include(u => u.DriverInfo)
                .Include(u => u.UserRatings)
                .ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            return await _dbContext.Users.Include(u => u.DriverInfo)
                .Include(u => u.UserRatings)
                .FirstOrDefaultAsync(u => u.Id.ToString() == id);
        }

        public async Task<User> GetUserByRefreshTokenAsync(string refreshToken)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(c => c.RefreshToken == refreshToken);
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _dbContext.Users
                .Include(c => c.UserRatings)
                .FirstOrDefaultAsync(c => c.Username == username);
        }

        public async Task CreateUserRatingAsync(UserRating userRating)
        {
            await _dbContext.UserRatings.AddAsync(userRating);
        }

        public async Task<bool> IsPhoneUniqueAsync(string phone, CancellationToken cancellationToken)
        {
            return !await _dbContext.Users.AnyAsync(c => c.Phone == phone);
        }

        public async Task<bool> IsUsernameUniqueAsync(string username, CancellationToken cancellationToken)
        {
            return !await _dbContext.Users.AnyAsync(c => c.Username == username);
        }

        public bool SaveChanges()
        {
            return (_dbContext.SaveChanges() >= 0);
        }
    }
}