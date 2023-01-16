using Microsoft.Data.SqlClient;
using System;

namespace TestDotnetAPI.Services.Database;
class DatabaseService
{
    public static SqlConnection conn = null;

    private static void connect(string[] args)
    {
        try
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.DataSource = "172.16.1.118";
            builder.UserID = "thuctap";
            builder.Password = "tt@123";
            builder.InitialCatalog = "DbThucTap";
            Console.WriteLine(builder.ConnectionString);
            conn = new SqlConnection(builder.ConnectionString);
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
    public static ErrorOr<SqlDataReader> query(string sql)
    {
        SqlDataReader reader;
        try
        {
            if (conn == null) connect();
            conn.Open();
            using (SqlCommand command = new SqlCommand(sql, conn))
            {
                reader = command.ExecuteReader();
            }
            conn.Close();
            return reader;
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.ToString());
        }
    }
    public static void update(string sql, char type)
    {
        SqlDataAdapter adapter = new SqlDataAdapter();
        try
        {
            if (conn == null) connect();
            conn.Open();
            using (SqlCommand command = new SqlCommand(sql, conn))
            {
                if (type == 'i') adapter.InsertCommand = new SqlCommand(sql, conn);
                else if (type == 'd') adapter.DeleteCommand = new SqlCommand(sql, conn);
                else if (type == 'u') adapter.UpdateCommand = new SqlCommand(sql, conn);
                adapter.UpdateCommand.ExecuteNonQuery();
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}