using AutoMapper;
using BAL.Dtos;
using DAL.Models;
using DAL.Repositories;
using DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services
{
    public class DriversService
    {
        private readonly IMapper _mapper;

        private readonly IDriverRepository _driverRepo;

        public DriversService(IMapper mapper, IDriverRepository driverRepo)
        {
            _mapper = mapper;

            _driverRepo = driverRepo;
        }
        public async Task<IEnumerable<DriverReadDto>> GetAllDriversAsync()
        {
            var drivers = await _driverRepo.GetAllDriversAsync();
            
            var driversReadDtos = _mapper.Map<List<DriverReadDto>>(drivers);
            
            return driversReadDtos;
        }

        public async Task<DriverReadDto> GetDriverByIdAsync(int id)
        {
            var driver = await _driverRepo.GetDriverByIdAsync(id);

            var driverDto = _mapper.Map<DriverReadDto>(driver);

            return driverDto;
        }

        public async Task<DriverReadDto> CreateDriverAsync(DriverCreateDto driverCreateDto)
        {
            var driver = _mapper.Map<Driver>(driverCreateDto);

            await _driverRepo.CreateDriverAsync(driver);
            
            _driverRepo.SaveChanges();
            
            var driverReadDto = _mapper.Map<DriverReadDto>(driver);
               
            return driverReadDto;
        }
    }
}
