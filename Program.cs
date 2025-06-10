
using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Taxi_Booking.Context;
using Taxi_Booking.Helpers;
using Taxi_Booking.Hubs;
using Taxi_Booking.Middlewares;

using Taxi_Booking.Repositories.Drivers;
using Taxi_Booking.Repositories.Passengers;
using Taxi_Booking.Repositories.Rides;
using Taxi_Booking.Repositories.Vehicles;
using Taxi_Booking.Services;
using Taxi_Booking.Services.Drivers;
using Taxi_Booking.Services.Passengers;
using Taxi_Booking.Services.Rides;
using Taxi_Booking.Services.SignarRServices;
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

                //ASP.NET Core uses the System.Text.Json serializer by default, which is case -insensitive when deserializing incoming JSON requests.
                //By default, .NET responds using PascalCase unless you configure it.
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
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

                // //in OnMessageReceived event, you check if the incoming HTTP request has a cookie named "access_token".
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        // If the request is for our hub...
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/taxiBookingHub"))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            //By default, ASP.NET Core API automatically checks ModelState.IsValid after model binding(when it processes the input model). before entring to controller
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            //Registers the CORS policy and gives it a name("AllowAngularApp")
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularApp", policy =>
                {
                    policy.WithOrigins("http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials(); // Important for SignalR!
                });
            });
            builder.Services.AddSignalR();
            builder.Services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
            });

            builder.Services.AddHostedService<DriverLocationBroadcastService>();


            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddScoped<IDriverRepository, DriverRepository>();
            builder.Services.AddScoped<IDriverService, DriverService>();
            builder.Services.AddScoped<IPassengerRepository, PassengerRepository>();
            builder.Services.AddScoped<IPassengerService, PassengerService>();
            builder.Services.AddScoped<PasswordHasher>();
            builder.Services.AddScoped<TaxiBookingHub>();
            builder.Services.AddScoped<ITokenService,TokenService>();
            builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
            builder.Services.AddScoped<IVehicleService, VehicleService>();
            builder.Services.AddScoped<IRideRepository, RideRepository>();
            builder.Services.AddScoped<IRideService, RideService>();
            builder.Services.AddScoped<ISignalRService, SignalRService>();





            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //Applies that policy to the request pipeline.
            app.UseCors("AllowAngularApp");
            app.UseMiddleware<GlobalExceptionHandler>();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            //app.UseHttpsRedirection();
            app.MapControllers();
            app.MapHub<TaxiBookingHub>(pattern: "/taxiBookingHub");


            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapHub<TaxiBookingHub>("/taxiBookingHub");
            //    endpoints.MapControllers();
            //});

            app.Run();
        }
    }
}
