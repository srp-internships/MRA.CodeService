namespace Application.DotNetCodeAnalyzer.DTO
{
    public record CodeAnalyzeOutput
    {
        public bool Success { get; init; }

        public string Errors { get; init; }
        public bool InternalError { get; init; }
    }
}
