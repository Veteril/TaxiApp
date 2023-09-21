using DAL.Data;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class DriverRepository : IDriverRepository
    {
        private readonly UserDbContext _dbContext;

        public DriverRepository(UserDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateDriverAsync(Driver driver)
        {
            await _dbContext.Drivers.AddAsync(driver);
        }

        public async Task<IEnumerable<Driver>> GetAllDriversAsync()
        {
            return await _dbContext.Drivers.ToListAsync();
        }

        public async Task<Driver> GetDriverByUsernameAsync(string username)
        {
            return await _dbContext.Drivers
                .Include(c => c.DriverRatings)
                .FirstOrDefaultAsync(c => c.Username == username);
        }

        public async Task<Driver> GetDriverByIdAsync(int id)
        {
            return await _dbContext.Drivers
                .Include(d => d.DriverRatings)
                .FirstOrDefaultAsync(d => d.Id.ToString() == id.ToString());
        }

        public bool SaveChanges()
        {
            return (_dbContext.SaveChanges() >= 0);
        }
    }
}