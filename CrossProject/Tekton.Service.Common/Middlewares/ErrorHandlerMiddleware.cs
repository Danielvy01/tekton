using Tekton.Application.Common.Exceptions;
using Tekton.Application.Common.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json;
using System.Diagnostics;
using System.Text;
using static IdentityModel.OidcConstants;
using Microsoft.AspNetCore.Http.Extensions;
using System.IO;

namespace Tekton.Service.Common.Middlewares;
/// <summary>
/// Error Handler
/// </summary>
[ExcludeFromCodeCoverage]
public class ErrorHandlerMiddleware : IMiddleware
{
    /// <summary>
    /// _logger
    /// </summary>


    /// <summary>
    /// ErrorHandlerMiddleware
    /// </summary>
    /// <param name="logger"></param>
    public ErrorHandlerMiddleware()
    {
    }

    /// <summary>
    /// InvokeAsync
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        try
        {


            Stopwatch timeMeasure = new Stopwatch();
            timeMeasure.Start();
            context.Request.EnableBuffering();


            await next(context);
            timeMeasure.Stop();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Request: {context.Request.GetEncodedUrl()} ");
            using (var bodyStream = new StreamReader(context.Request.Body))
            {
                bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
                var bodyText = await bodyStream.ReadToEndAsync();
                sb.AppendLine($"trama: {bodyText} ");
            }
            sb.AppendLine($"Tiempo: {timeMeasure.Elapsed.TotalMilliseconds} ms");
            sb.AppendLine($"Precision: {(1.0 / Stopwatch.Frequency).ToString("E")} segundos");
            Serilog.Log.Information(sb.ToString());

        }
        catch (AccessViolationException avEx)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            var message = "Access violation error from the custom middleware";
            var responseModel = new Response
            {
                Success = false,
                Message = avEx.Message,
                StackTrace = avEx.StackTrace ?? "",
                Errors = new List<string> { message }
            };
            var result = JsonSerializer.Serialize(responseModel, options);
            Console.WriteLine(result);
            await context.Response.WriteAsync(result);
        }
        catch (SqlException sqlEx)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            var responseModel = new Response<string>(string.Empty, string.Empty) { Success = false };

            switch (sqlEx.Number)
            {
                case 547:
                    response.StatusCode = (int)HttpStatusCode.OK;
                    responseModel.Message = "Error: El valor de clave externa no es válido.";
                    break;
                case 2601:
                case 2627:
                    response.StatusCode = (int)HttpStatusCode.OK;
                    responseModel.Message = "Error: El valor de clave única ya existe.";
                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.OK;
                    responseModel.Message = "Error interno de la base de datos.";
                    break;
            }
            var result = JsonSerializer.Serialize(responseModel, options);
            Console.WriteLine(result);
            await response.WriteAsync(result);
        }
        catch (Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            var responseModel = new Response() { Success = false, Message = error.Message, StackTrace = error.StackTrace };

            switch (error)
            {
                case ApiException:
                case KeyNotFoundException:
                    response.StatusCode = (int)HttpStatusCode.OK;
                    responseModel.Message = error.Message;
                    break;
                case ValidationException e:
                    response.StatusCode = (int)HttpStatusCode.OK;
                    responseModel.Message = e.Errors.FirstOrDefault() ?? e.Message + "-" + error.StackTrace;
                    responseModel.Errors = e.Errors;
                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.OK;
                    responseModel.Message = error.Message + "-" + error.StackTrace;
                    break;
            }
            var result = JsonSerializer.Serialize(responseModel, options);
            Console.WriteLine(result);
            await response.WriteAsync(result);
        }


    }
}
