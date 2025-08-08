using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Web_API.Data;
using Web_API.Services;
using Web_API.Services.Interfaces;
using Web_API.Repositories;
using Web_API.Repositories.Interfaces;
using Web_API.Security.Handlers;
using Web_API.Security.Requirements;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<LoyaltyCardContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Repository interfaces
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ILoyaltyCardRepository, LoyaltyCardRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IRewardRepository, RewardRepository>();
builder.Services.AddScoped<IShopRepository, ShopRepository>();

// Register Service interfaces
builder.Services.AddScoped<ILoyaltyCardService, LoyaltyCardService>();
builder.Services.AddScoped<ITransactionService, LoyaltyTransactionService>();
builder.Services.AddScoped<IRewardService, RewardService>();
builder.Services.AddScoped<IShopService, ShopService>();
builder.Services.AddScoped<ISystemService, SystemService>();

// Security services (interfaces created but implementations not yet done)
// builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
// builder.Services.AddScoped<IApiKeyService, ApiKeyService>();

// Configure Authorization
builder.Services.AddAuthorization(options =>
{
    // Define Admin policy
    options.AddPolicy("AdminOnly", policy =>
        policy.Requirements.Add(new AdminRequirement()));
    
    // Define Shop policy for API key authentication
    options.AddPolicy("ShopOnly", policy =>
        policy.Requirements.Add(new RoleRequirement("Shop")));
    
    // Define Customer ownership policy
    options.AddPolicy("CustomerOwnership", policy =>
        policy.Requirements.Add(new CustomerOwnershipRequirement()));
});

// Register Authorization Handlers
builder.Services.AddScoped<IAuthorizationHandler, AdminAuthorizationHandler>();
builder.Services.AddScoped<IAuthorizationHandler, RoleAuthorizationHandler>();
builder.Services.AddScoped<IAuthorizationHandler, CustomerOwnershipHandler>();

// Add Authentication configuration
if (builder.Environment.IsDevelopment())
{
    // Development authentication for testing
    builder.Services.AddAuthentication("Development")
        .AddScheme<DevelopmentAuthenticationSchemeOptions, DevelopmentAuthenticationHandler>("Development", options => { });
}
else
{
    // Production authentication (placeholder)
    builder.Services.AddAuthentication();
}

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    if (builder.Environment.IsDevelopment())
    {
        c.AddSecurityDefinition("DevelopmentAuth", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Name = "X-Test-Role",
            Description = "For testing: Use 'Admin', 'Customer', or 'Shop'"
        });
        
        c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
        {
            {
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference
                    {
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                        Id = "DevelopmentAuth"
                    }
                },
                new string[] { }
            }
        });
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
