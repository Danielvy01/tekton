namespace Tekton.Configuration.Application.Parameters
{
    [ExcludeFromCodeCoverage]
    public class FileParameter
    {
        public string? Name { get; set; }
        public string? Type { get; set; }
        public byte[]? ByteFile { get; set; }
    }
}
