using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using aspnetapp.Utils;

namespace aspnetapp.Dao
{
    public class UserInfo
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int? Age { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }

        [JsonIgnore]
        public AppDb Db { get; set; }

        public UserInfo(AppDb db=null)
        {
            Db = db;
        }

        public async Task InsertAsync()
        {
            var cmd = Db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO `UserInfo` (`Name`, `Age`, `Address`,`Gender`) VALUES (@name, @age, @address, @gender);";
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            Id = (int) cmd.LastInsertedId;
        }

        public async Task UpdateAsync()
        {
            var cmd = Db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE `UserInfo` SET `Name` = @name, `Address` = @address, `Age` = @age, `Gender` = @gender WHERE `Id` = @id;";
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync()
        {
            var cmd = Db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM `UserInfo` WHERE `Id` = @id;";
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = Id,
            });
        }

        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@name",
                DbType = DbType.String,
                Value = Name,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@address",
                DbType = DbType.String,
                Value = Address,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@age",
                DbType = DbType.String,
                Value = Age,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@gender",
                DbType = DbType.String,
                Value = Gender,
            });
        }

    }
}