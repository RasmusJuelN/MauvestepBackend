using API.Interfaces;
using API.Services;
using Database.Interfaces;
using Database.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// CORS (Cross-Origin Resource Sharing)
// Allows API to accept requests from frontend app.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins(
            // "http://localhost:3000", // Local development server
            "https://mauvestepbackend.onrender.com" // Deployed app on render.com
        )
        .AllowAnyHeader() // Allow all HTTP headers (e.g., Authorization, Content-Type)
        .AllowAnyMethod(); // Allow all HTTP methods (e.g., GET, POST, PUT, DELETE)
    });
});

// Configure JWT authentication
// This ensures that only requests with valid JWT tokens can access protected endpoints.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true, // Validate the signing key to ensure the token is authentic
            // Secret key used to sign the token
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"])), 
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true, // Validate token expiration
            ClockSkew = TimeSpan.Zero 
        };
    });


builder.Services.AddDbContext<Database.Context.AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("RenderConnection")));
    // options.UseNpgsql(builder.Configuration.GetConnectionString("LocalhostConnection")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Input JWT token"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Register HTTP Context Accessor (needed to access current user in services)
builder.Services.AddHttpContextAccessor();

// Register Database Services (Repositories)
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<INewsArticleRepository, NewsRepository>();
builder.Services.AddScoped<ISupportTicketRepository, SupportTicketRepository>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
builder.Services.AddScoped<IFAQRepository, FAQRepository>();
builder.Services.AddScoped<IBugReportRepository, BugReportRepository>();
builder.Services.AddScoped<IForumCategoryRepository, ForumCategoryRepository>();
builder.Services.AddScoped<IForumThreadRepository, ForumThreadRepository>();
builder.Services.AddScoped<IForumPostRepository, ForumPostRepository>();
builder.Services.AddScoped<IForumCommentRepository, ForumCommentRepository>();
builder.Services.AddScoped<IPostRatingRepository, PostRatingRepository>();
builder.Services.AddScoped<ICommentRatingRepository, CommentRatingRepository>();
builder.Services.AddScoped<IHighscoreRepository, HighscoreRepository>();

// Register Application Services
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<INewsArticleService, NewsArticleService>();
builder.Services.AddScoped<ISupportTicketService, SupportTicketService>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<IFAQService, FAQService>();
builder.Services.AddScoped<IBugReportService, BugReportService>();
builder.Services.AddScoped<IForumCategoryService, ForumCategoryService>();
builder.Services.AddScoped<IForumThreadService, ForumThreadService>();
builder.Services.AddScoped<IForumPostService, ForumPostService>();
builder.Services.AddScoped<IForumCommentService, ForumCommentService>();
builder.Services.AddScoped<IHighscoreService, HighscoreService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowReactApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
