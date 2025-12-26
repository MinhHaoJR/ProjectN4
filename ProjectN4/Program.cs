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
            Application.Run(new frmQuanLyPhong());
=======
            Application.Run(new FormDangNhap());
>>>>>>> b7509e79ad521cf0c39b4e7fbc815720fd257609
        }
    }
}