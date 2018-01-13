using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RV.Test.Infra.Repositories;
using RV.Test.Web.Models;
using RV.Test.Web.Services;
using RV.Test.Web.ViewModels.Admin;
using System.Threading.Tasks;

namespace RV.Test.Web.Controllers
{
    [Route("[controller]")]
    public class AdminController : LoggedUserController
    {
        private JwtAuthenticationService _authService;

        public AdminController(IRepository<Admin> repository, JwtAuthenticationService authService) : base(repository)
        {
            _authService = authService;
        }

        /// <summary>
        /// Try to add a new Admin to the system.
        /// </summary>
        /// <returns>
        /// 200 if the admin was created successfully, 422 if some information was missing in the object in the request, 
        /// 400 if the username already exists.
        /// </returns>
        /// <param name="viewModel">Object to make new admin, with password confirmation.</param>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> SignUp([FromBody]SignUpViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return StatusCode(422);

            var adminWithSameUsername = await _repository.GetWhereAsync(x => x.Username == viewModel.Username);

            if (adminWithSameUsername.Count > 0)
                return BadRequest();

            var admin = new Admin()
            {
                Username = viewModel.Username,
                Password = viewModel.Password
            };

            await _repository.InsertAsync(admin);
            await _repository.SaveAsync();

            return Ok();
        }

        /// <summary>
        /// Try authenticate an Admin to the system.
        /// </summary>
        /// <returns>
        /// 200 with an object that represents a Jwt token and an Expiration time in minutes, 403 if admin doesn't exists.
        /// </returns>
        /// <param name="admin">Main object that represents and System Admin</param>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Authenticate([FromBody]Admin admin)
        {
            var jwt = await _authService.SignWithJwt(admin);
            if (jwt == null)
                return Forbid();

            return Ok(jwt);
        }

        //[HttpPut]
        //[Authorize(Policy = "SystemAdmin")]
        //public IActionResult Put([FromBody] Admin admin)
        //{
        //    if (admin.Id != LoggedUser.Id)
        //        return Forbid();

        //    _repository.Update(admin);
        //    return Ok();
        //}

        //[HttpGet]
        //[Authorize(Policy = "SystemAdmin")]
        //public IActionResult Get()
        //{
        //    return Ok(LoggedUser);
        //}
    }
}
