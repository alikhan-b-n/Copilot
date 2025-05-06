
using Calabonga.UnitOfWork;
using Copilot.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Copilot.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.AddDbContext<ApplicationContext>(options => options
            .UseNpgsql(configuration.GetConnectionString("NpgSql")));

        serviceCollection.AddUnitOfWork<ApplicationContext>();
        
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
                
        return serviceCollection;
    }
}