using DAL.Models;

namespace DAL.Repositories
{
    public interface IClientRepository
    {
        bool SaveChanges();

        Task<IEnumerable<Client>> GetAllClientsAsync();

        Task<Client> GetClientByIdAsync(int id);

        Task CreateClientAsync(Client client);

        Task<Client> GetClientByUsernameAsync(string username);
    }
}