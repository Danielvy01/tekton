using Tekton.Configuration.Damain.Entities.Producto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tekton.Configuration.Application.Features.Producto.VIewModel.Request
{
    [ExcludeFromCodeCoverage]
    public class ProductoRequest
    {
        /// <summary>
        /// Nombre del producto
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Estado id del producto
        /// </summary>
        public short Status { get; set; }
        /// <summary>
        /// Cantidad real del producto
        /// </summary>
        public int Stock { get; set; }
        /// <summary>
        /// Descripción del producto
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// Precio del producto
        /// </summary>
        public decimal? Price { get; set; }
        /// <summary>
        /// Porcentage de descuento del producto
        /// </summary>
        public decimal? Discount { get; set; }
    }


}
