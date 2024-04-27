using CinemaWeb.Entities;
using CinemaWeb.Payloads.DataRequests;
using CinemaWeb.Payloads.DataResponses;
using CinemaWeb.Payloads.Responses;

namespace CinemaWeb.Services.Interfaces
{
    public interface ISeat
    {
        ResponseObject<DataResponsesSeat> CreateSeat(int roomId, Requests_CreateSeat request);
        List<Seat> CreateListSeat(int roomId, List<Requests_CreateSeat> requests);

        ResponseObject<DataResponsesRoom> UpdateSeat(int roomId, List<Requests_UpdateSeats> requests);
        public string DeleteSeat(int seatId);
    }
}
