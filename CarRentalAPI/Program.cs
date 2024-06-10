using MongoDB.Driver;
using Uzduotis01;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IDatabaseRepository, DatabaseRepository>(_ => new DatabaseRepository("Server=DESKTOP-OD4Q280;Database=CarRental;Integrated Security=True;"));

builder.Services.AddSingleton<IMongoClient, MongoClient>(sp => new MongoClient(builder.Configuration.GetConnectionString("MongoDb")));
builder.Services.AddSingleton<IMongoDBRepository, MongoDBRepository>();

builder.Services.AddSingleton<IRentService, RentService>(_ => new RentService(_.GetService<IDatabaseRepository>(), _.GetService<IMongoDBRepository>()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
