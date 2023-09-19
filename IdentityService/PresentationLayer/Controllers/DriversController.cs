using AutoMapper;
using BAL.Dtos;
using BAL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriversController : ControllerBase
    {
        private readonly DriversService _driverServ;
        
        private readonly IMapper _mapper;

        public DriversController(DriversService driverServ, IMapper mapper)
        {
            _driverServ = driverServ;
            
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DriverReadDto>>> GetALlDriversAsync()
        {
            var drivers = await _driverServ.GetAllDriversAsync();
           
            return Ok(drivers);
        }

        [HttpGet("{id}", Name = "GetDriverByIdAsync")]
        public async Task<ActionResult<DriverReadDto>> GetDriverByIdAsync(int id)
        {
            var driverReadDto = await _driverServ.GetDriverByIdAsync(id);
            
            if (driverReadDto == null)
            {
                return NotFound();
            }

            return Ok(driverReadDto);
        }

        [HttpPost]
        public async Task<ActionResult> CreateDriverAsync(DriverCreateDto driverCreateDto)
        {
            var driverReadDto = await _driverServ.CreateDriverAsync(driverCreateDto);
           
            return CreatedAtRoute(nameof(GetDriverByIdAsync), new { Id = driverReadDto.Id }, driverReadDto);
        }
    }
}
