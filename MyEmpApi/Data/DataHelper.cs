using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace MyEmpApi.Data
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;

        public DatabaseHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DataTable ExecuteScriptFromFile(string relativePath)
        {
            string basePath = Directory.GetCurrentDirectory();
            string fullPath = Path.Combine(basePath, relativePath);
            string script;
            using (StreamReader reader = new StreamReader(fullPath))
            {
                script = reader.ReadToEnd();
            }
            return ExecuteScript(script);
        }

        public DataTable ExecuteScript(string script)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(script, con);
                cmd.CommandType = CommandType.Text;
                DataTable dt = new DataTable();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd)) // Corrected the typo here
                {
                    da.Fill(dt);
                }
                return dt;
            }
        }
    }
}
