using Dating_App.API.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dating_App.API
{
    public class Startup
    {
        // Loading up our configuration with our Constructor
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // Dependency ejection container 

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Order does not matter in here

            // Have to add UseSqlite from NuGet pkg mgr
            // Configuration has our connection string -> peek appsettings.json
            // "Default Connection" => key
            services.AddDbContext<DataContext>(x => x.UseSqlite(
                Configuration.GetConnectionString("DefaultConnection")));
            services.AddControllers();
            services.AddCors();
            // services.addmvc difference 2.0
            // addmvc does not support our endpoint ctrlers 
        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // You can add middleware, think of Redux-Saga or Redux-Thunk in react
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // redirects http to https
            // app.UseHttpsRedirection();

            // Controllers are going to handle our routing 
            app.UseRouting();

            app.UseAuthorization();

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            // allows our controllers or our end points in our controllers to be mapped into our middleware
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
