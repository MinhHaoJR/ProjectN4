using System;
using System.Data;
using System.Windows.Forms;
using ProjectN4.DAL;
using ProjectN4.DTO;

namespace ProjectN4.GUI
{
    public partial class frmQuanLyKhachHang : Form
    {
        // Khởi tạo lớp DAL để tương tác với SQL
        KhachHangDAL dal = new KhachHangDAL();

        public frmQuanLyKhachHang()
        {
            InitializeComponent();
        }

        // 1. SỰ KỆN KHI MỞ FORM (Load dữ liệu lên bảng)
        private void frmQuanLyKhachHang_Load(object sender, EventArgs e)
        {
            LoadData(); // Tải toàn bộ danh sách

            // Cấu hình bảng cho đẹp (nếu chưa chỉnh bên Designer)
            dgvKhachHang.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvKhachHang.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvKhachHang.MultiSelect = false;
            dgvKhachHang.ReadOnly = true;
        }

        // Hàm dùng chung để tải dữ liệu (có hỗ trợ tìm kiếm)
        void LoadData(string tuKhoa = "")
        {
            dgvKhachHang.DataSource = dal.GetKhachHang(tuKhoa);

            // Đặt tên tiếng Việt cho tiêu đề cột
            if (dgvKhachHang.Columns["MaKH"] != null) dgvKhachHang.Columns["MaKH"].HeaderText = "Mã KH";
            if (dgvKhachHang.Columns["HoTen"] != null) dgvKhachHang.Columns["HoTen"].HeaderText = "Họ và Tên";
            if (dgvKhachHang.Columns["CCCD_Passport"] != null) dgvKhachHang.Columns["CCCD_Passport"].HeaderText = "CCCD/Passport";
            if (dgvKhachHang.Columns["SDT"] != null) dgvKhachHang.Columns["SDT"].HeaderText = "Điện thoại";
            if (dgvKhachHang.Columns["Email"] != null) dgvKhachHang.Columns["Email"].HeaderText = "Email";
            if (dgvKhachHang.Columns["DiaChi"] != null) dgvKhachHang.Columns["DiaChi"].HeaderText = "Địa chỉ";
            if (dgvKhachHang.Columns["QuocTich"] != null) dgvKhachHang.Columns["QuocTich"].HeaderText = "Quốc tịch";
        }

        // 2. SỰ KỆN CLICK VÀO DÒNG TRONG BẢNG -> HIỆN LÊN TEXTBOX
        private void dgvKhachHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Kiểm tra xem có click đúng dòng dữ liệu không
            {
                DataGridViewRow row = dgvKhachHang.Rows[e.RowIndex];

                // Đổ dữ liệu vào các ô nhập
                txtMaKH.Text = row.Cells["MaKH"].Value.ToString();
                txtHoTen.Text = row.Cells["HoTen"].Value.ToString();
                txtCCCD.Text = row.Cells["CCCD_Passport"].Value.ToString();
                txtSDT.Text = row.Cells["SDT"].Value.ToString();
                txtEmail.Text = row.Cells["Email"].Value.ToString();
                txtDiaChi.Text = row.Cells["DiaChi"].Value.ToString();
                txtQuocTich.Text = row.Cells["QuocTich"].Value.ToString();
            }
        }

        // 3. NÚT THÊM MỚI
        private void btnThem_Click(object sender, EventArgs e)
        {
            // Kiểm tra nhập liệu
            if (string.IsNullOrWhiteSpace(txtHoTen.Text) || string.IsNullOrWhiteSpace(txtCCCD.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Họ tên và CCCD!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Tạo đối tượng DTO
            KhachHangDTO kh = new KhachHangDTO();
            kh.HoTen = txtHoTen.Text.Trim();
            kh.CCCD_Passport = txtCCCD.Text.Trim();
            kh.SDT = txtSDT.Text.Trim();
            kh.Email = txtEmail.Text.Trim();
            kh.DiaChi = txtDiaChi.Text.Trim();
            kh.QuocTich = txtQuocTich.Text.Trim();

            try
            {
                if (dal.ThemKhachHang(kh))
                {
                    MessageBox.Show("Thêm khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData(); // Tải lại bảng
                    ResetInput(); // Xóa trắng ô nhập
                }
                else
                {
                    MessageBox.Show("Thêm thất bại! Vui lòng thử lại.", "Lỗi");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi (Có thể do trùng số CCCD): " + ex.Message, "Lỗi hệ thống");
            }
        }

        // 4. NÚT CẬP NHẬT (SỬA)
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaKH.Text))
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần sửa từ danh sách!", "Chưa chọn khách");
                return;
            }

            KhachHangDTO kh = new KhachHangDTO();
            kh.MaKH = int.Parse(txtMaKH.Text); // Lấy ID để biết sửa ai
            kh.HoTen = txtHoTen.Text.Trim();
            kh.CCCD_Passport = txtCCCD.Text.Trim();
            kh.SDT = txtSDT.Text.Trim();
            kh.Email = txtEmail.Text.Trim();
            kh.DiaChi = txtDiaChi.Text.Trim();
            kh.QuocTich = txtQuocTich.Text.Trim();

            if (dal.SuaKhachHang(kh))
            {
                MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo");
                LoadData();
            }
            else
            {
                MessageBox.Show("Cập nhật thất bại!", "Lỗi");
            }
        }

        // 5. NÚT XÓA KHÁCH HÀNG
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaKH.Text))
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần xóa!", "Chưa chọn khách");
                return;
            }

            // Hỏi xác nhận trước khi xóa
            DialogResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa khách hàng: {txtHoTen.Text}?",
                                                  "Xác nhận xóa",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    if (dal.XoaKhachHang(int.Parse(txtMaKH.Text)))
                    {
                        MessageBox.Show("Đã xóa thành công!");
                        LoadData();
                        ResetInput();
                    }
                }
                catch
                {
                    // Lỗi này xảy ra nếu khách hàng đã từng Đặt phòng (Ràng buộc khóa ngoại SQL)
                    MessageBox.Show("Không thể xóa khách hàng này vì họ đã có lịch sử đặt phòng/hóa đơn trong hệ thống.\n(Chỉ được phép sửa thông tin).",
                                    "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // 6. NÚT LÀM MỚI (Xóa trắng ô nhập để nhập người mới)
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ResetInput();
            txtTimKiem.Clear();
            LoadData(); // Load lại toàn bộ danh sách gốc
        }

        // Hàm phụ để xóa trắng các ô Textbox
        void ResetInput()
        {
            txtMaKH.Clear();
            txtHoTen.Clear();
            txtCCCD.Clear();
            txtSDT.Clear();
            txtEmail.Clear();
            txtDiaChi.Clear();
            txtQuocTich.Text = "Việt Nam"; // Reset về mặc định
            txtHoTen.Focus();
        }

        // 7. TÌM KIẾM (Gõ chữ là tự lọc)
        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            LoadData(txtTimKiem.Text.Trim());
        }
    }
}