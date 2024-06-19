using Tekton.Configuration.Application.Features.ConfiguracionTramite.Query;
using Tekton.Configuration.Application.Features.Departamento.Query;
using Tekton.Configuration.WebApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tekton.Configuration.xUnitTestes.Departamento
{
	public class DepartamentoControllerTests
	{
		private readonly Mock<IMediator> _mediator;
        public DepartamentoControllerTests()
        {
			_mediator = new Mock<IMediator>();
		}

		private DepartamentoController CreateDepartamentoController()
		{
			return new DepartamentoController(
				_mediator.Object);
		}

		[Fact]
		public async Task Obtener_Ok()
		{
			// Arrange
			var departamentoController = CreateDepartamentoController();
			var getDepartamentoQuery = new GetDepartamentoQuery { };

			// Act
			var result = await departamentoController.GetDepartamento(
				getDepartamentoQuery);

			// Assert
			Assert.IsType<OkObjectResult?>(result);
		}
	}
}
