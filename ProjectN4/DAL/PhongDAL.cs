using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic; // Để dùng List
using ProjectN4.DTO;

namespace ProjectN4.DAL
{
    public class PhongDAL
    {
        // 1. Lấy danh sách phòng
        public DataTable GetDanhSachPhong()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                if (conn == null) return null;
                try
                {
                    string query = "SELECT * FROM PHONG";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    da.Fill(dt);
                }
                catch { }
            }
            return dt;
        }

        // 2. Thêm phòng mới
        public bool ThemPhong(PhongDTO p)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                if (conn == null) return false;
                string query = "INSERT INTO PHONG (MaChiNhanh, SoPhong, LoaiPhong, GiaPhong, TrangThai) VALUES (@MaCN, @SoPhong, @LoaiPhong, @Gia, @TrangThai)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaCN", p.MaChiNhanh);
                cmd.Parameters.AddWithValue("@SoPhong", p.SoPhong);
                cmd.Parameters.AddWithValue("@LoaiPhong", p.LoaiPhong);
                cmd.Parameters.AddWithValue("@Gia", p.GiaPhong);
                cmd.Parameters.AddWithValue("@TrangThai", p.TrangThai);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // 3. Sửa thông tin phòng
        public bool SuaPhong(PhongDTO p)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                if (conn == null) return false;
                string query = "UPDATE PHONG SET SoPhong=@SoPhong, LoaiPhong=@LoaiPhong, GiaPhong=@Gia, TrangThai=@TrangThai WHERE MaPhong=@MaPhong";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaPhong", p.MaPhong);
                cmd.Parameters.AddWithValue("@SoPhong", p.SoPhong);
                cmd.Parameters.AddWithValue("@LoaiPhong", p.LoaiPhong);
                cmd.Parameters.AddWithValue("@Gia", p.GiaPhong);
                cmd.Parameters.AddWithValue("@TrangThai", p.TrangThai);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // 4. Xóa phòng
        public bool XoaPhong(int maPhong)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                if (conn == null) return false;
                string query = "DELETE FROM PHONG WHERE MaPhong = @MaPhong";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaPhong", maPhong);
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}