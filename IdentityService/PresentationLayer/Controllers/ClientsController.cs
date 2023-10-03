using AutoMapper;
using BAL.Dtos;
using BAL.Services;
using BAL.Validators;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly ClientsService _clientService;
        
        public ClientsController(ClientsService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientReadDto>>> GetALlClientsAsync()
        {
            var clients = await _clientService.GetAllClientsAsync();
            
            return Ok(clients);
        }

        [HttpGet("{id}", Name = "GetClientByIdAsync")]
        public async Task<ActionResult<ClientReadDto>> GetClientByIdAsync(string id)
        {
            var clientReadDto = await _clientService.GetClientByIdAsync(id);

            return Ok(clientReadDto);
        }

        [Authorize(Roles = "Client")]
        [HttpDelete]
        public async Task<ActionResult<ClientReadDto>> DeleteClientAsync()
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var clientReadDto = await _clientService.DeleteClientAsync(id);

            return Ok(clientReadDto);
        }
    }
}