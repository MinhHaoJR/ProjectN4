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
        DataTable dt;

        public frmQuanLyPhong()
        {
            InitializeComponent();
        }

        private void frmQuanLyPhong_Load(object sender, EventArgs e)
        {
            try
            {
                // 1. Tắt tạm DataGridView
                dgvPhong.Visible = false;

                // 2. CẤU HÌNH GIAO DIỆN
                grpThongTin.Dock = DockStyle.Top;
                grpThongTin.Height = 160;

                pnlChucNang.Dock = DockStyle.Top;
                pnlChucNang.Height = 80;

                grpTimKiem.Dock = DockStyle.Top;
                grpTimKiem.Height = 70;

                dgvPhong.Dock = DockStyle.Fill;

                // 3. TRANG TRÍ BẢNG
                dgvPhong.EnableHeadersVisualStyles = false;
                dgvPhong.ColumnHeadersDefaultCellStyle.BackColor = Color.Teal;
                dgvPhong.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgvPhong.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                dgvPhong.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvPhong.RowHeadersVisible = false;
                dgvPhong.AllowUserToAddRows = false; // Tắt dòng thêm mới tự động
                dgvPhong.ReadOnly = true; // Chỉ đọc
                dgvPhong.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                // 4. SET MẶC ĐỊNH BỘ LỌC
                if (cboLocLoaiPhong.Items.Count > 0) cboLocLoaiPhong.SelectedIndex = 0;
                if (cboLocTrangThai.Items.Count > 0) cboLocTrangThai.SelectedIndex = 0;

                // 5. TẢI DỮ LIỆU
                HienThiDanhSach();

                // 6. Bật lại DataGridView
                dgvPhong.Visible = true;

                KhoaDieuKhien(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi động: " + ex.Message + "\n" + ex.StackTrace);
            }
        }


        private void HienThiDanhSach()
        {
            // Gọi hàm tìm kiếm để nạp dữ liệu
            btnTimKiem_Click(null, null);
        }

        // =================================================================================
        // HÀM TÌM KIẾM & LỌC (CORE) - ĐÃ SỬA ĐỂ HIỂN THỊ ĐƯỢC DỮ LIỆU
        // =================================================================================
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            using (SqlConnection ketNoi = new SqlConnection(chuoiketNoi))
            {
                try
                {
                    ketNoi.Open();

                    // SQL lấy dữ liệu cơ bản
                    string sql = @"
                        SELECT p.MaPhong, p.SoPhong, p.LoaiPhong, p.GiaPhong, p.TrangThai, p.MaChiNhanh
                        FROM PHONG p
                        WHERE 1=1 ";

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = ketNoi;

                    // --- ĐIỀU KIỆN LỌC ---
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

                    // Nếu có tìm CCCD thì dùng câu lệnh SQL phức tạp hơn (JOIN)
                    if (txtTimCCCD.Text.Trim().Length > 0)
                    {
                        sql = @"SELECT DISTINCT p.MaPhong, p.SoPhong, p.LoaiPhong, p.GiaPhong, p.TrangThai, p.MaChiNhanh
                                FROM PHONG p
                                JOIN DAT_PHONG dp ON p.MaPhong = dp.MaPhong AND dp.TrangThai = N'Đang ở'
                                JOIN KHACH_HANG kh ON dp.MaKH = kh.MaKH
                                WHERE kh.CCCD_Passport LIKE @CCCD";
                        cmd.Parameters.AddWithValue("@CCCD", "%" + txtTimCCCD.Text.Trim() + "%");
                    }

                    cmd.CommandText = sql;

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        dt = new DataTable();
                        da.Fill(dt);

                        // === KHẮC PHỤC LỖI KHÔNG HIỆN DỮ LIỆU ===
                        dgvPhong.DataSource = null; // Ngắt nguồn cũ
                        dgvPhong.Columns.Clear();   // Xóa sạch cột cũ bị lỗi
                        dgvPhong.AutoGenerateColumns = true; // Bắt buộc tự tạo cột mới

                        dgvPhong.DataSource = dt; // Gán nguồn mới
                        dgvPhong.Refresh(); // Vẽ lại lưới

                        // (Tùy chọn) Đặt tên tiếng Việt cho đẹp
                        if (dgvPhong.Columns.Count > 0)
                        {
                            if (dgvPhong.Columns.Contains("MaPhong")) dgvPhong.Columns["MaPhong"].HeaderText = "Mã";
                            if (dgvPhong.Columns.Contains("SoPhong")) dgvPhong.Columns["SoPhong"].HeaderText = "Số Phòng";
                            if (dgvPhong.Columns.Contains("LoaiPhong")) dgvPhong.Columns["LoaiPhong"].HeaderText = "Loại";
                            if (dgvPhong.Columns.Contains("GiaPhong"))
                            {
                                dgvPhong.Columns["GiaPhong"].HeaderText = "Giá";
                                dgvPhong.Columns["GiaPhong"].DefaultCellStyle.Format = "N0"; // Định dạng tiền
                            }
                            if (dgvPhong.Columns.Contains("TrangThai")) dgvPhong.Columns["TrangThai"].HeaderText = "Trạng Thái";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
                }
            }
        }

        // Sự kiện phím Enter
        private void txtTimCCCD_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) btnTimKiem_Click(sender, e);
        }

        // Sự kiện thay đổi Combobox -> Tự lọc luôn
        private void cboLocLoaiPhong_SelectedIndexChanged(object sender, EventArgs e) => btnTimKiem_Click(null, null);
        private void cboLocTrangThai_SelectedIndexChanged(object sender, EventArgs e) => btnTimKiem_Click(null, null);

        // =================================================================================
        // CÁC CHỨC NĂNG KHÁC (GIỮ NGUYÊN)
        // =================================================================================
        private void dgvPhong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow r = dgvPhong.Rows[e.RowIndex];
                txtMaPhong.Text = r.Cells["MaPhong"].Value.ToString();

                // Kiểm tra null an toàn
                if (r.Cells["MaChiNhanh"].Value != DBNull.Value)
                    txtChiNhanh.Text = r.Cells["MaChiNhanh"].Value.ToString();

                txtSoPhong.Text = r.Cells["SoPhong"].Value.ToString();
                txtGiaPhong.Text = r.Cells["GiaPhong"].Value.ToString();
                cboLoaiPhong.Text = r.Cells["LoaiPhong"].Value.ToString();
                cboTrangThai.Text = r.Cells["TrangThai"].Value.ToString();

                KhoaDieuKhien(false);
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
            if (MessageBox.Show("Xóa phòng này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
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
                        lenh.Parameters.AddWithValue("@MaCN", maCN);
                        decimal gia = 0; decimal.TryParse(txtGiaPhong.Text, out gia);
                        lenh.Parameters.AddWithValue("@Gia", gia);
                        lenh.Parameters.AddWithValue("@So", txtSoPhong.Text);
                        lenh.Parameters.AddWithValue("@Loai", cboLoaiPhong.Text);
                        lenh.Parameters.AddWithValue("@TrangThai", cboTrangThai.Text);
                        if (hanhDong != "Thêm") lenh.Parameters.AddWithValue("@Ma", txtMaPhong.Text);

                        if (lenh.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Thành công!");
                            HienThiDanhSach();
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
            HienThiDanhSach();
            KhoaDieuKhien(true);
        }

        private void btnCheckIn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaPhong.Text)) return;
            if (cboTrangThai.Text == "Đang ở") { MessageBox.Show("Phòng đang có người!"); return; }
            frmCheckIn f = new frmCheckIn(); f.MaPhongCanCheckIn = txtMaPhong.Text;
            if (f.ShowDialog() == DialogResult.OK) { HienThiDanhSach(); XoaTrangO(); }
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaPhong.Text)) return;
            if (cboTrangThai.Text != "Đang ở") { MessageBox.Show("Phòng chưa có khách!"); return; }
            frmCheckOut f = new frmCheckOut(); f.MaPhongCanCheckOut = txtMaPhong.Text;
            if (f.ShowDialog() == DialogResult.OK) { HienThiDanhSach(); XoaTrangO(); }
        }

        private void btnChiTiet_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra đã chọn phòng chưa
            if (string.IsNullOrEmpty(txtMaPhong.Text))
            {
                MessageBox.Show("Vui lòng chọn phòng cần xem!", "Thông báo");
                return;
            }

            // 2. Kết nối CSDL lấy thông tin người đang ở
            using (SqlConnection conn = new SqlConnection(chuoiketNoi))
            {
                try
                {
                    conn.Open();
                    // Câu lệnh truy vấn nối 3 bảng: PHONG - DAT_PHONG - KHACH_HANG
                    // Chỉ lấy dữ liệu của khách đang ở (TrangThai = N'Đang ở')
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
                            // Lấy dữ liệu từ SQL
                            string tenKhach = reader["HoTen"].ToString();
                            string cmnd = reader["CCCD_Passport"].ToString();
                            DateTime ngayVao = Convert.ToDateTime(reader["NgayCheckIn"]);
                            decimal tienCoc = Convert.ToDecimal(reader["TienCoc"]);

                            // Hiển thị lên MessageBox
                            string thongTin = $"--- THÔNG TIN KHÁCH ĐANG Ở ---\n\n" +
                                              $"👤 Khách hàng: {tenKhach}\n" +
                                              $"🆔 CMND/CCCD: {cmnd}\n" +
                                              $"🕒 Giờ vào: {ngayVao:dd/MM/yyyy HH:mm}\n" +
                                              $"💰 Tiền cọc: {tienCoc:N0} VNĐ";

                            MessageBox.Show(thongTin, "Chi tiết phòng " + txtSoPhong.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            // Nếu phòng đang trạng thái "Đang ở" mà không tìm thấy khách -> Có thể lỗi dữ liệu
                            if (cboTrangThai.Text == "Đang ở")
                                MessageBox.Show("Lỗi dữ liệu: Phòng đang ở nhưng không tìm thấy thông tin khách!", "Cảnh báo");
                            else
                                MessageBox.Show("Phòng này hiện đang trống hoặc chưa có khách.", "Thông tin");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }

        private void KhoaDieuKhien(bool t)
        {
            btnThem.Enabled = true;
            btnSua.Enabled = !t;
            btnXoa.Enabled = !t;
            btnCheckIn.Enabled = !t;
            btnCheckOut.Enabled = !t;
            btnChiTiet.Enabled = !t;
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
    }
}