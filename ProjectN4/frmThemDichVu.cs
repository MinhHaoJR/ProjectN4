using ProjectN4.DAL;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows.Forms;

namespace ProjectN4 // Đảm bảo namespace này đúng với dự án của bạn (ProjectN4 hoặc ProjectN4.GUI)
{
    public partial class frmThemDichVu : Form
    {
        // Biến nhận Mã Phòng (VD: "P01") từ Form Quản Lý truyền sang
        public string MaPhongDuocChon { get; set; }

        private int _maDatPhong = 0;
        private readonly string _connStr = $"Data Source={DbSettings.ServerIP};Initial Catalog={DbSettings.DatabaseName};User ID={DbSettings.UserID};Password={DbSettings.Password};";

        public frmThemDichVu()
        {
            InitializeComponent();
        }

        // QUAN TRỌNG: Tên hàm này phải là _Load_1 để khớp với Designer của bạn
        private void frmThemDichVu_Load_1(object sender, EventArgs e)
        {
            SetupControls();

            if (string.IsNullOrEmpty(MaPhongDuocChon))
            {
                MessageBox.Show("Lỗi: Không nhận được Mã Phòng từ form cha.");
                return;
            }

            // 1. Tìm Mã Đặt Phòng dựa trên Mã Phòng (P01)
            if (TimMaDatPhongTuMaPhong(MaPhongDuocChon))
            {
                // 2. Load Số Phòng (VD: A01) và Tên Khách để hiển thị
                LoadThongTinHienThi();

                // 3. Load danh sách Dịch vụ và Lịch sử sử dụng
                LoadDichVu();
                LoadDichVuDaDung();
            }
            else
            {
                MessageBox.Show($"Không tìm thấy khách đang ở tại phòng mã: {MaPhongDuocChon}", "Thông báo");
                this.Close();
            }
        }

        private void SetupControls()
        {
            txtDonGia.ReadOnly = true;
            txtSoPhong.ReadOnly = true;   // Dùng đúng tên txtSoPhong
            txtTenKhach.ReadOnly = true;  // Dùng đúng tên txtTenKhach
            numSoLuong.Minimum = 1;
            numSoLuong.Value = 1;
            lblThanhTien.Text = "0 VND";
        }

