namespace Core.Exceptions
{
    public record ValidationError
    {
        public string PropertyName { get; set; }

        public string ErrorMessage { get; set; }
    }
}
