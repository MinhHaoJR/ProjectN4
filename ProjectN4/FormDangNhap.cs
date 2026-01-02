using ProjectN4.BUS;
using ProjectN4.DAL;
using ProjectN4.DTO;
using ProjectN4.GUI;
using System.Data.SqlClient;
using System;
using System.Windows.Forms;

namespace ProjectN4
{
    public partial class FormDangNhap : Form
    {
        // Biến kiểm soát việc tự động đăng nhập (để tránh vòng lặp khi Đăng xuất)
        // Bên frmMain sẽ gọi: FormDangNhap.AllowAutoLogin = false; khi bấm Đăng xuất
        public static bool AllowAutoLogin = true;

        public FormDangNhap()
        {
            InitializeComponent();
            this.KeyPreview = true; // Để bắt phím Enter/Esc

            // Gán sự kiện Load (Phòng trường hợp bên Designer quên gán)
            this.Load += Form1_Load;
        }

        // =======================================================================
        // 1. SỰ KỆN FORM LOAD: XỬ LÝ TỰ ĐỘNG ĐĂNG NHẬP
        // =======================================================================
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra xem người dùng có chọn "Ghi nhớ" không
                if (Properties.Settings.Default.RememberMe)
                {
                    // Lấy User và Pass đã lưu
                    string savedUser = Properties.Settings.Default.SavedUsername;
                    string savedPass = Properties.Settings.Default.SavedPassword; // Cần tạo trong Settings

                    // Điền vào ô nhập liệu
                    textBox1.Text = savedUser;
                    textBox2.Text = savedPass;
                    checkBox1.Checked = true;

                    // 🔥 LOGIC AUTO LOGIN:
                    // Nếu được phép Auto (Mới mở App) VÀ có dữ liệu -> Tự bấm đăng nhập
                    if (AllowAutoLogin && !string.IsNullOrEmpty(savedUser) && !string.IsNullOrEmpty(savedPass))
                    {
                        // Gọi hàm đăng nhập ngay lập tức
                        DangNhap_Click_1(sender, e);
                    }
                    else
                    {
                        // Nếu vừa Đăng xuất ra -> Không Auto -> Focus vào ô Pass để nhập lại
                        textBox2.Focus();
                    }
                }
                else
                {
                    // Nếu không nhớ -> Focus vào ô User
                    textBox1.Focus();
                }

