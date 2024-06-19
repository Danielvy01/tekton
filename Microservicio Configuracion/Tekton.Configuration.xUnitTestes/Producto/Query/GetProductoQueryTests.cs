using AutoMapper;
using Tekton.Configuration.Application.Features.ConfiguracionTramite.Query;
using Tekton.Configuration.Application.Features.Departamento.Query;
using Tekton.Configuration.Application.Features.Departamento.ViewModel.Response;
using Tekton.Configuration.Damain.Entities.ConfiguracionTramite.Repositories;
using Tekton.Configuration.Damain.Entities.Departamento.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tekton.Configuration.Application.Features.Departamento.Query.GetDepartamentoQuery;

namespace Tekton.Configuration.xUnitTestes.Departamento.Query
{
	public class GetDepartamentoQueryTests
	{
		private readonly Mock<IDepartamentoRepository> _departamentoRepository;
		private readonly Mock<IMapper> _mapper;
		private readonly GetDepartamentoHandler _handler;

        public GetDepartamentoQueryTests()
        {
			_departamentoRepository = new Mock<IDepartamentoRepository>();
			_mapper = new Mock<IMapper>();
			_handler = new GetDepartamentoHandler(_departamentoRepository.Object, _mapper.Object);
		}

		[Fact]
		public async Task Handle_GetDepartamentoQuery_ResultsSuccess()
		{
			// Arrange
			var request = new GetDepartamentoQuery {};
			CancellationToken cancellationToken = new CancellationToken();

			// Act
			var result = await _handler.Handle(
				request,
				cancellationToken);

			// Assert
			Assert.IsType<GetDepartamentoDto>(result);
		}
	}
}
