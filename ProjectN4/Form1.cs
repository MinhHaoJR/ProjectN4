using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectN4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Gọi hàm Test từ lớp DAL
            if (ProjectN4.DAL.DatabaseHelper.TestConnection())
            {
                MessageBox.Show("Thành công! Đã kết nối tới SQL qua Tailscale.", "Thông báo");
            }
            else
            {
                MessageBox.Show("Thất bại. Kiểm tra lại IP, User, Pass hoặc Tường lửa máy kia.", "Lỗi");
            }
        }

        private void DangNhapHeThong_Click(object sender, EventArgs e)
        {

        }

        private void TenDangNhap_Click(object sender, EventArgs e)
        {

        }

        private void MatKhau_Click(object sender, EventArgs e)
        {

        }

        private void NhapTenDangNhap_TextChanged(object sender, EventArgs e)
        {

        }

        private void NhapMatKhau_TextChanged(object sender, EventArgs e)
        {

        }

        private void DangNhap_Click_1(object sender, EventArgs e)
        {

        }
    }
}
