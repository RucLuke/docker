using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using User.API.Controllers;
using User.API.Data;
using User.API.Models;
using Xunit;

namespace User.API.UnitTest
{
    public class UserControllerUnitTests
    {
        private UserContext GetUserContext()
        {
            var options = new DbContextOptionsBuilder<UserContext>()
                            .UseInMemoryDatabase(Guid.NewGuid().ToString())
                            .Options;

            var userContext = new UserContext(options);

            userContext.Users.Add(new AppUser()
            {
                Id = 1,
                Name = "xink"
            });
            userContext.SaveChanges();

            return userContext;
        }


        [Fact]
        public async Task Get_ReturnRightUser_WithExpectedParams()
        {
            // 测试主体(方法名)+ 期望的结果 + 参数(什么情况下)

            var context = GetUserContext();
            var loggerMoq = new Mock<ILogger<UserController>>();
            var logger = loggerMoq.Object;
            var controller = new UserController(context, logger);
            var response = await controller.Get();

            Assert.IsType<JsonResult>(response);
        }
    }
}
