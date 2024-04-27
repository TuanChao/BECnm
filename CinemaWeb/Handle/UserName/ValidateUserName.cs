using System.Text.RegularExpressions;

namespace CinemaWeb.Handle.UserName
{
    public class ValidateUserName
    {
        public static bool IsValidUser(string userName)
        {
            string checkUserName = @"^[A-Z][a-zA-Z0-9]*$";
            return Regex.IsMatch(userName, checkUserName);
        }
    }
}
