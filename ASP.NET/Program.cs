using ASP.NET.Models;
using ASP.NET.Security;
using ASP.NET.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(x =>
{
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
})
    .AddNewtonsoftJson(x =>
    {
        x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        x.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
    }); ;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme.",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
        }
    );

    options.OperationFilter<SecurityFilter>();
});

//DB:
var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<TestContext>(options => options.UseSqlServer(connection));

builder
    .Services.AddIdentity<User, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<TestContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthorization();
builder
    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!)
            ),
            ValidateIssuerSigningKey = true,
        };
        //REDIS
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = async context =>
            {
                var token = context.Request.Headers.Authorization.ToString().Replace("Bearer ", "");

                var cacheKey = $"denylist:{token}";
                var cache =
                    context.HttpContext.RequestServices.GetRequiredService<IDistributedCache>();

                var isInDenyList = await cache.GetStringAsync(cacheKey);

                if (!string.IsNullOrEmpty(isInDenyList))
                {
                    context.Fail("Token is revoked");
                }
            },
        };
    });

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPostService, PostService>();

builder.Services.AddScoped<UserManager<User>>();

builder.Services.AddCors(options =>
{

    options.AddDefaultPolicy(
        builder =>
        {

            //you can configure your custom policy
            builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});



var app = builder.Build();
app.UseCors();

//Db init and update
//using var serviceScope = app.Services.CreateScope();
//var dbContext = serviceScope.ServiceProvider.GetService<TestContext>();
//dbContext?.Database.Migrate(); //Migration //ERROR???

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.Use(
    async (context, next) =>
    {
        try
        {
            await next();
        }
        catch (Exception e)
        {
            await IHelper.HandleExceptionAsync(context, e);
        }
    }
);

app.MapControllers();

app.Run();
