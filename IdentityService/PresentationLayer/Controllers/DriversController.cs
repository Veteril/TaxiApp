using AutoMapper;
using BAL.Dtos;
using BAL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriversController : ControllerBase
    {
        private readonly DriversService _driverService;
        
        private readonly IMapper _mapper;

        public DriversController(DriversService driverServ, IMapper mapper)
        {
            _driverService = driverServ;
            
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DriverReadDto>>> GetALlDriversAsync()
        {
            var drivers = await _driverService.GetAllDriversAsync();
           
            return Ok(drivers);
        }

        [HttpGet("{id}", Name = "GetDriverByIdAsync")]
        public async Task<ActionResult<DriverReadDto>> GetDriverByIdAsync(string id)
        {
            var driverReadDto = await _driverService.GetDriverByIdAsync(id);
            
            if (driverReadDto == null)
            {
                return NotFound();
            }

            return Ok(driverReadDto);
        }

        [Authorize(Roles = "Driver")]
        [HttpDelete]
        public async Task<ActionResult<ClientReadDto>> DeleteClientAsync()
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var driverReadDto = await _driverService.DeleteDriverAsync(id);

            return Ok(driverReadDto);
        }
    }
}