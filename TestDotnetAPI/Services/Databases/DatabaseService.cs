using Microsoft.Data.SqlClient;
using ErrorOr;
using TestDotnetAPI.ServiceErrors;
using System;
using System.Data;

namespace TestDotnetAPI.Services.Database;
class DatabaseService
{
    public const string CONNECTION_STRING = "Data Source=172.16.1.118;Initial Catalog=DbThucTap;User ID=thuctap;Password=tt@123;Trust Server Certificate=True;";
    public const string ACCOUNT_TABLE = "[dbo].[Account]";
    public const string EVENT_TABLE = "[dbo].[Event]";
    public const string STREAM_TABLE = "[dbo].[Stream]";
    public const string EVENT_STREAM_TABLE = "[dbo].[EventStream]";
    public const string ATTENDANCE_TABLE = "[dbo].[Attendance]";
    public static SqlConnection conn = null;

    private static void connect()
    {
        try
        {
            // SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            // builder.DataSource = "172.16.1.118";
            // builder.UserID = "thuctap";
            // builder.Password = "tt@123";
            // builder.InitialCatalog = "DbThucTap";
            // string connectionString = builder.ConnectionString + ";Trust Server Certificate=True;";
            Console.WriteLine(CONNECTION_STRING);
            conn = new SqlConnection(CONNECTION_STRING);
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.ToString());
        }

        // String sql = "SELECT name, collation_name FROM sys.databases";

        // using (SqlCommand command = new SqlCommand(sql, connection))
        // {
        //     using (SqlDataReader reader = command.ExecuteReader())
        //     {
        //         while (reader.Read())
        //         {
        //             Console.WriteLine("{0} {1}", reader.GetString(0), reader.GetString(1));
        //         }
        //     }
        // }
    }
    public static ErrorOr<DataTable> query(string sql)
    {
        System.Console.WriteLine(sql);
        SqlDataReader reader;
        var dt = new DataTable();
        try
        {
            if (conn == null) connect();
            conn.Open();
            using (SqlDataAdapter command = new SqlDataAdapter(sql, conn))
            {
                command.Fill(dt);
                // reader = command.ExecuteReader();
            }
            conn.Close();
            return dt;
        }
        catch (SqlException e)
        {
            conn.Close();
            return Errors.Database.QueryError(e.ToString());
        }
    }
    public static ErrorOr<Created> insert(string sql)
    {
        SqlDataAdapter adapter = new SqlDataAdapter();
        System.Console.WriteLine(sql);
        try
        {
            if (conn == null) connect();
            conn.Open();
            using (SqlCommand command = new SqlCommand(sql, conn))
            {
                adapter.InsertCommand = command;
                adapter.InsertCommand.ExecuteNonQuery();
            }
            conn.Close();
            return Result.Created;
        }
        catch (SqlException e)
        {
            conn.Close();
            return Errors.Database.UpdateError(e.ToString());
        }
    }
    public static ErrorOr<Updated> update(string sql)
    {
        SqlDataAdapter adapter = new SqlDataAdapter();
        System.Console.WriteLine(sql);
        try
        {
            if (conn == null) connect();
            conn.Open();
            using (SqlCommand command = new SqlCommand(sql, conn))
            {
                adapter.UpdateCommand = command;
                adapter.UpdateCommand.ExecuteNonQuery();
            }
            conn.Close();
            return Result.Updated;
        }
        catch (SqlException e)
        {
            conn.Close();
            return Errors.Database.UpdateError(e.ToString());
        }
    }
    public static ErrorOr<Deleted> delete(string sql)
    {
        SqlDataAdapter adapter = new SqlDataAdapter();
        System.Console.WriteLine(sql);
        try
        {
            if (conn == null) connect();
            conn.Open();
            using (SqlCommand command = new SqlCommand(sql, conn))
            {
                adapter.DeleteCommand = command;
                adapter.DeleteCommand.ExecuteNonQuery();
            }
            conn.Close();
            return Result.Deleted;
        }
        catch (SqlException e)
        {
            conn.Close();
            return Errors.Database.UpdateError(e.ToString());
        }
    }
}