namespace A1.SAS.Api.Dtos
{
    public class ImageDtos
    {
        public Guid Id { get; set; }
        public Guid? AccountId { get; set; }
        public Guid? EmployeeId { get; set; }
        public string URL { get; set; }
    }
}
