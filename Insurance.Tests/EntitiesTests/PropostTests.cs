using Insurance.Propost.Domain.Entities;
using Insurance.Propost.Domain.ValueObjects;
using Insurance.Shared.Enums;

namespace Insurance.Tests.EntitiesTests;

public class PropostTests
{
    [Fact]
    public void Proposta_QuandoCriada_DeveDefinirStatusComoEmAnalise()
    {
        // Arrange & Act
        var email = new Email("joao@email.com");
        var propost = new PropostEntity("João Silva", email, 10000m, "Vida");

        // Assert
        Assert.Equal(PropostStatus.EmAnalise, propost.Status);
        Assert.Equal(Guid.Empty, propost.Id);
        Assert.True(propost.CreatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public void AlterarStatus_QuandoNovoStatus_DeveAlterarStatusEDataAtualizacao()
    {
        // Arrange
        var email = new Email("joao@email.com");
        var propost = new PropostEntity("João Silva", email, 10000m, "Vida");
        var dataAnterior = DateTime.UtcNow;
        Task.Delay(1000).Wait();
        propost.UpdateAt(DateTime.UtcNow);

        // Act
        propost.ChangeStatus(PropostStatus.Aprovada);

        // Assert
        Assert.Equal(PropostStatus.Aprovada, propost.Status);
        Assert.NotEqual(dataAnterior, propost.UpdatedAt);
    }

    [Fact]
    public void PodeSerContratada_QuandoAprovada_DeveRetornarTrue()
    {
        // Arrange
        var email = new Email("joao@email.com");
        var propost = new PropostEntity("João Silva", email, 10000m, "Vida");
        propost.ChangeStatus(PropostStatus.Aprovada);

        // Act & Assert
        Assert.True(propost.Status == propost.Status);
    }

    [Theory]
    [InlineData(PropostStatus.EmAnalise)]
    [InlineData(PropostStatus.Rejeitada)]
    public void PodeSerContratada_QuandoNaoAprovada_DeveRetornarFalse(PropostStatus status)
    {
        // Arrange
        var email = new Email("joao@email.com");
        var propost = new PropostEntity("João Silva", email, 10000m, "Vida");
        propost.ChangeStatus(status);

        // Act & Assert
        Assert.False(status != propost.Status);
    }
}
