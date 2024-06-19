using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Tekton.Elastic.Log.Elastic;

[ExcludeFromCodeCoverage]
public static class ExtensionsElastic
{
    /// <summary>
    /// ConfigureLog
    /// </summary>
    /// <param name="configuration"></param>
    public static void ConfigureLog(IConfiguration configuration)
    {
        string? env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        string? elasticapp = configuration["logElastic:app"];

        if (env == null || elasticapp == null)
            return;

        string environment = env.ToString();

        Serilog.Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .WriteTo.Debug()
            .WriteTo.Console()
            .WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment))
            .Enrich.WithProperty("Environment", environment)
            .Enrich.WithProperty("Application", elasticapp.ToString())
            .ReadFrom.Configuration(configuration)
            .WriteTo.File(@"\log\log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
    }

    /// <summary>
    /// ConfigureElasticSink
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="environment"></param>
    /// <returns></returns>
    private static ElasticsearchSinkOptions ConfigureElasticSink(IConfiguration configuration, string environment)
    {
        string? elasticUri = configuration["logElastic:uri"];
        string? elasticIndex = configuration["logElastic:index"];
        var salida = new ElasticsearchSinkOptions();
        if (elasticUri != null && elasticIndex != null)
        {
            salida = new ElasticsearchSinkOptions(new Uri(elasticUri))
            {
                AutoRegisterTemplate = true,
                IndexFormat = $"{elasticIndex}-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
            };
        }

        Serilog.Log.Information($"Elastic {elasticUri} - {elasticIndex}");

        return salida;

    }
}
