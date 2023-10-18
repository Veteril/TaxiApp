using AutoMapper;
using BAL.Dtos;
using BAL.Exceptions;
using DAL.Models;
using DAL.Repositories;

namespace BAL.Services
{
    public class UserService : IUserService
    {
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IHashService _hashService;

        public UserService(IMapper mapper, IUserRepository clientRepo, ITokenService tokenService, IHashService hashService)
        {
            _mapper = mapper;
            _userRepository = clientRepo;
            _tokenService = tokenService;
            _hashService = hashService;
        }
        public async Task<IEnumerable<DriverReadDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();

            var userDtos = _mapper.Map<List<DriverReadDto>>(users);

            return userDtos;
        }

        public async Task<DriverReadDto> GetUserByIdAsync(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null)
            {
                throw new NotFoundException($"Can`t find user with such id: {id}");
            }

            var userDto = _mapper.Map<DriverReadDto>(user);

            return userDto;
        }

        public async Task<ClientReadDto> GetUserByUsernameAsync(string username)
        {
            var client = await _userRepository.GetUserByUsernameAsync(username);

            var clientDto = _mapper.Map<ClientReadDto>(client);

            return clientDto;
        }

        public async Task<UserTokenDto> CreateDriverAsync(DriverCreateDto driverCreateDto)
        {
            var driver = _mapper.Map<User>(driverCreateDto);

            var hash = _hashService.CreateHash(driverCreateDto.Password);
            driver.PasswordHash = hash[0];
            driver.PasswordSalt = hash[1];

            driver.Id = Guid.NewGuid();
            driver.Role = "Driver";

            var driverInfo = new DriverInfo
            {
                Experience = driverCreateDto.Experience,
                Name = driverCreateDto.Name,
                Id = Guid.NewGuid(),
                UserId = driver.Id
            };
            await _userRepository.CreateDriverInfoAsync(driverInfo);

            driver.DriverInfoId = driverInfo.Id;

            var token = _tokenService.GetToken(driver.Username, driver.Id, driver.Role);
            var refreshToken = _tokenService.GetRefreshToken(driver.Id);
            driver.RefreshToken = refreshToken;

            var userTokenDto = _mapper.Map<UserTokenDto>(driver);
            userTokenDto.Token = token;
            userTokenDto.RefreshToken = refreshToken;

            await _userRepository.CreateUserAsync(driver);

            _userRepository.SaveChanges();

            return userTokenDto;
        }

        public async Task<UserTokenDto> CreateClientAsync(ClientCreateDto clientCreateDto)
        {
            var client = _mapper.Map<User>(clientCreateDto);

            var hash = _hashService.CreateHash(clientCreateDto.Password);
            client.PasswordHash = hash[0];
            client.PasswordSalt = hash[1];

            client.Id = Guid.NewGuid();
            client.Role = "Client";

            var token = _tokenService.GetToken(client.Username, client.Id, client.Role);
           
            var refreshToken = _tokenService.GetRefreshToken(client.Id);
            client.RefreshToken = refreshToken;

            var userTokenDto = _mapper.Map<UserTokenDto>(client);
            userTokenDto.Token = token;
            userTokenDto.RefreshToken = refreshToken;

            await _userRepository.CreateUserAsync(client);

            _userRepository.SaveChanges();

            return userTokenDto;
        }

        public async Task<UserTokenDto> SignInUserAsync(UserSignInDto userSignInDto)
        {
            var user = await _userRepository.GetUserByUsernameAsync(userSignInDto.Username);

            var passwordHash = _hashService.ComputeHash(userSignInDto.Password, user.PasswordSalt);
            if (passwordHash != user.PasswordHash)
            {
                throw new ArgumentException();
            }
            var userTokenDto = _mapper.Map<UserTokenDto>(user);
            var token = _tokenService.GetToken(user.Username, user.Id, user.Role);
            var refreshToken = _tokenService.GetRefreshToken(user.Id);
            
            user.RefreshToken = refreshToken;
            _userRepository.SaveChanges();

            userTokenDto.RefreshToken = refreshToken;
            userTokenDto.Token = token;

            return userTokenDto;
        }

        public async Task<DriverReadDto> DeleteUserAsync(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null)
            {
                throw new NotFoundException($"Can`t find user with such id: {id}");
            }

            _userRepository.DeleteUser(user);
            _userRepository.SaveChanges();

            var userReadDto = _mapper.Map<DriverReadDto>(user);

            return userReadDto;
        }

        public async Task LogoutAsync(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                throw new NotFoundException($"Can`t find user with such id: {id}");
            }

            user.RefreshToken = null;
            _userRepository.SaveChanges();

            return;
        }

        public async Task<UserTokenDto> RefreshAccessTokenAsync(string refreshToken)
        {
            var user = await _userRepository.GetUserByRefreshTokenAsync(refreshToken);
            if (user == null)
            {
                throw new NotFoundException($"Invalid refresh token");
            }

            var userTokenDto = _mapper.Map<UserTokenDto>(user);
            var token = _tokenService.GetToken(user.Username, user.Id, user.Role);
            userTokenDto.Token = token;
            
            return userTokenDto;
        }
    }
}
