using Insurance.Hiring.Domain.Domain;
using Insurance.Shared.Enums;

namespace Insurance.Tests.EntitiesTests;

public class ContractTests
{
    [Fact]
    public void Contratacao_QuandoCriada_DeveDefinirPropriedadesCorretamente()
    {
        // Arrange
        var propostId = Guid.NewGuid();
        var CustomerName = "João Silva";
        var CoverageAmount = 10000m;

        // Act
        var contract = new ContractEntity(propostId, CustomerName, CoverageAmount);

        // Assert
        Assert.NotEqual(Guid.Empty, contract.Id);
        Assert.Equal(propostId, contract.PropostId);
        Assert.Equal(CustomerName, contract.CustomerName);
        Assert.Equal(CoverageAmount, contract.CoverageAmount);
        Assert.Equal("Aprovada", PropostStatus.Aprovada.ToString());
        Assert.True(contract.ContractDate <= DateTime.UtcNow);
    }

    [Fact]
    public void Contratacao_QuandoNomeClienteNulo_DeveLancarArgumentNullException()
    {
        // Arrange
        var propostId = Guid.NewGuid();
        var CoverageAmount = 10000m;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new ContractEntity(propostId, null!, CoverageAmount));
    }
}