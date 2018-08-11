using User.API.Dtos;

namespace User.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class BaseController : Controller
    {
        protected UserIdentity UserIdentity => new UserIdentity()
        {
            UserId = 1,
            Avator = string.Empty,
            Name = "xink"
        };
    }
}
