using CinemaWeb.Payloads.DataRequests;
using CinemaWeb.Services.ImPlements;
using CinemaWeb.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaWeb.Controllers
{
    [Route("api/Member")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly ICinema _iCinemaService;
        private readonly ISchedule _iScheduleServices;
        private readonly IVNPay _vNPayServices;
        private readonly IBill _billService;
        private readonly IPromotion _promotionService;
        private readonly IMovie _movieService;
        public UserController(ICinema _iCinemaService, ISchedule iScheduleServices, IVNPay vNPayServices, IBill _billService, IPromotion promotionService, IMovie movieService)
        {
            this._iCinemaService = _iCinemaService;
            _iScheduleServices = iScheduleServices;
            _vNPayServices = vNPayServices;
            this._billService = _billService;
            _promotionService = promotionService;
            _movieService = movieService;
        }
        [HttpGet("getCinemaByMovie")]
        public async Task<IActionResult> GetCinemaByMovie(int movieId, int pageSize = 10, int pageNumber = 1)
        {
            return Ok(await _iCinemaService.GetCinemaByMovie(movieId, pageSize, pageNumber));
        }
        [HttpGet("getListCinema")]
        [Authorize(Roles = "Admin,Member")]
        public async Task<IActionResult> GetListCinema(int pageNumber = 1)
        {
            int pageSize = -1;
            return Ok(await _iCinemaService.GetListCinema(pageSize, pageNumber));
        }
        [HttpPost("createSchedule")]
        [Authorize(Roles = "Admin, Manager, Staff, Member")]
        public async Task<IActionResult> CreateSchedule(Requests_CreateSchedule request)
        {
            return Ok(await _iScheduleServices.CreateSchedule(request));
        }
        [HttpPut("updateSchedule")]
        [Authorize(Roles = "Admin, Manager, Staff, Member")]
        public async Task<IActionResult> UpdateSchedule(Requests_UpdateSchedule request)
        {
            return Ok(await _iScheduleServices.UpdateSchedule(request));
        }
        [HttpGet("getAllSchedules")]
        public async Task<IActionResult> GetAllSchedules([FromQuery] InputScheduleData input, int pageSize = 10, int pageNumber = 1)
        {
            return Ok(await _iScheduleServices.GetAlls(input, pageSize, pageNumber));
        }
        [HttpGet("getSchedulesByDay")]
        public async Task<IActionResult> GetSchedulesByDay(DateTime startAt, int pageSize = 10, int pageNumber = 1)
        {
            return Ok(await _iScheduleServices.GetSchedulesByDay(startAt, pageSize, pageNumber));
        }
        [HttpPost("CreateListBillTicket")]
        [Authorize(Roles = "Admin,Censor, Member")]
        public async Task<IActionResult> CreateListBillTicket(int billId, List<Requests_CreateBillTicket> requests)
        {
            return Ok(await _billService.CreateListBillTicket(billId, requests));
        }
        [HttpPost("CreateListBillFood")]
        [Authorize(Roles = "Admin,Censor,Member")]
        public async Task<IActionResult> CreateListBillFood(int billId, List<Requests_CreateBillFood> requests)
        {
            return Ok(await _billService.CreateListBillFood(billId, requests));
        }
        [HttpPost("CreateBillTicket")]
        [Authorize(Roles = "Admin,Censor,Member")]
        public async Task<IActionResult> CreateBillTicket(int billId, Requests_CreateBillTicket request)
        {
            return Ok(await _billService.CreateBillTicket(billId, request));
        }
        [HttpPost("CreateBillFood")]
        [Authorize(Roles = "Admin,Censor,Member")]
        public async Task<IActionResult> CreateBillFood(int billId, Requests_CreateBillFood request)

        {
            return Ok(await _billService.CreateBillFood(billId, request));
        }
        [HttpPost("CreateBill")]
        [Authorize(Roles = "Admin,Censor,Member")]
        public async Task<IActionResult> CreateBill(Requests_CreateBill request)
        {
            return Ok(await _billService.CreateBill(request));
        }
        [HttpPost]
        [Route("/Vnpay/CreatePaymentUrl")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreatePaymentUrl(int billId)
        {
            int id = int.Parse(HttpContext.User.FindFirst("UserId").Value);
            return Ok(await _vNPayServices.CreatePaymentUrl(billId, HttpContext, id));
        }
        [HttpGet]
        [Route("/Vnpay/return")]
        public async Task<IActionResult> Return()
        {
            var vnpayData = HttpContext.Request.Query;

            return Ok(await _vNPayServices.VNPayReturn(vnpayData));
        }
        [HttpGet("GetAllPromotions")]
        public async Task<IActionResult> GetAllPromotions(int pageSize = 10, int pageNumber = 1)
        {
            return Ok(await _promotionService.GetAllPromotions(pageSize, pageNumber));
        }
        [HttpGet("GetAllMovieTypes")]
        public async Task<IActionResult> GetAllMovieTypes(int pageSize = 10, int pageNumber = 1)
        {
            return Ok(await _movieService.GetAllMovieTypes(pageSize, pageNumber));
        }
        [HttpGet("GetMovieTypeById/{movieTypeId}")]
        public async Task<IActionResult> GetMovieTypeById([FromRoute] int movieTypeId)
        {
            return Ok(await _movieService.GetMovieTypeById(movieTypeId));
        }

    }
}
