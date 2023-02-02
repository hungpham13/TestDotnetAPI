using TestDotnetAPI.Services.Breakfasts;
using TestDotnetAPI.Services.Users;
using TestDotnetAPI.Services.Authentication;
using TestDotnetAPI.Common.Authentication;
using TestDotnetAPI.Services.Attendances;
using TestDotnetAPI.Services.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("http://example.com");
        });
});

{
    // Add services to the container.
    builder.Services.AddScoped<IBreakfastService, BreakfastService>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IEventService, EventService>();
    builder.Services.AddScoped<IAttendanceService, AttendanceService>();
    builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
    builder.Services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

    builder.Services.AddControllers();
}

var app = builder.Build();

{
    app.UseExceptionHandler("/error");
    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();

    app.UseCors();

    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}