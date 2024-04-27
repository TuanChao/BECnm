using CinemaWeb.Entities;
using CinemaWeb.Payloads.Converters;
using CinemaWeb.Payloads.DataRequests;
using CinemaWeb.Payloads.DataResponses;
using CinemaWeb.Payloads.Responses;
using CinemaWeb.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CinemaWeb.Services.ImPlements
{
    public class SeatServices : BaseServices, ISeat
    {
        private readonly ResponseObject<DataResponsesRoom> responseObject;
        private readonly SeatConverter _seatConverter;
        private readonly RoomConverter _roomConverter;
        private readonly ResponseObject<DataResponsesSeat> _responseObject;
        public SeatServices(ResponseObject<DataResponsesRoom> responseObject, SeatConverter _seatConverter, RoomConverter _roomConverter, ResponseObject<DataResponsesSeat> _responseObject)
        {
            this.responseObject = responseObject;
            this._seatConverter = _seatConverter;
            this._roomConverter = _roomConverter;
            this._responseObject = _responseObject;
        }
        public List<Seat> CreateListSeat(int roomId, List<Requests_CreateSeat> requests)
        {
            var room = _appDbContext.Rooms.SingleOrDefault(x => x.Id == roomId);
            if (room is null)
            {
                return null;
            }
            List<Seat> listseat = new List<Seat>();
            foreach (var request in requests)
            {
                Seat seat = new Seat();
                seat.SeatStatusId = 1;
                seat.Line = request.Line;
                seat.Number = request.Number;
                seat.RoomId = roomId;
                seat.SeatTypeId = request.SeatTypeId;
                seat.IsActive = true;
                listseat.Add(seat);
            }
            _appDbContext.Seats.AddRange(listseat);
            _appDbContext.SaveChanges();
            return listseat;
        }

        public ResponseObject<DataResponsesSeat> CreateSeat(int roomId, Requests_CreateSeat request)
        {
            var room = _appDbContext.Rooms.SingleOrDefault(x => x.Id == roomId);
            if (room == null)
            {
                return _responseObject.ResponseFail(StatusCodes.Status404NotFound, "Không tìm thấy phòng", null);
            }
            Seat seat = new Seat()
            {
                Line = request.Line,
                Number = request.Number,
                SeatTypeId = request.SeatTypeId,
                RoomId = roomId,
                SeatStatusId = 1,
            };
            _appDbContext.Seats.Add(seat);
            _appDbContext.SaveChanges();
            return _responseObject.ResponseSucess("Thêm ghế thành công", _seatConverter.ConvertDt(seat));
        }

        public ResponseObject<DataResponsesRoom> UpdateSeat(int roomId, List<Requests_UpdateSeats> requests)
        {
            var room = _appDbContext.Rooms.Include(x => x.Seats).SingleOrDefault(x => x.Id == roomId);
            if (room == null)
            {
                return responseObject.ResponseFail(StatusCodes.Status404NotFound, "Không tìm thấy phòng", null);
            }

            var seatDict = room.Seats.ToDictionary(s => s.Id, s => s);

            foreach (var request in requests)
            {
                if (!seatDict.TryGetValue(request.SeatId, out var seat))
                {
                    return responseObject.ResponseFail(StatusCodes.Status404NotFound, "Không tìm thấy ghế", null);
                }
                seat.SeatStatusId = request.SeatStatusId;
                seat.RoomId = roomId;
                seat.Number = request.Number;
                seat.Line = request.Line;
                seat.SeatTypeId = request.SeatTypeId;
                _appDbContext.Seats.Update(seat);
            }

            _appDbContext.SaveChanges();
            return responseObject.ResponseSucess("Cập nhật thông tin ghế trong phòng thành công", _roomConverter.ConvertDt(room));
        }

        public string DeleteSeat(int seatId)
        {
            var seatdelete = _appDbContext.Seats.SingleOrDefault(s => s.Id == seatId);
            if (seatdelete is null) { return "Không Tìm Thấy Id Ghế!"; }
            seatdelete.IsActive = false;
            _appDbContext.Seats.Update(seatdelete);
            _appDbContext.SaveChanges();
            return "Xóa Ghế Thành Công!";
        }
    }
}
