using BAL.Dtos;
using BAL.Services;
using BAL.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly ClientCreateDtoValidator _clientValidator;
        private readonly DriverCreateDtoValidator _driverValidator;
        private readonly ClientsService _clientsService;
        private readonly DriversService _driversService;

        public AuthorizationController(ClientsService clientsService, DriversService driversService, ClientCreateDtoValidator clientValidator, DriverCreateDtoValidator driverValidator)
        {
            _clientsService = clientsService;
            _driversService = driversService;
            _clientValidator = clientValidator;
            _driverValidator = driverValidator;
        }

        [HttpPost("signup/client")]
        public async Task<ActionResult<UserTokenDto>> RegisterClientAsync(ClientCreateDto clientCreateDto)
        {
            var validationResult = await _clientValidator.ValidateAsync(clientCreateDto);
            if (!validationResult.IsValid)
            {
                var modelStateDictionary = new ModelStateDictionary();
                foreach (var item in validationResult.Errors)
                {
                    modelStateDictionary.AddModelError(
                        item.PropertyName,
                        item.ErrorMessage
                        );
                    return ValidationProblem(modelStateDictionary);
                }
            }
            var userTokenDto = await _clientsService.CreateClientAsync(clientCreateDto);

            return CreatedAtRoute(nameof(ClientsController.GetClientByIdAsync), new { Id = userTokenDto.Id }, userTokenDto);
        }

        [HttpPost("signin/client")]
        public async Task<ActionResult> SignInClientAsync(UserSignInDto clientSignInDto)
        {
            var userTokenDto = await _clientsService.SignInClientAsync(clientSignInDto);

            return Ok(userTokenDto);
        }

        [HttpPost("signup/driver")]
        public async Task<ActionResult<UserTokenDto>> RegisterDriverAsync(DriverCreateDto driverCreateDto)
        {
            var validationResult = await _driverValidator.ValidateAsync(driverCreateDto);
            if (!validationResult.IsValid)
            {
                var modelStateDictionary = new ModelStateDictionary();
                foreach (var item in validationResult.Errors)
                {
                    modelStateDictionary.AddModelError(
                        item.PropertyName,
                        item.ErrorMessage
                        );
                    return ValidationProblem(modelStateDictionary);
                }
            }

            var userTokenDto = await _driversService.CreateDriverAsync(driverCreateDto);

            return CreatedAtRoute(nameof(DriversController.GetDriverByIdAsync), new { Id = userTokenDto.Id }, userTokenDto);
        }

        [HttpPost("signin/driver")]
        public async Task<ActionResult<UserTokenDto>> SignInDtiverAsync(UserSignInDto driverSignInDto)
        {
            var userTokenDto = await _driversService.SignInDriverAsync(driverSignInDto);

            return Ok(userTokenDto);
        }
    }
}
