using System;
using MySql.Data.MySqlClient;

namespace aspnetapp.Utils
{
    public class AppDb : IDisposable
    {
        public MySqlConnection Connection;
        private static AppDb _instance = new AppDb("");
        private AppDb(string connectionString)
        {
            //server=127.0.0.1;user id=mysqltest;password=test;port=3306;database=blog;
            if (connectionString==null || connectionString.Length == 0) {
               string server = Environment.GetEnvironmentVariable ("MYSQL_HOST");
               string user = Environment.GetEnvironmentVariable ("MYSQL_USER");
               string pass = Environment.GetEnvironmentVariable ("MYSQL_PASSWORD");
               string port = Environment.GetEnvironmentVariable ("MYSQL_PORT");
               string database = Environment.GetEnvironmentVariable ("MYSQL_DATABASE");
               if (server != null && server.Length>0 ) {
                  connectionString = String.Format("server={0};user id={1};password={2};port={3};database={4};",server,user,pass,port,database);
               }
            }
            Console.Error.WriteLine("connectionString:{0}",connectionString);
            if (connectionString != null && connectionString.Length>0){
              Connection = new MySqlConnection(connectionString);
              InitDB();
            }
            Console.Error.WriteLine("Connection:{0}",Connection);
        }

        public static AppDb GetAppDb(){
            if (_instance.Connection == null) {
                return null;
            }
            var connectState = _instance.Connection.State.ToString(); 
            if (connectState == "Closed") {
                _instance.Connection.Open();
            }
            return _instance;
        }
        public void Dispose()
        {
            if (Connection != null) {
               Connection.Close();
            }
        }
        private async void InitDB() {
            await Connection.OpenAsync();
            var cmd = Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS `UserInfo` (
                                `Id` int(11) NOT NULL AUTO_INCREMENT,
                                `Name` VARCHAR(100) NOT NULL,
                                `Address` VARCHAR(100) NOT NULL,
                                `Age` int(3) NOT NULL,
                                `Gender` VARCHAR(2) NOT NULL,
                                PRIMARY KEY (`Id`)
                                ) ENGINE=InnoDB CHARSET=utf8;";
            await cmd.ExecuteNonQueryAsync();
        }
    }
}