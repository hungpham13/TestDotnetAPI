namespace TestDotnetAPI.Common.Utils;

public static class Util
{
    public static T ConvertFromDBVal<T>(object obj)
    {
        if (obj == null || obj == DBNull.Value)
        {
            return default(T); // returns the default value for the type
        }
        else
        {
            return (T)obj;
        }
    }
}