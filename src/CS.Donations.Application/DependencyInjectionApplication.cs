using CS.Donations.Application.Messaging.Consumers;
using Microsoft.Extensions.DependencyInjection;

namespace CS.Donations.Application;

public static class DependencyInjectionApplication
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IDoacaoRecebidaMessage, DoacaoRecebidaConsumer>();
        return services;
    }
}
