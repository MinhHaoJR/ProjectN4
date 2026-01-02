using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ProjectN4.DAL;
using ProjectN4.DTO;

namespace ProjectN4
{
    public partial class frmQuanLyNhanVien : Form
    {
        NhanVienDAL dal = new NhanVienDAL();

        public frmQuanLyNhanVien()
        {
            InitializeComponent();
        }

        // ===========================================
        // 1. FORM LOAD
        // ===========================================
        private void frmQuanLyNhanVien_Load(object sender, EventArgs e)
        {
            // Tạo Placeholder cho ô tìm kiếm (Dùng hàm tự chế bên dưới)
            SetPlaceholder(txtTimKiem, "Nhập tên hoặc mã NV...");

            // Nạp dữ liệu vào ComboBox Chức vụ
            cboChucVu.Items.Clear();
            cboChucVu.Items.Add("Quản lý");
            cboChucVu.Items.Add("Lễ tân");
            cboChucVu.Items.Add("Kế toán");
            cboChucVu.SelectedIndex = 1; // Mặc định chọn Lễ tân

            // Nạp dữ liệu vào ComboBox Trạng thái
            cboTrangThai.Items.Clear();
            cboTrangThai.Items.Add("Đang làm việc");
            cboTrangThai.Items.Add("Đã nghỉ");
            cboTrangThai.SelectedIndex = 0;

            // Nạp dữ liệu Chi nhánh (Tạm thời add cứng vì chưa làm Form Chi nhánh)
            cboChiNhanh.Items.Clear();
            cboChiNhanh.Items.Add("Chi nhánh Chính");
            cboChiNhanh.SelectedIndex = 0;

            // Tải danh sách lên lưới
            LoadData();

            // Cấu hình bảng cho đẹp
            dgvNhanVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvNhanVien.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvNhanVien.MultiSelect = false;
            dgvNhanVien.ReadOnly = true;
        }

        // Hàm tải dữ liệu chung
        void LoadData(string tuKhoa = "")
        {
            dgvNhanVien.DataSource = dal.GetNhanVien(tuKhoa);

            // Đặt tên cột tiếng Việt cho dễ nhìn
            if (dgvNhanVien.Columns["MaNV"] != null) dgvNhanVien.Columns["MaNV"].HeaderText = "Mã NV";
            if (dgvNhanVien.Columns["HoTen"] != null) dgvNhanVien.Columns["HoTen"].HeaderText = "Họ và Tên";
            if (dgvNhanVien.Columns["NgaySinh"] != null) dgvNhanVien.Columns["NgaySinh"].HeaderText = "Ngày sinh";
            if (dgvNhanVien.Columns["SDT"] != null) dgvNhanVien.Columns["SDT"].HeaderText = "SĐT";
            if (dgvNhanVien.Columns["ChucVu"] != null) dgvNhanVien.Columns["ChucVu"].HeaderText = "Chức vụ";
            if (dgvNhanVien.Columns["TenDangNhap"] != null) dgvNhanVien.Columns["TenDangNhap"].HeaderText = "Tài khoản";

            // Ẩn cột Mật khẩu đi (Bảo mật)
            if (dgvNhanVien.Columns["MatKhau"] != null) dgvNhanVien.Columns["MatKhau"].Visible = false;
        }

        // ===========================================
        // 2. CLICK VÀO BẢNG -> HIỆN THÔNG TIN
        // ===========================================
        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvNhanVien.Rows[e.RowIndex];

                txtMaNV.Text = row.Cells["MaNV"].Value.ToString();
                txtHoTen.Text = row.Cells["HoTen"].Value.ToString();
                txtSDT.Text = row.Cells["SDT"].Value.ToString();
                txtTaiKhoan.Text = row.Cells["TenDangNhap"].Value.ToString();
                txtMatKhau.Text = row.Cells["MatKhau"].Value.ToString();

                // Xử lý Ngày sinh (Tránh lỗi nếu trong SQL đang NULL)
                if (row.Cells["NgaySinh"].Value != DBNull.Value)
                {
                    dtpNgaySinh.Value = Convert.ToDateTime(row.Cells["NgaySinh"].Value);
                }
                else
                {
                    dtpNgaySinh.Value = DateTime.Now;
                }

                cboChucVu.Text = row.Cells["ChucVu"].Value.ToString();
                cboTrangThai.Text = row.Cells["TrangThai"].Value.ToString();

