using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using Store.Maro.Core.Entities.Identity;
using Store.Maro.Core.Services.Contract;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Sockets;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.IdentityModel.Tokens;

namespace Store.Maro.Services.Services.TokenServices
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> _userManager)
        {
            #region Token Explanation
            // 1. header 
            ///The header typically consists of two parts:
            ///     The type of the token, which is JWT 
            ///     The signing algorithm, such as HS256 (HMAC SHA256)

            // 2. payload , The payload contains the claims.
            /// Claims  informations about the user and token metadata
            /// 1. Identify the User => helps your server know who is making the request (e.g admin , normal user ...)
            /// 2. Authorize Access => You can include roles(admin,...) or permissions (read, write)
            /// 3. Control Token Expiry and Timing 
            /// iss	Issuer → Who created the token (e.g., your app)
            /// sub Subject → Whom the token refers to(user ID)
            /// aud Audience → Who the token is for (e.g., service name) exp Expiration time(Unix timestamp)
            /// nbf Not before → Token is not valid before this time
            /// iat Issued at → When the token was created
            /// jti JWT ID → Unique ID for the token

            // 3. signature To prevent tampering
            /// Its purpose is to ensure the token hasn’t been changed and to prove it came from a trusted source.
            /// it has Header → Info about the algorithm (e.g., HS256)
            /// Payload → Claims(user data, token metadata)
            /// Signature → A hashed string created from the header +payload + secret key
            // what happens step-by-step during a request using a JWT 
            ///1.User sends a login request:
            ///2.Server checks credentials in the database.
            ///3.If valid, the server generates a JWT
            ///4.Server returns the token to the client:  "token": "<header>.<payload>.<signature>"
            //Now, the user wants to access a protected API: GET /api/profile
            ///When the request reaches the server:
            ///The server reads the token from the header.
            ///It decodes the header and payload.
            ///It re-generates the signature using the secret key and the received header + payload.
            ///It compares its generated signature with the one in the token. 
            #endregion

            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email , user.Email),
                new Claim(ClaimTypes.MobilePhone , user.PhoneNumber),
                new Claim(ClaimTypes.GivenName , user.DisplayName),
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var item in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, item));
            }

            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"], 
                audience: _configuration["JWT:Audience"],
                expires: DateTime.Now.AddDays(double.Parse(_configuration["JWT:DurationInDays"])),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
