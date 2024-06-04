using System.Data;
using System.Data.SqlClient;

namespace MyEmpApi.Data
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;
        public DatabaseHelper(string connectionString)
        {
            _connectionString = connectionString;
        }
        public DataTable ExecuteQuery(string query)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
    }
}
