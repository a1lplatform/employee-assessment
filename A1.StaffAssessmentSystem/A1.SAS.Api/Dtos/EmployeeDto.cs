namespace A1.SAS.Api.Dtos
{
    public class EmployeeDto : PostEmployeeDto
    {
        public IList<AssessmentDto>? Assessments { get; set; }        
    }
    public class PostEmployeeDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string CCCD { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public DateTime? Birthday { get; set; }
        public short Gender { get; set; }
        public IList<ImageDtos>? Images { get; set; }
    }
}
