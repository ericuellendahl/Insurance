using Flurl.Http.Testing;
using Insurance.Hiring.Adapter.ExternalServices;
using Insurance.Shared.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace Insurance.Tests.ServicesTest;
public class PropostServiceClientTests : IDisposable
{
    private readonly HttpTest _httpTest;
    private readonly PropostServiceClient _client;
    private readonly string _baseUrl = "https://localhost:5001";

    public PropostServiceClientTests()
    {
        _httpTest = new HttpTest();

        var configMock = new Mock<IConfiguration>();
        configMock.Setup(x => x["Services:PropostService:BaseUrl"])
       .Returns(_baseUrl);

        var loggerMock = new Mock<ILogger<PropostServiceClient>>();

        _client = new PropostServiceClient(configMock.Object, loggerMock.Object);
    }



    [Fact]
    public async Task GetPropostaAsync_QuandoPropostaExiste_DeveRetornarProposta()
    {
        // Arrange
        var propostaId = Guid.NewGuid();
        var expectedResponse = new
        {
            Id = propostaId,
            CustomerName = "João Silva",
            Email = "joao@email.com",
            CoverageAmount = 10000m,
            InsuranceType = "Vida",
            Status = PropostStatus.Aprovada.ToString(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = (DateTime?)null
        };

        _httpTest.RespondWithJson(expectedResponse);

        // Act
        var result = await _client.GetPropostAsync(propostaId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(propostaId, result.Id);
        Assert.Equal("João Silva", result.CustomerName);
        Assert.True(result.IsApproved());

        _httpTest.ShouldHaveCalled($"{_baseUrl}/api/propost/{propostaId}")
            .WithVerb(HttpMethod.Get);
    }

    [Fact]
    public async Task GetPropostaAsync_QuandoPropostaNaoExiste_DeveLancarExcecao()
    {
        // Arrange
        var propostaId = Guid.NewGuid();
        _httpTest.RespondWith(status: 404);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _client.GetPropostAsync(propostaId));

        Assert.Contains("not found", ex.Message);
    }

    public void Dispose()
    {
        _httpTest.Dispose();
    }

}

