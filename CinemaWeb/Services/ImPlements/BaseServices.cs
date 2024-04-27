using CinemaWeb.Entities;

namespace CinemaWeb.Services.ImPlements
{
    public class BaseServices
    {
        public readonly AppDbContext _appDbContext;
        public BaseServices()
        {
            _appDbContext = new AppDbContext();
        }
    }
}
