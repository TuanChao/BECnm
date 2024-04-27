using CinemaWeb.Entities;
using CinemaWeb.Payloads.DataRequests;
using CinemaWeb.Payloads.DataResponses;
using CinemaWeb.Payloads.Responses;

namespace CinemaWeb.Services.Interfaces
{
    public interface IRoom
    {
        Task<ResponseObject<DataResponsesRoom>> CreateRoom(int cinemaId, Requests_CreateRoom requests);
        List<Room> CreateListRoom(int cinemaId, List<Requests_CreateRoom> requests);
        ResponseObject<DataResponsesRoom> UpdateRoom(Requests_UpdateRoom requests);

        string DeleteRoom(int roomId);
    }
}
