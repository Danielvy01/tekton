namespace Tekton.Configuration.Application.Parameters
{
    [ExcludeFromCodeCoverage]
    public class RequestParameter
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;

        public RequestParameter()
        {
            this.PageNumber = 1;
            this.PageSize = 20;
        }

        public RequestParameter(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > 20 ? 20 : pageSize;
        }
    }
}

