using AutoMapper;
using BAL.Dtos;
using BAL.Services;
using DAL.Models;
using DAL.Repositories;
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

        public ClientsController(ClientsService clientServ, IMapper mapper)
        {
            _clientServ = clientServ;
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
            var clientReadDto = await _clientServ.GetClientByIdAsync(id);
            
            if(clientReadDto == null)
            {
                return NotFound();
            }

            return Ok(clientReadDto);
        }
    }
}
