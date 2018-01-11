using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RV.Test.Infra.Repositories;
using RV.Test.Web.Authentication;
using RV.Test.Web.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace RV.Test.Web.Services
{
    public class JwtAuthenticationService
    {
        private IRepository<Admin> _repository;
        private JwtIssuerOptions _jwtOptions;

        public JwtAuthenticationService(IOptions<JwtIssuerOptions> jwtOptions, IRepository<Admin> repository)
        {
            _repository = repository;
            _jwtOptions = jwtOptions.Value;
        }


        public async Task<string> SignWithJwt(Admin admin)
        {
            var tuple = await GetClaimsIdentity(admin);

            if (tuple == null)
                return null;

            var identity = tuple.Item1;
            var applicationUser = tuple.Item2;

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, admin.Username),
                new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                new Claim(JwtRegisteredClaimNames.NameId, applicationUser.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, applicationUser.Id.ToString()),
                identity.FindFirst("LoggedColiver")
            };

            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: _jwtOptions.NotBefore,
                expires: _jwtOptions.Expiration,
                signingCredentials: _jwtOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            admin.Password = "";
            
            var response = new
            {
                access_token = encodedJwt,
                expires_in = (int)_jwtOptions.ValidFor.TotalSeconds,
                user = applicationUser
            };

            var json = JsonConvert.SerializeObject(response);
            return json;
        }

        private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));
            }

            if (options.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
            }
        }

        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        private async Task<Tuple<ClaimsIdentity, Admin>> GetClaimsIdentity(Admin admin)
        {
            var existentUsers = await _repository.GetWhereAsync(x => x.Username == admin.Username && x.Password == admin.Password);
            var existentUser = existentUsers.ToList().FirstOrDefault();

            if (existentUser == null)
                return null;

            return new Tuple<ClaimsIdentity, Admin>(new ClaimsIdentity(new GenericIdentity(existentUser.Username, "Token"),
                new[]
                {
                    new Claim("LoggedSystemAdmin", "Admin")
                }), existentUser);
        }
    }
}
