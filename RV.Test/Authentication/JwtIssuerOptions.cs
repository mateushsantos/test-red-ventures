using Microsoft.IdentityModel.Tokens;
using System;
using System.Threading.Tasks;

namespace RV.Test.Web.Authentication
{
    public class JwtIssuerOptions
    {
        public string Issuer { get; set; }

        public string Subject { get; set; }

        public string Audience { get; set; }

        public int MinutesToExpire { get; set; } = 60;

        public DateTime NotBefore => DateTime.UtcNow;

        public DateTime IssuedAt => DateTime.UtcNow;

        public TimeSpan ValidFor
        {
            get
            {
                return TimeSpan.FromMinutes(MinutesToExpire);
            }
        }

        public DateTime Expiration => IssuedAt.Add(ValidFor);

        public Func<Task<string>> JtiGenerator =>
          () => Task.FromResult(Guid.NewGuid().ToString());

        public SigningCredentials SigningCredentials { get; set; }
    }
}
