namespace Application.Identity
{
    public record AuthenticationResponse
    {
        public bool Success { get; set; }

        public string Error { get; set; }
    }
}
