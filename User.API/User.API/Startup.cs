using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using User.API.Data;
using User.API.Filters;
using User.API.Models;

namespace User.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<UserContext>(options =>
                {
                    //options.UseMySQL(Configuration.GetConnectionString("MysqlUserConnection"));
                    options.UseMySql(Configuration.GetConnectionString("MysqlUserConnection"));
                });

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(GlobalExceptionFilter));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            //InitUserDatabase(app);
            UserContextSeed.SeedAsync(app).Wait();
            //app.UseHttpsRedirection();
            app.UseMvc();
        }

        //public void InitUserDatabase(IApplicationBuilder app)
        //{
        //    using (var scope = app.ApplicationServices.CreateScope())
        //    {
        //        var userContext = scope.ServiceProvider.GetRequiredService<UserContext>();
        //        userContext.Database.Migrate();
        //        if (userContext.Users.Any()) return;

        //        // add a default user
        //        userContext.Users.Add(new AppUser() { Name = "xink" });
        //        userContext.SaveChanges();
        //    }
        //}
    }
}
