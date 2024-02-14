namespace Core.DTO;

public record ServerResponseDTO
{
    public bool Success { get; set; }
    public string Message { get; set; }
}
