namespace A1.SAS.Domain.Entities
{
    public class TblEmployee: BaseEntity
    {
        public string FullName { get; set; }
        public string CCCD { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public DateTime? Birthday { get; set; }
        public short Gender { get; set; }
    }
}
