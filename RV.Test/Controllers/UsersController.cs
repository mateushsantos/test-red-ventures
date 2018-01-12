using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RV.Test.Infra.Repositories;
using RV.Test.Web.Models;
using RV.Test.Web.Controllers;

namespace RV.Test.Controllers
{
    [Route("[controller]")]
    public class UsersController : ControllerWithRepository<User>
    {

        public UsersController(IRepository<User> repository) : base(repository)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _repository.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _repository.GetByIdAsync(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

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
