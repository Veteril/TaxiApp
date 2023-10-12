using AutoMapper;
using BAL.Dtos;
using BAL.Exceptions;
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
        private readonly TokenService _tokenService;
        private readonly IDriverRepository _driverRepo;
        private readonly HashService _hashService;

        public DriversService(IMapper mapper, IDriverRepository driverRepo, TokenService tokenService, HashService hashService)
        {
            _mapper = mapper;
            _driverRepo = driverRepo;
            _tokenService = tokenService;
            _hashService = hashService;
        }

        public async Task<IEnumerable<DriverReadDto>> GetAllDriversAsync()
        {
            var drivers = await _driverRepo.GetAllDriversAsync();

            var driversReadDtos = _mapper.Map<List<DriverReadDto>>(drivers);

            return driversReadDtos;
        }

        public async Task<DriverReadDto> GetDriverByIdAsync(string id)
        {
            var driver = await _driverRepo.GetDriverByIdAsync(id);

            if (driver == null)
            {
                throw new NotFoundException($"Can`t find driver with such id: {id}");
            }

            var driverDto = _mapper.Map<DriverReadDto>(driver);

            return driverDto;
        }

        public async Task<UserTokenDto> CreateDriverAsync(DriverCreateDto driverCreateDto)
        {
            var driver = _mapper.Map<Driver>(driverCreateDto);

            var hash = _hashService.CreateHash(driverCreateDto.Password);
            driver.PasswordHash = hash[0];
            driver.PasswordSalt = hash[1];

            driver.Id = Guid.NewGuid();

            var token = _tokenService.GetToken(driver.Username, driver.Id, "Driver");

            var userTokenDto = _mapper.Map<UserTokenDto>(driver);
            userTokenDto.Token = token;

            await _driverRepo.CreateDriverAsync(driver);

            _driverRepo.SaveChanges();

            return userTokenDto;
        }

        public async Task<UserTokenDto> SignInDriverAsync(UserSignInDto driverSignInDto)
        {
            var driver = await _driverRepo.GetDriverByUsernameAsync(driverSignInDto.Username);

            var passwordHash = _hashService.ComputeHash(driverSignInDto.Password, driver.PasswordSalt);
            if (passwordHash != driver.PasswordHash)
            {
                throw new ArgumentException();
            }
            var userTokenDto = _mapper.Map<UserTokenDto>(driver);
            var token = _tokenService.GetToken(driver.Username, driver.Id, "Driver");
            userTokenDto.Token = token;

            return userTokenDto;
        }

        public async Task<DriverReadDto> DeleteDriverAsync(string id)
        {
            var driver = await _driverRepo.GetDriverByIdAsync(id);

            if (driver == null)
            {
                throw new NotFoundException($"Can`t find driver with such id: {id}");
            }

            _driverRepo.DeleteDriver(driver);
            _driverRepo.SaveChanges();

            var driverReadDto = _mapper.Map<DriverReadDto>(driver);
            return driverReadDto;
        }
    }
}
