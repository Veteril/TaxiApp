using AutoMapper;
using BAL.Dtos;
using BAL.Services;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly ClientsService _clientServ;
        private readonly IMapper _mapper;

        public ClientsController(ClientsService clientServ, IMapper mapper)
        {
            _clientServ = clientServ;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientReadDto>>> GetALlClientsAsync()
        {
            var clients = await _clientServ.GetAllClientsAsync();
            return Ok(clients);
        }

        [HttpGet("{id}", Name = "GetClientByIdAsync")]
        public async Task<ActionResult<ClientReadDto>> GetClientByIdAsync(int id)
        {
            var client = await _clientServ.GetClientByIdAsync(id);
            return Ok(client);
        }

        [HttpPost]
        public async Task<ActionResult> CreateClientAsync(ClientCreateDto clientCreateDto)
        {
            var clientReadDto = await _clientServ.CreateClientAsync(clientCreateDto);
            if (clientReadDto != null)
            {
                return CreatedAtRoute(nameof(GetClientByIdAsync), new { Id = clientReadDto.Id }, clientReadDto);
            }
            else
            {
                return BadRequest();
            }

        }
    }
}
