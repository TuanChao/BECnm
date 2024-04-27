namespace CinemaWeb.Services.Interfaces
{
    public interface IVNPay
    {
        Task<string> CreatePaymentUrl(int billId, HttpContext httpContext, int id);
        Task<string> VNPayReturn(IQueryCollection vnpayData);
    }
}
