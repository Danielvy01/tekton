using AutoMapper;
using Tekton.Configuration.Application.Features.Producto.VIewModel.Response;
using Tekton.Configuration.Damain.Entities.Producto.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tekton.Configuration.Application.Features.Producto.Query
{
	public class GetListProductoQuery:IRequest<GetListProductoDto>
	{
		public int? ProductId { get; set; }

		public class GetListProductoByYearHandler : IRequestHandler<GetListProductoQuery, GetListProductoDto>
		{
			private readonly IProductoRepository _repository;
			private readonly IMapper _mapper;

            public GetListProductoByYearHandler(IProductoRepository repository,IMapper mapper)
            {
                _repository = repository;
				_mapper = mapper;
            }
            public async Task<GetListProductoDto> Handle(GetListProductoQuery request, CancellationToken cancellationToken)
			{
				var result = await _repository.GetProducto(request.ProductId);

				var mapresult = _mapper.Map<List<GetListProductoResponse>>(result);

				var response = new GetListProductoDto()
				{
					Data = mapresult,
					Message = mapresult != null ? "Ejecutado Correctamente" : "Ejecutado Correctamente, no se encontró información",
					Success = mapresult != null
				};

				return response;
			}
		}
	}
}
