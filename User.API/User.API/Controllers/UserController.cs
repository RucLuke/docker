using Microsoft.Extensions.Logging;

namespace User.API.Controllers
{
    using System.Threading.Tasks;
    using Data;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly UserContext _userContext;
        private ILogger<UserController> _logger;
        public UserController(UserContext userContext, ILogger<UserController> logger)
        {
            _userContext = userContext;
            _logger = logger;
        }
        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user = await _userContext.Users
                 .AsNoTracking()
                 .Include(u => u.Properties)
                 .SingleOrDefaultAsync(u => u.Id == UserIdentity.UserId);

            if (user == null)
            {
                throw new UserOperationException($"错误的用户上下文:Id {UserIdentity.UserId}");
            }

            return Json(user);
        }


        [HttpPatch]
        public async Task<IActionResult> Patch()
        {
            return new JsonResult(await _userContext.Users.SingleOrDefaultAsync(u => u.Name == "xink"));
        }
    }
}
