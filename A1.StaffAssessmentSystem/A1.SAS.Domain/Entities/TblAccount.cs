namespace A1.SAS.Domain.Entities
{
    public class TblAccount : BaseEntity
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public int Point { get; set; }
        public string RoleName { get; set; }
        public string FullName { get; set; }
        public string CCCD { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public DateTime? Birthday { get; set; }
        public short Gender { get; set; }
        public Guid RangeId { get; set; }
    }
}
