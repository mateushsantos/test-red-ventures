using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RV.Test.Infra.Repositories;
using RV.Test.Web.Models;
using System.Threading.Tasks;

namespace RV.Test.Web.Controllers
{
    [Authorize(Policy = "SystemAdmin")]
    [Route("[controller]")]
    public class WidgetController : ControllerWithRepository<Widget>
    {
        public WidgetController(IRepository<Widget> repository) : base(repository)
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
        public async Task<IActionResult> Post([FromBody]Widget widget)
        {
            if (!ModelState.IsValid)
                return StatusCode(422);

            await _repository.InsertAsync(widget);
            await _repository.SaveAsync();

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]Widget widget)
        {
            if (!ModelState.IsValid)
                return StatusCode(422);

            var oldWidget = await _repository.GetByIdAsync(id);

            if (oldWidget == null)
                return BadRequest();

            widget.Id = id;

            _repository.Update(widget);
            await _repository.SaveAsync();

            return Ok();
        }
    }
}

