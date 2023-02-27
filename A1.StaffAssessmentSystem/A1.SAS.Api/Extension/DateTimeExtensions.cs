namespace A1.SAS.Api.Extension
{
    public static class DateTimeExtensions
    {
        public static int ToUnixTime(this DateTime input)
        {
            return (int)input.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }
    }
}
