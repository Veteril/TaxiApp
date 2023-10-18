using BAL.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services
{
    public interface IUserService
    {
        Task<IEnumerable<DriverReadDto>> GetAllUsersAsync();

        Task<DriverReadDto> GetUserByIdAsync(string id);

        Task<ClientReadDto> GetUserByUsernameAsync(string username);

        Task<UserTokenDto> SignInUserAsync(UserSignInDto clientSignInDto);

        Task<UserTokenDto> CreateClientAsync(ClientCreateDto clientCreateDto);

        Task<UserTokenDto> CreateDriverAsync(DriverCreateDto driverCreateDto);

        Task<DriverReadDto> DeleteUserAsync(string id);

        Task LogoutAsync(string id);

        Task<UserTokenDto> RefreshAccessTokenAsync(string refreshToken);
    }
}