                // Thiết lập thứ tự Tab (để bấm Tab chuyển ô cho mượt)
                textBox1.TabIndex = 0;
                textBox2.TabIndex = 1;
                button1.TabIndex = 2;
                checkBox1.TabIndex = 3;
            }
            catch (Exception ex)
            {
                // Nếu lỗi Settings (do chưa cấu hình) thì lờ đi, để người dùng nhập tay
                textBox1.Focus();
            }
        }

        // =======================================================================
        // 2. HÀM KIỂM TRA DỮ LIỆU ĐẦU VÀO
        // =======================================================================
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox2.Focus();
                return false;
            }
            return true;
        }

        // =======================================================================
        // 3. SỰ KỆN NÚT ĐĂNG NHẬP (CHÍNH)
        // =======================================================================
        private void DangNhap_Click_1(object sender, EventArgs e)
        {
            // Bước 1: Validate
            if (!ValidateInput()) return;

            string username = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();

            // Hiệu ứng Loading
            button1.Enabled = false;
            button1.Text = "Đang xử lý...";
            this.Cursor = Cursors.WaitCursor;

            try
            {
                NhanVienBUS bus = new NhanVienBUS();
                NhanVienDTO nhanVien = bus.DangNhap(username, password);

                if (nhanVien != null)
                {
                    // ✅ ĐĂNG NHẬP THÀNH CÔNG
                    Session.NhanVienHienTai = nhanVien;

                    // --- XỬ LÝ LƯU SETTINGS (USER + PASS) ---
                    if (checkBox1.Checked)
                    {
                        Properties.Settings.Default.SavedUsername = username;
                        Properties.Settings.Default.SavedPassword = password; // Lưu cả Pass
                        Properties.Settings.Default.RememberMe = true;
                        Properties.Settings.Default.Save(); // Lệnh ghi xuống ổ cứng
                    }
                    else
                    {
                        // Nếu bỏ tích -> Xóa sạch
                        Properties.Settings.Default.SavedUsername = "";
                        Properties.Settings.Default.SavedPassword = "";
                        Properties.Settings.Default.RememberMe = false;
                        Properties.Settings.Default.Save();
                    }

                    // --- CHUYỂN SANG FORM MAIN ---
                    this.Hide(); // Ẩn Login

                    frmMain f = new frmMain();
                    f.ShowDialog(); // Chương trình dừng tại dòng này chờ Main đóng

                    // --- KHI FORM MAIN ĐÓNG (ĐĂNG XUẤT) ---
                    // Code sẽ chạy tiếp xuống dưới đây
                    this.Show(); // Hiện lại Login
                    this.Cursor = Cursors.Default;
                    button1.Enabled = true;
                    button1.Text = "Đăng Nhập";

                    // Nếu không chọn ghi nhớ -> Xóa trắng ô Pass
                    if (!checkBox1.Checked)
                    {
                        textBox2.Clear();
                    }
                    textBox2.Focus();
                }
                else
                {
                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!", "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox2.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối: {ex.Message}", "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Reset trạng thái nút bấm dù thành công hay thất bại
                button1.Enabled = true;
                button1.Text = "Đăng Nhập";
                this.Cursor = Cursors.Default;
            }
        }

        // =======================================================================
        // 4. XỬ LÝ PHÍM TẮT (ENTER, ESC)
        // =======================================================================
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Enter ở TextBox1 -> Nhảy sang TextBox2
            if (keyData == Keys.Enter && textBox1.Focused)
            {
                textBox2.Focus();
                return true;
            }

            // Enter ở TextBox2 -> Bấm Đăng nhập
            if (keyData == Keys.Enter && textBox2.Focused)
            {
                DangNhap_Click_1(this, EventArgs.Empty);
                return true;
            }

            // ESC -> Hỏi thoát
            if (keyData == Keys.Escape)
            {
                DialogResult result = MessageBox.Show("Bạn có muốn thoát chương trình?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    Application.Exit();
                }
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        // =======================================================================
        // 5. CÁC TIỆN ÍCH KHÁC (HIỆN PASS, FIX SQL)
        // =======================================================================

        // Checkbox hiển thị mật khẩu
        private void checkPass_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                textBox2.PasswordChar = '\0'; // Hiện chữ
                textBox2.UseSystemPasswordChar = false;
            }
            else
            {
                textBox2.PasswordChar = '●'; // Hiện chấm tròn
                textBox2.UseSystemPasswordChar = true;
            }
        }

        // Nút Fix lỗi dữ liệu SQL (Admin dùng khi lỗi khoảng trắng)
        private void btnFixData_Click(object sender, EventArgs e)
        {
            string chuoiketNoi = $"Data Source={DbSettings.ServerIP};Initial Catalog={DbSettings.DatabaseName};User ID={DbSettings.UserID};Password={DbSettings.Password};";

            using (SqlConnection conn = new SqlConnection(chuoiketNoi))
            {
                try
                {
                    conn.Open();
                    string sql = @"UPDATE NHAN_VIEN
                                   SET TenDangNhap = RTRIM(LTRIM(TenDangNhap)),
                                       MatKhau = RTRIM(LTRIM(MatKhau))
                                   WHERE TenDangNhap LIKE 'admin%'";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    int rows = cmd.ExecuteNonQuery();
                    MessageBox.Show($"Đã chuẩn hóa {rows} dòng dữ liệu.", "Thành công");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }

        // Các hàm sự kiện thừa (Nếu Designer lỡ tạo thì để trống, đừng xóa kẻo lỗi Designer)
        private void checkRememberMe_CheckedChanged(object sender, EventArgs e) { }
        private void lbltille_Click(object sender, EventArgs e) { }
        private void TenDangNhap_Click(object sender, EventArgs e) { }
        private void MatKhau_Click(object sender, EventArgs e) { }
        private void NhapTenDangNhap_TextChanged(object sender, EventArgs e) { }
        private void NhapMatKhau_TextChanged(object sender, EventArgs e) { }
        private void button1_Click(object sender, EventArgs e) { }
    }
}