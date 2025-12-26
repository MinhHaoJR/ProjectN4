using ProjectN4.GUI;
using System;
using System.Windows.Forms;

namespace ProjectN4
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // CHỌN 1 TRONG 2 DÒNG DƯỚI ĐÂY ĐỂ CHẠY:

            // Cách 1: Chạy thẳng vào Form Quản Lý Phòng (Để bạn test code nãy giờ)
            //Application.Run(new frmQuanLyPhong());
            //Application.Run(new frmDichVu());
            // Cách 2: Chạy từ màn hình Đăng Nhập (Dùng khi nào làm xong hết ứng dụng)
            //Application.Run(new FormDangNhap()); 
            Application.Run(new frmThemDichVu(1)); // Pass a valid maDatPhong value here
        }
    }
}