using DoAn.Data;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Connection
builder.Services.AddTransient<MySqlConnection>(_ => new MySqlConnection(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddDbContext<DoAnContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("Default")
    ,Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.23-mysql"));
});
//builder.Services.AddDbContext<DoAnContext>(options => options.UseSqlServer("name=ConnectionStrings:Default"));
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
