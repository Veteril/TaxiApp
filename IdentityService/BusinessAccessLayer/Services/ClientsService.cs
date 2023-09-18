using AutoMapper;
using BAL.Dtos;
using DataAccessLayer.Data;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services
{
    public class ClientsService
    {
        private readonly IMapper _mapper;
        private readonly IClientRepo _clientRepo;
        public ClientsService(IMapper mapper, IClientRepo clientRepo)
        {
            _mapper = mapper;
            _clientRepo = clientRepo;
        }
        public async Task<IEnumerable<ClientReadDto>> GetAllClientsAsync()
        {
            var clients  = await _clientRepo.GetAllClientsAsync();
            List<ClientReadDto> clientReadDtos = new List<ClientReadDto>();
            foreach (var client in clients) 
            {
                var clientDto = _mapper.Map<ClientReadDto>(client);
                clientReadDtos.Add(clientDto);
            }
            return clientReadDtos;
        }

        public async Task<ClientReadDto> GetClientByIdAsync(int id)
        {
            var client = await _clientRepo.GetClientByIdAsync(id);
            var clientDto = _mapper.Map<ClientReadDto>(client);
            return clientDto;
        }

        public async Task<ClientReadDto> CreateClientAsync(ClientCreateDto clientCreateDto)
        {
            var client = _mapper.Map<Client>(clientCreateDto);
            await _clientRepo.CreateClientAsync(client);
            if (_clientRepo.SaveChanges())
            {
                var clientReadDto = _mapper.Map<ClientReadDto>(client);
                return clientReadDto;
            }
            else
            {
                return null;
            }

        }
    }
}
