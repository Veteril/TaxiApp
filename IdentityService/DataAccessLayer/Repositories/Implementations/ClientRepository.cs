using DAL.Data;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace DAL.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly UserDbContext _dbContext;

        public ClientRepository(UserDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task CreateClientAsync(Client client)
        {
            await _dbContext.Clients.AddAsync(client);
        }

        public void DeleteClient(Client client)
        {
            _dbContext.Clients.Remove(client);
        }

        public async Task<IEnumerable<Client>> GetAllClientsAsync()
        {
            return await _dbContext.Clients.ToListAsync();
        }

        public async Task<Client> GetClientByIdAsync(string id)
        {
            return await _dbContext.Clients
                .Include(c => c.ClientRatings)
                .FirstOrDefaultAsync(c => c.Id.ToString() == id.ToString());
        }

        public async Task<Client> GetClientByRefreshTokenAsync(string refreshToken)
        {
            return await _dbContext.Clients.FirstOrDefaultAsync(c => c.RefreshToken == refreshToken);
        }

        public async Task<Client> GetClientByUsernameAsync(string username)
        {
            return await _dbContext.Clients
                .Include(c => c.ClientRatings)
                .FirstOrDefaultAsync(c => c.Username == username);
        }

        public async Task<bool> IsPhoneUniqueAsync(string phone, CancellationToken cancellationToken)
        {
            return !await _dbContext.Clients.AnyAsync(c => c.Phone == phone);
        }

        public async Task<bool> IsUsernameUniqueAsync(string username, CancellationToken cancellationToken)
        {
            return !await _dbContext.Clients.AnyAsync(c => c.Username == username);
        }

        public bool SaveChanges()
        {
            return (_dbContext.SaveChanges() >= 0);
        }
    }
}