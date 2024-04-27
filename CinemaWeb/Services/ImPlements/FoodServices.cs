using CinemaWeb.Entities;
using CinemaWeb.Handle.HandleImage;
using CinemaWeb.Payloads.Converters;
using CinemaWeb.Payloads.DataRequests;
using CinemaWeb.Payloads.DataResponses;
using CinemaWeb.Payloads.Responses;
using CinemaWeb.Services.Interfaces;

namespace CinemaWeb.Services.ImPlements
{
    public class FoodServices : BaseServices, IFood
    {
        private readonly ResponseObject<DataResponsesFood> _responseObjectFood;
        private readonly FoodConverter _foodconverter;
        public FoodServices()
        {
            _foodconverter = new FoodConverter();
            _responseObjectFood = new ResponseObject<DataResponsesFood>();
        }
        public async Task<ResponseObject<DataResponsesFood>> CreateFood(Requests_CreateFood requests)
        {
            if (requests.Price == null ||
               requests.Image == null || requests.Image.Length == 0 ||
               string.IsNullOrEmpty(requests.Description) ||
               string.IsNullOrEmpty(requests.NameOfFood))
            {
                return _responseObjectFood.ResponseFail(StatusCodes.Status400BadRequest, "Chưa Điền Đầy Đủ Thông Tin!!", null);
            }
            Food createfood = new Food
            {
                Price = requests.Price,
                Image = await HandleUploadFileImages.UploadLoadFile(requests.Image),
                Description = requests.Description,
                NameOfFood = requests.NameOfFood,
            };
            _appDbContext.Foods.Add(createfood);
            _appDbContext.SaveChanges();
            return _responseObjectFood.ResponseSucess("Thêm Thông Tin Đồ Ăn Mới Thành Công!!", _foodconverter.ConvertDt(createfood));
        }

        public string DeleteFood(int foodId)
        {
            var deletefood = _appDbContext.Foods.SingleOrDefault(x => x.Id == foodId);
            if (deletefood == null)
            {
                return "Không Tìm Thấy Id Đồ Ăn!!";
            }
            deletefood.IsActive = false;
            _appDbContext.Foods.Update(deletefood);
            _appDbContext.SaveChanges();
            return "Xóa Đồ Ăn Thành Công!!";
        }

        public async Task<ResponseObject<DataResponsesFood>> UpdateFood(Requests_UpdateFood requests)
        {
            var foodupdate = _appDbContext.Foods.SingleOrDefault(x => x.Id == requests.Id);
            if (foodupdate == null)
            {
                return _responseObjectFood.ResponseFail(StatusCodes.Status404NotFound, "Không tồn tại Id Đồ Ăn", null);
            }
            foodupdate.Description = requests.Description;
            foodupdate.Price = requests.Price;
            foodupdate.NameOfFood = requests.NameOfFood;
            foodupdate.Image = await HandleUploadFileImages.UpdateFile(foodupdate.Image, requests.Image);
            _appDbContext.Foods.Update(foodupdate);
            _appDbContext.SaveChanges();
            return _responseObjectFood.ResponseSucess("Cập nhật thông tin đồ ăn thành công", _foodconverter.ConvertDt(foodupdate));
        }
    }
}
