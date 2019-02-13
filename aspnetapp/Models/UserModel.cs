using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using aspnetapp.Utils;
using aspnetapp.Dao;

namespace aspnetapp.Models
{
    public class UserInfoQuery
    {

        public readonly AppDb Db;
        public UserInfoQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<UserInfo> FindOneAsync(int id)
        {
            var cmd = Db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT `Id`, `Name`, `Address`, `Age`, `Gender` FROM `UserInfo` WHERE `Id` = @id";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<UserInfo>> LatestPostsAsync()
        {
            var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `Id`, `Name`, `Address`, `Age`, `Gender` FROM `UserInfo` ORDER BY `Id` DESC LIMIT 10;";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task DeleteAllAsync()
        {
            var txn = await Db.Connection.BeginTransactionAsync();
            try
            {
                var cmd = Db.Connection.CreateCommand();
                cmd.CommandText = @"DELETE FROM `UserInfo`";
                await cmd.ExecuteNonQueryAsync();
                await txn.CommitAsync();
            }
            catch
            {
                await txn.RollbackAsync();
                throw;
            }
        }

        private async Task<List<UserInfo>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<UserInfo>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new UserInfo(Db)
                    {
                        Id = await reader.GetFieldValueAsync<int>(0),
                        Name = await reader.GetFieldValueAsync<string>(1),
                        Address = await reader.GetFieldValueAsync<string>(2),
                        Age = await reader.GetFieldValueAsync<int>(3),
                        Gender = await reader.GetFieldValueAsync<string>(4)
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }
    }
}