namespace Tekton.Configuration.Application.Wrappers
{
    [ExcludeFromCodeCoverage]
    public class StorageFileResponse
    {
        public bool success { get; set; }
        public string? message { get; set; }
        public byte[] fileBytes { get; set; }
        public string? name { get; set; }
        public int fileSize { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class StorageDirectoryResponse
    {
        public bool success { get; set; }
        public string? message { get; set; }
        public List<ArchivosDeDirectorio> data { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Directorio
    {
        public string? Path { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class RegistrarArchivo
    {
        public string? FilePath { get; set; }
        public byte[] byteFile { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class LeerArchivo
    {
        public string? FilePath { get; set; }
        public string? FileName { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class LeerDirectorio
    {
        public string? FilePath { get; set; }
        public string? Search { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class ArchivosDeDirectorio
    {
        public string? FilePath { get; set; }
        public string? Name { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
