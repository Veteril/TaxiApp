using BAL.Authentication;
using BAL.Dtos;
using BAL.Services;
using BAL.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
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
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AuthorizationController(IUserService userService,
            ClientCreateDtoValidator clientValidator,
            DriverCreateDtoValidator driverValidator,
            ITokenService tokenService)
        {
            _userService = userService;
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
                return ReturnValidationProblem(validationResult);
            }

            var userTokenDto = await _userService.CreateClientAsync(clientCreateDto);

            var refreshToken = userTokenDto.RefreshToken;

            var cookieOptions = _tokenService.GetCookieOptionsForRefreshToken();

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);

            return CreatedAtRoute(nameof(UsersController.GetUserByIdAsync), new { Id = userTokenDto.Id }, userTokenDto);
        }

        [HttpPost("signup/driver")]
        public async Task<ActionResult<UserTokenDto>> RegisterDriverAsync(DriverCreateDto driverCreateDto)
        {
            var validationResult = await _driverValidator.ValidateAsync(driverCreateDto);
            if (!validationResult.IsValid)
            {
                return ReturnValidationProblem(validationResult);
            }

            var userTokenDto = await _userService.CreateDriverAsync(driverCreateDto);

            var refreshToken = userTokenDto.RefreshToken;

            var cookieOptions = _tokenService.GetCookieOptionsForRefreshToken();

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);

            return CreatedAtRoute(nameof(UsersController.GetUserByIdAsync), new { Id = userTokenDto.Id }, userTokenDto);
        }

        [HttpPost("signin")]
        public async Task<ActionResult> SignInUserAsync(UserSignInDto clientSignInDto)
        {
            var userTokenDto = await _userService.SignInUserAsync(clientSignInDto);

            var refreshToken = userTokenDto.RefreshToken;

            var cookieOptions = _tokenService.GetCookieOptionsForRefreshToken();

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);

            return Ok(userTokenDto);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult> LogoutAsync()
        {
            Response.Cookies.Delete("refreshToken");
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _userService.LogoutAsync(id);

            return Ok();
        }

        [HttpGet("refresh")]
        public async Task<ActionResult<UserTokenDto>> RefreshTokenAsync()
        {
            string refreshToken = Request.Cookies["refreshToken"];
            if (refreshToken == null) { return Unauthorized(); }

            var userTokenDto = await _userService.RefreshAccessTokenAsync(refreshToken);

            return Ok(userTokenDto);
        }

        private ActionResult ReturnValidationProblem(FluentValidation.Results.ValidationResult validationResult)
        {
            var modelStateDictionary = new ModelStateDictionary();
            foreach (var item in validationResult.Errors)
            {
                modelStateDictionary.AddModelError(
                    item.PropertyName,
                    item.ErrorMessage
                    );
            }
            return ValidationProblem(modelStateDictionary);
        }
    }
}