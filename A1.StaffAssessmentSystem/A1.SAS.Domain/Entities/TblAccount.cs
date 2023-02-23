namespace A1.SAS.Domain.Entities
{
    public class TblAccount : BaseEntity
    {
        public string AccountCode { get; set; }

        public string Username { get; set; }

        public string PasswordHash { get; set; }

        public string FullName { get; set; }
    }
}
