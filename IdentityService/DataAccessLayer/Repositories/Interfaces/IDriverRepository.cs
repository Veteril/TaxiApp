using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IDriverRepository
    {
        bool SaveChanges();

        void DeleteDriver(Driver driver);

        Task<IEnumerable<Driver>> GetAllDriversAsync();

        Task<Driver> GetDriverByIdAsync(string id);

        Task CreateDriverAsync(Driver driver);
        
        Task<Driver> GetDriverByUsernameAsync(string username);

        Task<bool> IsPhoneUniqueAsync(string phone, CancellationToken cancellationToken);

        Task<bool> IsUsernameUniqueAsync(string username, CancellationToken cancellationToken);
    }
}
