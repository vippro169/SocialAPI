using MySql.Data.MySqlClient;
using SocialNetwork.Domain;
using SocialNetwork.Persistence.MySql.ApplicationDbContext;
using System.Threading.Tasks;

namespace SocialNetwork.Persistence.MySql.Authentication
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly IApplicationDbContext _db;
        //private static readonly string AccountsTable = "accounts";

        public AuthenticationRepository(IApplicationDbContext db)
        {
            _db = db;
            _db.Connection.OpenAsync();
        }

        public async Task CreatAccountAsync(Account account)
        {
            var cmd = _db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = $"INSERT INTO `accounts` (`Id`, `Email`, `PasswordHash`)" +
                              $" VALUES ('{account.Id}', '{account.Email}', '{account.PasswordHash}');";
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<string> GetPasswordHashAsync(string email)
        {
            var cmd = _db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = $"SELECT PasswordHash FROM accounts WHERE Email = '{email}';";
            var reader = await cmd.ExecuteReaderAsync();
            var result = "";
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    result = await reader.GetFieldValueAsync<string>(0);
                }
            }
            return result;
        }
    }
}
