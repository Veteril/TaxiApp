using DataAccessLayer.Data;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    public class ClientRepo : IClientRepo
    {
        private readonly UserDbContext _dbContext;

        public ClientRepo(UserDbContext dbContext) 
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
                .Include(c => c.UserRatings)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public bool SaveChanges()
        {
            return (_dbContext.SaveChanges() >= 0);
        }
    }
}
