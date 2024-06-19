using Tekton.Configuration.Application.Features.Producto.Command.Create;
using Tekton.Configuration.Application.Features.Producto.Command.Update;
using Tekton.Configuration.Application.Features.Producto.Query;
using Tekton.Configuration.Application.Features.Producto.VIewModel.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Diagnostics;

namespace Tekton.Configuration.WebApi.Controllers
{
    /// <summary>
    /// Controlador para manejar la configuación base de los productos
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductoController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// Obtiene información de un producto o todos los productos
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("GetById")]
        [ProducesResponseType(typeof(GetListProductoDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetById([FromQuery] GetListProductoQuery query)
        {
            return Ok(await _mediator.Send(query));
        }
        /// <summary>
        /// Registra la información de un producto
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("Insert")]
        [ProducesResponseType(typeof(InsertProductoDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Create([FromBody] InsertProductoCommand command)
        {
            var salida = Ok(await _mediator.Send(command));
            return salida;

        }
        /// <summary>
        /// Actualiza la información de un producto
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        [ProducesResponseType(typeof(UpdateProductoDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Update([FromBody] UpdateProductoCommand query)
        {
            return Ok(await _mediator.Send(query));
        }
        /// <summary>
        /// Elimina Productos po id
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPut("Delete")]
        [ProducesResponseType(typeof(EliminarProductoDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Delete([FromBody] EliminarProductoCommand query)
        {
            return Ok(await _mediator.Send(query));
        }
    }
}
