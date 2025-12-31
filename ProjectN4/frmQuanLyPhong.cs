using ProjectN4.DAL;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace ProjectN4.GUI
{
    public partial class frmQuanLyPhong : Form
    {
        string chuoiketNoi = $"Data Source={DbSettings.ServerIP};Initial Catalog={DbSettings.DatabaseName};User ID={DbSettings.UserID};Password={DbSettings.Password};";

        public frmQuanLyPhong()
        {
            InitializeComponent();
        }

        private void frmQuanLyPhong_Load(object sender, EventArgs e)
        {
            try
            {
                dgvPhong.Visible = false;
                SetupGiaoDien();

                if (cboLocLoaiPhong.Items.Count > 0) cboLocLoaiPhong.SelectedIndex = 0;
                if (cboLocTrangThai.Items.Count > 0) cboLocTrangThai.SelectedIndex = 0;

                TaiDanhSachPhong();
                
                dgvPhong.Visible = true;
                KhoaDieuKhien(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi động: " + ex.Message);
            }
        }

        private void SetupGiaoDien()
        {
            // --- 1. Cấu hình vị trí (Docking & Z-Order) ---
            // Để đảm bảo không bị che lấp, ta sẽ dock các phần tử Top trước, sau đó mới đến Fill.

            // Phần thông tin (Trên cùng)
            grpThongTin.Dock = DockStyle.Top;
            grpThongTin.Height = 160;
            grpThongTin.BringToFront(); // Đảm bảo nổi lên trên cùng

            // Phần chức năng (Ngay dưới thông tin)
            pnlChucNang.Dock = DockStyle.Top;
            pnlChucNang.Height = 60;
            pnlChucNang.BringToFront();

            // Phần tìm kiếm (Dưới chức năng)
            grpTimKiem.Dock = DockStyle.Top;
            grpTimKiem.Height = 80;
            grpTimKiem.BringToFront();

            // Phần danh sách (Chiếm toàn bộ phần còn lại)
            dgvPhong.Dock = DockStyle.Fill;
            dgvPhong.BringToFront(); // THAY ĐỔI: Thử BringToFront thay vì SendToBack để xem nó có hiện lên không.
                                     // Thông thường Dock=Fill sẽ tự động nằm dưới Dock=Top nếu được add sau.
                                     // Nhưng nếu bị mất, hãy thử đưa nó lên để kiểm tra. 
                                     // Tốt nhất là kiểm tra Document Outline trong Designer để sắp xếp lại.

            // --- 2. Cấu hình giao diện Bảng (Visual Style) ---
            dgvPhong.BackgroundColor = Color.White;
            dgvPhong.BorderStyle = BorderStyle.Fixed3D; // Đổi sang Fixed3D để nhìn rõ biên grid

            // Bắt buộc hiển thị tiêu đề cột
            dgvPhong.ColumnHeadersVisible = true;
            dgvPhong.EnableHeadersVisualStyles = false; // Tắt style mặc định hệ thống để dùng style tùy chỉnh bên dưới

            // Style cho tiêu đề cột
            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();
            headerStyle.BackColor = Color.FromArgb(0, 122, 204);
            headerStyle.ForeColor = Color.White;
            headerStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            headerStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // Căn giữa tiêu đề
            dgvPhong.ColumnHeadersDefaultCellStyle = headerStyle;
            dgvPhong.ColumnHeadersHeight = 40;
            dgvPhong.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            // Style cho dòng dữ liệu
            dgvPhong.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            dgvPhong.RowTemplate.Height = 30;
            dgvPhong.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPhong.AllowUserToAddRows = false;
            dgvPhong.ReadOnly = true;
            dgvPhong.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // =================================================================================
        // 1. CÁC HÀM XỬ LÝ NGHIỆP VỤ (CORE LOGIC)
        // =================================================================================

        private void ThucHienCheckIn()
        {
            if (string.IsNullOrEmpty(txtMaPhong.Text)) { MessageBox.Show("Vui lòng chọn phòng (chuột phải vào dòng)!"); return; }
            
            if (cboTrangThai.Text == "Đang ở")
            {
                MessageBox.Show("Phòng đang có khách, không thể nhận phòng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            frmCheckIn f = new frmCheckIn();
            f.MaPhongCanCheckIn = txtMaPhong.Text;
            if (f.ShowDialog() == DialogResult.OK)
            {
                TaiDanhSachPhong();
                XoaTrangO();
            }
        }

        private void ThucHienCheckOut()
        {
            if (string.IsNullOrEmpty(txtMaPhong.Text)) { MessageBox.Show("Vui lòng chọn phòng!"); return; }

            if (cboTrangThai.Text != "Đang ở")
            {
                MessageBox.Show("Phòng chưa có khách, không thể trả phòng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            frmCheckOut f = new frmCheckOut();
            f.MaPhongCanCheckOut = txtMaPhong.Text;
            if (f.ShowDialog() == DialogResult.OK)
            {
                TaiDanhSachPhong();
                XoaTrangO();
            }
        }

        private int LayMaDatPhong(string maPhong)
        {
            using (SqlConnection conn = new SqlConnection(chuoiketNoi))
            {
                conn.Open();
                string sql = @"SELECT TOP 1 MaDatPhong FROM DAT_PHONG WHERE MaPhong = @MaPhong AND TrangThai = N'Đang ở'";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@MaPhong", maPhong);
                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }

        private void ThucHienThemDichVu()
        {
            if (string.IsNullOrEmpty(txtMaPhong.Text)) { MessageBox.Show("Vui lòng chọn phòng!"); return; }

            if (cboTrangThai.Text != "Đang ở")
            {
                MessageBox.Show("Phòng chưa có khách, không thể thêm dịch vụ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maDatPhong = LayMaDatPhong(txtMaPhong.Text);
            if (maDatPhong == 0)
            {
                MessageBox.Show("Không tìm thấy mã đặt phòng đang ở cho phòng này!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            frmThemDichVu f = new frmThemDichVu();
            f.MaPhongDuocChon = txtMaPhong.Text;
            f.ShowDialog();
        }

        private void ThucHienXemChiTiet()
        {
            if (string.IsNullOrEmpty(txtMaPhong.Text))
            {
                MessageBox.Show("Vui lòng chọn phòng cần xem!", "Thông báo");
                return;
            }

            if (cboTrangThai.Text != "Đang ở")
            {
                MessageBox.Show($"Thông tin phòng {txtSoPhong.Text}\nLoại: {cboLoaiPhong.Text}\nGiá: {txtGiaPhong.Text:N0} VNĐ\nTrạng thái: {cboTrangThai.Text}", "Thông tin phòng");
                return;
            }

            using (SqlConnection conn = new SqlConnection(chuoiketNoi))
            {
                try
                {
                    conn.Open();
                    string sql = @"SELECT k.HoTen, k.CCCD_Passport, d.NgayCheckIn, d.TienCoc
                                   FROM DAT_PHONG d
                                   JOIN KHACH_HANG k ON d.MaKH = k.MaKH
                                   WHERE d.MaPhong = @MaPhong AND d.TrangThai = N'Đang ở'";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@MaPhong", txtMaPhong.Text);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string tenKhach = reader["HoTen"].ToString();
                            string cmnd = reader["CCCD_Passport"].ToString();
                            DateTime ngayVao = Convert.ToDateTime(reader["NgayCheckIn"]);
                            decimal tienCoc = Convert.ToDecimal(reader["TienCoc"]);

                            string thongTin = $"--- THÔNG TIN KHÁCH ĐANG Ở ---\n\n" +
                                              $"👤 Khách hàng: {tenKhach}\n" +
                                              $"🆔 CMND/CCCD: {cmnd}\n" +
                                              $"🕒 Giờ vào: {ngayVao:dd/MM/yyyy HH:mm}\n" +
                                              $"💰 Tiền cọc: {tienCoc:N0} VNĐ";

                            MessageBox.Show(thongTin, "Chi tiết phòng " + txtSoPhong.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
            }
        }

        // =================================================================================
        // 2. SỰ KIỆN MENU CHUỘT PHẢI (CONTEXT MENU)
        // =================================================================================
        
        // Đây là nơi gọi các hàm nghiệp vụ khi bấm Menu
        private void mnuCheckIn_Click(object sender, EventArgs e) => ThucHienCheckIn();
        private void mnuCheckOut_Click(object sender, EventArgs e) => ThucHienCheckOut();
        private void mnuDichVu_Click(object sender, EventArgs e) => ThucHienThemDichVu();
        private void mnuChiTiet_Click(object sender, EventArgs e) => ThucHienXemChiTiet();

        // =================================================================================
        // 3. TẢI DỮ LIỆU & TÌM KIẾM
        // =================================================================================
        private void TaiDanhSachPhong()
        {
            using (SqlConnection ketNoi = new SqlConnection(chuoiketNoi))
            {
                try
                {
                    ketNoi.Open();
                    // ... (Phần code truy vấn SQL giữ nguyên như cũ của bạn) ...
                    string sql = @"SELECT p.MaPhong, p.SoPhong, p.LoaiPhong, p.GiaPhong, p.TrangThai, p.MaChiNhanh FROM PHONG p WHERE 1=1 ";
                    SqlCommand cmd = new SqlCommand(sql, ketNoi);

                    // ... (Phần thêm tham số lọc giữ nguyên) ...
                    if (cboLocLoaiPhong.SelectedIndex > 0 && cboLocLoaiPhong.Text != "Tất cả")
                    {
                        sql += " AND p.LoaiPhong = @Loai";
                        cmd.Parameters.AddWithValue("@Loai", cboLocLoaiPhong.Text);
                    }
                    if (cboLocTrangThai.SelectedIndex > 0 && cboLocTrangThai.Text != "Tất cả")
                    {
                        sql += " AND p.TrangThai = @TrangThai";
                        cmd.Parameters.AddWithValue("@TrangThai", cboLocTrangThai.Text);
                    }
                    // ...

                    cmd.CommandText = sql;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvPhong.DataSource = dt; // Gán dữ liệu

                    // --- QUAN TRỌNG: Đặt tên cột hiển thị ---
                    // Gọi hàm định dạng ngay sau khi gán nguồn dữ liệu
                    DinhDangLuoi();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
                }
            }
        }

        private void DinhDangLuoi()
        {
            if (dgvPhong.Columns.Count > 0)
            {
                if (dgvPhong.Columns.Contains("MaPhong"))
                {
                    dgvPhong.Columns["MaPhong"].HeaderText = "Mã Phòng";
                    dgvPhong.Columns["MaPhong"].Width = 80;
                    dgvPhong.Columns["MaPhong"].Visible = true;
                }

                if (dgvPhong.Columns.Contains("SoPhong"))
                {
                    dgvPhong.Columns["SoPhong"].HeaderText = "Số Phòng";
                    dgvPhong.Columns["SoPhong"].Width = 100;
                }

                if (dgvPhong.Columns.Contains("LoaiPhong"))
                    dgvPhong.Columns["LoaiPhong"].HeaderText = "Loại Phòng";

                if (dgvPhong.Columns.Contains("GiaPhong"))
                {
                    dgvPhong.Columns["GiaPhong"].HeaderText = "Giá Phòng";
                    dgvPhong.Columns["GiaPhong"].DefaultCellStyle.Format = "N0";
                    dgvPhong.Columns["GiaPhong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                if (dgvPhong.Columns.Contains("TrangThai"))
                    dgvPhong.Columns["TrangThai"].HeaderText = "Trạng Thái";

                if (dgvPhong.Columns.Contains("MaChiNhanh"))
                    dgvPhong.Columns["MaChiNhanh"].Visible = false;
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e) => TaiDanhSachPhong();
        private void cboLocLoaiPhong_SelectedIndexChanged(object sender, EventArgs e) => TaiDanhSachPhong();
        private void cboLocTrangThai_SelectedIndexChanged(object sender, EventArgs e) => TaiDanhSachPhong();
        private void txtTimCCCD_KeyDown(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Enter) TaiDanhSachPhong(); }

        // =================================================================================
        // 4. QUẢN LÝ THÊM / SỬA / XÓA (BUTTONS)
        // =================================================================================
        
        private void dgvPhong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow r = dgvPhong.Rows[e.RowIndex];
                txtMaPhong.Text = r.Cells["MaPhong"].Value?.ToString();
                txtChiNhanh.Text = r.Cells["MaChiNhanh"].Value?.ToString();
                txtSoPhong.Text = r.Cells["SoPhong"].Value?.ToString();
                txtGiaPhong.Text = r.Cells["GiaPhong"].Value?.ToString();
                cboLoaiPhong.Text = r.Cells["LoaiPhong"].Value?.ToString();
                cboTrangThai.Text = r.Cells["TrangThai"].Value?.ToString();

                KhoaDieuKhien(false);
            }
        }

        // Sự kiện: CHUỘT PHẢI VÀO BẢNG -> CHỌN DÒNG
        private void dgvPhong_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                dgvPhong.ClearSelection();
                dgvPhong.Rows[e.RowIndex].Selected = true;
                dgvPhong.CurrentCell = dgvPhong.Rows[e.RowIndex].Cells[0];
                dgvPhong_CellClick(sender, new DataGridViewCellEventArgs(0, e.RowIndex));
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            ThucThiLenhSQL("INSERT INTO PHONG (MaChiNhanh, SoPhong, LoaiPhong, GiaPhong, TrangThai) VALUES (@MaCN, @So, @Loai, @Gia, @TrangThai)", "Thêm");
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            ThucThiLenhSQL("UPDATE PHONG SET MaChiNhanh=@MaCN, SoPhong=@So, LoaiPhong=@Loai, GiaPhong=@Gia, TrangThai=@TrangThai WHERE MaPhong=@Ma", "Sửa");
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Xóa phòng này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ThucThiLenhSQL("DELETE FROM PHONG WHERE MaPhong=@Ma", "Xóa");
            }
        }

        private void ThucThiLenhSQL(string sql, string hanhDong)
        {
            try
            {
                using (SqlConnection ketNoi = new SqlConnection(chuoiketNoi))
                {
                    ketNoi.Open();
                    using (SqlCommand lenh = new SqlCommand(sql, ketNoi))
                    {
                        int maCN = 0; int.TryParse(txtChiNhanh.Text, out maCN);
                        decimal gia = 0; decimal.TryParse(txtGiaPhong.Text, out gia);

                        lenh.Parameters.AddWithValue("@MaCN", maCN);
                        lenh.Parameters.AddWithValue("@Gia", gia);
                        lenh.Parameters.AddWithValue("@So", txtSoPhong.Text);
                        lenh.Parameters.AddWithValue("@Loai", cboLoaiPhong.Text);
                        lenh.Parameters.AddWithValue("@TrangThai", cboTrangThai.Text);
                        
                        if (hanhDong != "Thêm") lenh.Parameters.AddWithValue("@Ma", txtMaPhong.Text);

                        if (lenh.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Thao tác thành công!", "Thông báo");
                            TaiDanhSachPhong();
                            XoaTrangO();
                            KhoaDieuKhien(true);
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi SQL: " + ex.Message); }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            XoaTrangO();
            cboLocLoaiPhong.SelectedIndex = 0;
            cboLocTrangThai.SelectedIndex = 0;
            txtTimCCCD.Clear();
            TaiDanhSachPhong();
            KhoaDieuKhien(true);
        }

        // HÀM KHÓA ĐIỀU KHIỂN - ĐÃ XÓA CÁC NÚT CHECK-IN/OUT CŨ
        private void KhoaDieuKhien(bool t)
        {
            btnThem.Enabled = true;
            btnSua.Enabled = !t;
            btnXoa.Enabled = !t;
            // Đã xóa btnCheckIn, btnCheckOut, btnChiTiet ở đây vì không còn trên form
        }

        private void XoaTrangO()
        {
            txtMaPhong.Clear(); txtChiNhanh.Clear(); txtSoPhong.Clear(); txtGiaPhong.Clear();
            cboLoaiPhong.SelectedIndex = -1; cboTrangThai.SelectedIndex = -1;
        }

        // CÁC HÀM RỖNG ĐỂ TRÁNH LỖI DESIGNER
        private void txtSoPhong_TextChanged(object sender, EventArgs e) { }
        private void txtGiaPhong_TextChanged(object sender, EventArgs e) { }
        private void cboLoaiPhong_SelectedIndexChanged(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void labelMaPhong_Click(object sender, EventArgs e) { }
        private void txtMaPhong_TextChanged(object sender, EventArgs e) { }
        private void txtChiNhanh_TextChanged(object sender, EventArgs e) { }
    }
}