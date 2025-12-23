using System;

namespace ProjectN4.DTO
{
    public class PhongDTO
    {
        public int MaPhong { get; set; }
        public int MaChiNhanh { get; set; }
        public string SoPhong { get; set; }
        public string LoaiPhong { get; set; } // Đơn, Đôi, VIP
        public decimal GiaPhong { get; set; }
        public string TrangThai { get; set; } // Trống, Đang ở...

        public PhongDTO() { }

        // Constructor tiện cho việc khởi tạo nhanh
        public PhongDTO(int maCN, string soPhong, string loaiPhong, decimal gia, string trangThai)
        {
            this.MaChiNhanh = maCN;
            this.SoPhong = soPhong;
            this.LoaiPhong = loaiPhong;
            this.GiaPhong = gia;
            this.TrangThai = trangThai;
        }
    }
}