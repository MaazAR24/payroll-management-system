using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PayrollManagementSystem.Data;
using PayrollManagementSystem.Repositories;
using PayrollManagementSystem.Services.Authentication;
using PayrollManagementSystem.Services.Employees;
using PayrollManagementSystem.Services.FileUploader;
using PayrollManagementSystem.Services.Requests;
using PayrollManagementSystem.Services.Salaries;
using PayrollManagementSystem.Services.Users;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    {
        c.AddSecurityDefinition("Bearer",
            new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Please enter Jwt like Bearer{space} token",
            });
        c.AddSecurityRequirement(
            new OpenApiSecurityRequirement 
            {
                { 
                    new OpenApiSecurityScheme
                    { 
                        Reference = new OpenApiReference
                        { 
                            Type = ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[]{ }
                }
            });
    }
);

builder.Services.AddDbContext<PayrollContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<ISalaryRepository, SalaryRepository>();
builder.Services.AddScoped<IRequestRepository, RequestRepository>();
builder.Services.AddScoped<IFileRepository, FileRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IFileService, FileService>();

// Register services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISalaryService, SalaryService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IRequestService, RequestService>();

builder.Services.AddScoped<IAuthService, AuthService>();


var key = Encoding.ASCII.GetBytes(builder.Configuration["SecretKey"]);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
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

app.UseStaticFiles();

app.MapControllers();

app.Run();
