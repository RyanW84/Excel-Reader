using ExcelReader.RyanW84;

builder.Services.AddDbContext<ExcelReaderDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))


if (app.Environment.IsDevelopment())
{
    Console.WriteLine("Development Mode");

    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ShiftsLoggerV2.RyanW84.Data.ShiftsLoggerDbContext>();
    dbContext.Database.EnsureDeleted();
    dbContext.Database.EnsureCreated();
}