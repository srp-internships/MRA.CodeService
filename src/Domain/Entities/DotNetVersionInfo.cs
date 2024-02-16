namespace Domain.Entities
{
    public class DotNetVersionInfo
    {
        public string Version { get; set; }

        public ICollection<string> AvailableMetaData { get; set; } = new List<string>();

        public ICollection<string> DefaultUsings { get; set; } = new List<string>();

        public string Language { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is DotNetVersionInfo netVersionInfo)
            {
                return Version == netVersionInfo.Version && Language == netVersionInfo.Language;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return $"{Version}_{Language}".GetHashCode();
        }
    }
}
