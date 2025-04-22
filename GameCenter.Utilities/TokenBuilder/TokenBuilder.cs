using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace GameCenter.Utilities.TokenBuilder
{
    public class TokenBuilder : ITokenBuilderConfigurated
    {
        public TokenBuilder(string key, int expirationTime = 4)
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            Credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            Expiration = DateTime.UtcNow.AddHours(expirationTime);
            Claims = new List<Claim>() {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            Handler = new JwtSecurityTokenHandler();
        }

        public SigningCredentials Credentials { get; protected set; }
        public List<Claim> Claims { get; protected set; }
        public DateTime Expiration { get; protected set; }
        public JwtSecurityTokenHandler Handler { get; protected set; }

        public static ITokenBuilderConfigurated Factory(string key, int expirationTime = 4)
        {
            return new TokenBuilder(key, expirationTime);
        }
        public ITokenBuilderConfigurated AddInformation(params (string key, string value)[] informations)
        {
            Claims.AddRange(informations.Select(information =>
            {
                return new Claim(information.key, information.value);
            }));
            return this;
        }
        public ITokenBuilderConfigurated AddInformation(string key, string value)
        {
            AddInformation((key, value));
            return this;
        }
        public ITokenBuilderConfigurated AddInformation(params Claim[] informations)
        {
            Claims.AddRange(informations);
            return this;
        }
        public string Build()
        {
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(Claims),
                Expires = Expiration,
                SigningCredentials = Credentials,

            };
            SecurityToken token = Handler.CreateToken(tokenDescriptor);

            return Handler.WriteToken(token);
        }
        public JwtSecurityToken Read(string token)
        {
            return Handler.ReadJwtToken(token);
        }
    };

}
