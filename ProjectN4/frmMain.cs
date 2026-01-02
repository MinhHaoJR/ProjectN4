using ProjectN4.DAL; // Để lấy chuỗi kết nối
using ProjectN4.DTO; // Để lấy thông tin Session nhân viên
using ProjectN4.GUI; // Để mở các form con
using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace ProjectN4.GUI
{
    public partial class frmMain : Form
    {
        // 1. Biến lưu form con đang mở (để đóng khi mở form khác)
        private Form currentChildForm;

        // 2. Biến kiểm tra xem là Đăng xuất hay Thoát App
        private bool isDangXuat = false;

        // 3. Chuỗi kết nối CSDL
        string chuoiketNoi = $"Data Source={DbSettings.ServerIP};Initial Catalog={DbSettings.DatabaseName};User ID={DbSettings.UserID};Password={DbSettings.Password};";

        public frmMain()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized; // Tự động phóng to toàn màn hình
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            // A. Hiển thị thông tin người dùng (Lấy từ Session khi đăng nhập)
            if (Session.NhanVienHienTai != null)
            {
                lblNguoiDung.Text = $"Xin chào: {Session.NhanVienHienTai.HoTen}";
            }
            else
            {
                lblNguoiDung.Text = "Xin chào: Admin";
            }

            // B. Load số liệu thống kê lên Dashboard ngay lập tức
            LoadDashboardStats();

            // C. Mặc định active nút Trang chủ
            ActivateButton(btnTrangChu);
            lblTitleChildForm.Text = "DASHBOARD THỐNG KÊ";
        }

        // =================================================================================
        // 1. LOGIC MỞ FORM CON VÀO PANEL
        // =================================================================================
        private void OpenChildForm(Form childForm)
        {
            // Nếu có form cũ đang mở -> Đóng nó lại
            if (currentChildForm != null)
            {
                currentChildForm.Close();
            }

            currentChildForm = childForm;

            // Cấu hình form con để nhúng vào Panel Desktop
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            // Thêm vào Panel Desktop
            pnlDesktop.Controls.Add(childForm);
            pnlDesktop.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        // =================================================================================
        // 2. LOGIC DASHBOARD (LẤY SỐ LIỆU TỪ SQL)
        // =================================================================================
        private void LoadDashboardStats()
        {
            // Nếu đang mở form con thì đóng lại để hiện Dashboard gốc
            if (currentChildForm != null)
            {
                currentChildForm.Close();
                currentChildForm = null;
            }

            // Kết nối CSDL
            using (SqlConnection conn = new SqlConnection(chuoiketNoi))
            {
                try
                {
                    conn.Open();

                    // Thẻ 1: Tổng số phòng
                    SqlCommand cmd1 = new SqlCommand("SELECT COUNT(*) FROM PHONG", conn);
                    int tongPhong = (int)cmd1.ExecuteScalar();
                    lblSoLieuTong.Text = tongPhong.ToString();

                    // Thẻ 2: Đang có khách
                    SqlCommand cmd2 = new SqlCommand("SELECT COUNT(*) FROM DAT_PHONG WHERE TrangThai = N'Đang ở'", conn);
                    int coKhach = (int)cmd2.ExecuteScalar();
                    lblSoLieuCoKhach.Text = coKhach.ToString();

                    // Thẻ 3: Phòng trống
                    SqlCommand cmd3 = new SqlCommand("SELECT COUNT(*) FROM PHONG WHERE TrangThai = N'Trống'", conn);
                    int phongTrong = (int)cmd3.ExecuteScalar();
                    lblSoLieuTrong.Text = phongTrong.ToString();

                    // Thẻ 4: Doanh thu hôm nay
                    string sqlDoanhThu = "SELECT ISNULL(SUM(ThucThu), 0) FROM HOA_DON WHERE CAST(NgayLap AS DATE) = CAST(GETDATE() AS DATE)";
                    SqlCommand cmd4 = new SqlCommand(sqlDoanhThu, conn);
                    object resultDT = cmd4.ExecuteScalar();
                    decimal doanhThu = Convert.ToDecimal(resultDT);

                    lblSoLieuDoanhThu.Text = doanhThu.ToString("N0") + " VNĐ";
                }
                catch (Exception ex)
                {
                    lblSoLieuTong.Text = "-";
                    lblSoLieuDoanhThu.Text = "0 VNĐ";
                }
            }
        }

        // =================================================================================
        // 3. XỬ LÝ GIAO DIỆN NÚT BẤM (ĐỔI MÀU KHI CHỌN)
        // =================================================================================
        private void ActivateButton(object senderBtn)
        {
            if (senderBtn != null)
            {
                ResetButtonColors();
                Button currentBtn = (Button)senderBtn;
                currentBtn.BackColor = Color.FromArgb(46, 51, 73);
            }
        }

        private void ResetButtonColors()
        {
            Color defaultColor = Color.FromArgb(24, 30, 54);

            btnTrangChu.BackColor = defaultColor;
            btnQuanLyPhong.BackColor = defaultColor;
            btnLichSuHD.BackColor = defaultColor;
            btnKhachHang.BackColor = defaultColor;
            btnNhanVien.BackColor = defaultColor;
        }

        // =================================================================================
        // 4. SỰ KIỆN CLICK MENU 
        // =================================================================================

        private void btnTrangChu_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            lblTitleChildForm.Text = "DASHBOARD THỐNG KÊ";
            LoadDashboardStats();
        }

        private void btnQuanLyPhong_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            lblTitleChildForm.Text = "SƠ ĐỒ PHÒNG & LƯU TRÚ";
            OpenChildForm(new frmQuanLyPhong());
        }

        private void btnLichSuHD_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            lblTitleChildForm.Text = "LỊCH SỬ GIAO DỊCH";
            OpenChildForm(new frmLichSuHoaDon());
        }

        private void btnKhachHang_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            lblTitleChildForm.Text = "QUẢN LÝ KHÁCH HÀNG";
            OpenChildForm(new frmQuanLyKhachHang());
        }

        private void btnNhanVien_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            lblTitleChildForm.Text = "QUẢN TRỊ NHÂN SỰ";
            OpenChildForm(new frmQuanLyNhanVien());
        }

        // =================================================================================
        // 5. XỬ LÝ ĐĂNG XUẤT VÀ THOÁT APP (QUAN TRỌNG)
        // =================================================================================

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                // 1. Chặn Auto Login cho phiên hiện tại (Logic cũ của bạn)
                FormDangNhap.AllowAutoLogin = false;

                // 2. 🔥 QUAN TRỌNG: Xóa Mật khẩu trong Settings (Để lần sau mở lên nó không đủ điều kiện tự đăng nhập)
                // (Vẫn giữ SavedUsername để tiện cho người dùng, chỉ bắt nhập lại Pass thôi)
                Properties.Settings.Default.SavedPassword = "";
                Properties.Settings.Default.Save(); // Nhớ lệnh Save

                isDangXuat = true;
                this.Close();
            }
        }

        // Sự kiện FormClosed: Chạy khi form đóng lại (do bấm X hoặc gọi Close)
        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (isDangXuat)
            {
                // Nếu là đăng xuất -> Không làm gì cả
                // Code bên FormDangNhap sẽ tự chạy tiếp để hiện lại màn hình đăng nhập
            }
            else
            {
                // Nếu bấm dấu X (không phải đăng xuất) -> Thoát toàn bộ ứng dụng
                Application.Exit();
            }
        }

        private void pnlLogo_Click(object sender, EventArgs e)
        {
            btnTrangChu_Click(btnTrangChu, e);
        }

        private void pictureBox1_click(object sender, EventArgs e)
        {
            // Placeholder
        }
    }
}