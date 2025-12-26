using ProjectN4.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            Application.Run(new frmQuanLyPhong());

            // Cách 2: Chạy từ màn hình Đăng Nhập (Dùng khi nào làm xong hết ứng dụng)
            //Application.Run(new FrmTraPhong());
        }
    }
}