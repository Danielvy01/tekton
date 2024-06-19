using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Tekton.Application.Common.Wrappers
{
    [ExcludeFromCodeCoverage]
    public class Response<T>
    {

        /// <summary>
        /// Response
        /// </summary>
        public Response()
        {
            Success = true;
            Message = string.Empty;
            Data = Activator.CreateInstance<T>();
            Errors = new List<string>();
        }
        public Response(T data)
        {
            Success = true;
            Message = string.Empty;
            Data = data;
            Errors = new List<string>();
        }

        /// <summary>
        /// Response
        /// </summary>
        public Response(T data, string message)
        {
            Success = true;
            Message = message;
            Data = data;
            Errors = new List<string>();
        }

        [JsonPropertyName("success")]
        /// <summary>
        /// Success
        /// </summary>
        public bool Success { get; set; }

        [JsonPropertyName("message")]
        /// <summary>
        /// Message
        /// </summary>
        public string? Message { get; set; }

        [JsonPropertyName("stackTrace")]
        /// <summary>
        /// StackTrace
        /// </summary>
        public string? StackTrace { get; set; }

        /// <summary>
        /// Errors
        /// </summary>
        [JsonPropertyName("errors")]
        public List<string> Errors { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("data")]
        /// <summary>
        /// Data
        /// </summary>
        public T Data { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Response
    {
        public Response()
        {
            Success = true;
            Message = string.Empty;
            Errors = new List<string>();
        }
        public Response(string _message)
        {
            Success = true;
            Message = _message;
            Errors = new List<string>();
        }
        [JsonPropertyName("success")]
        public bool Success { get; set; }
        [JsonPropertyName("message")]
        public string? Message { get; set; }
        [JsonPropertyName("stackTrace")]
        public string? StackTrace { get; set; }
        [JsonPropertyName("errors")]
        public List<string> Errors { get; set; }
    }

}
