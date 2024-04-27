namespace CinemaWeb.Handle.Generate
{
    public class GenerateCode
    {
        public static string GenerateCodes()
        {
            string codetime = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            Random rd = new Random();
            string rdnumber = rd.Next(100000, 999999).ToString();
            string finalcode = codetime + rdnumber;
            return finalcode;
        }
    }
}
