using AutoMapper;
using Tekton.Configuration.Application.Features.Producto.VIewModel.Request;
using Tekton.Configuration.Application.Features.Producto.VIewModel.Response;
using Tekton.Configuration.Damain.Entities.Producto.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tekton.Configuration.Application.Features.Producto.Command.Update
{
    public class EliminarProductoCommand : IRequest<EliminarProductoDto>
    {
        public int ProductId { get; set; }
        public string UsuarioEliminacion { get; set; }
        public class EliminarProductoCommandHandler : IRequestHandler<EliminarProductoCommand, EliminarProductoDto>
        {
            private readonly IProductoRepository _repository;

            public EliminarProductoCommandHandler(IProductoRepository repository)
            {
                _repository = repository;
            }
            public async Task<EliminarProductoDto> Handle(EliminarProductoCommand request, CancellationToken cancellationToken)
            {
                var response = new EliminarProductoDto();
                try
                {
                    if (request != null)
                    {
                        var result = await _repository.DeleteProducto(request.ProductId, request.UsuarioEliminacion);

                        response.Success = result.Success;
                        response.Message = result.Message;
                    }
                }
                catch
                {
                    response.Success = false;
                    response.Message = "Error en la eliminacion";
                }

                return response;
            }
        }
    }
}
