using Tekton.Elastic.Log.Elastic;
using Tekton.RedisCaching;
using Tekton.Seguridad;
using Tekton.Service.Common.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Serilog;
using System.Diagnostics.CodeAnalysis;

namespace Tekton.Service.Common.Extensiones
{
    /// <summary>
    /// app extension
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class AppExtensions
    {
        /// <summary>
        /// MyAllowSpecificOrigins
        /// </summary>
        private static string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        /// <summary>
        /// UseErrorHandlingMiddleware
        /// </summary>
        /// <param name="app"></param>
        public static void UseErrorHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
        }

        /// <summary>
        /// AddCommonConfiguration
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configuration"></param>
        public static void AddCommonConfiguration(this WebApplicationBuilder builder, IConfiguration configuration, bool sinseguridad = false)
        {
            builder.Logging.ClearProviders();
            builder.AddCustomCors(configuration);


            builder.Services.AddTransient<ErrorHandlerMiddleware>();
            builder.Services.AddRedis();
            builder.Services.AddSingleton<IRedisCacheService, RedisCacheService>();


            builder.Services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = HttpLoggingFields.All;
                logging.RequestHeaders.Add(HeaderNames.Authorization);
                logging.RequestHeaders.Add(HeaderNames.Accept);
                logging.RequestHeaders.Add(HeaderNames.ContentEncoding);
                logging.RequestHeaders.Add(HeaderNames.ContentDisposition);
                logging.RequestHeaders.Add(HeaderNames.ContentLength);
                logging.RequestHeaders.Add(HeaderNames.ContentType);

                logging.MediaTypeOptions.AddText("multipart/form-data");
                logging.MediaTypeOptions.AddText("application/json");

                logging.ResponseBodyLogLimit = 1024 * 50;
                logging.RequestBodyLogLimit = 1024 * 50;
            });
            builder.WebHost.UseKestrel(o => o.Limits.MaxRequestBodySize = null);

            builder.Services.Configure<FormOptions>(x =>
            {
                x.MultipartHeadersCountLimit = int.MaxValue;
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBoundaryLengthLimit = int.MaxValue;
                x.MultipartHeadersLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue;
            });
            if (!sinseguridad)
            {
                builder.Services.AddCustomSecurity(configuration);
            }
            ExtensionsElastic.ConfigureLog(configuration);
            builder.Host.UseSerilog();
        }

        /// <summary>
        /// AddCommonConfiguration
        /// </summary>
        /// <param name="app"></param>
        public static void AddCommonConfiguration(this IApplicationBuilder app)
        {
            app.UseErrorHandlingMiddleware();
            app.UseHttpLogging();
            app.UseSerilogRequestLogging();
            app.UseCors(MyAllowSpecificOrigins);


            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.yaml", "Dashboard API");
            });


            app.UseHsts();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

        }


        /// <summary>
        /// AddCommonConfiguration
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configuration"></param>
        public static void AddCommonSimpleConfiguration(this WebApplicationBuilder builder, IConfiguration configuration, bool sc)
        {
            builder.AddCustomCors(configuration);
            builder.Logging.ClearProviders();

            builder.Services.AddTransient<ErrorHandlerMiddleware>();
            builder.Services.AddSingleton<IRedisCacheService, RedisCacheService>();
            builder.Services.AddRedis();

            builder.Services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = HttpLoggingFields.All;
                logging.RequestHeaders.Add(HeaderNames.Accept);
                logging.RequestHeaders.Add(HeaderNames.ContentType);
                logging.RequestHeaders.Add(HeaderNames.ContentDisposition);
                logging.RequestHeaders.Add(HeaderNames.ContentEncoding);
                logging.RequestHeaders.Add(HeaderNames.ContentLength);
                logging.RequestHeaders.Add(HeaderNames.Authorization);

                logging.MediaTypeOptions.AddText("application/json");
                logging.MediaTypeOptions.AddText("multipart/form-data");

                logging.RequestBodyLogLimit = 1024 * 50;
                logging.ResponseBodyLogLimit = 1024 * 50;
            });
            builder.WebHost.UseKestrel(o => o.Limits.MaxRequestBodySize = null);

            builder.Services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue;
                x.MultipartBoundaryLengthLimit = int.MaxValue;
                x.MultipartHeadersCountLimit = int.MaxValue;
                x.MultipartHeadersLengthLimit = int.MaxValue;
            });
            ExtensionsElastic.ConfigureLog(configuration);
            builder.Host.UseSerilog();
        }


        /// <summary>
        /// AddCustomCors
        /// </summary>
        /// <param name="builder"></param>
        public static void AddCustomCors(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                    builder => builder
                    .WithOrigins((configuration["SecuritySettings:Cors"] ?? "").ToString().Split(','))
                    .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
                    .WithHeaders("x-autorization")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .SetPreflightMaxAge(TimeSpan.FromSeconds(3600))

                );
            });

        }
    }
}
