using AutoMapper;
using Dapper;
using Tekton.Configuration.Application.Mappings;
using Tekton.Configuration.Damain.Entities.ConfiguracionTramite;
using Tekton.Configuration.Damain.Entities.Departamento;
using Tekton.Configuration.Infraestructure.Data;
using Tekton.Configuration.Infraestructure.Repositories;
using Moq;
using Moq.Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tekton.Configuration.xUnitTestes.Departamento
{
	public class DepartamentoRepositoryTests
	{
		private readonly Mock<IConnectionFactory> _mockConnectionFactory;
		private readonly Mock<IDbConnection> _mockConnection;
		private readonly Mock<IDbTransaction> _mockTransaction;
		private readonly IMapper _mapper;

        public DepartamentoRepositoryTests()
        {
			 _mockConnectionFactory= new Mock<IConnectionFactory>();
			_mockConnection = new Mock<IDbConnection>();
			_mockTransaction = new Mock<IDbTransaction>();
			_mapper = new MapperConfiguration(cfg => cfg.AddProfile<AutomapperProfile>())
				.CreateMapper();
		}

		private DepartamentoRepository CreateDepartamentoRepository()
		{
			return new DepartamentoRepository(
				_mockConnectionFactory.Object);
		}

		[Fact]
		public async Task GetDepartamento_StateUnderTest_ExpectedBehavior()
		{

			// Arrange
			var departamentoRepository = CreateDepartamentoRepository();


			_mockConnectionFactory.Setup(c => c.GetConnection).Returns(_mockConnection.Object);
			_mockConnection.SetupDapperAsync(c => c.QueryAsync<DepartamentoEntity>(It.IsAny<string>(), It.IsAny<object>(), _mockTransaction.Object, null, It.IsAny<CommandType>()))
						   .ReturnsAsync(new List<DepartamentoEntity> { new DepartamentoEntity() {} });

			// Act
			var result = await departamentoRepository.GetDepartamento();

			// Assert
			Assert.IsType<List<DepartamentoEntity>?>(result);
		}

		[Fact]
		public async Task GetByProvinciaIdAsync_StateUnderTest_ExpectedBehavior()
		{

			// Arrange
			var departamentoRepository = CreateDepartamentoRepository();
			int provinciaId = 1;

			_mockConnectionFactory.Setup(c => c.GetConnection).Returns(_mockConnection.Object);
			_mockConnection.SetupDapperAsync(c => c.QueryFirstOrDefaultAsync<DepartamentoEntity>(It.IsAny<string>(), It.IsAny<object>(), _mockTransaction.Object, null, It.IsAny<CommandType>()))
						   .ReturnsAsync(new DepartamentoEntity { DepartamentoId= "1", Nombre="Lima" } );

			// Act
			var result = await departamentoRepository.GetByProvinciaIdAsync(provinciaId);

			// Assert
			Assert.IsType<DepartamentoEntity>(result);
		}
	}
}
