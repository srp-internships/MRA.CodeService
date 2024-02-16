namespace Domain.Entities
{
    public class Company : IDbEntity
    {
        public Guid Guid { get; set; }

        public string Name { get; set; }

        public string Host { get; set; }

        public virtual ApiKey ApiKey { get; set; }
    }
}
