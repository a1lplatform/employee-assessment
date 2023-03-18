namespace A1.SAS.Domain.Entities
{
    public class TblImages : BaseEntity
    {
        public string URL { get; set; }
        public Guid? EmployeeId { get; set; }
        public Guid? AccountId { get; set; }
    }
}
