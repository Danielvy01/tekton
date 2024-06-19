using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Tekton.RedisCaching
{
    [ExcludeFromCodeCoverage]
    public static class Extensions
    {
        /// <summary>
        /// SectionName
        /// </summary>
        private static readonly string SectionName = "redis";

        /// <summary>
        /// IServiceCollection
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddRedis(this IServiceCollection services)
        {
            IConfiguration configuration;
            using (var serviceProvider = services.BuildServiceProvider())
            {
                configuration = serviceProvider.GetService<IConfiguration>();
            }

            var options = configuration.GetOptions<RedisOptions>(SectionName);

            services.AddDistributedRedisCache(o =>
            {
                o.Configuration = options.ConnectionString;
            });

            return services;
        }

        /// <summary>
        /// GetOptions
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="configuration"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public static TModel GetOptions<TModel>(this IConfiguration configuration, string section) where TModel : new()
        {
            var model = new TModel();
            configuration.GetSection(section).Bind(model);

            return model;
        }
    }
}
