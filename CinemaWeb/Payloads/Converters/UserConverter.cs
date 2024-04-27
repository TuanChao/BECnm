using CinemaWeb.Entities;
using CinemaWeb.Payloads.DataResponses;

namespace CinemaWeb.Payloads.Converters
{
    public class UserConverter
    {
        private readonly AppDbContext _context;
        public UserConverter()
        {
            _context = new AppDbContext();
        }
        public DataResponsesUser ConvertDt(User user)
        {
            return new DataResponsesUser
            {
                Id = user.Id,
                Point = user.Point,
                Username = user.Username,
                Email = user.Email,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                Password = user.Password,
                RoleName = _context.Roles.FirstOrDefault(x => x.Id == user.RoleId).RoleName,
            };
        }
    }
}
