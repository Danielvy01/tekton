namespace Tekton.Configuration.Application.Wrappers
{
    /// <summary>
    /// PagedResponse
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [ExcludeFromCodeCoverage]
    public class PagedResponse<T> : Response<T>
    {
        /// <summary>
        /// PageNumber
        /// </summary>
        public int PageNumber { get; set; }
        /// <summary>
        /// PageSize
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// PagedResponse
        /// </summary>
        /// <param name="data"></param>

        public PagedResponse(T data)
            : base(data)
        {
        }

        /// <summary>
        /// PagedResponse
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        public PagedResponse(T data, int pageNumber, int pageSize) : base(data)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.Data = data;
            this.Message = string.Empty;
            this.Success = true;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class PagedResponse : Response
    {
        /// <summary>
        /// Numero de Pagina
        /// </summary>
        public int PageNumber { get; set; }
        /// <summary>
        /// Cantidad de Pagina
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// Total de Registros
        /// </summary>
        public int Total { get; set; }
        /// <summary>
        /// Total de Paginas
        /// </summary>
        public int TotalPage { get; set; }

    }
}
