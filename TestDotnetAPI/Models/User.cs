namespace TestDotnetAPI.Models;
public class User
{
    public Guid Id;
    public string UserName;
    public string Name;
    public string Password;
    public string Role;
    public string PhoneNumber;
    public bool Active;
    public DateTime ActiveTimeStart;
    public DateTime ActiveTimeEnd;
}