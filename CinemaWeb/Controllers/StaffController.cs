using CinemaWeb.Payloads.DataRequests;
using CinemaWeb.Services.ImPlements;
using CinemaWeb.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaWeb.Controllers
{
    [Route("api/staff")]
    [ApiController]
    public class StaffController : Controller
    {
        private readonly ICinema _iCinemaService;
        private readonly IMovie _iMovieService;
        private readonly ISchedule _iScheduleService;
        private readonly IBill _billService;
        private readonly ITicket _ticketService;
        private readonly IMovie _movieServices;
        public StaffController(ICinema _iCinemaService, IMovie _iMovieService, ISchedule _iScheduleService, IBill _billService, IMovie movieServices)
        {
            this._iCinemaService = _iCinemaService;
            this._iMovieService = _iMovieService;
            this._iScheduleService = _iScheduleService;
            this._billService = _billService;
            _movieServices = movieServices;
        }
        [HttpPost("CreateTicket")]
        [Authorize(Roles = "Admin,Censor")]
        public async Task<IActionResult> CreateTicket(int scheduleId, Requests_CreateTicket request)
        {
            return Ok(await _ticketService.CreateTicket(scheduleId, request));
        }
        [HttpPost("UpdateTicket")]
        [Authorize(Roles = "Admin,Censor")]
        public async Task<IActionResult> UpdateTicket(Requests_UpdateTicket request)
        {
            return Ok(await _ticketService.UpdateTicket(request));
        }
        [HttpGet("getListRoomInCinema")]
        [Authorize(Roles = "Admin,Censor")]
        public async Task<IActionResult> GetListRoomInCinema(int pageSize = 10, int pageNumber = 1)
        {
            return Ok(await _iCinemaService.GetListRoomInCinema(pageSize, pageNumber));
        }
        [HttpGet("GetAllMovie")]
        public async Task<IActionResult> GetAllMovie([FromQuery] InputFilter input, int pageSize = 10, int pageNumber = 1)
        {
            return Ok(await _movieServices.GetAllMovie(input, pageSize, pageNumber));
        }
        [HttpGet("GetMovieById")]
        public async Task<IActionResult> GetMovieById(int movieId)
        {
            return Ok(await _movieServices.GetMovieById(movieId));
        }
        [HttpGet("GetFeaturedMovies")]
        public async Task<IActionResult> GetFeaturedMovies(int pageSize = 10, int pageNumber = 1)
        {
            return Ok(await _movieServices.GetFeaturedMovies(pageSize, pageNumber));
        }
        [HttpGet("GetPaymentHistoryByBillId")]
        [Authorize(Roles = "Admin,Censor")]
        public async Task<IActionResult> GetPaymentHistoryByBillId(int billId)
        {
            return Ok(await _billService.GetPaymentHistoryByBillId(billId));
        }
        [HttpGet("GetAllBills")]
        [Authorize(Roles = "Admin,Censor")]
        public async Task<IActionResult> GetAllBills(int pageSize = 10, int pageNumber = 1)
        {
            return Ok(await _billService.GetAllBills(pageSize, pageNumber));
        }

        [HttpGet("SalesStatistics")]
        [Authorize(Roles = "Admin,Censor")]
        public async Task<IActionResult> SalesStatistics([FromQuery] InputStatistic input)
        {
            return Ok(await _billService.SalesStatistics(input));
        }
        [HttpGet("SalesStatisticsFood")]
        [Authorize(Roles = "Admin,Censor")]
        public async Task<IActionResult> SalesStatisticsFood([FromQuery] InputFoodStatistics input)
        {
            return Ok(await _billService.SalesStatisticsFood(input));
        }
        [HttpGet("getSchedulesByMovie")]
        public async Task<IActionResult> GetSchedulesByMovie(int movieId, int pageSize = 10, int pageNumber = 1)
        {
            return Ok(await _iScheduleService.GetSchedulesByMovie(movieId, pageSize, pageNumber));
        }
        [HttpPut("deleteSchedule/{scheduleId}")]
        [Authorize(Roles = "Admin,Censor")]
        public async Task<IActionResult> DeleteSchedule([FromRoute] int scheduleId)
        {
            return Ok(await _iScheduleService.DeleteSchedule(scheduleId));
        }
    }
}
