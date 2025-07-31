using Hellang.Middleware.ProblemDetails;
using Insurance.Propost.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});

builder.Services.AddPropostPersistence(builder.Configuration);
builder.Services.AddPropostApplication();
builder.Services.AddPropostValidation();
builder.Services.AddMessaging(builder.Configuration);
builder.Services.AddCustomProblemDetails(builder.Environment);

var app = builder.Build();

app.UseProblemDetails();
app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
