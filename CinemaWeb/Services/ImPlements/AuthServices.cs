using BCrypt.Net;
using BCryptpw = BCrypt.Net.BCrypt;
using CinemaWeb.Entities;
using CinemaWeb.Handle.Email;
using CinemaWeb.Handle.Global;
using CinemaWeb.Handle.HandlePagination;
using CinemaWeb.Handle.Name;
using CinemaWeb.Handle.PhoneNumber;
using CinemaWeb.Handle.UserName;
using CinemaWeb.Payloads.Converters;
using CinemaWeb.Payloads.DataRequests;
using CinemaWeb.Payloads.DataResponses;
using CinemaWeb.Payloads.Responses;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using CinemaWeb.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CinemaWeb.Services.ImPlements
{
    public class AuthServices : BaseServices ,IAuth
    {
        private readonly ResponseObject<DataResponsesUser> _responseObject;
        private readonly UserConverter _converter;
        private readonly IConfiguration _configuration;
        private readonly ResponseObject<DataResponsesToken> _responseTokenObject;
        private readonly IHttpContextAccessor _contextAccessor;
        public AuthServices(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _converter = new UserConverter();
            _responseObject = new ResponseObject<DataResponsesUser>();
            _configuration = configuration;
            _contextAccessor = httpContextAccessor;
            _responseTokenObject = new ResponseObject<DataResponsesToken>();
        }
        private string RandomActiveCode()
        {
            Random rd = new Random();
            string character = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string code = new string(Enumerable.Repeat(character, 6).Select(s => s[rd.Next(s.Length)]).ToArray());
            return code;
        }

        public string SendMail(SendEmail e)
        {
            var mail = "mrchaos235@gmail.com";
            var password = "pxaajermhydfzdqg";

            if (!ValidateEmail.IsValidEmail(e.Email))
            {
                return "Định Dạng Email Không Đúng";
            }

            var smtpClient = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password),
            };
            try
            {
                smtpClient.Send(new MailMessage(
                    from: mail,
                    to: e.Email,
                    subject: e.Title,
                    body: e.Body)
                {
                    IsBodyHtml = true
                });

                return "Gửi Mã Về Mail Thành Công!!!";
            }
            catch (Exception ex)
            {
                return "Lỗi Email: " + ex.Message;
            }
        }
        public async Task<ResponseObject<DataResponsesUser>> Register(Requests_Register requests)
        {
            if (string.IsNullOrEmpty(requests.Username) ||
            string.IsNullOrEmpty(requests.Email) ||
                string.IsNullOrEmpty(requests.Name) ||
                string.IsNullOrEmpty(requests.PhoneNumber) ||
                string.IsNullOrEmpty(requests.Password))
            {
                return _responseObject.ResponseFail(StatusCodes.Status400BadRequest, "Thông Tin Cần Nhập Đầy Đủ", null);
            }
            if (_appDbContext.Users.Any(x => x.Email.Equals(requests.Email)))
            {
                return _responseObject.ResponseFail(StatusCodes.Status400BadRequest, "Email Này Đã Tồn Tại", null);
            }
            if (_appDbContext.Users.Any(x => x.Username.Equals(requests.Username)))
            {
                return _responseObject.ResponseFail(StatusCodes.Status400BadRequest, "Username Này Đã Tồn Tại", null);
            }
            if (!ValidateEmail.IsValidEmail(requests.Email))
            {
                return _responseObject.ResponseFail(StatusCodes.Status400BadRequest, "Định Dạng Email Không Đúng", null);
            }
            if (!ValidateUserName.IsValidUser(requests.Username))
            {
                return _responseObject.ResponseFail(StatusCodes.Status400BadRequest, "Định Dạng UserName Không Đúng", null);
            }
            if (!ValidatePhoneNumber.IsValidPN(requests.PhoneNumber))
            {
                return _responseObject.ResponseFail(StatusCodes.Status400BadRequest, "Định Dạng PhoneNumber Không Đúng", null);
            }
            if (!ValidateName.IsValidName(requests.Name))
            {
                return _responseObject.ResponseFail(StatusCodes.Status400BadRequest, "Định Dạng Name không đúng", null);
            }
            User user = new User();
            user.Email = requests.Email;
            user.Name = requests.Name;
            user.PhoneNumber = requests.PhoneNumber;
            user.Password = BCryptpw.HashPassword(requests.Password);
            user.Username = requests.Username;
            user.IsActive = false;
            user.RoleId = 3;
            user.UserStatusId = 1;
            user.Point = 0;
            await _appDbContext.Users.AddAsync(user);
            await _appDbContext.SaveChangesAsync();
            ConfirmEmail confirmEmail = new ConfirmEmail();
            confirmEmail.UserId = user.Id;
            confirmEmail.ExpiredTime = DateTime.UtcNow.AddMinutes(30);
            confirmEmail.ConfirmCode = RandomActiveCode();
            confirmEmail.IsConfirm = false;
            await _appDbContext.ConfirmEmails.AddAsync(confirmEmail);
            await _appDbContext.SaveChangesAsync();
            DataResponsesUser result = _converter.ConvertDt(user);
            string message = SendMail(new SendEmail
            {
                Email = requests.Email,
                Title = "KÍCH HOẠT TÀI KHOẢN MỚI: ",
                Body = "MÃ KÍCH HOẠT:[<strong style='font-weight:900; font-size:25px;'>" + confirmEmail.ConfirmCode + "</strong>] Có hiệu lực 30 phút"
            });
            return _responseObject.ResponseSucess("Đăng Ký Tài Khoản Thành Công!Hãy Kiểm Tra Mail Để Xác Thực Tài Khoản", result);

        }
        public async Task<ResponseObject<DataResponsesUser>> ConfirmNewAcc(Requests_ConfirmEmail requests)
        {
            ConfirmEmail confirmEmail = _appDbContext.ConfirmEmails.Where(x => x.ConfirmCode.Equals(requests.ConfirmCode)).SingleOrDefault();
            if (confirmEmail == null)
            {
                return _responseObject.ResponseFail(StatusCodes.Status400BadRequest, "Mã Xác Thực Không Đúng ,Xác Minh Tài Khoản Thất Bại", null);
            }
            if (confirmEmail.ExpiredTime < DateTime.UtcNow)
            {
                User userdetele = _appDbContext.Users.FirstOrDefault(x => x.Id == confirmEmail.UserId);
                _appDbContext.ConfirmEmails.Remove(confirmEmail);
                _appDbContext.Users.Remove(userdetele);
                _appDbContext.SaveChanges();
                return _responseObject.ResponseFail(StatusCodes.Status400BadRequest, "Mã Xác Thực Hết Hiệu Lực ! Hãy Đăng Kí Lại Tài Khoản (T.T)", null);
            }
            User user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == confirmEmail.UserId);
            user.UserStatusId = 2;
            user.IsActive = true;
            _appDbContext.ConfirmEmails.Remove(confirmEmail);
            _appDbContext.Users.Update(user);
            await _appDbContext.SaveChangesAsync();
            DataResponsesUser result = _converter.ConvertDt(user);
            return _responseObject.ResponseSucess("Xác Thực Tài Khoản Thành Công!", result);
        }
        public string GenerateRefreshToken()
        {
            var random = new byte[64];
            using (var item = RandomNumberGenerator.Create())
            {
                item.GetBytes(random);
                return Convert.ToBase64String(random);
            }
        }
        public DataResponsesToken GenerateAccessToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyByte = System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:SecretKey").Value);

            var role = _appDbContext.Roles.SingleOrDefault(x => x.Id == user.RoleId);
            var tokenDescription = new SecurityTokenDescriptor // mô tả về token
            {
                Subject = new System.Security.Claims.ClaimsIdentity(
                new[]
                {
                    new Claim("UserId",user.Id.ToString()),
                    new Claim("Email",user.Email),
                    new Claim("Name",user.Name),
                    new Claim(ClaimTypes.Role,role?.Code ?? ""),

                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyByte), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = jwtTokenHandler.CreateToken(tokenDescription);//Tạo ra token dựa trên token đã mô tả
            var accessToken = jwtTokenHandler.WriteToken(token);//Security token chuyeen sang string
            var refreshToKen = GenerateRefreshToken();
            RefreshToken rf = new RefreshToken
            {
                Token = refreshToKen,
                ExpiredTime = DateTime.UtcNow.AddHours(2),
                UserId = user.Id,
            };//lamf moi accesstoken khi no het han
            _appDbContext.RefreshTokens.Add(rf);
            _appDbContext.SaveChanges();
            DataResponsesToken result = new DataResponsesToken
            {
                AccessToken = accessToken,
                RefreshToken = refreshToKen,
            };
            return result;
        }

        public ResponseObject<DataResponsesToken> RestartAccessToKen(Requests_RestartToken requests)
        {
            try
            {
                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var secretKey = _configuration.GetSection("AppSettings:SecretKey").Value;

                var tokenValidation = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey))
                };
                var tokenAuthentication = jwtTokenHandler.ValidateToken(requests.AccessToken, tokenValidation, out var validatedToken);
                if (!(validatedToken is JwtSecurityToken jwtSecurityToken) || jwtSecurityToken.Header.Alg != SecurityAlgorithms.HmacSha256)
                {
                    return _responseTokenObject.ResponseFail(StatusCodes.Status400BadRequest, "Token không hợp lệ", null);
                }
                var refreshToken = _appDbContext.RefreshTokens.SingleOrDefault(x => x.Token.Equals(requests.RefreshToKen));
                if (refreshToken == null)
                {
                    return _responseTokenObject.ResponseFail(StatusCodes.Status404NotFound, "RefreshToken không tồn tại trong database", null);
                }
                if (refreshToken.ExpiredTime < DateTime.UtcNow)
                {
                    return _responseTokenObject.ResponseFail(StatusCodes.Status401Unauthorized, "RefreshToken đã hết hạn", null);
                }
                var user = _appDbContext.Users.SingleOrDefault(x => x.Id == refreshToken.UserId);
                if (user is null)
                {
                    return _responseTokenObject.ResponseFail(StatusCodes.Status404NotFound, "Người dùng không tồn tại", null);
                }
                var newToken = GenerateAccessToken(user);
                return _responseTokenObject.ResponseSucess("Token đã được làm mới thành công", newToken);
            }
            catch (SecurityTokenValidationException ex)
            {
                return _responseTokenObject.ResponseFail(StatusCodes.Status400BadRequest, "Lỗi xác thực token: " + ex.Message, null);
            }
            catch (Exception ex)
            {
                return _responseTokenObject.ResponseFail(StatusCodes.Status500InternalServerError, "Lỗi không xác định: " + ex.Message, null);
            }
        }

        public async Task<ResponseObject<DataResponsesToken>> LoginAcc(Requests_Login requests)
        {
            var user = await _appDbContext.Users.SingleOrDefaultAsync(x => x.Username.Equals(requests.UserName));
            bool checkPass = BCryptpw.Verify(requests.Password, user.Password);
            if (string.IsNullOrEmpty(requests.UserName) ||
               string.IsNullOrEmpty(requests.Password))
            {
                return _responseTokenObject.ResponseFail(StatusCodes.Status400BadRequest, "Username và Password đang bị trống!", null);
            }
            if (user == null)
            {
                return _responseTokenObject.ResponseFail(StatusCodes.Status400BadRequest, "Username không tồn tại! T.T", null);
            }
            if (user.UserStatusId == 1) //có thể vào
            {
                return _responseTokenObject.ResponseFail(StatusCodes.Status401Unauthorized, "Tài Khoản của bạn chưa được kích hoạt!", null);
            }
            if (user.IsActive == false)
            {
                return _responseTokenObject.ResponseFail(StatusCodes.Status400BadRequest, "Tài Khoản của bạn đã bị xóa!", null);
            }
            if (!checkPass)
            {
                return _responseTokenObject.ResponseFail(StatusCodes.Status400BadRequest, "Password không chính xác! T.T", null);
            }
            return _responseTokenObject.ResponseSucess("Đăng Nhập Thành Công ^^!", GenerateAccessToken(user));
        }

        public IQueryable<DataResponsesUser> GetAllInfomation()
        {
            var currentUser = _contextAccessor.HttpContext.User;
            if (!currentUser.Identity.IsAuthenticated)
            {
                throw new UnauthorizedAccessException("Không xác định được người dùng!!!");
            }
            if (!currentUser.IsInRole("Admin") && !currentUser.IsInRole("Censor"))
            {
                throw new UnauthorizedAccessException("Không đủ điều kiện để thực hiện chức năng này!!!!");
            }
            var result = _appDbContext.Users.Select(x => _converter.ConvertDt(x));
            return result;
        }

        public async Task<ResponseObject<DataResponsesUser>> ChangeYourPassword(int usid, Requests_ChangePass requests)
        {
            var user = await _appDbContext.Users.SingleOrDefaultAsync(x => x.Id == usid);
            if (!requests.NewPassword.Equals(requests.ConfirmPassword))
            {
                return _responseObject.ResponseFail(StatusCodes.Status400BadRequest, "Mật khẩu không trùng nhau!", null);
            }
            user.Password = BCryptpw.HashPassword(requests.NewPassword);
            _appDbContext.Users.Update(user);
            await _appDbContext.SaveChangesAsync();
            return _responseObject.ResponseSucess("Đổi Mật Khẩu Thành Công!!!", _converter.ConvertDt(user));
        }


        private string LinkActive(User user, string cfc)
        {
            DataResponsesToken response = GenerateAccessToken(user);
            string accessToken = response.AccessToken;
            string url = Global.DomainName + "api/auth/authentication/reset-password/token/" + accessToken + "/email/" + user.Email + "/code/" + cfc;
            string form = $@"<div style=""text-align: center;"">
                            <h2 style=""color: #3b4151;
                                       font-family: sans-serif;
                                       font-size: 36px;
                                       margin: 0;"">Forgot Password</h2>
                            <h3 style=""color: #3b4151;
                                       font-family: sans-serif;
                                       font-size: 24px;
                                       margin: 0;"">Click The Button Below To Confirm Password Change</h3>
                            <form method=""get"" action=""{url}"" style=""display: inline;"">
                                <button type=""submit"" style=""text-align: center;
                                                             font-weight: 700;
                                                             background-color: #4990e2;
                                                             box-shadow: 0 1px 2px rgba(0,0,0,.1);
                                                             border-color: #4990e2;
                                                             font-size: 14px;
                                                             line-height: 1.15;
                                                             border-radius: 4px;
                                                             color: #ffffff;
                                                             font-family: sans-serif;
                                                             cursor: pointer;
                                                             transition: all .3s;
                                                             padding: 8px 40px;"">
                                    Confirm Email
                                </button>
                            </form>
                        </div>
                        ";

            return form;
        }

        public async Task<ResponseObject<DataResponsesUser>> ConfirmEmailLink(Requests_RsPass requests)
        {
            if (string.IsNullOrEmpty(requests.Email))
            {
                return _responseObject.ResponseFail(StatusCodes.Status400BadRequest, "Thông Tin Cần Nhập Đầy Đủ", null);
            }
            var user = _appDbContext.Users.FirstOrDefault(x => x.Email.Equals(requests.Email));
            if (user is null)
            {
                return _responseObject.ResponseFail(StatusCodes.Status400BadRequest, "Email Này Không Tồn Tại", null);
            }
            ConfirmEmail confirmEmail = new ConfirmEmail();
            confirmEmail.UserId = user.Id;
            confirmEmail.ExpiredTime = DateTime.UtcNow.AddMinutes(30);
            confirmEmail.ConfirmCode = RandomActiveCode();
            string cfc = confirmEmail.ConfirmCode;
            confirmEmail.IsConfirm = false;
            await _appDbContext.ConfirmEmails.AddAsync(confirmEmail);
            await _appDbContext.SaveChangesAsync();
            string message = SendMail(new SendEmail
            {
                Email = requests.Email,
                Title = "QUÊN MẬT KHẨU: Hiệu Lực 30 Phút",
                Body = LinkActive(user, cfc),
            });
            DataResponsesUser result = _converter.ConvertDt(user);
            return _responseObject.ResponseSucess("Đã Gửi Link Qua Mail ! Hãy Kiểm Tra Mail ", result);

        }

        public async Task<ResponseObject<DataResponsesUser>> ResetPasswordconfirmlink(string code, Requests_ChangePass requests1)
        {
            ConfirmEmail cfe = _appDbContext.ConfirmEmails.Where(x => x.ConfirmCode.Equals(code)).FirstOrDefault();
            if (cfe == null) { return _responseObject.ResponseFail(StatusCodes.Status400BadRequest, "Cần Phải nhấn vào link ở email!!", null); }
            if (cfe.ExpiredTime < DateTime.UtcNow)
            {
                return _responseObject.ResponseFail(StatusCodes.Status400BadRequest, "Link Đã Hết Thời Hạn!!", null);
            }
            User user = _appDbContext.Users.FirstOrDefault(x => x.Id == cfe.UserId);
            if (requests1.NewPassword != requests1.ConfirmPassword)
            {
                return _responseObject.ResponseFail(StatusCodes.Status400BadRequest, "Mật Khẩu Không Trùng Nhau!!", null);
            }
            var confirmEmailsToDelete = _appDbContext.ConfirmEmails.Where(x => x.UserId == user.Id);
            _appDbContext.ConfirmEmails.RemoveRange(confirmEmailsToDelete);
            user.Password = BCryptpw.HashPassword(requests1.NewPassword);
            _appDbContext.Users.Update(user);
            await _appDbContext.SaveChangesAsync();
            DataResponsesUser result = _converter.ConvertDt(user);
            return _responseObject.ResponseSucess("Cập Nhập Lại Mật Khẩu Thành Công!", result);
        }


        public async Task<PageResult<DataResponsesUser>> GetAllUsers(InputUser input, int pageSize, int pageNumber)
        {
            var query = await _appDbContext.Users.Include(x => x.RankCustomer).AsNoTracking().OrderBy(x => x.RankCustomer.Point).Where(x => x.IsActive == true).ToListAsync();
            if (!string.IsNullOrEmpty(input.Name))
            {
                query = query.Where(x => x.Name.ToLower().Contains(input.Name.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(input.Email))
            {
                query = query.Where(x => x.Email.ToLower().Contains(input.Email.ToLower())).ToList();
            }
            if (input.RoleId.HasValue)
            {
                query = query.Where(x => x.RoleId == input.RoleId).ToList();
            }
            var result = query.Select(x => _converter.ConvertDt(x)).AsQueryable();
            var data = Pagination.GetPagedData(result, pageSize, pageNumber);
            return data;
        }

        public async Task<PageResult<DataResponsesUser>> GetListUserByRank(int pageSize, int pageNumber)
        {
            var query = _appDbContext.Users.Include(x => x.RankCustomer).AsNoTracking().OrderBy(x => x.RankCustomer.Point).Select(x => _converter.ConvertDt(x));
            var result = Pagination.GetPagedData(query, pageSize, pageNumber);
            return result;
        }

        public async Task<PageResult<DataResponsesUser>> GetUserByName(string name, int pageSize, int pageNumber)
        {
            var query = _appDbContext.Users.Where(x => x.Name.Equals(name)).Select(x => _converter.ConvertDt(x));
            var result = Pagination.GetPagedData(query, pageSize, pageNumber);
            return result;
        }

        public async Task<ResponseObject<DataResponsesUser>> ChangeDecentralization(Requests_ChageDecentralization request)
        {
            var user = await _appDbContext.Users.SingleOrDefaultAsync(x => x.Id == request.UserId);
            if (user is null)
            {
                return _responseObject.ResponseFail(StatusCodes.Status404NotFound, "Không tìm thấy id người dùng", null);
            }
            var currentUser = _contextAccessor.HttpContext.User;

            if (!currentUser.Identity.IsAuthenticated)
            {
                return _responseObject.ResponseFail(StatusCodes.Status401Unauthorized, "Người dùng không được xác thực hoặc không được xác định", null);
            }

            if (!currentUser.IsInRole("Admin"))
            {
                return _responseObject.ResponseFail(StatusCodes.Status403Forbidden, "Không đủ quyền sử dụng chức năng này", null);
            }
            user.RoleId = request.RoleId;
            _appDbContext.Users.Update(user);
            await _appDbContext.SaveChangesAsync();
            return _responseObject.ResponseSucess("Thay đổi quyền người dùng thành công", _converter.ConvertDt(user));
        }

        public async Task<ResponseObject<DataResponsesUser>> UpdateUserInformation(int userId, Requests_UpdateUserInformation request)
        {
            var user = await _appDbContext.Users.SingleOrDefaultAsync(x => x.Id == userId);

            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.PhoneNumber) || string.IsNullOrWhiteSpace(request.Email))
            {
                return _responseObject.ResponseFail(StatusCodes.Status400BadRequest, "Vui lòng điền đầy đủ thông tin", null);
            }
            if (!ValidateEmail.IsValidEmail(request.Email))
            {
                return _responseObject.ResponseFail(StatusCodes.Status400BadRequest, "Định dạng email không hợp lệ", null);
            }
            if (!ValidatePhoneNumber.IsValidPN(request.PhoneNumber))
            {
                return _responseObject.ResponseFail(StatusCodes.Status400BadRequest, "Định dạng số điện thoại không hợp lệ", null);
            }
            user.Username = request.Username;
            user.Name = request.Name;
            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;
            _appDbContext.Users.Update(user);
            await _appDbContext.SaveChangesAsync();
            return _responseObject.ResponseSucess("Cập nhật thông tin thành công", _converter.ConvertDt(user));
        }
    }
}
