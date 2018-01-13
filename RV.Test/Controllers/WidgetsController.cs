using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RV.Test.Infra.Repositories;
using RV.Test.Web.Models;
using System.Threading.Tasks;

namespace RV.Test.Web.Controllers
{
    [Authorize(Policy = "SystemAdmin")]
    [Route("[controller]")]
    public class WidgetsController : ControllerWithRepository<Widget>
    {
        public WidgetsController(IRepository<Widget> repository) : base(repository)
        {
        }

        /// <summary>
        /// Get all Widgets from database
        /// </summary>
        /// <returns>
        /// A list of widgets
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var widgets = await _repository.GetAllAsync();
            return Ok(widgets);
        }

        /// <summary>
        /// Try Get a specified widget
        /// </summary>
        /// <returns>
        /// 200 with widget, 404 if widget doesn't exist.
        /// </returns>
        /// <param name="id">Integer that represents an Widget id</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var widgets = await _repository.GetByIdAsync(id);

            if (widgets == null)
                return NotFound();

            return Ok(widgets);
        }

        /// <summary>
        /// Try to create a new widget.
        /// </summary>
        /// <returns>
        /// 200 if created, 422 if required information was not set.
        /// </returns>
        /// <param name="widget">Object that represents and Widget</param>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Widget widget)
        {
            if (!ModelState.IsValid)
                return StatusCode(422);

            await _repository.InsertAsync(widget);
            await _repository.SaveAsync();

            return Ok();
        }

        /// <summary>
        /// Try to update a existent widget.
        /// </summary>
        /// <returns>
        /// 200 if updated, 422 if required information was not set, 400 if widget not exists.
        /// </returns>
        /// <param name="id">Integer that represents an Widget id</param>
        /// <param name="widget">Object that represents and Widget</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]Widget widget)
        {
            if (!ModelState.IsValid)
                return StatusCode(422);

            var oldWidget = await _repository.GetByIdAsync(id);

            if (oldWidget == null)
                return BadRequest();

            UpdateWidgetProperties(widget, oldWidget);

            _repository.Update(oldWidget);
            await _repository.SaveAsync();

            return Ok();
        }

        private static void UpdateWidgetProperties(Widget widget, Widget oldWidget)
        {
            oldWidget.Color = widget.Color;
            oldWidget.Inventory = widget.Inventory;
            oldWidget.Melts = widget.Melts;
            oldWidget.Name = widget.Name;
            oldWidget.Price = widget.Price;
        }
    }
}

