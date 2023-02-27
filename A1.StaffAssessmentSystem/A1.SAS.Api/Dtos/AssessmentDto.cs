namespace A1.SAS.Api.Dtos
{
    public class AssessmentDto
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public DateTime AssessmentDate { get; set; }
        public string Content { get; set; }
        public bool IsActive { get; set; }
    }
}
