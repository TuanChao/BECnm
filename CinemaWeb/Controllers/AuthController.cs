using CinemaWeb.Handle.UserName;
using CinemaWeb.Payloads.DataRequests;
using CinemaWeb.Services.ImPlements;
using CinemaWeb.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaWeb.Controllers
{
    [Route("/api/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuth _userServices;
        public AuthController(IAuth userServices)
        {
            _userServices = userServices;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Requests_Register requests)
        {
            var result = await _userServices.Register(requests);
            if (result.Status == 400) { return BadRequest(result); }
            else if (result.Status == 404) { return NotFound(result); }
            return Ok(result);

        }
        [HttpPost("confirmNewAcc")]
        public IActionResult ConfirmNewAcc([FromBody] Requests_ConfirmEmail requests)
        {
            return Ok(_userServices.ConfirmNewAcc(requests));
        }
        [HttpPost("renewAccessToken")]
        public IActionResult RestartAccessToKen([FromBody] Requests_RestartToken request)
        {
            return Ok(_userServices.RestartAccessToKen(request));
        }
        [HttpPut("updateUserInformation")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateUserInformation(Requests_UpdateUserInformation request)
        {
            int id = int.Parse(HttpContext.User.FindFirst("UserId").Value);
            return Ok(await _userServices.UpdateUserInformation(id, request));
        }
        [HttpPut("changeDecentralization")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeDecentralization(Requests_ChageDecentralization request)
        {
            return Ok(await _userServices.ChangeDecentralization(request));
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginaAcc([FromBody] Requests_Login requests)
        {
            var result = await _userServices.LoginAcc(requests);
            if (result.Status == 400) { return BadRequest(result); }
            else if (result.Status == 404) { return NotFound(result); }
            else if (result.Status == 500) { return NotFound(result); }
            return Ok(result);
        }
        [HttpPost("confirmemaillink")]
        public async Task<IActionResult> ConfirmEmailusingLink([FromBody] Requests_RsPass requests)
        {
            var result = await _userServices.ConfirmEmailLink(requests);
            return Ok(result);
        }
        [HttpGet("authentication/reset-password/token/{token}/email/{email}/code/{code}")]
        public IActionResult GetLinkfromemail(string token, string email, string code)
        {
            var form = $@"<!DOCTYPE html>
                    <html>
                    <head>
                        <meta name=""viewport"" content=""width-device-width"" />
                        <title>ResetPassword</title>
                    </head>
                    <body>
                    <h1>Reset Password</h1>
                    <form action=""/api/auth/resetpass/{code}"" method=""post"">
                        <input type=""hidden"" name=""Id"" value=""{code}"" />
                        <table>
                            <tr>
                                <td>New Password:</td>
                                <td>
                                    <input type=""password"" name=""NewPassword"" />
                                </td>
                            </tr>
                            <tr>
                                <td>Confirm Password: </td>
                                <td>
                                    <input type=""password"" name=""ConfirmPassword"" />
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <button type=""submit"">Submit</button>
                                </td>
                            </tr>
                        </table>
                    </form>
                </body>
                </html>";

            return Content(form, "text/html");
        }
        [HttpPost("resetpass/{code}")]
        public async Task<IActionResult> ResetPass(string code)
        {
            var newPassword = Request.Form["NewPassword"];
            var confirmPassword = Request.Form["ConfirmPassword"];
            var requests = new Requests_ChangePass
            {
                NewPassword = newPassword,
                ConfirmPassword = confirmPassword
            };
            var result = await _userServices.ResetPasswordconfirmlink(code, requests);
            return Ok(result);
        }
        [HttpGet("getallinformation")]
        [Authorize(Roles = "Admin,Censor")]
        public IActionResult GetAllInformation()
        {
            var results = _userServices.GetAllInfomation();
            return Ok(results);
        }
        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ChangePassword([FromForm] Requests_ChangePass requests)
        {
            var userClaim = HttpContext.User.FindFirst("UserId");
            if (userClaim == null)
            {
                return BadRequest("Không tìm thấy token !!.");
            }
            int id;
            string str = userClaim.Value;
            bool parseResult = int.TryParse(str, out id);
            if (!parseResult)
            {
                return BadRequest("Id không tồn tại!!.");
            }
            return Ok(_userServices.ChangeYourPassword(id, requests));
        }
        [HttpGet("getAllUsers")]
        public async Task<IActionResult> GetAllUsers([FromQuery] InputUser input, int pageSize = 10, int pageNumber = 1)
        {
            return Ok(await _userServices.GetAllUsers(input, pageSize, pageNumber));
        }
        [HttpPost("getListUserByRank")]
        public async Task<IActionResult> GetListUserByRank([FromBody] PaginationInputUser input)
        {
            var result = await _userServices.GetListUserByRank(input.PageSize, input.PageNumber);
            return Ok(result);
        }
        [HttpPost("getUserByName")]
        public async Task<IActionResult> GetUserByName(string name, [FromBody] PaginationInputUser input)
        {
            var result = await _userServices.GetUserByName(name, input.PageSize, input.PageNumber);
            return Ok(result);
        }
    }
}
