using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Superbrands.BusinessProcessor.Application
{
    public static class ApplicationDependencies
    {
        public static IServiceCollection RegisterApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IBusinessProcessorClient, Camunda.SuperbrandsCamundaClient>();
            return services;
        }
    }
}