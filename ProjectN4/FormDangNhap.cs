using ProjectN4.BUS;
using ProjectN4.DTO;
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
    public partial class FormDangNhap : Form
    {
        public FormDangNhap()
        {
            InitializeComponent();

            // Bật nhận sự kiện phím
            this.KeyPreview = true;

            // Focus vào ô tài khoản khi mở form
            this.Load += Form1_Load;
        }

        // Sự kiện khi Form Load
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra có tài khoản đã lưu không
                if (Properties.Settings.Default.RememberMe)
                {
                    // Load tài khoản đã lưu
                    textBox1.Text = Properties.Settings.Default.SavedUsername;
                    checkBox1.Checked = true;

                    // Focus vào ô mật khẩu (vì tài khoản đã có sẵn)
                    textBox2.Focus();
                }
                else
                {
                    // Focus vào textBox1 (tài khoản)
                    textBox1.Focus();
                }
            }
            catch
            {
                // Nếu lỗi Settings, focus bình thường
                textBox1.Focus();
            }

            // Thiết lập Tab Order
            textBox1.TabIndex = 0;
            textBox2.TabIndex = 1;
            button1.TabIndex = 2;
            checkBox1.TabIndex = 3;
            checkBox2.TabIndex = 4;
        }

        // HÀM KIỂM TRA DỮ LIỆU NHẬP
        private bool ValidateInput()
        {
            // Kiểm tra tài khoản
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show(
                    "Vui lòng nhập tên đăng nhập!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                textBox1.Focus();
                return false;
            }

            // Kiểm tra mật khẩu
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show(
                    "Vui lòng nhập mật khẩu!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                textBox2.Focus();
                return false;
            }

            return true;
        }

        // SỰ KIỆN NÚT ĐĂNG NHẬP
        private void DangNhap_Click_1(object sender, EventArgs e)
        {
            // Bước 1: Kiểm tra validation
            if (!ValidateInput())
                return;

            // Bước 2: Lấy thông tin nhập vào
            string username = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();

            // Bước 3: Hiển thị loading
            button1.Enabled = false;
            button1.Text = "Đang xử lý...";
            this.Cursor = Cursors.WaitCursor;

            try
            {
                // Bước 4: Gọi BUS để xử lý đăng nhập
                NhanVienBUS bus = new NhanVienBUS();
                NhanVienDTO nhanVien = bus.DangNhap(username, password);

                if (nhanVien != null)
                {
                    // ✅ ĐĂNG NHẬP THÀNH CÔNG

                    // Lưu thông tin vào Session
                    Session.NhanVienHienTai = nhanVien;

                    // ═══════════════════════════════════════════════════
                    // XỬ LÝ GHI NHỚ ĐĂNG NHẬP
                    // ═══════════════════════════════════════════════════
                    if (checkBox1.Checked)
                    {
                        try
                        {
                            // Lưu tài khoản vào Settings
                            Properties.Settings.Default.SavedUsername = username;
                            Properties.Settings.Default.RememberMe = true;
                            Properties.Settings.Default.Save();
                        }
                        catch
                        {
                            // Nếu lỗi Settings, bỏ qua (không ảnh hưởng đăng nhập)
                        }
                    }
                    else
                    {
                        try
                        {
                            // Xóa thông tin đã lưu
                            Properties.Settings.Default.SavedUsername = "";
                            Properties.Settings.Default.RememberMe = false;
                            Properties.Settings.Default.Save();
                        }
                        catch
                        {
                            // Bỏ qua lỗi
                        }
                    }

                    // Thông báo thành công
                    MessageBox.Show(
                        $"Đăng nhập thành công!\n\n" +
                        $"Xin chào: {nhanVien.HoTen}\n" +
                        $"Chức vụ: {nhanVien.ChucVu}",
                        "Thành công",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    // Mở Form Main (nếu đã tạo)
                    // FormMain mainForm = new FormMain();
                    // mainForm.Show();

                    // Ẩn form đăng nhập
                    this.Hide();

                    // Hoặc đóng form nếu không cần dùng lại
                    // this.Close();
                }
                else
                {
                    // ❌ ĐĂNG NHẬP THẤT BẠI

                    MessageBox.Show(
                        "Tên đăng nhập hoặc mật khẩu không đúng!\n\nVui lòng kiểm tra lại.",
                        "Lỗi đăng nhập",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    // Clear mật khẩu và focus lại
                    textBox2.Clear();

                    // Bỏ check "hiển thị mật khẩu" nếu đang bật
                    checkBox2.Checked = false;

                    textBox2.Focus();
                }
            }
            catch (Exception ex)
            {
                // ⚠️ XỬ LÝ LỖI

                MessageBox.Show(
                    $"Lỗi kết nối cơ sở dữ liệu!\n\nChi tiết: {ex.Message}",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                // Khôi phục button
                button1.Enabled = true;
                button1.Text = "Đăng Nhập";
                this.Cursor = Cursors.Default;
            }
        }

        // XỬ LÝ PHÍM TẮT
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Nhấn Enter ở textBox1 → Chuyển sang textBox2
            if (keyData == Keys.Enter && textBox1.Focused)
            {
                textBox2.Focus();
                return true;
            }

            // Nhấn Enter ở textBox2 → Đăng nhập
            if (keyData == Keys.Enter && textBox2.Focused)
            {
                DangNhap_Click_1(this, EventArgs.Empty);
                return true;
            }

            // Nhấn ESC → Thoát
            if (keyData == Keys.Escape)
            {
                DialogResult result = MessageBox.Show(
                    "Bạn có chắc chắn muốn thoát chương trình?",
                    "Xác nhận thoát",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    Application.Exit();
                }
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        // SỰ KIỆN TEST KẾT NỐI (Nút cũ)
        private void button1_Click(object sender, EventArgs e)
        {
            // Test kết nối database
            if (ProjectN4.DAL.DatabaseHelper.TestConnection())
            {
                MessageBox.Show(
                    "Thành công! Đã kết nối tới SQL qua Tailscale.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(
                    "Thất bại. Kiểm tra lại IP, User, Pass hoặc Tường lửa máy kia.",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ═══════════════════════════════════════════════════════════
        // CHECKBOX 1 - GHI NHỚ ĐĂNG NHẬP
        // ═══════════════════════════════════════════════════════════
        private void checkRememberMe_CheckedChanged(object sender, EventArgs e)
        {
            // Chỉ thông báo, không lưu ngay
            // Sẽ lưu khi đăng nhập thành công

            if (checkBox1.Checked)
            {
                // Tùy chọn: Hiển thị tooltip hoặc thông báo nhẹ
                // MessageBox.Show("Tài khoản sẽ được lưu sau khi đăng nhập thành công", 
                //     "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // ═══════════════════════════════════════════════════════════
        // CHECKBOX 2 - HIỂN THỊ MẬT KHẨU
        // ═══════════════════════════════════════════════════════════
        private void checkPass_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                // HIỂN THỊ mật khẩu
                textBox2.PasswordChar = '\0';
                textBox2.UseSystemPasswordChar = false;
            }
            else
            {
                // ẨN mật khẩu
                textBox2.PasswordChar = '●';
                textBox2.UseSystemPasswordChar = true;
            }
        }

        // CÁC SỰ KIỆN KHÔNG CẦN THIẾT (Có thể xóa)
        private void lbltille_Click(object sender, EventArgs e)
        {
            // Không cần xử lý
        }

        private void TenDangNhap_Click(object sender, EventArgs e)
        {
            // Không cần xử lý
        }

        private void MatKhau_Click(object sender, EventArgs e)
        {
            // Không cần xử lý
        }

        private void NhapTenDangNhap_TextChanged(object sender, EventArgs e)
        {
            // Có thể thêm validation real-time ở đây nếu cần
            // Ví dụ: Chỉ cho phép chữ cái và số
        }

        private void NhapMatKhau_TextChanged(object sender, EventArgs e)
        {
            // Có thể thêm kiểm tra độ mạnh mật khẩu ở đây nếu cần
        }
    }
}