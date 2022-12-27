using BuberBreakfast.Services.Breakfasts;

var builder = WebApplication.CreateBuilder(args);

{
    // Add services to the container.
    builder.Services.AddScoped<IBreakfastService, BreakfastService>();

    builder.Services.AddControllers();
}

var app = builder.Build();

{
    app.UseExceptionHandler("/error");
    app.UseHttpsRedirection();
    app.MapControllers();
    app.Run();
}