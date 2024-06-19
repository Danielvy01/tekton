using Tekton.Configuration.Damain.Entities.Producto.Repositories;
using Tekton.Configuration.Damain.Helper.Repository;
using Tekton.Configuration.Infraestructure.Data;
using Tekton.Configuration.Infraestructure.Helper;
using Tekton.Configuration.Infraestructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Tekton.Configuration.Infraestructure;

[ExcludeFromCodeCoverage]
public static class ServiceExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddTransient<IUnitOfWork, UnitOfWork>();


        #region Repositories 
        services.AddTransient<IConnectionFactory, ConnectionFactory>();
        services.AddTransient<IProductoRepository, ProductoRepository>();
     
        #endregion Repositories
    }
}
