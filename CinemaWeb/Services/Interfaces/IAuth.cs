using CinemaWeb.Entities;
using CinemaWeb.Handle.HandlePagination;
using CinemaWeb.Handle.UserName;
using CinemaWeb.Payloads.DataRequests;
using CinemaWeb.Payloads.DataResponses;
using CinemaWeb.Payloads.Responses;

namespace CinemaWeb.Services.Interfaces
{
    public interface IAuth
    {
        Task<ResponseObject<DataResponsesUser>> Register(Requests_Register requests);
        Task<ResponseObject<DataResponsesUser>> ConfirmNewAcc(Requests_ConfirmEmail requests);
        DataResponsesToken GenerateAccessToken(User user);
        ResponseObject<DataResponsesToken> RestartAccessToKen(Requests_RestartToken requests);
        string GenerateRefreshToken();
        Task<ResponseObject<DataResponsesUser>> ConfirmEmailLink(Requests_RsPass requests);
        Task<ResponseObject<DataResponsesUser>> ResetPasswordconfirmlink(string code, Requests_ChangePass requests1);
        Task<ResponseObject<DataResponsesToken>> LoginAcc(Requests_Login requests);
        IQueryable<DataResponsesUser> GetAllInfomation();
        Task<ResponseObject<DataResponsesUser>> ChangeDecentralization(Requests_ChageDecentralization request);
        Task<ResponseObject<DataResponsesUser>> UpdateUserInformation(int userId, Requests_UpdateUserInformation request);
        Task<ResponseObject<DataResponsesUser>> ChangeYourPassword(int usid, Requests_ChangePass requests);
        Task<PageResult<DataResponsesUser>> GetAllUsers(InputUser input, int pageSize, int pageNumber);
        Task<PageResult<DataResponsesUser>> GetListUserByRank(int pageSize, int pageNumber);
        Task<PageResult<DataResponsesUser>> GetUserByName(string name, int pageSize, int pageNumber);
    }
}
