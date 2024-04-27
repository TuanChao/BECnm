using CinemaWeb.Entities;
using CinemaWeb.Payloads.DataRequests;
using CinemaWeb.Payloads.DataResponses;
using CinemaWeb.Payloads.Responses;

namespace CinemaWeb.Services.Interfaces
{
    public interface ITicket
    {
        Task<ResponseObject<DataResponsesTicket>> CreateTicket(int scheduleId, Requests_CreateTicket request);
        Task<ResponseObject<DataResponsesTicket>> UpdateTicket(Requests_UpdateTicket request);
        List<Ticket> CreateListTicket(int scheduleId, List<Requests_CreateTicket> requests);
    }
}
