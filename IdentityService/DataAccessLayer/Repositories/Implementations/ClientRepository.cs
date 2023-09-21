using DAL.Data;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IEnumerable<Client>> GetAllClientsAsync()
        {
            return await _dbContext.Clients.ToListAsync();
        }

        public async Task<Client> GetClientByIdAsync(int id)
        {
            return await _dbContext.Clients
                .Include(c => c.ClientRatings)
                .FirstOrDefaultAsync(c => c.Id.ToString() == id.ToString());
        }

        public async Task<Client> GetClientByUsernameAsync(string username)
        {
            return await _dbContext.Clients
                .Include(c => c.ClientRatings)
                .FirstOrDefaultAsync(c => c.Username == username);
        }

        public bool SaveChanges()
        {
            return (_dbContext.SaveChanges() >= 0);
        }
    }
}