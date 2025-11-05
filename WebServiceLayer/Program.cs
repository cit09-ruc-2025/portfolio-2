using System.Text;
using DataServiceLayer.Services;
using DataServiceLayer.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using WebServiceLayer.Auth_Middleware;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        builder =>
        {
            builder.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
        });
});

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var secret = builder.Configuration.GetSection("Auth:Secret").Value;

builder.Services.AddScoped<IUserService>(provider =>
    new UserService(connectionString));

builder.Services.AddScoped<IReviewService>(provider =>
    new ReviewService(connectionString));

builder.Services.AddScoped<IMediaService>(provider =>
    new MediaService(connectionString));

builder.Services.AddScoped<IPlaylistService>(provider =>
    new PlaylistService(connectionString));

builder.Services.AddScoped<IWatchHistoryService>(provider =>
    new WatchHistoryService(connectionString));

builder.Services.AddSingleton<IGenreService>(new GenreService(connectionString));

builder.Services.AddScoped<IFavoriteService>(provider =>
    new FavoriteService(connectionString));

builder.Services.AddScoped<IPeopleService>(provider =>
    new PeopleService(connectionString));

builder.Services.AddScoped<IMediaGenreService>(provider =>
    new MediaGenreService(connectionString));

builder.Services.AddScoped<ISimilarMovieService>(provider =>
    new SimilarMovieService(connectionString));

builder.Services.AddScoped<IActorService>(provider =>
    new ActorService(connectionString));

builder.Services.AddScoped<ISearchHistoryService>(provider =>
    new SearchHistoryService(connectionString));

builder.Services.AddScoped<IRecentlyVisited>(provider =>
    new RecentlyVisitedService(connectionString));

builder.Services.AddMapster();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorizationBuilder()
                    .AddPolicy("SameUser", policy => policy.Requirements.Add(new SameUserRequirement()));

builder.Services.AddSingleton<IAuthorizationHandler, SameUserHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
