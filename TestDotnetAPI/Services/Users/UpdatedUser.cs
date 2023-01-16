using TestDotnetAPI.Models;
using ErrorOr;
namespace TestDotnetAPI.Services.Users;

public record struct UpdatedUser(bool isNewlyCreated);