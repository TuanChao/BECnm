using CinemaWeb.Entities;
using CinemaWeb.Payloads.DataResponses;

namespace CinemaWeb.Payloads.Converters
{
    public class MovieTypeConverter
    {
        private readonly AppDbContext _appDbContext;
        private readonly MovieConverter _converter;
        public MovieTypeConverter()
        {
            _appDbContext = new AppDbContext();
            _converter = new MovieConverter();
        }
        public DataResponsesMovieType ConvertDt(MovieType movieType)
        {
            return new DataResponsesMovieType
            {
                Id = movieType.Id,
                MovieTypeName = movieType.MovieTypeName,
                Movies = _appDbContext.Movies.Where(x => x.MovieTypeId == movieType.Id).Select(x => _converter.ConvertDt(x)),
            };
        }
    }
}
