using System.Data;
using ProjectN4.DAL;
using ProjectN4.DTO;

namespace ProjectN4.BUS
{
    public class PhongBUS
    {
        private PhongDAL dal = new PhongDAL();

        public DataTable LayDSPhong()
        {
            return dal.GetDanhSachPhong();
        }

        public bool ThemPhong(PhongDTO p)
        {
            // Có thể thêm logic: Kiểm tra số phòng đã tồn tại chưa ở đây
            return dal.ThemPhong(p);
        }

        public bool SuaPhong(PhongDTO p)
        {
            return dal.SuaPhong(p);
        }

        public bool XoaPhong(int maPhong)
        {
            return dal.XoaPhong(maPhong);
        }
    }
}