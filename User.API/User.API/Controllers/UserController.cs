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
        public UserController(UserContext userContext)
        {
            _userContext = userContext;
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
                return NotFound();

            return Json(user);
        }


        [HttpPatch]
        public async Task<IActionResult> Patch()
        {
            return new JsonResult(await _userContext.Users.SingleOrDefaultAsync(u => u.Name == "xink"));
        }
    }
}
