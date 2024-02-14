namespace Core.Extensions
{
    public static class CommonExtensions
    {
        public static string GetDirectoryName(this string path)
        {
            return new DirectoryInfo(path).Name;
        }

        public static string GetDirectoryPathFromFile(this string filePath)
        {
            return new DirectoryInfo(filePath).Parent.FullName;
        }

        public static bool TryGetInnerMessageOfException(this Exception ex, string exceptionName, out string innerMessage)
        {
            innerMessage = string.Empty;
            if (ex.GetType().Name == exceptionName)
            {
                innerMessage = ex.Message;
            }
            var innerException = ex.InnerException;
            while (innerException != null)
            {
                innerException.TryGetInnerMessageOfException(exceptionName, out innerMessage);
                innerException = innerException.InnerException;
            }
            return !string.IsNullOrEmpty(innerMessage);
        }
    }
}
