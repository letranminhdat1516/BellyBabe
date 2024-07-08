using Microsoft.EntityFrameworkCore;
using SWP391.DAL.Repositories.ProductRepository;
using SWP391.BLL.Services.ProductServices;
using SWP391.DAL.Swp391DbContext;
using SWP391.BLL.Services.CartServices;
using SWP391.DAL.Repositories.Contract;
using SWP391.BLL.Services.LoginService;
using SWP391.DAL.Repositories.UserRepository;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SWP391.BLL.Services;
using SWP391.DAL.Repositories.OrderRepository;
using SWP391.BLL.Services.OrderServices;
using SWP391.DAL.Repositories.CumulativeScoreRepository;
using SWP391.BLL.Services.CumulativeScoreServices;
using SWP391.DAL.Repositories.BlogCategoryRepository;
using SWP391.DAL.Repositories.BlogRepository;
using SWP391.DAL.Repositories.BrandRepository;
using SWP391.DAL.Repositories.ProductCategoryRepository;
using SWP391.DAL.Repositories.DeliveryRepository;
using SWP391.DAL.Repositories.RatingRepository;
using SWP391.DAL.Repositories.PreOrderRepository;
using SWP391.BLL.Services.PreOrderService;
using SWP391.DAL.Repositories.FeedbackRepository;
using SWP391.BLL.Services.PaymentService;
using AutoMapper;
using SWP391.BLL.Config;
using SWP391.DAL.Entities.payment.Request;
using SWP391.DAL.Entities.payment.Response;
using SWP391.BLL.Mapper;
using SWP391.DAL.Entities;
using SWP391.DAL.Repositories.OrderStatusRepository;
using SWP391.BLL.Services.OrderStatusServices;
using SWP391.DAL.Repositories;
using SWP391.DAL.Services.StatisticsServices;
using SWP391.DAL.Repositories.VoucherRepository;
using SWP391.DAL.Repositories.RatingCategoryRepository;
using SWP391.BLL.Services.RatingCategoryServices;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SWP391.DAL.Repositories.CumulativeScoreTransactionRepository;
using SWP391.BLL.Services.CumulativeScoreTransactionServices;

namespace SWP391.APIs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add database context
            builder.Services.AddDbContext<Swp391Context>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Register repositories
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ProductRepository>();
            builder.Services.AddScoped<OrderRepository>();
            builder.Services.AddScoped<CumulativeScoreRepository>();
            builder.Services.AddScoped<BlogCategoryRepository>();
            builder.Services.AddScoped<BlogRepository>();
            builder.Services.AddScoped<BrandRepository>();
            builder.Services.AddScoped<ProductCategoryRepository>();
            builder.Services.AddScoped<DeliveryRepository>();
            builder.Services.AddScoped<RatingRepository>();
            builder.Services.AddScoped<PreOrderRepository>();
            builder.Services.AddScoped<FeedbackRepository>();
            builder.Services.AddScoped<OrderStatusRepository>();
            builder.Services.AddScoped<CartRepository>();
            builder.Services.AddScoped<StatisticsRepository>();
            builder.Services.AddScoped<VoucherRepository>();    
            builder.Services.AddScoped<RatingCategoryRepository>();
            builder.Services.AddScoped<CumulativeScoreTransactionRepository>();

            // Register services
            builder.Services.AddScoped<EmailService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<OtpService>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<ProductService>();
            builder.Services.AddScoped<CartService>();
            builder.Services.AddScoped<OrderService>();
            builder.Services.AddScoped<CumulativeScoreService>();
            builder.Services.AddScoped<BlogCategoryService>();
            builder.Services.AddScoped<BlogService>();
            builder.Services.AddScoped<BrandService>();
            builder.Services.AddScoped<ProductCategoryService>();
            builder.Services.AddScoped<DeliveryService>();
            builder.Services.AddScoped<RatingService>();
            builder.Services.AddScoped<PreOrderService>();
            builder.Services.AddScoped<FeedbackService>();
            builder.Services.AddScoped<OrderStatusService>();
            builder.Services.AddScoped<ChatService>();
            builder.Services.AddScoped<VoucherService>();
            builder.Services.AddScoped<StatisticsService>();
            builder.Services.AddScoped<CartService>();
            builder.Services.AddScoped<RatingCategoryService>();
            builder.Services.AddScoped<CumulativeScoreTransactionService>();

            builder.Services.AddSignalR();


            builder.Services.AddHttpContextAccessor();


            // Add mapper
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ApplicationMapper());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            builder.Services.AddSingleton(mapper);

            // Get config vnpay from appsettings.json
            builder.Services.Configure<VnpayConfig>(
                builder.Configuration.GetSection(VnpayConfig.ConfigName));

            builder.Services.AddScoped<VnpayService>();
            builder.Services.AddScoped<VnpayPayResponse>();
            builder.Services.AddScoped<VnpayPayRequest>();

            // Add JSON options
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
            });

            // Add JWT authentication
            var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["Jwt:Audience"]
                };
            });
            // CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });
            var app = builder.Build();
            app.UseCors("AllowLocalhost");
            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.MapHub<ChatHub>("/chatHub");
            app.Run();
        }
    }
}
