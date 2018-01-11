using Microsoft.AspNetCore.Mvc;
using RV.Test.Infra.Repositories;
using RV.Test.Web.Models;
using RV.Test.Web.ViewModels.Admin;
using System.Threading.Tasks;

namespace RV.Test.Web.Controllers
{
    public class AdminController : ControllerWithRepository<Admin>
    {
        public AdminController(IRepository<Admin> repository) : base(repository)
        {
        }

        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody]SignUpViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return StatusCode(422);

            var adminWithSameUsername = await _repository.GetWhereAsync(x => x.Username == viewModel.Username);

            if (adminWithSameUsername != null)
                return BadRequest();

            var admin = new Admin()
            {
                Username = viewModel.Username,
                Password = viewModel.Password
            };

            await _repository.InsertAsync(admin);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody]Admin admin)
        {
            var loggedAdmin = await _repository.GetWhereAsync(x => x.Username == admin.Username && x.Password == admin.Password);

            if (loggedAdmin == null)
                return Forbid();

            return Ok();
        }
    }
}
