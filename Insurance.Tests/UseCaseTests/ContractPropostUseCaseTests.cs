using Insurance.Hiring.Application.DTOs;
using Insurance.Hiring.Application.UseCase;
using Insurance.Hiring.Domain.Domain;
using Insurance.Hiring.Domain.DTOs;
using Insurance.Hiring.Domain.Ports;
using Insurance.Shared.Enums;
using Moq;

namespace Insurance.Tests.UseCaseTests
{
    public class ContractPropostUseCaseTests
    {
        private readonly Mock<IContractRepository> _repositoryMock;
        private readonly Mock<IPropostServiceClient> _propostaClientMock;
        private readonly CreateContractUseCase _useCase;

        public ContractPropostUseCaseTests()
        {
            _repositoryMock = new Mock<IContractRepository>();
            _propostaClientMock = new Mock<IPropostServiceClient>();
            _useCase = new CreateContractUseCase(_repositoryMock.Object, _propostaClientMock.Object);
        }

        [Fact]
        public async Task ExecuteAsync_QuandoPropostaNaoEncontrada_DeveLancarExcecao()
        {
            // Arrange
            var request = new ContractPropostRequest(Guid.NewGuid());

            _repositoryMock.Setup(x => x.GetByProposalIdAsync(request.PropostId))
                .ReturnsAsync((ContractEntity?)null);

            _propostaClientMock.Setup(x => x.GetPropostAsync(request.PropostId))
                .ReturnsAsync((PropostDto?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _useCase.ExecuteAsync(request));

            Assert.Equal("Propost not found", exception.Message);
        }

        [Fact]
        public async Task ExecuteAsync_QuandoPropostaNaoAprovada_DeveLancarExcecao()
        {
            // Arrange
            var propostaId = Guid.NewGuid();
            var request = new ContractPropostRequest(propostaId);
            var proposta = new PropostDto(
                propostaId, "João Silva", "joao@email.com", 10000m, "Vida",
                "Rejeitada", DateTime.UtcNow, null); 

            _repositoryMock.Setup(x => x.GetByProposalIdAsync(request.PropostId))
                .ReturnsAsync((ContractEntity?)null);

            _propostaClientMock.Setup(x => x.GetPropostAsync(request.PropostId))
                .ReturnsAsync(proposta);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _useCase.ExecuteAsync(request));

            Assert.Equal("Only approved proposals can be contracted", exception.Message);
        }

        [Fact]
        public async Task ExecuteAsync_QuandoPropostaAprovada_DeveCriarContratacao()
        {
            // Arrange
            var propostaId = Guid.NewGuid();
            var request = new ContractPropostRequest(propostaId);
            var proposta = new PropostDto(
                propostaId, "João Silva", "joao@email.com", 10000m, "Vida",
                "Aprovada", DateTime.UtcNow, null); // Status 2 = Aprovada

            _repositoryMock.Setup(x => x.GetByProposalIdAsync(request.PropostId))
                .ReturnsAsync((ContractEntity?)null);

            _propostaClientMock.Setup(x => x.GetPropostAsync(request.PropostId))
                .ReturnsAsync(proposta);

            _repositoryMock.Setup(x => x.CreateAsync(It.IsAny<ContractEntity>()))
                .ReturnsAsync((ContractEntity c) => c);

            // Act
            var result = await _useCase.ExecuteAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(propostaId, result.PropostId);
            Assert.Equal(proposta.CustomerName, result.CustomerName);
            Assert.Equal(proposta.CoverageAmount, result.CoverageAmount);
            Assert.Equal("Aprovada", PropostStatus.Aprovada.ToString());

            _repositoryMock.Verify(x => x.CreateAsync(It.IsAny<ContractEntity>()), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_QuandoContratacaoJaExiste_DeveLancarExcecao()
        {
            // Arrange
            var propostaId = Guid.NewGuid();
            var request = new ContractPropostRequest(propostaId);
            var contratacaoExistente = new ContractEntity(propostaId, "João Silva", 10000m);

            _repositoryMock.Setup(x => x.GetByProposalIdAsync(request.PropostId))
                .ReturnsAsync(contratacaoExistente);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _useCase.ExecuteAsync(request));

            Assert.Equal("This proposal has already been contracted", exception.Message);
        }
    }
}
