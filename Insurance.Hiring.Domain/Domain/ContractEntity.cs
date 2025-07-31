namespace Insurance.Hiring.Domain.Domain;

public class ContractEntity
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid PropostId { get; private set; }
    public string CustomerName { get; private set; }
    public decimal CoverageAmount { get; private set; }
    public DateTime ContractDate { get; private set; } = DateTime.UtcNow;

    public ContractEntity(Guid propostId, string customerName, decimal coverageAmount)
    {
        if (propostId == Guid.Empty)
            throw new ArgumentException("Proposta ID não pode ser vazio.", nameof(propostId));

        if (string.IsNullOrWhiteSpace(customerName))
            throw new ArgumentNullException(nameof(customerName), "Nome do cliente é obrigatório.");

        if (coverageAmount <= 0)
            throw new ArgumentOutOfRangeException(nameof(coverageAmount), "Valor de cobertura deve ser maior que zero.");

        PropostId = propostId;
        CustomerName = customerName;
        CoverageAmount = coverageAmount;
    }
}
