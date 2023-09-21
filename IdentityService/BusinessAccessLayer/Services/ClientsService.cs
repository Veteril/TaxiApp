using AutoMapper;
using BAL.Dtos;
using DAL.Data;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services
{
    public class ClientsService
    {
        private readonly TokenService _tokenService;

        private readonly IMapper _mapper;
        
        private readonly IClientRepository _clientRepo;

        public ClientsService(IMapper mapper, IClientRepository clientRepo, TokenService tokenService)
        {
            _mapper = mapper;

            _clientRepo = clientRepo;
            _tokenService = tokenService;
        }
        public async Task<IEnumerable<ClientReadDto>> GetAllClientsAsync()
        {
            var clients  = await _clientRepo.GetAllClientsAsync();
            
            var clientReadDtos = _mapper.Map<List<ClientReadDto>>(clients);

            return clientReadDtos;
        }

        public async Task<ClientReadDto> GetClientByIdAsync(int id)
        {
            var client = await _clientRepo.GetClientByIdAsync(id);
            
            var clientDto = _mapper.Map<ClientReadDto>(client);
            
            return clientDto;
        }

        public async Task<ClientReadDto> GetClientByUsernameAsync(string username)
        {
            var client = await _clientRepo.GetClientByUsernameAsync(username);

            var clientDto = _mapper.Map<ClientReadDto>(client);

            return clientDto;
        }

        public async Task<UserTokenDto> CreateClientAsync(ClientCreateDto clientCreateDto)
        {
            var client = _mapper.Map<Client>(clientCreateDto);

            var hash = _tokenService.CreateHash(clientCreateDto.Password);
            client.PasswordHash = hash[0];
            client.PasswordSalt = hash[1];
            
            client.Id = Guid.NewGuid();

            var token = _tokenService.GetToken(client.Username, client.Id);

            var userTokenDto = _mapper.Map<UserTokenDto>(client);
            userTokenDto.Token = token;

            await _clientRepo.CreateClientAsync(client);

            _clientRepo.SaveChanges();
                
            return userTokenDto;
        }

        public async Task<UserTokenDto> SignInClientAsync(UserSignInDto clientSignInDto)
        {
            var client = await _clientRepo.GetClientByUsernameAsync(clientSignInDto.Username);

            var passwordHash = _tokenService.ComputeHash(clientSignInDto.Password, client.PasswordSalt);
            if (passwordHash != client.PasswordHash) 
            {
                throw new ArgumentException();
            }
            else
            {
                var userTokenDto = _mapper.Map<UserTokenDto>(client);
                var token = _tokenService.GetToken(client.Username, client.Id);
                userTokenDto.Token = token;

                return userTokenDto;
            }
        }
    }
}
