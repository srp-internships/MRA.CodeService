namespace Domain.Entities
{
    public class ApiKey : IDbEntity
    {
        public string SecretKey { get; set; }

        public Guid Guid { get; set; }

        public virtual Company Company { get; set; }

        public Guid? CompanyGuid { get; set; }
    }
}
