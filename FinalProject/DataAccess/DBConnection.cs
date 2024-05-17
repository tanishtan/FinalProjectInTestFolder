using Microsoft.Data.SqlClient;
using System.Data;

namespace FinalProject.DataAccess
{
    public class DBConnection
    {



        protected SqlConnection connection;
        private string connectionString;
        //  @"Server=(local);database=BankingApp;integrated security=sspi;trustservercertificate=true";
        // "Server=tcp:pctbankingdb.database.windows.net,1433;Initial Catalog=PCTBankingDatabase;Persist Security Info=False;User ID=GroubBDBAdmin;Password=Admin@groupBdb;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";


        public DBConnection(IConfiguration config)
        {
            connectionString = config.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");
        }

        protected void OpenConnection()
        {
            if (connection is null)
                connection = new SqlConnection(connectionString);
            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();
        }
        protected void CloseConnection()
        {
            if (connection is not null)
                if (connection.State != System.Data.ConnectionState.Closed)
                    connection.Close();
        }

        public SqlDataReader ExecuteReader(string sqltext, CommandType commandType, params SqlParameter[] parameters)
        {
            OpenConnection();
            var command = connection.CreateCommand();
            command.CommandType = commandType;
            command.CommandText = sqltext;
            if (parameters.Length > 0)
            {
                command.Parameters.AddRange(parameters);
            }
            return command.ExecuteReader();

        }
        public void ExecuteNonQuery(string sqltext, CommandType commandType, params SqlParameter[] parameters)
        {
            OpenConnection();
            var command = connection.CreateCommand();
            command.CommandType = commandType;
            command.CommandText = sqltext;
            if (parameters.Length > 0)
            {
                command.Parameters.AddRange(parameters);
            }
            command.ExecuteNonQuery();
            return;
        }



    }
}
