
using Azure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System;
using System.Diagnostics;
using System.Text;
using System.Text.Json.Serialization;
using Taxi_Booking.Context;
using Taxi_Booking.Helpers;
using Taxi_Booking.Middlewares;

using Taxi_Booking.Repositories.Drivers;
using Taxi_Booking.Repositories.Passengers;
using Taxi_Booking.Repositories.Vehicles;
using Taxi_Booking.Services.Drivers;
using Taxi_Booking.Services.Passengers;
using Taxi_Booking.Services.Token;
using Taxi_Booking.Services.Vehicles;




namespace Taxi_Booking
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                //This lets you customize how JSON is serialized and deserialized in the request/ response pipeline.
                //By default, enums are serialized as numbers in JSON.
                //This line adds a converter so that enums are serialized as strings instead.
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            }); 
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();



            var ConnectionString= builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<TaxiBookingContext>(options => options.UseSqlServer(ConnectionString));
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:ValidAudience"],
                    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
                };
            });
            //By default, ASP.NET Core API automatically checks ModelState.IsValid after model binding(when it processes the input model). before entring to controller
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddScoped<IDriverRepository, DriverRepository>();
            builder.Services.AddScoped<IDriverService, DriverService>();
            builder.Services.AddScoped<IPassengerRepository, PassengerRepository>();
            builder.Services.AddScoped<IPassengerService, PassengerService>();
            builder.Services.AddScoped<PasswordHasher>();
            builder.Services.AddScoped<ITokenService,TokenService>();
            builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
            builder.Services.AddScoped<IVehicleService, VehicleService>();



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMiddleware<GlobalExceptionHandler>();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHttpsRedirection();
            app.MapControllers();

            app.Run();
        }
    }
}
