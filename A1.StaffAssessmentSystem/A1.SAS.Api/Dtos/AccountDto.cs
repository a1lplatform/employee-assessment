namespace A1.SAS.Api.Dtos
{
    public class AccountDto
    {
        public Guid Id { get; set; }  

        public string AccountCode { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string FullName { get; set; }
    }
}
