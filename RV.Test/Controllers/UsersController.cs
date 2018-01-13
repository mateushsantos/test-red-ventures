using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RV.Test.Infra.Repositories;
using RV.Test.Web.Models;
using RV.Test.Web.Controllers;
using Microsoft.AspNetCore.Authorization;

namespace RV.Test.Controllers
{
    [Authorize(Policy = "SystemAdmin")]
    [Route("[controller]")]
    public class UsersController : ControllerWithRepository<User>
    {

        public UsersController(IRepository<User> repository) : base(repository)
        {
        }

        /// <summary>
        /// Get all Users from database
        /// </summary>
        /// <returns>
        /// A list of users
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _repository.GetAllAsync();
            return Ok(users);
        }


        /// <summary>
        /// Try Get a specified user
        /// </summary>
        /// <returns>
        /// 200 with user, 404 if user doesn't exist.
        /// </returns>
        /// <param name="id">Integer that represents and User id</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _repository.GetByIdAsync(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }


        /// <summary>
        /// Try to create a new user.
        /// </summary>
        /// <returns>
        /// 200 if created, 422 if required information was not set.
        /// </returns>
        /// <param name="user">Object that represents an User</param>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]User user)
        {
            if (!ModelState.IsValid)
                return StatusCode(422);

            await _repository.InsertAsync(user);
            await _repository.SaveAsync();

            return Ok();
        }
    }
}
