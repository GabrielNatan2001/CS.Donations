using CS.Donations.Application.Messaging;
using CS.Donations.Domain.Interfaces;
using CS.Donations.Infrastructure.Data;
using CS.Donations.Infrastructure.Data.Repositories;
using CS.Donations.Infrastructure.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CS.Donations.Infrastructure;

public static class DependencyInjectionInfrastructure
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IDoacaoProcessamentoRepository, DoacaoProcessamentoRepository>();
        services.AddSingleton<IMessageBus, MessageBus>();
        return services;
    }
}
