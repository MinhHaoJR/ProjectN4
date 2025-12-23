using ProjectN4.DAL; // Nơi chứa file DbSettings
using System;
using System.Data;
using System.Data.SqlClient; // Thư viện kết nối SQL Server
using System.Drawing; // Thư viện giao diện
using System.Windows.Forms;

namespace ProjectN4.GUI
{
    public partial class frmQuanLyPhong : Form
    {
        // =================================================================================
        // 1. CẤU HÌNH KẾT NỐI
        // =================================================================================
        // Lấy thông tin từ Class DbSettings. Nếu đổi Server chỉ cần sửa trong DbSettings là xong.
        string chuoiketNoi = $"Data Source={DbSettings.ServerIP};Initial Catalog={DbSettings.DatabaseName};User ID={DbSettings.UserID};Password={DbSettings.Password};";

        // Biến lưu trữ dữ liệu tạm thời để hiển thị lên lưới
        DataTable dt;

        public frmQuanLyPhong()
        {
            InitializeComponent();
        }

        // =================================================================================
        // 2. SỰ KIỆN KHI MỞ FORM (FORM LOAD)
        // =================================================================================
        private void frmQuanLyPhong_Load(object sender, EventArgs e)
        {
            try
            {
                // A. Trang trí giao diện bảng (Màu xanh cổ vịt, font chữ đậm)
                dgvPhong.EnableHeadersVisualStyles = false;
                dgvPhong.ColumnHeadersDefaultCellStyle.BackColor = Color.Teal;
                dgvPhong.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgvPhong.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                dgvPhong.ColumnHeadersHeight = 40;
                dgvPhong.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
                dgvPhong.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // Đưa bảng lên lớp trên cùng để tránh bị panel khác che khuất
                dgvPhong.BringToFront();

                // B. Tải dữ liệu từ Database lên
                HienThiDanhSach();

                // C. Khóa các nút chức năng khi chưa chọn dòng nào
                KhoaDieuKhien(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi khi khởi động Form: " + ex.Message);
            }
        }

        // =================================================================================
        // 3. CÁC HÀM XỬ LÝ CHÍNH (LOGIC DÙNG CHUNG)
        // =================================================================================

        // Hàm tải danh sách phòng từ SQL
        private void HienThiDanhSach()
        {
            try
            {
                string sql = "SELECT * FROM Phong";

                // Sử dụng 'using' để đảm bảo kết nối tự động đóng ngay sau khi lấy xong dữ liệu -> Giúp app nhẹ hơn
                using (SqlConnection ketNoi = new SqlConnection(chuoiketNoi))
                {
                    ketNoi.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(sql, ketNoi))
                    {
                        dt = new DataTable();
                        da.Fill(dt); // Đổ dữ liệu vào kho chứa
                        dgvPhong.DataSource = dt; // Hiển thị lên lưới
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách: " + ex.Message, "Lỗi Kết Nối");
            }
        }

        // Hàm xử lý Thêm/Sửa/Xóa tập trung (Để code gọn gàng, dễ sửa chữa)
        private void ThucThiLenhSQL(string sql, string hanhDong)
        {
            try
            {
                using (SqlConnection ketNoi = new SqlConnection(chuoiketNoi))
                {
                    ketNoi.Open();
                    using (SqlCommand lenh = new SqlCommand(sql, ketNoi))
                    {
                        // --- A. XỬ LÝ THAM SỐ CHI NHÁNH (SỬA LỖI TẠI ĐÂY) ---
                        // Kiểm tra xem người dùng có nhập đúng số vào ô Chi Nhánh không
                        int maCN = 1;
                        if (!int.TryParse(txtChiNhanh.Text, out maCN))
                        {
                            MessageBox.Show("Mã Chi Nhánh phải là một con số!", "Cảnh báo");
                            return; // Dừng lại không lưu nữa
                        }
                        lenh.Parameters.AddWithValue("@MaCN", maCN);

                        // --- B. XỬ LÝ THAM SỐ GIÁ TIỀN ---
                        decimal giaTien = 0;
                        if (!decimal.TryParse(txtGiaPhong.Text, out giaTien))
                        {
                            MessageBox.Show("Giá phòng phải là số!", "Cảnh báo");
                            return;
                        }
                        lenh.Parameters.AddWithValue("@Gia", giaTien);

                        // --- C. CÁC THAM SỐ CÒN LẠI ---
                        lenh.Parameters.AddWithValue("@So", txtSoPhong.Text);
                        lenh.Parameters.AddWithValue("@Loai", cboLoaiPhong.Text);
                        lenh.Parameters.AddWithValue("@TrangThai", cboTrangThai.Text);

                        // Nếu là Sửa/Xóa thì cần thêm Mã Phòng để biết sửa dòng nào
                        if (hanhDong != "Thêm")
                            lenh.Parameters.AddWithValue("@Ma", txtMaPhong.Text);

                        // --- D. THỰC THI ---
                        int ketQua = lenh.ExecuteNonQuery();
                        if (ketQua > 0)
                        {
                            MessageBox.Show($"Đã {hanhDong} thành công!", "Thông báo");
                            HienThiDanhSach(); // Tải lại bảng ngay lập tức
                            XoaTrangO();       // Xóa trắng ô nhập
                            KhoaDieuKhien(true); // Reset trạng thái nút
                        }
                        else
                        {
                            MessageBox.Show($"Thao tác {hanhDong} thất bại! Có thể mã phòng không tồn tại.", "Lỗi");
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                // Bắt lỗi riêng của SQL (ví dụ nhập trùng mã, sai khóa ngoại Chi nhánh)
                if (sqlEx.Number == 547) // Mã lỗi khóa ngoại
                    MessageBox.Show("Lỗi: Mã Chi Nhánh này không tồn tại trong hệ thống!", "Lỗi Dữ Liệu");
                else
                    MessageBox.Show("Lỗi SQL: " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi chung: " + ex.ToString());
            }
        }

        // Hàm quản lý ẩn hiện nút bấm
        private void KhoaDieuKhien(bool trangThai)
        {
            btnThem.Enabled = true;       // Luôn cho phép thêm
            btnSua.Enabled = !trangThai;  // Chỉ sáng khi chọn dòng
            btnXoa.Enabled = !trangThai;  // Chỉ sáng khi chọn dòng
        }

        // Hàm dọn dẹp ô nhập liệu
        private void XoaTrangO()
        {
            txtMaPhong.Clear();
            txtChiNhanh.Clear(); // Nhớ xóa cả ô Chi Nhánh
            txtSoPhong.Clear();
            txtGiaPhong.Clear();
            cboLoaiPhong.SelectedIndex = -1;
            cboTrangThai.SelectedIndex = -1;
            txtSoPhong.Focus();
        }

        // =================================================================================
        // 4. SỰ KIỆN TƯƠNG TÁC (CLICK, BUTTON)
        // =================================================================================

        // Khi bấm vào bảng -> Đổ dữ liệu lên TextBox
        private void dgvPhong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow dong = dgvPhong.Rows[e.RowIndex];

                // Đảm bảo tên cột trong ["..."] khớp chính xác với SQL
                txtMaPhong.Text = dong.Cells["MaPhong"].Value.ToString();

                // Kiểm tra xem trong SQL có cột MaChiNhanh không trước khi gán để tránh lỗi
                if (dong.Cells["MaChiNhanh"].Value != null)
                    txtChiNhanh.Text = dong.Cells["MaChiNhanh"].Value.ToString();

                txtSoPhong.Text = dong.Cells["SoPhong"].Value.ToString();
                txtGiaPhong.Text = dong.Cells["GiaPhong"].Value.ToString();
                cboLoaiPhong.Text = dong.Cells["LoaiPhong"].Value.ToString();
                cboTrangThai.Text = dong.Cells["TrangThai"].Value.ToString();

                KhoaDieuKhien(false); // Mở khóa nút Sửa/Xóa
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSoPhong.Text) || string.IsNullOrEmpty(txtChiNhanh.Text))
            {
                MessageBox.Show("Vui lòng nhập Số phòng và Chi nhánh!", "Thiếu thông tin");
                return;
            }

            string sql = "INSERT INTO Phong (MaChiNhanh, SoPhong, LoaiPhong, GiaPhong, TrangThai) VALUES (@MaCN, @So, @Loai, @Gia, @TrangThai)";
            ThucThiLenhSQL(sql, "Thêm");
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaPhong.Text)) return;

            string sql = "UPDATE Phong SET MaChiNhanh=@MaCN, SoPhong=@So, LoaiPhong=@Loai, GiaPhong=@Gia, TrangThai=@TrangThai WHERE MaPhong=@Ma";
            ThucThiLenhSQL(sql, "Sửa");
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaPhong.Text)) return;

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa phòng này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                string sql = "DELETE FROM Phong WHERE MaPhong=@Ma";
                ThucThiLenhSQL(sql, "Xóa");
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            XoaTrangO();
            HienThiDanhSach();
            KhoaDieuKhien(true);
        }

        // Các hàm sự kiện thừa (có thể xóa đi nếu muốn code gọn hơn nữa)
        private void txtSoPhong_TextChanged(object sender, EventArgs e) { }
        private void txtGiaPhong_TextChanged(object sender, EventArgs e) { }
        private void cboLoaiPhong_SelectedIndexChanged(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
    }
}