using CinemaWeb.Handle.Global;
using CinemaWeb.Payloads.Converters;
using CinemaWeb.Payloads.DataResponses;
using CinemaWeb.Payloads.Responses;
using CinemaWeb.Services.ImPlements;
using CinemaWeb.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
public class Program
{


    public Program(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    public IConfiguration Configuration { get; }
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddScoped<IAuth, AuthServices>();
        builder.Services.AddScoped<ICinema, CinemaServices>();
        builder.Services.AddScoped<ISeat, SeatServices>();
        builder.Services.AddScoped<ITicket, TicketServices>();
        builder.Services.AddScoped<ISchedule, ScheduleServices>();
        builder.Services.AddScoped<IBanner, BannerServices>();
        builder.Services.AddScoped<IRoom, RoomServices>();
        builder.Services.AddScoped<IFood, FoodServices>();
        builder.Services.AddScoped<IPromotion, PromotionServices>();
        builder.Services.AddScoped<IRankCustomer, RankCustomerServices>();
        builder.Services.AddScoped<IMovie, MovieServices>();
        builder.Services.AddScoped<IVNPay, VNPayServices>();
        builder.Services.AddScoped<IBill, BillServices>();

        builder.Services.AddSingleton<ResponseObject<DataResponsesUser>>();
        builder.Services.AddSingleton<ResponseObject<DataResponsesToken>>();
        builder.Services.AddSingleton<ResponseObject<DataResponsesMovieType>>();
        builder.Services.AddSingleton<ResponseObject<DataResponsesCinema>>();
        builder.Services.AddSingleton<ResponseObject<DataResponsesMovie>>();
        builder.Services.AddSingleton<ResponseObject<DataResponsesRoom>>();
        builder.Services.AddSingleton<ResponseObject<DataResponsesFood>>();
        builder.Services.AddSingleton<ResponseObject<DataResponsesBanner>>();
        builder.Services.AddSingleton<ResponseObject<DataResponsesPromotion>>();
        builder.Services.AddSingleton<ResponseObject<DataResponsesRankCustomer>>();
        builder.Services.AddSingleton<ResponseObject<DataResponsesMovie>>();
        builder.Services.AddSingleton<ResponseObject<DataResponsesCinema>>();
        builder.Services.AddSingleton<ResponseObject<DataResponsesSeat>>();
        builder.Services.AddSingleton<ResponseObject<DataResponsesTicket>>();
        builder.Services.AddSingleton<ResponseObject<DataResponsesSchedule>>();
        builder.Services.AddSingleton<ResponseObject<DataResponsesBill>>();
        builder.Services.AddSingleton<ResponseObject<DataResponsesBillFood>>();
        builder.Services.AddSingleton<ResponseObject<DataResponsesBillTicket>>();

        builder.Services.AddScoped<AuthServices>();


        builder.Services.AddSingleton<UserConverter>();
        builder.Services.AddSingleton<TicketConverter>();
        builder.Services.AddSingleton<SeatConverter>();
        builder.Services.AddSingleton<SchedulesConverter>();
        builder.Services.AddSingleton<RoomConverter>();
        builder.Services.AddSingleton<RankCustomerConverter>();
        builder.Services.AddSingleton<BannerConverter>();
        builder.Services.AddSingleton<PromotionConverter>();
        builder.Services.AddSingleton<MovieTypeConverter>();
        builder.Services.AddSingleton<MovieConverter>();
        builder.Services.AddSingleton<CinemaConverter>();
        builder.Services.AddSingleton<FoodConverter>();
        builder.Services.AddSingleton<BillConverter>();
        builder.Services.AddSingleton<BillTicketConverter>();
        builder.Services.AddSingleton<BillFoodConverter>();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddControllers();
        builder.Services.AddRazorPages();
        builder.Services.AddScoped<AuthServices>();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(x =>
        {
            x.AddSecurityDefinition("Auth", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Description = "Example:{Token} ",
                Name = "Authorization",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
            });
            x.OperationFilter<SecurityRequirementsOperationFilter>();
        });
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    builder.Configuration.GetSection("AppSettings:SecretKey").Value!))
            };
        });
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
        });
        Global.DomainName = builder.Configuration["DomainName"];

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
