using Insurance.Propost.Application.DTOs;
using Insurance.Propost.Application.UseCase;
using Insurance.Propost.Domain.Entities;
using Insurance.Propost.Domain.Ports;
using Insurance.Propost.Domain.ValueObjects;
using Insurance.Shared.Enums;
using Insurance.Shared.Events;
using Moq;

namespace Insurance.Tests.UseCaseTests
{
    public class ChangePropostStatusUseCaseTests
    {
        private readonly Mock<IPropostRepository> _repositoryMock;
        private readonly Mock<IEventPublisher> _eventPublisherMock;
        private readonly ChangePropostStatusUseCase _useCase;

        public ChangePropostStatusUseCaseTests()
        {
            _repositoryMock = new Mock<IPropostRepository>();
            _eventPublisherMock = new Mock<IEventPublisher>();
            _useCase = new ChangePropostStatusUseCase(_repositoryMock.Object, _eventPublisherMock.Object);
        }

        [Fact]
        public async Task ExecuteAsync_QuandoPropostaNaoExiste_DeveLancarExcecao()
        {
            // Arrange
            var request = new ChangePropostStatusRequest(Guid.NewGuid(), PropostStatus.Aprovada);
            _repositoryMock.Setup(x => x.GetByIdAsync(request.PropostId))
                .ReturnsAsync((PropostEntity?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _useCase.ExecuteAsync(request));

            Assert.Equal("Proposal not found", exception.Message);
        }

        [Fact]
        public async Task ExecuteAsync_QuandoAlteraStatus_DevePublicarEvento()
        {
            // Arrange
            var email = new Email("joao@email.com");
            var proposta = new PropostEntity("João Silva", email, 10000m, "Vida");
            var request = new ChangePropostStatusRequest(proposta.Id, PropostStatus.Aprovada);

            _repositoryMock.Setup(x => x.GetByIdAsync(request.PropostId))
                .ReturnsAsync(proposta);
            _repositoryMock.Setup(x => x.UpdateAsync(It.IsAny<PropostEntity>()))
                .ReturnsAsync((PropostEntity p) => p);

            // Act
            var result = await _useCase.ExecuteAsync(request);

            // Assert
            Assert.Equal(PropostStatus.Aprovada.ToString(), result.Status);

            _repositoryMock.Verify(x => x.UpdateAsync(It.IsAny<PropostEntity>()), Times.Once);
            _eventPublisherMock.Verify(x => x.PublishAsync<PropostStatusChangedEvent>(It.IsAny<PropostStatusChangedEvent>()), Times.Once);

        }
    }
}
