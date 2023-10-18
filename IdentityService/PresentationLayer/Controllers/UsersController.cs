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
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DriverReadDto>>> GetAllUsersAsync()
        {
            var users = await _userService.GetAllUsersAsync();
            
            return Ok(users);
        }

        [HttpGet("{id}", Name = "GetUserByIdAsync")]
        public async Task<ActionResult<DriverReadDto>> GetUserByIdAsync(string id)
        {
            var userReadDto = await _userService.GetUserByIdAsync(id);

            return Ok(userReadDto);
        }

        [Authorize(Roles = "Client,Driver")]
        [HttpDelete]
        public async Task<ActionResult<DriverReadDto>> DeleteClientAsync()
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userReadDto = await _userService.DeleteUserAsync(id);

            return Ok(userReadDto);
        }
    }
}