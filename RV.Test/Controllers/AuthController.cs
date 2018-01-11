using RV.Test.Infra.Repositories;
using RV.Test.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RV.Test.Web.Controllers
{
    public class AuthController : ControllerWithRepository<Admin>
    {
        private Admin _admin;
        public AuthController(IRepository<Admin> repository) : base(repository)
        {
        }
        public Admin LoggedUser
        {
            get
            {
                if (_admin == null)
                    if (int.TryParse(User.Identity.Name, out int adminId))
                        _admin = _repository.GetByIdAsync(adminId).Result;

                return _admin;
            }
        }
    }
}
