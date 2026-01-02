using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ProjectN4.DTO;

namespace ProjectN4.DAL
{
    public class KhachHangDAL
    {
        // 1. Lấy danh sách khách hàng (Có hỗ trợ tìm kiếm)
        public DataTable GetKhachHang(string tuKhoa = "")
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                string query = "SELECT * FROM KHACH_HANG";

                // Nếu có từ khóa thì thêm điều kiện tìm kiếm
                if (!string.IsNullOrEmpty(tuKhoa))
                {
                    query += " WHERE HoTen LIKE @TuKhoa OR CCCD_Passport LIKE @TuKhoa OR SDT LIKE @TuKhoa";
                }

                SqlCommand cmd = new SqlCommand(query, conn);
                if (!string.IsNullOrEmpty(tuKhoa))
                {
                    cmd.Parameters.AddWithValue("@TuKhoa", "%" + tuKhoa + "%");
                }

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }
            return dt;
        }

        // 2. Thêm khách hàng mới
        public bool ThemKhachHang(KhachHangDTO kh)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                string query = @"INSERT INTO KHACH_HANG (HoTen, CCCD_Passport, SDT, Email, DiaChi, QuocTich) 
                                 VALUES (@HoTen, @CCCD, @SDT, @Email, @DiaChi, @QuocTich)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@HoTen", kh.HoTen);
                cmd.Parameters.AddWithValue("@CCCD", kh.CCCD_Passport);
                cmd.Parameters.AddWithValue("@SDT", kh.SDT);
                cmd.Parameters.AddWithValue("@Email", kh.Email);
                cmd.Parameters.AddWithValue("@DiaChi", kh.DiaChi);
                cmd.Parameters.AddWithValue("@QuocTich", kh.QuocTich);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // 3. Cập nhật thông tin
        public bool SuaKhachHang(KhachHangDTO kh)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                string query = @"UPDATE KHACH_HANG 
                                 SET HoTen = @HoTen, CCCD_Passport = @CCCD, SDT = @SDT, 
                                     Email = @Email, DiaChi = @DiaChi, QuocTich = @QuocTich
                                 WHERE MaKH = @MaKH";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaKH", kh.MaKH);
                cmd.Parameters.AddWithValue("@HoTen", kh.HoTen);
                cmd.Parameters.AddWithValue("@CCCD", kh.CCCD_Passport);
                cmd.Parameters.AddWithValue("@SDT", kh.SDT);
                cmd.Parameters.AddWithValue("@Email", kh.Email);
                cmd.Parameters.AddWithValue("@DiaChi", kh.DiaChi);
                cmd.Parameters.AddWithValue("@QuocTich", kh.QuocTich);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // 4. Xóa khách hàng
        public bool XoaKhachHang(int maKH)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }                // Lưu ý: Nếu khách đã từng đặt phòng (có trong bảng DAT_PHONG), SQL sẽ chặn xóa (Khóa ngoại)
                // Cần try-catch ở bên GUI để báo lỗi này.
                string query = "DELETE FROM KHACH_HANG WHERE MaKH = @MaKH";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaKH", maKH);

                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}