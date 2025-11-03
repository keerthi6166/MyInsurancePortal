using AutoMapper;
using MyInsurancePortal.AutoMapper;
using MyInsurancePortal.Interfaces;
using MyInsurancePortal.Models;
using MyInsurancePortal.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var loggerFactory = LoggerFactory.Create(config =>
{
    config.AddConsole();
    config.AddDebug();
});

// ? Configure AutoMapper properly
var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.AddProfile<MappingProfile>();
}, loggerFactory);

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);


// DbContext and DI registrations here
builder.Services.AddDbContext<InsuranceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IPolicyService, PolicyServiceRepo>();
builder.Services.AddScoped<ICustomerService, CustomerServiceRepo>();
builder.Services.AddScoped<IClaimService, ClaimServiceRepo>();
builder.Services.AddScoped<IPaymentService, PaymentServiceRepo>();

var app = builder.Build();

// ---------------------------
// 1?? Swagger available always
// ---------------------------
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Insurance Portal API V1");
    //c.RoutePrefix = string.Empty; // Swagger at root: http://localhost:<port>/
});

// ---------------------------
// 2?? Middleware
// ---------------------------
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

//Why AddScoped for Repositories?
//Repository depends on DbContext
//InsuranceDbContext is usually registered as scoped (per HTTP request).
//If you made the repository Transient, it might create multiple DbContext instances in a single request, causing tracking and save issues.
//If you made it Singleton, the repository would share a single DbContext across requests, which is not thread-safe.
//One repository per HTTP request
//Each request (API call) gets its own repository and DbContext.
//Ensures data consistency, transaction safety, and avoids memory leaks.
//Performance-friendly
//Not creating a new repository every time you call it in the same request saves overhead.
//Think of Scoped like a “request notebook”:
//Every API call (request) gets its own notebook.
//You write everything in that notebook during the request.
//When the request ends, the notebook is discarded.
//If it were Singleton, everyone would write in the same notebook, which would get messy.
//If it were Transient, every time you ask for the notebook, you get a new empty one — not efficient for tracking what you already wrote during the request.


