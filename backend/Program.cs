
using Microsoft.EntityFrameworkCore;
using Payment_Idempotency_Service_Backend.Data;
using Payment_Idempotency_Service_Backend.Repository;
using Payment_Idempotency_Service_Backend.Repository.Interfaces;
using Payment_Idempotency_Service_Backend.Service;

namespace Payment_Idempotency_Service_Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddScoped<PaymentService>();
            builder.Services.AddScoped<IdempotencyService>();

            builder.Services.AddScoped<IPaymentRepository,PaymentRepository>();
            builder.Services.AddScoped<IIdempotencyRepository,IdempotencyRepository>();

            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


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
}
