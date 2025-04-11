using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MedicineStoreAPI.Models;
using MedicineStoreAPI.Data;
using System.Linq;

namespace MedicineStoreAPI.Services
{
    public class AuthService
    {
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _dbContext;

        public AuthService(IConfiguration config, ApplicationDbContext dbContext)
        {
            _config = config;
            _dbContext = dbContext;
        }

        // Register user (store in DB)
        public bool RegisterUser(string username, string password)
        {
            if (_dbContext.Users.Any(u => u.Username == username))
                return false;

            var user = new User
            {
                Username = username,
                Password = password
            };

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return true;
        }

        // Validate credentials from DB
        public bool ValidateUser(string username, string password)
        {
            return _dbContext.Users.Any(u => u.Username == username && u.Password == password);
        }

        // Generate JWT token
        public string GenerateJwtToken(string username)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username)
            };

            var keyString = _config["Jwt:Key"];
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];

            if (string.IsNullOrEmpty(keyString))
                throw new Exception("JWT Key is missing in configuration.");
            if (string.IsNullOrEmpty(issuer))
                throw new Exception("JWT Issuer is missing.");
            if (string.IsNullOrEmpty(audience))
                throw new Exception("JWT Audience is missing.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

