using BAL.Authentication;
using BAL.Dtos;
using BAL.Services;
using BAL.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Claims;

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
        private readonly TokenService _tokenService;

        public AuthorizationController(ClientsService clientsService, DriversService driversService, ClientCreateDtoValidator clientValidator, DriverCreateDtoValidator driverValidator, TokenService tokenService)
        {
            _clientsService = clientsService;
            _driversService = driversService;
            _clientValidator = clientValidator;
            _driverValidator = driverValidator;
            _tokenService = tokenService;
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

            var refreshToken = userTokenDto.RefreshToken;

            var cookieOptions = _tokenService.GetCookieOptionsForRefreshToken();

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);

            return CreatedAtRoute(nameof(ClientsController.GetClientByIdAsync), new { Id = userTokenDto.Id }, userTokenDto);
        }

        [HttpPost("signin/client")]
        public async Task<ActionResult> SignInClientAsync(UserSignInDto clientSignInDto)
        {
            var userTokenDto = await _clientsService.SignInClientAsync(clientSignInDto);

            var refreshToken = userTokenDto.RefreshToken;

            var cookieOptions = _tokenService.GetCookieOptionsForRefreshToken();

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);

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

            var refreshToken = userTokenDto.RefreshToken;

            var cookieOptions = _tokenService.GetCookieOptionsForRefreshToken();

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);

            return CreatedAtRoute(nameof(DriversController.GetDriverByIdAsync), new { Id = userTokenDto.Id }, userTokenDto);
        }

        [HttpPost("signin/driver")]
        public async Task<ActionResult<UserTokenDto>> SignInDtiverAsync(UserSignInDto driverSignInDto)
        {
            var userTokenDto = await _driversService.SignInDriverAsync(driverSignInDto);

            var refreshToken = userTokenDto.RefreshToken;

            var cookieOptions = _tokenService.GetCookieOptionsForRefreshToken();


            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);

            return Ok(userTokenDto);
        }

        [Authorize]
        [HttpPost("client/logout")]
        public async Task<ActionResult> LogoutAsync()
        {
            Response.Cookies.Delete("refreshToken");
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _clientsService.LogoutAsync(id);

            return Ok();
        }

        [HttpGet("client/refresh")]
        public async Task<ActionResult<UserTokenDto>> RefreshTokenAsync()
        {
            string refreshToken = Request.Cookies["refreshToken"];
            if (refreshToken == null) { return Unauthorized(); }

            var userTokenDto = await _clientsService.RefreshAccessTokenAsync(refreshToken);

            return Ok(userTokenDto);
        }
    }
}