using DAL.Models;

namespace DAL.Repositories
{
    public interface IUserRepository
    {
        bool SaveChanges();

        Task<IEnumerable<User>> GetAllUsersAsync();

        Task<User> GetUserByIdAsync(string id);

        Task CreateUserAsync(User client);

        Task CreateDriverInfoAsync(DriverInfo driverInfo);

        void DeleteUser(User client);

        Task<User> GetUserByUsernameAsync(string username);

        Task<bool> IsPhoneUniqueAsync(string phone, CancellationToken cancellationToken);
        
        Task<bool> IsUsernameUniqueAsync(string username, CancellationToken cancellationToken);

        Task<User> GetUserByRefreshTokenAsync(string refreshToken);
    }
} 