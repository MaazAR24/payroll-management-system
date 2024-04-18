using Microsoft.IdentityModel.Tokens;
using PayrollManagementSystem.Models;
using PayrollManagementSystem.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PayrollManagementSystem.Services.Authentication
{
    public class AuthService : IAuthService
    {
        private User _user;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }
        public async Task<bool> AuthenticateUserAsync(string username, string password)
        {
            _user = await _userRepository.GetUserAsync(username, password);
            if(_user is not null)
                return true;

            return false;
        }

        public string GenerateToken()
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var claims = new List<Claim>{
                new Claim(ClaimTypes.Name, _user.UserName),
                new Claim(ClaimTypes.Role, _user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: "https://localhost:44333",
                audience: "https://localhost:44333",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
