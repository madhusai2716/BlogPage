using BloggingCode.API.Repo.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Elfie.Extensions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BloggingCode.API.Repo.Implementation
{
    public class TokenRepo : ITokenRepo
    {
        private readonly IConfiguration configuration;
        public TokenRepo(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string CreateJwtToken(IdentityUser user, List<string> roles)
        {
            var claims = new List<Claim>
            {
               new Claim(ClaimTypes.Email, user.Email)

            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            //create jwt securit token
            //to create a token we need the security key kept in appsettings .json
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

            //hmacsha 256 is a an algorithm

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //by using jwt package we are generating a token which can give you access upto 15 minutes 
            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:issuer"],
                audience: configuration["jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token); 
        }

    }
}
