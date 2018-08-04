using MySql.Data.MySqlClient;
using System;

namespace SocialNetwork.Persistence.MySql.ApplicationDbContext
{
    public class ApplicationDbContext : IApplicationDbContext
    {
        public MySqlConnection Connection { get; set; }

        public ApplicationDbContext(string connectionString)
        {
            Connection = new MySqlConnection(connectionString);
        }

        public void Dispose()
        {
            Connection.Close();
        }
    }
}
