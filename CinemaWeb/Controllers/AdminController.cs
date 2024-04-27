using CinemaWeb.Payloads.DataRequests;
using CinemaWeb.Payloads.DataResponses;
using CinemaWeb.Payloads.Responses;
using CinemaWeb.Services.ImPlements;
using CinemaWeb.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CinemaWeb.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly ICinema cinemaServices;
        private readonly IRoom roomServices;
        private readonly ISeat seatServices;
        private readonly IMovie movieServices;
        private readonly IFood foodServices;
        private readonly IBanner bannerServices;
        private readonly IPromotion promotionServices;
        private readonly IRankCustomer rankCustomerServices;
        public AdminController(ICinema cinemaServices, IRoom roomServices, ISeat seatServices, IMovie movieServices, IFood foodServices, IBanner bannerServices, IPromotion promotionServices, IRankCustomer rankCustomerServices)
        {
            this.cinemaServices = cinemaServices;
            this.roomServices = roomServices;
            this.seatServices = seatServices;
            this.movieServices = movieServices;
            this.foodServices = foodServices;
            this.bannerServices = bannerServices;
            this.promotionServices = promotionServices;
            this.rankCustomerServices = rankCustomerServices;
        }
        [HttpPost("CreateCinema")]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateCinema(Requests_CreateCinema requests)
        {
            return Ok(cinemaServices.CreateCinema(requests));
        }

        private IActionResult Ok(ResponseObject<DataResponsesCinema> responseObject)
        {
            throw new NotImplementedException();
        }

        [HttpPost("UpdateCinema")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateCinema(Requests_UpdateCinema requests)
        {
            return Ok(cinemaServices.UpdateCinema(requests));
        }
        [HttpPut("DeleteCinema")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteCinema(int cinemaId)
        {
            return Ok(cinemaServices.DeleteCinema(cinemaId));
        }
        [HttpPost("CreateRoom")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRoom(int cinemaId, Requests_CreateRoom requests)
        {
            return Ok(await roomServices.CreateRoom(cinemaId, requests));
        }
        [HttpPost("CreateListRoom")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateListRoom(int cinemaId, List<Requests_CreateRoom> requests)
        {
            return Ok(roomServices.CreateListRoom(cinemaId, requests));
        }
        [HttpPost("UpdateRoom")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateRoom(Requests_UpdateRoom requests)
        {
            return Ok(roomServices.UpdateRoom(requests));
        }
        [HttpPut("DeleteRoom")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteRoom(int roomId)
        {
            return Ok(roomServices.DeleteRoom(roomId));
        }
        [HttpPost("CreateSeat")]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateSeat(int roomId, Requests_CreateSeat requests)
        {
            return Ok(seatServices.CreateSeat(roomId, requests));
        }
        [HttpPost("CreateListSeat")]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateListSeat(int roomId, [FromBody] List<Requests_CreateSeat> requests)
        {
            return Ok(seatServices.CreateListSeat(roomId, requests));
        }
        [HttpPost("UpdateSeat")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateSeat(int roomId, [FromBody] List<Requests_UpdateSeats> requests)
        {
            return Ok(seatServices.UpdateSeat(roomId, requests));
        }
        [HttpPut("DeleteSeat")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteSeat(int seatId)
        {
            return Ok(seatServices.DeleteSeat(seatId));
        }
        [HttpPost("CreateMovie")]
        [Authorize(Roles = "Admin")]
        [Consumes(contentType: "multipart/form-data")]
        public async Task<IActionResult> CreateMovie([FromForm] Requests_CreateMovie requests)
        {
            return Ok(await movieServices.CreateMovie(requests));
        }
        [HttpPost("CreateMovieType")]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateMovieType([FromForm] Requests_CreateMovieType requests)
        {
            return Ok(movieServices.CreateMovieType(requests));
        }
        [HttpPost("CreateRate")]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateRate([FromForm] Requests_CreateRate requests)
        {
            return Ok(movieServices.CreateRate(requests));
        }
        [HttpPost("CreateFood")]
        [Authorize(Roles = "Admin")]
        [Consumes(contentType: "multipart/form-data")]
        public async Task<IActionResult> CreateFood([FromForm] Requests_CreateFood requests)
        {
            return Ok(await foodServices.CreateFood(requests));
        }
        [HttpPut("DeleteMovie")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteMovie([FromBody] int movieId)
        {
            return Ok(movieServices.DeleteMovie(movieId));
        }
        [HttpPut("DeleteMovieType")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteMovieType([FromBody] int movietypeId)
        {
            return Ok(movieServices.DeleteMovieType(movietypeId));
        }
        [HttpPut("DeleteRate")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteRate([FromBody] int rateId)
        {
            return Ok(movieServices.DeleteRate(rateId));
        }
        [HttpPut("DeleteFood")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteFood([FromBody] int foodId)
        {
            return Ok(foodServices.DeleteFood(foodId));
        }
        [HttpPut("UpdateMovie")]
        [Authorize(Roles = "Admin")]
        [Consumes(contentType: "multipart/form-data")]
        public async Task<IActionResult> UpdateMovie([FromForm] Requests_UpdateMovie requests)
        {
            return Ok(await movieServices.UpdateMovie(requests));
        }
        [HttpPut("UpdateMovieType")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateMovieType([FromForm] Requests_UpdateMovieType requests)
        {
            return Ok(movieServices.UpdateMovieType(requests));
        }
        [HttpPut("UpdateRate")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateRate([FromForm] Requests_UpdateRate requests)
        {
            return Ok(movieServices.UpdateRate(requests));
        }
        [HttpPut("UpdateFood")]
        [Authorize(Roles = "Admin")]
        [Consumes(contentType: "multipart/form-data")]
        public async Task<IActionResult> UpdateFood([FromForm] Requests_UpdateFood requests)
        {
            return Ok(await foodServices.UpdateFood(requests));
        }
        [HttpPost("CreateBanner")]
        [Authorize(Roles = "Admin, Censor")]
        [Consumes(contentType: "multipart/form-data")]
        public async Task<IActionResult> CreateBanner(Requests_CreateBanner request)
        {
            return Ok(await bannerServices.CreateBanner(request));
        }
        [HttpDelete("DeleteBanner/{bannerId}")]
        [Authorize(Roles = "Admin, Censor")]
        public async Task<IActionResult> DeleteBanner([FromRoute] int bannerId)
        {
            return Ok(await bannerServices.DeleteBanner(bannerId));
        }
        [HttpGet("GetAllBanners")]
        public async Task<IActionResult> GetAllBanners(int pageSize = 10, int pageNumber = 1)
        {
            return Ok(await bannerServices.GetAllBanners(pageSize, pageNumber));
        }
        [HttpGet("GetBannerById/{bannerId}")]
        public async Task<IActionResult> GetBannerById([FromRoute] int bannerId)
        {
            return Ok(await bannerServices.GetBannerById(bannerId));
        }
        [HttpPut("UpdateBanner")]
        [Authorize(Roles = "Admin, Censor")]
        [Consumes(contentType: "multipart/form-data")]
        public async Task<IActionResult> UpdateBanner(Requests_UpdateBanner request)
        {
            return Ok(await bannerServices.UpdateBanner(request));
        }
        [HttpPost("CreatePromotion")]
        [Authorize(Roles = "Admin, Censor")]
        public async Task<IActionResult> CreatePromotion(Requests_CreatePromotion request)
        {
            return Ok(await promotionServices.CreatePromotion(request));
        }
        [HttpPut("UpdatePromotion")]
        [Authorize(Roles = "Admin, Censor")]
        public async Task<IActionResult> UpdatePromotion(Requests_UpdatePromotion request)
        {
            return Ok(await promotionServices.UpdatePromotion(request));
        }
        [HttpPost("CreateRankCustomer")]
        [Authorize(Roles = "Admin, Censor")]
        public async Task<IActionResult> CreateRankCustomer(Requests_CreateRankCustomer request)
        {
            return Ok(await rankCustomerServices.CreateRankCustomer(request));
        }
        [HttpPut("UpdateRankCustomer")]
        [Authorize(Roles = "Admin, Censor")]
        public async Task<IActionResult> UpdateRankCustomer(Requests_UpdateRankCustomer request)
        {
            return Ok(await rankCustomerServices.UpdateRankCustomer(request));
        }
    }
}
