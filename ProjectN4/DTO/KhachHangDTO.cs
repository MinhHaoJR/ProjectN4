using System;

namespace ProjectN4.DTO
{
    public class KhachHangDTO
    {
        public int MaKH { get; set; }
        public string HoTen { get; set; }
        public string CCCD_Passport { get; set; } // Quan trọng để định danh
        public string SDT { get; set; }
        public string Email { get; set; }
        public string DiaChi { get; set; }
        public string QuocTich { get; set; }
    }
}