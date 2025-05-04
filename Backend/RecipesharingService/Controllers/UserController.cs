using Microsoft.AspNetCore.Mvc;
using RecipesharingService.Models;
using RecipesharingService.Services;

namespace RecipesharingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _service;

        public UserController(UserService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _service.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            await _service.AddUserAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] User user)
        {
            if (id != user.Id)
            {
                return BadRequest("User ID mismatch.");
            }

            await _service.UpdateUserAsync(user);
            return NoContent();
        }
    }

}
