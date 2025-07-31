using Insurance.Propost.Application.DTOs;
using Insurance.Propost.Application.UseCase;
using Insurance.Propost.Domain.Entities;
using Insurance.Propost.Domain.Ports;
using Moq;

namespace Insurance.Tests.UseCaseTests;

public class CreatePropostUseCaseTests
{
    private readonly Mock<IPropostRepository> _repositoryMock;
    private readonly CreatePropostUseCase _useCase;

    public CreatePropostUseCaseTests()
    {
        _repositoryMock = new Mock<IPropostRepository>();
        _useCase = new CreatePropostUseCase(_repositoryMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_QuandoRequestValido_DeveCriarProposta()
    {
        // Arrange
        var request = new CreatePropostRequest("João Silva", "joao@email.com", 10000m, "Vida");

        _repositoryMock.Setup(x => x.CreateAsync(It.IsAny<PropostEntity>()))
            .ReturnsAsync((PropostEntity p) => p);

        // Act
        var result = await _useCase.ExecuteAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(request.CustomerName, result.CustomerName);
        Assert.Equal(request.Email, result.Email);
        Assert.Equal(request.CoverageAmount, result.CoverageAmount);
        Assert.Equal(request.InsuranceType, result.InsuranceType);

        _repositoryMock.Verify(x => x.CreateAsync(It.IsAny<PropostEntity>()), Times.Once);
    }
}