        // --- Logic 1: Tìm Mã Đặt Phòng (ID của lần khách thuê này) ---
        private bool TimMaDatPhongTuMaPhong(string maPhong)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();
                    // Dùng LIKE để tìm chính xác, tránh lỗi thừa khoảng trắng
                    string sql = @"SELECT TOP 1 MaDatPhong FROM DAT_PHONG 
                                   WHERE MaPhong = @MaPhong AND TrangThai LIKE N'Đang ở%' 
                                   ORDER BY NgayCheckIn DESC";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaPhong", maPhong);
                        object result = cmd.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int maDP))
                        {
                            _maDatPhong = maDP;
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi tìm mã đặt phòng: " + ex.Message); }
            return false;
        }

        // --- Logic 2: Lấy Số Phòng (A01) và Tên Khách để hiện lên TextBox ---
        private void LoadThongTinHienThi()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();
                    // Join bảng để lấy Số Phòng thân thiện (A01) thay vì Mã Phòng (P01)
                    string sql = @"SELECT p.SoPhong, kh.HoTen
                                   FROM DAT_PHONG dp
                                   JOIN PHONG p ON dp.MaPhong = p.MaPhong
                                   JOIN KHACH_HANG kh ON dp.MaKH = kh.MaKH
                                   WHERE dp.MaDatPhong = @MaDatPhong";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaDatPhong", _maDatPhong);

                        using (SqlDataReader rd = cmd.ExecuteReader())
                        {
                            if (rd.Read())
                            {
                                // Điền dữ liệu vào đúng TextBox trong Designer
                                txtSoPhong.Text = rd["SoPhong"].ToString();
                                txtTenKhach.Text = rd["HoTen"].ToString();

                                this.Text = $"Thêm Dịch Vụ - Phòng {rd["SoPhong"]}";
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi hiển thị thông tin: " + ex.Message); }
        }

        // --- Logic 3: Load ComboBox Dịch Vụ ---
        private void LoadDichVu()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();
                    string query = "SELECT MaDV, TenDV FROM DICH_VU";
                    using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        cboDichVu.SelectedIndexChanged -= cboDichVu_SelectedIndexChanged;
                        cboDichVu.DataSource = dt;
                        cboDichVu.DisplayMember = "TenDV";
                        cboDichVu.ValueMember = "MaDV";
                        cboDichVu.SelectedIndex = -1;
                        cboDichVu.SelectedIndexChanged += cboDichVu_SelectedIndexChanged;
                    }
                }
            }
            catch { }
        }

        // --- Logic 4: Load Lưới Dịch Vụ Đã Dùng ---
        private void LoadDichVuDaDung()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    string sql = @"SELECT dv.TenDV, ct.SoLuong, ct.DonGia, ct.ThanhTien
                                   FROM CHI_TIET_SD_DV ct
                                   JOIN DICH_VU dv ON ct.MaDV = dv.MaDV
                                   WHERE ct.MaDatPhong = @MaDatPhong
                                   ORDER BY ct.MaCTSDDV DESC";

                    using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
                    {
                        da.SelectCommand.Parameters.AddWithValue("@MaDatPhong", _maDatPhong);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dgvDichVuDaDung.DataSource = dt;
                        DinhDangCotLuoi();
                    }
                }
            }
            catch { }
        }

        private void DinhDangCotLuoi()
        {
            dgvDichVuDaDung.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            if (dgvDichVuDaDung.Columns.Contains("TenDV")) dgvDichVuDaDung.Columns["TenDV"].HeaderText = "Tên Dịch Vụ";
            if (dgvDichVuDaDung.Columns.Contains("SoLuong")) dgvDichVuDaDung.Columns["SoLuong"].HeaderText = "SL";
            if (dgvDichVuDaDung.Columns.Contains("DonGia"))
            {
                dgvDichVuDaDung.Columns["DonGia"].HeaderText = "Đơn Giá";
                dgvDichVuDaDung.Columns["DonGia"].DefaultCellStyle.Format = "N0";
            }
            if (dgvDichVuDaDung.Columns.Contains("ThanhTien"))
            {
                dgvDichVuDaDung.Columns["ThanhTien"].HeaderText = "Thành Tiền";
                dgvDichVuDaDung.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";
            }
        }

        // --- CÁC SỰ KIỆN (EVENTS) ---

        private void cboDichVu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDichVu.SelectedIndex == -1) return;
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();
                    string query = "SELECT DonGia FROM DICH_VU WHERE MaDV = @MaDV"; // Tìm theo Mã DV
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaDV", cboDichVu.SelectedValue);
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            txtDonGia.Text = decimal.Parse(result.ToString()).ToString("N0");
                        }
                    }
                }
                TinhThanhTien();
            }
            catch { }
        }

        private void numSoLuong_ValueChanged(object sender, EventArgs e) => TinhThanhTien();

        private void TinhThanhTien()
        {
            string donGiaStr = txtDonGia.Text.Replace(".", "").Replace(",", "").Replace(" VND", "").Trim();
            if (decimal.TryParse(donGiaStr, out decimal donGia))
            {
                lblThanhTien.Text = (donGia * numSoLuong.Value).ToString("N0") + " VND";
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (cboDichVu.SelectedIndex == -1) { MessageBox.Show("Vui lòng chọn dịch vụ!"); return; }
            if (_maDatPhong == 0) return;

            try
            {
                string donGiaStr = txtDonGia.Text.Replace(".", "").Replace(",", "").Replace(" VND", "").Trim();
                decimal.TryParse(donGiaStr, out decimal donGia);

                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();
                    string sql = @"INSERT INTO CHI_TIET_SD_DV (MaDatPhong, MaDV, SoLuong, DonGia) 
                                   VALUES (@MaDP, @MaDV, @SL, @DG)";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaDP", _maDatPhong);
                        cmd.Parameters.AddWithValue("@MaDV", cboDichVu.SelectedValue);
                        cmd.Parameters.AddWithValue("@SL", numSoLuong.Value);
                        cmd.Parameters.AddWithValue("@DG", donGia);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Thêm thành công!");
                LoadDichVuDaDung();
            }
            catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
        }

        private void btnDong_Click(object sender, EventArgs e) => this.Close();

        // --- CÁC HÀM RỖNG (Giữ lại để Designer không báo lỗi) ---
        private void lblTenKhach_Click(object sender, EventArgs e) { }
        private void txtDonGia_TextChanged(object sender, EventArgs e) { }
        private void dgvDichVuDaDung_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void lblChonDV_Click(object sender, EventArgs e) { }
        private void lblDonGia_Click(object sender, EventArgs e) { }
        private void lblSoLuong_Click(object sender, EventArgs e) { }
        private void lblThanhTienTiltle_Click(object sender, EventArgs e) { }
        private void txtTenKhach_TextChanged(object sender, EventArgs e) { }
        private void txtSoPhong_TextChanged(object sender, EventArgs e) { }
        private void lblThanhTien_Click(object sender, EventArgs e) { }
        private void lblSoPhong_Click(object sender, EventArgs e) { }
    }
}