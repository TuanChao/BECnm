using CinemaWeb.Entities;
using CinemaWeb.Payloads.DataResponses;

namespace CinemaWeb.Payloads.Converters
{
    public class FoodConverter
    {
        private readonly AppDbContext _appDbContext;
        public FoodConverter()
        {
            _appDbContext = new AppDbContext();
        }
        public DataResponsesFood ConvertDt(Food food)
        {
            return new DataResponsesFood
            {
                Id = food.Id,
                Price = food.Price,
                Description = food.Description,
                Image = food.Image,
                NameOfFood = food.NameOfFood,
            };
        }
    }
}
