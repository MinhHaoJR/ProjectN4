using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ProjectN4.DAL
{
    public class DatabaseHelper
    {
        // Tạo chuỗi kết nối từ thông tin bên file DbSettings
        public static string ConnectionString = $"Data Source={DbSettings.ServerIP},1433;Initial Catalog={DbSettings.DatabaseName};User ID={DbSettings.UserID};Password={DbSettings.Password};Network Library=DBMSSOCN;";

        // Hàm lấy kết nối
        public static SqlConnection GetConnection()
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            try
            {
                conn.Open();
                return conn;
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Lỗi kết nối Server: " + ex.Message, "Lỗi Kết Nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        // Hàm kiểm tra kết nối (Test)
        public static bool TestConnection()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}