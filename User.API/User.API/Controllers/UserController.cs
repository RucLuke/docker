using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using User.API.Data;

namespace User.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
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
            return new JsonResult(await _userContext.Users.SingleOrDefaultAsync(u => u.Name == "xink"));
        }
    }
}
