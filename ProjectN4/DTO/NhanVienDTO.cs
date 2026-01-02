using System;

namespace ProjectN4.DTO
{
    public class NhanVienDTO
    {
        public int MaNV { get; set; }
        public int MaChiNhanh { get; set; }
        public string HoTen { get; set; }

        // 👇 Đã thêm 2 dòng này cho khớp với SQL 
        public DateTime NgaySinh { get; set; }
        public string SDT { get; set; }

        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }
        public string ChucVu { get; set; }
        public string TrangThai { get; set; }

        public NhanVienDTO() { }
    }
}