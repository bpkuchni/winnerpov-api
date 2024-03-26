using Microsoft.EntityFrameworkCore;

using WinnerPOV_API.Database;
using WinnerPOV_API.Providers;

namespace WinnerPOV_API
{
    public class Program
    {
        //https://app.swaggerhub.com/apis-docs/Henrik-3/HenrikDev-API/3.0.0#/
        public static void Main(string[] args)
        {


            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins, policy =>
                {
                    policy.WithOrigins("http://winnerpov.brandonkuchnicki.net", "http://winnerpov.brandonkuchnicki.com", "https://winnerpov.brandonkuchnicki.net", "http://winnerpov.brandonkuchnicki.com");
                });
            });

            // Add services to the container.
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<ValorantContext>();
            builder.Services.AddHttpClient();
            //builder.Services.AddScoped<IValorantApiProvider, HenrikApiProvider>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseSwagger();
            app.UseSwaggerUI();


            app.UseHttpsRedirection();

            app.UseCors();

            app.UseAuthorization();


            app.MapControllers();

            NightlyBatchJob job = new();

            app.Run();
        }
    }
}