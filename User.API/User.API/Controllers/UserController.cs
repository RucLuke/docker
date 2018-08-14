using System.Linq;

namespace User.API.Controllers
{
    using System.Threading.Tasks;
    using Data;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.AspNetCore.JsonPatch;
    using Models;

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
        public async Task<IActionResult> Patch([FromBody]JsonPatchDocument<AppUser> patch)
        {
            var user = await _userContext.Users
                .Include(u => u.Properties)
                .SingleOrDefaultAsync(u => u.Id == UserIdentity.UserId);

            var originalProperties = user.Properties;
            patch.ApplyTo(user);

            var allProperties = originalProperties.Union(user.Properties).Distinct();

            var removedProperties = originalProperties.Except(user.Properties);
            var newProperties = allProperties.Except(originalProperties);

            foreach (var property in removedProperties)
            {
                _userContext.Remove(property);
            }

            foreach (var property in newProperties)
            {
                _userContext.Add(property);
            }

            _userContext.Users.Update(user);
            _userContext.SaveChanges();
            return Json(user);
            //return new JsonResult(await _userContext.Users.SingleOrDefaultAsync(u => u.Name == "xink"));
        }
    }
}
