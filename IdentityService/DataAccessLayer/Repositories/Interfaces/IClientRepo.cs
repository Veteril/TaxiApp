using DataAccessLayer.Models;

namespace DataAccessLayer.Repositories
{
    public interface IClientRepo
    {
        bool SaveChanges();

        Task<IEnumerable<Client>> GetAllClientsAsync();

        Task<Client> GetClientByIdAsync(int id);

        Task CreateClientAsync(Client client);
    }
}
