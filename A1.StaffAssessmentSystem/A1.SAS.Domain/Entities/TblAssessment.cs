namespace A1.SAS.Domain.Entities
{
    public class TblAssessment: BaseEntity
    {
        public Guid EmployeeId { get; set; }
        public DateTime AssessmentDate { get; set; }
        public string Content { get; set; }
        public bool IsActive { get; set; }
    }
}
