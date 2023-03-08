using ExchangeRatePreviewer.Core.Interfaces.Services;
using ExchangeRatePreviewer.Core.Interfaces.Services.SOAP;
using ExchangeRatePreviewer.Core.Interfaces.Services.Validators;
using ExchangeRatePreviewer.Core.Services;
using ExchangeRatePreviewer.Core.Validation;
using ExchangeRatePreviewer.Core.Services.SOAP;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", b =>
        b.AllowAnyHeader()
        .AllowAnyOrigin()
        .AllowAnyMethod());
});

builder.Services.Configure<BankOfLithuaniaSoapClientSettings>(
    builder.Configuration.GetSection("BankOfLithuaniaSoapClientSettings"));

builder.Host.UseSerilog((ctx, lc) =>
    lc.WriteTo.Console()
        .ReadFrom.Configuration(ctx.Configuration));

builder.Services.AddScoped<IDateValidation, DateValidation>();
builder.Services.AddSingleton<IBankOfLithuaniaSoapClientSettings>((_) => builder.Configuration
    .GetSection("BankOfLithuaniaSoapClientSettings")
    .Get<BankOfLithuaniaSoapClientSettings>());
builder.Services.AddScoped<IBankOfLithuaniaSoapClient, BankOfLithuaniaSoapClient>();
builder.Services.AddScoped<IExchangeRateService, ExchangeRateService>();
builder.Services.AddScoped<IResultParser, ResultParser>();

builder.Services.AddResponseCaching(options =>
{
    options.MaximumBodySize = 1024;
    options.UseCaseSensitivePaths = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseResponseCaching();

app.Use(async (context, next) =>
{
    context.Response.GetTypedHeaders().CacheControl =
        new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
        {
            Public = true,
            MaxAge = TimeSpan.FromSeconds(10)
        };
    context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
        new string[] { "Accept-Encoding" };

    await next();
});

app.UseAuthorization();

app.MapControllers();

app.Run();