                // Khi Sửa thì KHÔNG cho đổi tên đăng nhập (tránh lỗi)
                txtTaiKhoan.ReadOnly = true;
            }
        }

        // ===========================================
        // 3. CÁC NÚT CHỨC NĂNG
        // ===========================================

        // Nút THÊM
        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtHoTen.Text) || string.IsNullOrWhiteSpace(txtTaiKhoan.Text))
            {
                MessageBox.Show("Vui lòng nhập Họ tên và Tài khoản!", "Thiếu thông tin");
                return;
            }

            // Kiểm tra xem Tài khoản đã tồn tại chưa
            if (dal.KiemTraTrungTaiKhoan(txtTaiKhoan.Text.Trim()))
            {
                MessageBox.Show("Tên đăng nhập này đã có người dùng! Vui lòng chọn tên khác.", "Trùng lặp");
                return;
            }

            NhanVienDTO nv = new NhanVienDTO();
            nv.MaChiNhanh = 1; // Giả sử chi nhánh ID=1
            nv.HoTen = txtHoTen.Text.Trim();
            nv.NgaySinh = dtpNgaySinh.Value;
            nv.SDT = txtSDT.Text.Trim();
            nv.ChucVu = cboChucVu.Text;
            nv.TenDangNhap = txtTaiKhoan.Text.Trim();
            nv.MatKhau = txtMatKhau.Text.Trim();
            nv.TrangThai = cboTrangThai.Text;

            if (dal.ThemNhanVien(nv))
            {
                MessageBox.Show("Thêm nhân viên thành công!");
                LoadData();
                ResetInput();
            }
            else MessageBox.Show("Thêm thất bại!");
        }

        // Nút SỬA
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaNV.Text))
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần sửa!");
                return;
            }

            NhanVienDTO nv = new NhanVienDTO();
            nv.MaNV = int.Parse(txtMaNV.Text);
            nv.MaChiNhanh = 1;
            nv.HoTen = txtHoTen.Text.Trim();
            nv.NgaySinh = dtpNgaySinh.Value;
            nv.SDT = txtSDT.Text.Trim();
            nv.ChucVu = cboChucVu.Text;
            nv.MatKhau = txtMatKhau.Text.Trim(); // Cho phép đổi mật khẩu
            nv.TrangThai = cboTrangThai.Text;

            if (dal.SuaNhanVien(nv))
            {
                MessageBox.Show("Cập nhật thành công!");
                LoadData();
                ResetInput();
            }
            else MessageBox.Show("Cập nhật thất bại!");
        }

        // Nút XÓA (Cảnh báo kỹ)
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaNV.Text)) return;

            DialogResult rs = MessageBox.Show($"Bạn có chắc muốn xóa nhân viên {txtHoTen.Text}?\n\nDữ liệu sẽ mất vĩnh viễn.",
                                              "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (rs == DialogResult.Yes)
            {
                try
                {
                    if (dal.XoaNhanVien(int.Parse(txtMaNV.Text)))
                    {
                        MessageBox.Show("Đã xóa!");
                        LoadData();
                        ResetInput();
                    }
                }
                catch
                {
                    MessageBox.Show("Không thể xóa nhân viên này vì họ đã lập nhiều Hóa đơn trong hệ thống.\nHãy chuyển trạng thái sang 'Đã nghỉ' để khóa tài khoản.", "Ràng buộc dữ liệu");
                }
            }
        }

        // Nút LÀM MỚI
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ResetInput();
            txtTimKiem.Clear();
            LoadData(); // Load lại danh sách gốc
        }

        // Hàm xóa trắng các ô nhập
        void ResetInput()
        {
            txtMaNV.Clear();
            txtHoTen.Clear();
            txtSDT.Clear();
            txtTaiKhoan.Clear();
            txtMatKhau.Clear();
            txtTaiKhoan.ReadOnly = false; // Mở lại ô User để nhập mới
            cboChucVu.SelectedIndex = 1;
            cboTrangThai.SelectedIndex = 0;
            dtpNgaySinh.Value = DateTime.Now;
            txtHoTen.Focus();
        }

        // Tìm kiếm khi gõ chữ
        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            if (txtTimKiem.ForeColor == Color.Gray) return; // Bỏ qua placeholder
            LoadData(txtTimKiem.Text);
        }

        // ===========================================
        // 4. HÀM TẠO PLACEHOLDER (Do dùng .NET cũ)
        // ===========================================
        private void SetPlaceholder(TextBox txt, string placeholder)
        {
            txt.Text = placeholder;
            txt.ForeColor = Color.Gray;

            txt.GotFocus += (s, e) => {
                if (txt.Text == placeholder)
                {
                    txt.Text = "";
                    txt.ForeColor = Color.Black;
                }
            };

            txt.LostFocus += (s, e) => {
                if (string.IsNullOrWhiteSpace(txt.Text))
                {
                    txt.Text = placeholder;
                    txt.ForeColor = Color.Gray;
                }
            };
        }
    }
}