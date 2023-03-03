namespace A1.SAS.Api.Dtos
{
    public class AccountDto
    {
        public Guid? Id { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
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
        public RangeDto? Range { get; set; }
        public IList<ImageDtos>? Images { get; set; }
    }
}
