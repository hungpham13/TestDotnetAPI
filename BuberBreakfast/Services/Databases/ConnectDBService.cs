using Microsoft.Data.SqlClient;

namespace BuberBreakfast.Services.Database;
class ConnectDatabaseService
{
    static void Main(string[] args)
    {
        try
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.DataSource = "172.16.1.118";
            builder.UserID = "thuctap";
            builder.Password = "tt@123";
            builder.InitialCatalog = "DbThucTap";

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                Console.WriteLine("\nQuery data example:");
                Console.WriteLine("=========================================\n");

                connection.Open();

                String sql = "SELECT name, collation_name FROM sys.databases";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine("{0} {1}", reader.GetString(0), reader.GetString(1));
                        }
                    }
                }
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.ToString());
        }
        Console.WriteLine("\nDone. Press enter.");
        Console.ReadLine();
    }
}