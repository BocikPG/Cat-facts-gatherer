using CatFacts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.ClearProviders();
builder.Logging.AddFileLogger(configure =>
{
    configure.minLogLevel = LogLevel.Information;
    configure.LogsPath = "/Logs";
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//var logger = app.Services.GetRequiredService<ILogger<Program>>();

app.UseHttpsRedirection();

app.AddCatFactsEndpoints();

app.Run();