using Microsoft.AspNetCore.Mvc;
using RecipesharingService.Models;
using RecipesharingService.Services;
using System.Security.Claims;

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

        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var firebaseUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (firebaseUserId == null)
            {
                return Unauthorized("User not authenticated.");
            }

            var user = await _service.GetUserByGoogleIdAsync(firebaseUserId);
            if (user == null)
            {
                var email = User.FindFirst(ClaimTypes.Email)?.Value;
                var name = User.FindFirst("name")?.Value;
                var picture = User.FindFirst("picture")?.Value;

                user = new User
                {
                    GoogleId = firebaseUserId,
                    Email = email ?? string.Empty,
                    Name = name,
                    ProfilePictureUrl = picture,
                    Role = "User"
                };

                await _service.AddUserAsync(user);
            }

            return Ok(user);
        }
    }
}
