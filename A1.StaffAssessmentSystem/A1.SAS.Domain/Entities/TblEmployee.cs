namespace A1.SAS.Domain.Entities
{
    public class TblEmployee: BaseEntity
    {
        public string FullName { get; set; }
        public DateTime? BirthDate { get; set; }
        public short Gender { get; set; }
        public string PartpostId { get; set; }
        public TblRange Range { get; set; }
        public IList<TblAssessment> Assessments { get; set; }
    }
}
