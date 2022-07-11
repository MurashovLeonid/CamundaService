using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Superbrands.BusinessProcessor.Application;
using Superbrands.BusinessProcessor.Bus;
using Superbrands.Libs.MicroservicesFramework;

namespace Superbrands.BusinessProcessor.WebApi
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
            services.AddSuperbrandsDefaults(Configuration, "CamundaApi");
            services.RegisterApplication(Configuration);
            services.AddBus(Configuration);
            services.Configure<BusinessProcessorOptions>(Configuration.GetSection(nameof(BusinessProcessorOptions)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider provider)
        {
            app.InitBus(provider);
            app.UseSuperbrandsDefaults(env, "CamundaApi");
            app.UseMvc();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}