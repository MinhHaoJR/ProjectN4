namespace ProjectN4.DTO // Nhớ đổi tên theo Project của bạn
{
    /// <summary>
    /// Lớp lưu trữ thông tin phiên đăng nhập
    /// </summary>
    public static class Session
    {
        // Biến này sẽ giữ toàn bộ thông tin (Mã NV, Họ tên, Chức vụ...) 
        // sau khi đăng nhập thành công.
        public static NhanVienDTO NhanVienHienTai = null;

        // Bạn có thể thêm hàm Đăng xuất tại đây
        public static void DangXuat()
        {
            NhanVienHienTai = null;
        }
    }
    namespace ProjectN4
    {
        public static class Session
        {
            public static DTO.NhanVienDTO NhanVienHienTai { get; set; }
        }
    }
}