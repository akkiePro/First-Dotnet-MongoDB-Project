using Microsoft.AspNetCore.Mvc;
using MongoDbDemo.Models;
using MongoDbDemo.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoDbDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly ItemService _itemService;

        public ItemsController(ItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Item>>> Get() =>
            await _itemService.GetAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> Get(string id)
        {
            var item = await _itemService.GetAsync(id);
            if (item == null) return NotFound();
            return item;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Item item)
        {
            await _itemService.CreateAsync(item);
            return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, Item item)
        {
            var existingItem = await _itemService.GetAsync(id);
            if (existingItem == null) return NotFound();

            item.Id = existingItem.Id;
            await _itemService.UpdateAsync(id, item);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existingItem = await _itemService.GetAsync(id);
            if (existingItem == null) return NotFound();

            await _itemService.RemoveAsync(id);
            return NoContent();
        }
    }
}
