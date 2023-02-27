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
        public DateTime? BirthDate { get; set; }
        public short Gender { get; set; }
        public string PartpostId { get; set; }
        public RangeDto Range { get; set; }
    }
}
