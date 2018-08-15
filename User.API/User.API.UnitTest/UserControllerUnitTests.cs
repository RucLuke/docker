using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
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


        private (UserController controller, UserContext userContext) GetUserController()
        {
            var context = GetUserContext();
            var loggerMoq = new Mock<ILogger<UserController>>();
            var logger = loggerMoq.Object;
            return (controller: new UserController(context, logger), userContext: context);
        }

        [Fact]
        public async Task Get_ReturnRightUser_WithExpectedParams()
        {
            // 测试主体(方法名)+ 期望的结果 + 参数(什么情况下)
            var (controller, userContext) = GetUserController();
            var response = await controller.Get();
            //Assert.IsType<JsonResult>(response);
            var result = response.Should().BeOfType<JsonResult>().Subject;
            var appUser = result.Value.Should().BeAssignableTo<AppUser>().Subject;
            appUser.Id.Should().Be(1);
            appUser.Name.Should().Be("xink");
        }

        [Fact]
        public async Task Patch_ReturnNewName_WithExpectedNewName()
        {
            var (controller, userContext) = GetUserController();
            var document = new JsonPatchDocument<AppUser>();
            document.Replace(u => u.Name, "xink1");

            //Assert response
            var response = await controller.Patch(document);
            var result = response.Should().BeOfType<JsonResult>().Subject;
            var appUser = result.Value.Should().BeAssignableTo<AppUser>().Subject;
            appUser.Name.Should().Be("xink1");

            //Assert name value in ef context
            var userModel = await userContext.Users.SingleOrDefaultAsync(u => u.Id == 1);
            userModel.Should().NotBeNull();
            userModel.Name.Should().Be("xink1");
        }

        [Fact]
        public async Task Patch_ReturnNewPeoperties_WithAddNewPeoperties()
        {
            var (controller, userContext) = GetUserController();
            var document = new JsonPatchDocument<AppUser>();
            document.Replace(u => u.Properties, new List<UserProperty>()
            {
                new UserProperty{Key="fin_industry",Value = "互联网",Text = "互联网"}
            });

            //Assert response
            var response = await controller.Patch(document);
            var result = response.Should().BeOfType<JsonResult>().Subject;
            var appUser = result.Value.Should().BeAssignableTo<AppUser>().Subject;
            appUser.Properties.Count.Should().Be(1);
            appUser.Properties.FirstOrDefault()?.Key.Should().Be("fin_industry");
            appUser.Properties.FirstOrDefault()?.Value.Should().Be("互联网");

            //Assert name value in ef context
            var userModel = await userContext.Users.SingleOrDefaultAsync(u => u.Id == 1);
            userModel.Properties.Count.Should().Be(1);
            userModel.Properties.FirstOrDefault()?.Key.Should().Be("fin_industry");
            userModel.Properties.FirstOrDefault()?.Value.Should().Be("互联网");
        }

        [Fact]
        public async Task Patch_ReturnNewPeoperties_WithRemovePeoperty()
        {
            var (controller, userContext) = GetUserController();
            var document = new JsonPatchDocument<AppUser>();
            document.Replace(u => u.Properties, new List<UserProperty>());

            //Assert response
            var response = await controller.Patch(document);
            var result = response.Should().BeOfType<JsonResult>().Subject;
            var appUser = result.Value.Should().BeAssignableTo<AppUser>().Subject;
            appUser.Properties.Should().BeEmpty();


            //Assert name value in ef context
            var userModel = await userContext.Users.SingleOrDefaultAsync(u => u.Id == 1);
            userModel.Properties.Should().BeEmpty();
        }
    }
}
