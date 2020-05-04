using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FancyMessengerApi.Extensions;
using FancyMessengerApi.Services;

namespace FancyMessengerApi
{
    public class Startup
    {
        private readonly IConfiguration _config;
        
        public Startup(IConfiguration config)
        {
            _config = config;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddJwtAuthentication(_config["Auth:SecretKey"]);
            
            services.AddControllers();
            
            services.AddSingleton(new AuthService(_config["Auth:SecretKey"]));
            
            // Docs
            services.AddHttpContextAccessor(); // TODO Move inside AddSwaggerGen?
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}