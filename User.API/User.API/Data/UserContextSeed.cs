using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using User.API.Models;

namespace User.API.Data
{
    public class UserContextSeed
    {
        public static async Task SeedAsync(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var userContext = scope.ServiceProvider.GetRequiredService<UserContext>();
                userContext.Database.Migrate();
                if (userContext.Users.Any()) return;

                // add a default user
                //userContext.Users.Add(new AppUser() { Name = "xink" });
                await userContext.Users.AddAsync(new AppUser() { Name = "xink" });
                userContext.SaveChanges();
            }
        }
    }
}
