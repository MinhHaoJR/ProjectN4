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
            Application.Run(new frmQuanLyPhong());
            //Application.Run(new FormDangNhap());
        }
    }
}