namespace TestDotnetAPI.Models;

public class Attendance
{
    public Guid Id { get; }
    public User User { get; }
    public Event Event { get; }
    public DateTime CreatedAt { get; }
    public DateTime ModifiedAt { get; }

    public string Status { get; }
    public string Token { get; }
};