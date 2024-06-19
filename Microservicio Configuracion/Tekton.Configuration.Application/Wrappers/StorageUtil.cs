using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tekton.Configuration.Application.Wrappers;
public interface IStorageUtil
{
    Task<StorageFileResponse> GetFile(string? filepath);
}

[ExcludeFromCodeCoverage]
public class StorageUtil : IStorageUtil
{
    private readonly string _BaseAddressStorage;

    public StorageUtil(IConfiguration configuration)
    {
        _BaseAddressStorage = configuration["Api:Storage"] ?? string.Empty;
    }

    public async Task<StorageFileResponse> GetFile(string? filepath)
    {
        return await GetFile(new LeerArchivo() { FilePath = filepath, FileName = Path.GetFileName(filepath) });
    }

    public async Task<StorageFileResponse> GetFile(LeerArchivo request)
    {
        var storageResponse = new StorageFileResponse();
        string svcEndPoint = "Documento/getfileBytes";

        try
        {
            dynamic? result = await FuncionStorage(request, _BaseAddressStorage, svcEndPoint);
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<StorageFileResponse>(content);
            }
        }
        catch
        {
            storageResponse.success = false;
            storageResponse.message = "Error al conectar servicio de fileserver";
        }

        return storageResponse;
    }

    public async static Task<dynamic>  FuncionStorage(dynamic request, dynamic _BaseAddressStorage, string svcEndPoint)
    {
        var requestJson = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        using (var client = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true }))
        {
            var fullUrl = string.Format("{0}/{1}", _BaseAddressStorage, svcEndPoint);

            var httpRequestMessage = new HttpRequestMessage { Content = requestJson, Method = HttpMethod.Post, RequestUri = new System.Uri(fullUrl) };

            var result = await client.SendAsync(httpRequestMessage);
            return result;
        }
    }
}
