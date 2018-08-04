using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.Persistence.MySql.ApplicationDbContext
{
    public interface IApplicationDbContext
    {
        MySqlConnection Connection { get; set; }
        void Dispose();
    }
}
