using TestDotnetAPI.Services.Breakfasts;
using TestDotnetAPI.Services.Users;
using TestDotnetAPI.Services.Authentication;
using TestDotnetAPI.Common.Authentication;

var builder = WebApplication.CreateBuilder(args);

{
    // Add services to the container.
    builder.Services.AddScoped<IBreakfastService, BreakfastService>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
    builder.Services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

    builder.Services.AddControllers();
}

var app = builder.Build();

{
    app.UseExceptionHandler("/error");
    app.UseHttpsRedirection();
    app.MapControllers();
    app.Run();
}