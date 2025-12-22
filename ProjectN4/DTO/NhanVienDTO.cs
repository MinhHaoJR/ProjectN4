using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace ProjectN4.DTO // Thay ProjectN4 bằng tên Project của nhóm
{
    public class NhanVienDTO
    {
        public int MaNV { get; set; }
        public int MaChiNhanh { get; set; }
        public string HoTen { get; set; }
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }
        public string ChucVu { get; set; }
        public string TrangThai { get; set; }

        public NhanVienDTO() { }
    }
}