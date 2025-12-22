using ProjectN4.DAL; // Kết nối với lớp xử lý SQL
using ProjectN4.DTO; // Kết nối với lớp chứa đối tượng nhân viên

namespace ProjectN4.BUS
{
    public class NhanVienBUS
    {
        // Khởi tạo đối tượng DAL để lát nữa gọi hàm kiểm tra trong Database
        private NhanVienDAL dal = new NhanVienDAL();

        /// <summary>
        /// Hàm xử lý logic Đăng nhập
        /// </summary>
        /// <param name="user">Tên đăng nhập từ giao diện</param>
        /// <param name="pass">Mật khẩu từ giao diện</param>
        /// <returns>Đối tượng NhanVienDTO nếu hợp lệ, ngược lại trả về null</returns>
        public NhanVienDTO DangNhap(string user, string pass)
        {
            // 1. KIỂM TRA NGHIỆP VỤ (Business Logic)
            // Nếu người dùng để trống một trong hai ô, chúng ta không cần gửi yêu cầu đến Database
            // Việc này giúp giảm tải cho server và tăng tốc độ ứng dụng.
            if (string.IsNullOrEmpty(user.Trim()) || string.IsNullOrEmpty(pass.Trim()))
            {
                return null;
            }

            // 2. GỌI LỚP DAL
            // Nếu dữ liệu nhập vào đã đầy đủ, BUS sẽ chuyển thông tin xuống DAL để tra cứu SQL
            return dal.KiemTraDangNhap(user, pass);
        }
    }
}