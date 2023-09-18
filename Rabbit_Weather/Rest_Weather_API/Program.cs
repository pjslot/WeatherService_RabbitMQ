using Serilog;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

//добавка сваггера
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

//инициализация логгера
Log.Logger = new LoggerConfiguration()
				.WriteTo.Console()
				.MinimumLevel.Debug()
				.CreateLogger();
