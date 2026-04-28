using backend.Data.Mongo;
using backend.Model;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<backend.Data.Mongo.DocumentDbSettings>(
    builder.Configuration.GetSection("DocumentDbSettings")
);

builder.Services.AddSingleton<IMongoClient>(ServiceProvider =>
{
    var connectionString = builder.Configuration.GetValue<string>(
        "DocumentDbSettings:ConnectionString"
    );

    return new MongoClient(connectionString);
});

builder.Services.AddScoped<ICarPartCollection, CarPartCollection>();

builder.Services.AddScoped<CarPartModel>();

builder.Services.AddOpenApi();

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowViteApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod();
        }
    );
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowViteApp");

app.MapControllers();

app.Run();
