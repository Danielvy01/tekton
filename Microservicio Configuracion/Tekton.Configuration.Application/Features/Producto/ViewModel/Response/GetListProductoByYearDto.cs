using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekton.Configuration.Damain.Entities.Producto;

namespace Tekton.Configuration.Application.Features.Producto.VIewModel.Response
{
	[ExcludeFromCodeCoverage]
	public class GetListProductoDto: Response<List<GetListProductoResponse>>
	{
	}

	[ExcludeFromCodeCoverage]
	public class GetListProductoResponse:ProductoEntity
	{
    }

}
