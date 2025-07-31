using Insurance.Shared.Enums;

namespace Insurance.Propost.Application.DTOs;

public record ChangePropostStatusRequest(Guid PropostId, PropostStatus NewStatus);
