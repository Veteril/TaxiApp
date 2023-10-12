using DAL.Models;

namespace DAL.Repositories
{
    public interface IClientRepository
    {
        bool SaveChanges();

        Task<IEnumerable<Client>> GetAllClientsAsync();

        Task<Client> GetClientByIdAsync(string id);

        Task CreateClientAsync(Client client);

        void DeleteClient(Client client);

        Task<Client> GetClientByUsernameAsync(string username);

        Task<bool> IsPhoneUniqueAsync(string phone, CancellationToken cancellationToken);
        
        Task<bool> IsUsernameUniqueAsync(string username, CancellationToken cancellationToken);
    }
} 