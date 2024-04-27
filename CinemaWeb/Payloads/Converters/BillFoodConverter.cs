using CinemaWeb.Entities;
using CinemaWeb.Payloads.DataResponses;

namespace CinemaWeb.Payloads.Converters
{
    public class BillFoodConverter
    {
        private readonly AppDbContext _context;
        public BillFoodConverter()
        {
            _context = new AppDbContext();
        }
        public DataResponsesBillFood ConvertDt(BillFood billFood)
        {
            return new DataResponsesBillFood
            {
                Id = billFood.Id,
                NameOfFood = _context.Foods.SingleOrDefault(x => x.Id == billFood.FoodId).NameOfFood,
                Quantity = billFood.Quantity
            };
        }
    }
}
