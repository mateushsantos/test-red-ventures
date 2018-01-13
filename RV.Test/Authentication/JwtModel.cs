using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RV.Test.Web.Authentication
{
    public class JwtModel
    {
        public string Token { get; set; }
        public int ExpiresIn { get; set; }
    }
}
