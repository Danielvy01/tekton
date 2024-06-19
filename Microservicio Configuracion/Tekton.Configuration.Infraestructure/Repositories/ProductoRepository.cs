using Dapper;
using Tekton.Configuration.Damain.Entities.Producto;
using Tekton.Configuration.Damain.Entities.Producto.Repositories;
using Tekton.Configuration.Infraestructure.Data;
using Tekton.Configuration.Infraestructure.Wrapper;
using System.Data;
using static Dapper.SqlMapper;

namespace Tekton.Configuration.Infraestructure.Repositories
{
	public class ProductoRepository : IProductoRepository
	{
		private readonly IConnectionFactory _connectionFactory;

        public ProductoRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public async Task<ResponseProductoEntity> DeleteProducto(int ProductId, string UsuarioEliminacion)
		{

			using var connection = _connectionFactory.GetConnection;
			var parameters = new DynamicParameters();
			parameters.Add("@ProductId", ProductId);
            parameters.Add("@UsuarioEliminacion", UsuarioEliminacion);
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
			parameters.Add("@Message", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);


			var response = await connection.QueryFirstOrDefaultAsync<ResponseProductoEntity>
				   (ConstStoreProcedure.SP_ELIMINAR_PRODUCTO,
				   parameters,
				   commandType: CommandType.StoredProcedure);

			response.Message = parameters.Get<string>("@Message");
			response.Success = parameters.Get<bool>("@Success");

			return response;
		}

		public async Task<IEnumerable<ProductoEntity>> GetProducto(int? productId)
		{
			using var connection = _connectionFactory.GetConnection;
			var parameters= new DynamicParameters();
			parameters.Add("@ProductId", productId);

			var result = await connection.QueryAsync<ProductoEntity>
				   (ConstStoreProcedure.SP_OBTENER_PRODUCTO,
				   parameters,
				   commandType: CommandType.StoredProcedure);

			return result;
		}

		public async Task<ResponseProductoEntity> InsertProducto(ProductoEntity entity)
		{
			using var connection = _connectionFactory.GetConnection;
			var parameters = new DynamicParameters();
			parameters.Add("@Name", entity.Name);
			parameters.Add("@Status", entity.Status);
			parameters.Add("@Stock", entity.Stock);
			parameters.Add("@Description", entity.Description);
			parameters.Add("@Price", entity.Price);
            parameters.Add("@Discount", entity.Discount);
            parameters.Add("@UsuarioCreacion", entity.UsuarioCreacion);
            parameters.Add("@Success", dbType:DbType.Boolean,direction:ParameterDirection.Output);
			parameters.Add("@Message", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);


			var response = await connection.QueryFirstOrDefaultAsync<ResponseProductoEntity>
				   (ConstStoreProcedure.SP_INSERT_PRODUCTO,
				   parameters,
				   commandType: CommandType.StoredProcedure);

			response.Message = parameters.Get<string>("@Message");
			response.Success = parameters.Get<bool>("@Success");
			
			return response;
		}

		public async Task<ResponseProductoEntity> UpdateProducto(ProductoEntity entity)
		{
			using var connection = _connectionFactory.GetConnection;
			var parameters = new DynamicParameters();
			parameters.Add("@ProductId", entity.ProductId);
			parameters.Add("@Name", entity.Name);
			parameters.Add("@Status", entity.Status);
            parameters.Add("@Stock", entity.Stock);
            parameters.Add("@Description", entity.Description);
            parameters.Add("@Price", entity.Price);
            parameters.Add("@Discount", entity.Discount);
			parameters.Add("@UsuarioModificacion", entity.UsuarioModificacion);
			parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
			parameters.Add("@Message", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);

			var response = await connection.QueryFirstOrDefaultAsync<ResponseProductoEntity>
				   (ConstStoreProcedure.SP_ACTUALIZAR_PRODUCTO,
				   parameters,
				   commandType: CommandType.StoredProcedure);

			response.Message = parameters.Get<string>("@Message");
			response.Success = parameters.Get<bool>("@Success");

			return response;
		}
	}
}
