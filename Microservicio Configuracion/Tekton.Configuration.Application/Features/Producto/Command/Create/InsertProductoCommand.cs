using AutoMapper;
using Tekton.Configuration.Application.Features.Producto.VIewModel.Request;
using Tekton.Configuration.Application.Features.Producto.VIewModel.Response;
using Tekton.Configuration.Damain.Entities.Producto;
using Tekton.Configuration.Damain.Entities.Producto.Repositories;


namespace Tekton.Configuration.Application.Features.Producto.Command.Create
{
	public class InsertProductoCommand: ProductoRequest, IRequest<InsertProductoDto>
	{
        public string UsuarioCreacion { get; set; }
        public class InsertProductoCommandHandler: IRequestHandler<InsertProductoCommand, InsertProductoDto>
		{
            public string UsuarioCreacion { get; set; }

            private readonly IProductoRepository _repository;
			private readonly IMapper _mapper;
			public InsertProductoCommandHandler( IProductoRepository repository, IMapper	mapper)
			{
				_repository = repository;
				_mapper = mapper;
			}
			public async Task<InsertProductoDto> Handle(InsertProductoCommand request, CancellationToken cancellationToken)
			{
				var response = new InsertProductoDto() {};
				try
				{
					if (request != null)
					{

						var mapresult = _mapper.Map<ProductoEntity>(request);
						var result = await _repository.InsertProducto(mapresult);

						response.Success = result.Success;
						response.Message = result.Message;

					}
				}
				catch
				{
					response.Success = false;
					response.Message = "Error en el registro";
				}
		
				return response;
			}
		}
    }
}
