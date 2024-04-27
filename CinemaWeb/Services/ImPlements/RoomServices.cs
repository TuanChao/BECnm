using CinemaWeb.Entities;
using CinemaWeb.Handle.Generate;
using CinemaWeb.Payloads.Converters;
using CinemaWeb.Payloads.DataRequests;
using CinemaWeb.Payloads.DataResponses;
using CinemaWeb.Payloads.Responses;
using CinemaWeb.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CinemaWeb.Services.ImPlements
{
    public class RoomServices : BaseServices, IRoom
    {
        private readonly ISeat _seatServices;
        private readonly RoomConverter _roomConverter;
        private readonly ResponseObject<DataResponsesRoom> _responseRoom;
        public RoomServices(ISeat seatServices, ResponseObject<DataResponsesRoom> responseRoom, RoomConverter roomConverter)
        {
            _seatServices = seatServices;
            _responseRoom = responseRoom;
            _roomConverter = roomConverter;
        }

        public List<Room> CreateListRoom(int cinemaId, List<Requests_CreateRoom> requests)
        {
            var cinema = _appDbContext.Cinemas.SingleOrDefault(x => x.Id == cinemaId);
            if (cinema is null)
            {
                return null;
            }

            List<Room> list = new List<Room>();
            foreach (var request in requests)
            {
                Room room = new Room
                {
                    Capacity = request.Capacity,
                    CinemaId = cinemaId,
                    Code = GenerateCode.GenerateCodes(),
                    Description = request.Description,
                    Name = request.Name,
                    Type = request.Type
                };

                _appDbContext.Rooms.Add(room);
                room.Seats = _seatServices.CreateListSeat(room.Id, request.Request_CreateSeats);
                list.Add(room);
            }
            _appDbContext.SaveChanges();

            return list;
        }

        public async Task<ResponseObject<DataResponsesRoom>> CreateRoom(int cinemaId, Requests_CreateRoom requests)
        {
            var cinema = await _appDbContext.Cinemas.SingleOrDefaultAsync(x => x.Id == cinemaId);
            if (cinema == null)
            {
                return _responseRoom.ResponseFail(StatusCodes.Status404NotFound, "Không tìm thấy id của rạp", null);
            }
            Room room = new Room
            {
                Capacity = requests.Capacity,
                CinemaId = cinemaId,
                Code = GenerateCode.GenerateCodes(),
                Description = requests.Description,
                Name = requests.Name,
                Type = requests.Type
            };
            await _appDbContext.Rooms.AddAsync(room);
            await _appDbContext.SaveChangesAsync();
            room.Seats = requests.Request_CreateSeats == null ? null : _seatServices.CreateListSeat(room.Id, requests.Request_CreateSeats);

            return _responseRoom.ResponseSucess("Thêm ghế thành công", _roomConverter.ConvertDt(room));
        }

        public string DeleteRoom(int roomId)
        {
            var roomdelete = _appDbContext.Rooms.SingleOrDefault(r => r.Id == roomId);
            if (roomdelete is null) { return "Không Tìm Thấy Id Phòng!"; }
            roomdelete.IsActive = false;
            _appDbContext.Rooms.Update(roomdelete);
            _appDbContext.SaveChanges();
            return "Xóa Phòng Thành Công!";
        }

        public ResponseObject<DataResponsesRoom> UpdateRoom(Requests_UpdateRoom requests)
        {
            var room = _appDbContext.Rooms.Include(x => x.Seats).AsNoTracking().SingleOrDefault(x => x.Id == requests.roomId);

            if (room == null)
            {
                return _responseRoom.ResponseFail(StatusCodes.Status404NotFound, "Không tìm thấy phòng", null);
            }

            room.Name = requests.Name;
            room.Capacity = requests.Capacity;
            room.Code = GenerateCode.GenerateCodes();
            room.Description = requests.Description;
            room.Type = requests.Type;
            var listSeat = _appDbContext.Seats.Include(x => x.Tickets).AsNoTracking().Where(x => x.RoomId == room.Id).ToList();
            foreach (var seat in listSeat)
            {
                var ticket = _appDbContext.Tickets.Include(x => x.BillTickets).Include(x => x.Schedule).AsNoTracking().Where(x => x.SeatId == seat.Id).ToList();
                _appDbContext.Tickets.RemoveRange(ticket);
                _appDbContext.Seats.Remove(seat);
            }

            room.Seats = requests.Request_UpdateSeats == null ? null : _seatServices.CreateListSeat(room.Id, requests.Request_UpdateSeats);

            _appDbContext.Rooms.Update(room);
            _appDbContext.SaveChanges();

            return _responseRoom.ResponseSucess("Cập nhật thông tin phòng thành công", _roomConverter.ConvertDt(room));
        }
    }
}
