namespace Core.Exceptions
{
    [Serializable]
    public class CompilerApiException : Exception
    {
        public CompilerApiException(string message)
            : base(message)
        {

        }
    }
}
