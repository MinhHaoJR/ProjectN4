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
<<<<<<< HEAD
            Application.Run(new FormDangNhap());
            //Application.Run(new frmQuanLyKhachHang());

=======
            Application.Run(new frmQuanLyPhong());
=======
            Application.Run(new FormDangNhap());
>>>>>>> b7509e79ad521cf0c39b4e7fbc815720fd257609
<<<<<<< HEAD
>>>>>>> 340c59e70acdf4c9886f0f9c571ef7f2cd1b687f
=======
>>>>>>> 340c59e70acdf4c9886f0f9c571ef7f2cd1b687f
        }
    }
}