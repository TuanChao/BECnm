namespace CinemaWeb.Payloads.DataResponses
{
    public class DataResponsesToken 
    {
        // Khi Đăng Nhập sẽ trả về access token.Acesstoken để giải mã thông tin có thể để tùy theo tgian mình đặt
        public string AccessToken { get; set; }

        //reset token sinh ra token mới để tránh bị log ra lưu trong database , tgian sẽ dài hơn accesstoken để đc reset
        public string RefreshToken { get; set; } 
        public DataResponsesUser DataResponseUser { get; set; }
    }
}
