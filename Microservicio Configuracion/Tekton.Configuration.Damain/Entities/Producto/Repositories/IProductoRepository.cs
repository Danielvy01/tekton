using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tekton.Configuration.Damain.Entities.Producto.Repositories
{
	public interface IProductoRepository
	{
		Task<IEnumerable<ProductoEntity>> GetProducto(int? productId);
		Task<ResponseProductoEntity> InsertProducto(ProductoEntity entity);
		Task<ResponseProductoEntity> UpdateProducto(ProductoEntity entity);
		Task<ResponseProductoEntity> DeleteProducto(int ProductId, string UsuarioEliminacion);



    }
}
