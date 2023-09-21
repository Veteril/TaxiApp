﻿using DAL.Models;
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

        Task<IEnumerable<Driver>> GetAllDriversAsync();

        Task<Driver> GetDriverByIdAsync(int id);

        Task CreateDriverAsync(Driver driver);

        Task<Driver> GetDriverByUsernameAsync(string username);
    }
}
