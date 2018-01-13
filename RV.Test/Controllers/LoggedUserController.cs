using RV.Test.Infra.Repositories;
using RV.Test.Web.Models;

namespace RV.Test.Web.Controllers
{
    public class LoggedUserController : ControllerWithRepository<Admin>
    {
        private Admin _admin;
        public LoggedUserController(IRepository<Admin> repository) : base(repository)
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
