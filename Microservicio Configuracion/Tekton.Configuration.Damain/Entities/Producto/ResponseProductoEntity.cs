using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tekton.Configuration.Damain.Entities.Producto
{
	[ExcludeFromCodeCoverage]
	public class ResponseProductoEntity
	{
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}
