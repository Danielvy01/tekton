using AutoMapper;
using Tekton.Configuration.Application.Features.Producto.VIewModel.Request;
using Tekton.Configuration.Application.Features.Producto.VIewModel.Response;
using Tekton.Configuration.Damain.Entities.Producto;
using Tekton.Configuration.Damain.Entities.Producto.Repositories;


namespace Tekton.Configuration.Application.Features.Producto.Command.Update
{
	public class UpdateProductoCommand:ProductoRequest, IRequest<UpdateProductoDto>
	{
        public int ProductId { get; set; }
        public string UsuarioModificacion { get; set; }
        public class UpdateProductoCommandHandler : IRequestHandler<UpdateProductoCommand, UpdateProductoDto>
		{
			private readonly IProductoRepository _repository;
			private readonly IMapper _mapper;
            public UpdateProductoCommandHandler(IProductoRepository repository, IMapper mapper)
            {
				_repository = repository;
				_mapper = mapper;
			}
            public async Task<UpdateProductoDto> Handle(UpdateProductoCommand request, CancellationToken cancellationToken)
			{
				var response = new UpdateProductoDto();
				try
				{
					if (request != null)
					{

						var mapresult = _mapper.Map<ProductoEntity>(request);
						var result = await _repository.UpdateProducto(mapresult);

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
