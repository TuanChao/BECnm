using System.Text.RegularExpressions;

namespace CinemaWeb.Handle.Name
{
    public class ValidateName
    {
        public static bool IsValidName(string Name)
        {
            string checkName = @"^[A-Z][a-zA-Z\s]*[A-Z][a-zA-Z\s]*$";
            return Regex.IsMatch(Name, checkName);
        }
    }
}
