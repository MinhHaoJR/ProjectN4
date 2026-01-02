using System;
using System.Data;
using System.Data.SqlClient;
using ProjectN4.DTO;

namespace ProjectN4.DAL
{
    public class NhanVienDAL
    {
        // ==========================================================
        // PHẦN 1: HÀM ĐĂNG NHẬP (GIỮ NGUYÊN CODE CŨ CỦA BẠN)
        // ==========================================================
        public NhanVienDTO DangNhap(string user, string pass)
        {
            NhanVienDTO nv = null;

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                if (conn == null) return null;

                try
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    string query = @"SELECT * FROM NHAN_VIEN 
                                     WHERE RTRIM(LTRIM(TenDangNhap)) = @User 
                                     AND RTRIM(LTRIM(MatKhau)) = @Pass 
                                     AND TrangThai = N'Đang làm việc'";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@User", user.Trim());
                    cmd.Parameters.AddWithValue("@Pass", pass.Trim());

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        nv = new NhanVienDTO();

                        if (reader["MaNV"] != DBNull.Value)
                            nv.MaNV = int.Parse(reader["MaNV"].ToString());

                        if (reader["MaChiNhanh"] != DBNull.Value)
                            nv.MaChiNhanh = int.Parse(reader["MaChiNhanh"].ToString());

                        nv.HoTen = reader["HoTen"] != DBNull.Value ? reader["HoTen"].ToString() : "";
                        nv.TenDangNhap = reader["TenDangNhap"] != DBNull.Value ? reader["TenDangNhap"].ToString() : "";
                        nv.ChucVu = reader["ChucVu"] != DBNull.Value ? reader["ChucVu"].ToString() : "";

                        // (Bạn có thể map thêm NgaySinh, SDT ở đây nếu muốn hiển thị lúc đăng nhập, 
                        // nhưng nếu không cần thì để như cũ vẫn chạy tốt).
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi truy vấn SQL: " + ex.Message);
                }
            }
            return nv;
        }

        // ==========================================================
        // PHẦN 2: CÁC HÀM MỚI (CRUD) CHO FORM QUẢN LÝ NHÂN VIÊN
        // ==========================================================

        // 1. Lấy danh sách nhân viên (Có tìm kiếm)
        public DataTable GetNhanVien(string tuKhoa = "")
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                string query = "SELECT * FROM NHAN_VIEN";

                // Tìm theo Tên, Tài khoản hoặc SĐT
                if (!string.IsNullOrEmpty(tuKhoa))
                {
                    query += " WHERE HoTen LIKE @TuKhoa OR TenDangNhap LIKE @TuKhoa OR SDT LIKE @TuKhoa";
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

        // 2. Thêm Nhân viên mới
        public bool ThemNhanVien(NhanVienDTO nv)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                string query = @"INSERT INTO NHAN_VIEN (MaChiNhanh, HoTen, NgaySinh, SDT, ChucVu, TenDangNhap, MatKhau, TrangThai) 
                                 VALUES (@MaChiNhanh, @HoTen, @NgaySinh, @SDT, @ChucVu, @TenDangNhap, @MatKhau, @TrangThai)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaChiNhanh", nv.MaChiNhanh);
                cmd.Parameters.AddWithValue("@HoTen", nv.HoTen);

                // Xử lý Ngày sinh (Tránh lỗi nếu chưa chọn)
                if (nv.NgaySinh == DateTime.MinValue)
                    cmd.Parameters.AddWithValue("@NgaySinh", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@NgaySinh", nv.NgaySinh);

                cmd.Parameters.AddWithValue("@SDT", nv.SDT);
                cmd.Parameters.AddWithValue("@ChucVu", nv.ChucVu);
                cmd.Parameters.AddWithValue("@TenDangNhap", nv.TenDangNhap);
                cmd.Parameters.AddWithValue("@MatKhau", nv.MatKhau);
                cmd.Parameters.AddWithValue("@TrangThai", nv.TrangThai);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // 3. Sửa Nhân viên
        public bool SuaNhanVien(NhanVienDTO nv)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                string query = @"UPDATE NHAN_VIEN 
                                 SET HoTen = @HoTen, NgaySinh = @NgaySinh, SDT = @SDT, 
                                     ChucVu = @ChucVu, MatKhau = @MatKhau, TrangThai = @TrangThai, MaChiNhanh = @MaChiNhanh
                                 WHERE MaNV = @MaNV";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaNV", nv.MaNV);
                cmd.Parameters.AddWithValue("@MaChiNhanh", nv.MaChiNhanh);
                cmd.Parameters.AddWithValue("@HoTen", nv.HoTen);

                if (nv.NgaySinh == DateTime.MinValue)
                    cmd.Parameters.AddWithValue("@NgaySinh", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@NgaySinh", nv.NgaySinh);

                cmd.Parameters.AddWithValue("@SDT", nv.SDT);
                cmd.Parameters.AddWithValue("@ChucVu", nv.ChucVu);
                cmd.Parameters.AddWithValue("@MatKhau", nv.MatKhau);
                cmd.Parameters.AddWithValue("@TrangThai", nv.TrangThai);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // 4. Xóa Nhân viên
        public bool XoaNhanVien(int maNV)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                string query = "DELETE FROM NHAN_VIEN WHERE MaNV = @MaNV";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaNV", maNV);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // 5. Kiểm tra trùng Tài khoản (Để tránh tạo 2 nick giống nhau)
        public bool KiemTraTrungTaiKhoan(string user)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                string query = "SELECT COUNT(*) FROM NHAN_VIEN WHERE TenDangNhap = @User";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@User", user);

                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }
    }
}