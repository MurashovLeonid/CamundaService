using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Superbrands.Libs.Bus.Client;

namespace Superbrands.BusinessProcessor.Bus
{
    public static class BusDependencies
    {
        public static IServiceCollection AddBus(this IServiceCollection services, IConfiguration configuration)
        {
            var busAssembly = typeof(BusDependencies).Assembly;
            services.AddBusAndAutoSubscription(configuration, busAssembly);
            return services;
        }

        public static IApplicationBuilder InitBus(this IApplicationBuilder app, IServiceProvider provider)
        {
            app.InitAutoSubscription(provider);
            return app;
        }
    }
}