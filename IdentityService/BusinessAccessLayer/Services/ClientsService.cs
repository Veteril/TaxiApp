using AutoMapper;
using BAL.Dtos;
using BAL.Exceptions;
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
        private readonly HashService _hashService;

        public ClientsService(IMapper mapper, IClientRepository clientRepo, TokenService tokenService, HashService hashService)
        {
            _mapper = mapper;
            _clientRepo = clientRepo;
            _tokenService = tokenService;
            _hashService = hashService;
        }
        public async Task<IEnumerable<ClientReadDto>> GetAllClientsAsync()
        {
            var clients = await _clientRepo.GetAllClientsAsync();

            var clientReadDtos = _mapper.Map<List<ClientReadDto>>(clients);

            return clientReadDtos;
        }

        public async Task<ClientReadDto> GetClientByIdAsync(string id)
        {
            var client = await _clientRepo.GetClientByIdAsync(id);

            if (client == null)
            {
                throw new NotFoundException($"Can`t find client with such id: {id}");
            }

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

            var hash = _hashService.CreateHash(clientCreateDto.Password);
            client.PasswordHash = hash[0];
            client.PasswordSalt = hash[1];

            client.Id = Guid.NewGuid();

            var token = _tokenService.GetToken(client.Username, client.Id, "Client");
           
            var refreshToken = _tokenService.GetRefreshToken(client.Id);
            client.RefreshToken = refreshToken;

            var userTokenDto = _mapper.Map<UserTokenDto>(client);
            userTokenDto.Token = token;
            userTokenDto.RefreshToken = refreshToken;

            await _clientRepo.CreateClientAsync(client);

            _clientRepo.SaveChanges();

            return userTokenDto;
        }

        public async Task<UserTokenDto> SignInClientAsync(UserSignInDto clientSignInDto)
        {
            var client = await _clientRepo.GetClientByUsernameAsync(clientSignInDto.Username);

            var passwordHash = _hashService.ComputeHash(clientSignInDto.Password, client.PasswordSalt);
            if (passwordHash != client.PasswordHash)
            {
                throw new ArgumentException();
            }
            var userTokenDto = _mapper.Map<UserTokenDto>(client);
            var token = _tokenService.GetToken(client.Username, client.Id, "Client");
            var refreshToken = _tokenService.GetRefreshToken(client.Id);
            
            client.RefreshToken = refreshToken;
            _clientRepo.SaveChanges();

            userTokenDto.RefreshToken = refreshToken;
            userTokenDto.Token = token;

            return userTokenDto;
        }

        public async Task<ClientReadDto> DeleteClientAsync(string id)
        {
            var client = await _clientRepo.GetClientByIdAsync(id);

            if (client == null)
            {
                throw new NotFoundException($"Can`t find client with such id: {id}");
            }

            _clientRepo.DeleteClient(client);
            _clientRepo.SaveChanges();

            var clientReadDto = _mapper.Map<ClientReadDto>(client);

            return clientReadDto;
        }

        public async Task LogoutAsync(string id)
        {
            var client = await _clientRepo.GetClientByIdAsync(id);
            if (client == null)
            {
                throw new NotFoundException($"Can`t find client with such id: {id}");
            }

            client.RefreshToken = null;
            _clientRepo.SaveChanges();

            return;
        }

        public async Task<UserTokenDto> RefreshAccessTokenAsync(string refreshToken)
        {
            var client = await _clientRepo.GetClientByRefreshTokenAsync(refreshToken);
            if (client == null)
            {
                throw new NotFoundException($"Invalid refresh token");
            }

            var userTokenDto = _mapper.Map<UserTokenDto>(client);
            var token = _tokenService.GetToken(client.Username, client.Id, "Client");
            userTokenDto.Token = token;
            
            return userTokenDto;
        }
    }
}
