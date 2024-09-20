using MyFileManager;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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

app.Run();
