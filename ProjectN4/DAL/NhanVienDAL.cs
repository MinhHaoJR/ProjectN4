using System;
using System.Data;
using System.Data.SqlClient; // Thư viện để làm việc với SQL Server
using ProjectN4.DTO;        // Kết nối với lớp DTO bạn đã tạo ở bước 2

namespace ProjectN4.DAL
{
    public class NhanVienDAL
    {
        // Hàm này sẽ kiểm tra đăng nhập bằng cách truy vấn vào bảng NHAN_VIEN
        public NhanVienDTO KiemTraDangNhap(string user, string pass)
        {
            NhanVienDTO nv = null;

            // DatabaseHelper.GetConnection() là hàm lấy chuỗi kết nối (Cần có file DatabaseHelper trước đó)
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                if (conn == null) return null;

                try
                {
                    conn.Open(); // Phải mở kết nối trước khi thực thi

                    // Câu lệnh SQL truy vấn thông tin
                    string query = "SELECT * FROM NHAN_VIEN WHERE TenDangNhap = @User AND MatKhau = @Pass AND TrangThai = N'Đang làm việc'";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    // Sử dụng Parameter để chống tấn công SQL Injection
                    cmd.Parameters.AddWithValue("@User", user);
                    cmd.Parameters.AddWithValue("@Pass", pass);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read()) // Nếu tìm thấy bản ghi trùng khớp
                    {
                        nv = new NhanVienDTO();
                        // Ánh xạ dữ liệu từ SQL vào đối tượng C# (DTO)
                        nv.MaNV = int.Parse(reader["MaNV"].ToString());
                        nv.MaChiNhanh = int.Parse(reader["MaChiNhanh"].ToString());
                        nv.HoTen = reader["HoTen"].ToString();
                        nv.TenDangNhap = reader["TenDangNhap"].ToString();
                        nv.ChucVu = reader["ChucVu"].ToString();
                        // Lưu ý: Không lưu mật khẩu vào DTO để tăng tính bảo mật
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    // Bạn có thể dùng MessageBox.Show(ex.Message) để xem lỗi nếu kết nối thất bại
                    Console.WriteLine("Lỗi kết nối DAL: " + ex.Message);
                }
                finally
                {
                    conn.Close(); // Luôn đóng kết nối để giải phóng tài nguyên
                }
            }
            return nv;
        }
    }
}