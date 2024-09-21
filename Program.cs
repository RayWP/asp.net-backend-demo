using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFileManager;
using MyFileManager.Controllers.DTO;
using MyFileManager.Entity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<MyDbContext>(options =>
{
    options.UseSqlite("Data Source=MyData.db");
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.UseStaticFiles();

app.MapGet("/Hello", () =>
{
    // minimal API
    return new { msg = " hello world " };
});

app.MapGet("/Hello/{name}", (string name) =>
{
    // minimal API with path parameter
    return new { msg = $" hello {name} " };
});

app.MapPost("/Hello", (MyData data) =>
{
    // minimal API with post request
    return new { msg = $" my name is {data.Name}, I live in {data.Address} " };
});

app.MapPost("/my-data", (MyDbContext context, [FromBody] MyDataRequest request) =>
{
    var myData = new MyData
    {
        Name = request.Name,
        Address = request.Address,
        Phone = request.Phone
    };

    var result = context.MyDatas.Add(myData).Entity;
    context.SaveChanges();

    var response = new MyDataResponse
    {
        Id = result.Id,
        Name = result.Name,
        Phone = result.Phone
    };

    return new { msg = $" {result.Name} is added to the database ", data = response };
});

app.MapGet("/my-data", (MyDbContext context) =>
{
    var myDatas = context.MyDatas.ToList();
    var response = myDatas.Select(data => new MyDataResponse
    {
        Id = data.Id,
        Name = data.Name,
        Phone = data.Phone
    });
    return new { data = response }; 
});

app.Run();
