namespace A1.SAS.Api.Dtos
{
    public class HistoryDto
    {
        public Guid Id { get; set; }
        public string SearchContent { get; set; }
        public string CreatedBy { get; set; }
        public string? CreatedByName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
