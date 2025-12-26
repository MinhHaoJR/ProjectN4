using ProjectN4.DAL;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows.Forms;

namespace ProjectN4
{
    public partial class frmThemDichVu : Form
    {
        private readonly int _maDatPhong;
        private readonly string _connStr = $"Data Source={DbSettings.ServerIP};" +
                                           $"Initial Catalog={DbSettings.DatabaseName};" +
                                           $"User ID={DbSettings.UserID};Password={DbSettings.Password};";

        public frmThemDichVu(int maDatPhong)
        {
            InitializeComponent();
            _maDatPhong = maDatPhong;
        }

        private void frmThemDichVu_Load(object sender, EventArgs e)
        {
            SetupControls();
            LoadThongTinPhong();
            LoadDichVu(); // Chỉ load MaDV và TenDV, không cần DonGia nữa
            LoadDichVuDaDung();
        }

        private void SetupControls()
        {
            txtDonGia.ReadOnly = true;
            numSoLuong.Minimum = 1;
            numSoLuong.Value = 1;
        }

        private void LoadThongTinPhong()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    string sql = @"SELECT p.SoPhong, kh.HoTen
                                   FROM DAT_PHONG dp
                                   JOIN PHONG p ON dp.MaPhong = p.MaPhong
                                   JOIN KHACH_HANG kh ON dp.MaKH = kh.MaKH
                                   WHERE dp.MaDatPhong = @MaDatPhong";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaDatPhong", _maDatPhong);
                        conn.Open();
                        using (SqlDataReader rd = cmd.ExecuteReader())
                        {
                            if (rd.Read())
                            {
                                txtSoPhong.Text = rd["SoPhong"].ToString();
                                txtTenKhach.Text = rd["HoTen"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải thông tin phòng: " + ex.Message);
            }
        }

        private void LoadDichVu()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();
                    // Chỉ lấy MaDV và TenDV để hiển thị, không lấy DonGia nữa
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
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load danh sách dịch vụ: " + ex.Message);
            }
        }

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

                        dgvDichVuDaDung.Columns["TenDV"].HeaderText = "Tên Dịch Vụ";
                        dgvDichVuDaDung.Columns["SoLuong"].HeaderText = "Số Lượng";
                        dgvDichVuDaDung.Columns["DonGia"].HeaderText = "Đơn Giá";
                        dgvDichVuDaDung.Columns["ThanhTien"].HeaderText = "Thành Tiền";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải lịch sử dịch vụ: " + ex.Message);
            }
        }

        // === PHẦN MỚI: Trích xuất đơn giá từ DB dựa vào tên dịch vụ ===
        private void cboDichVu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDichVu.SelectedIndex == -1 || cboDichVu.Text.Trim() == "") return;

            string tenDichVu = cboDichVu.Text.Trim();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();
                    string query = "SELECT DonGia FROM DICH_VU WHERE TenDV = @TenDV";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TenDV", tenDichVu);
                        object result = cmd.ExecuteScalar();

                        if (result != null && decimal.TryParse(result.ToString(), out decimal donGia))
                        {
                            txtDonGia.Text = donGia.ToString("N0") + " VND";
                        }
                        else
                        {
                            txtDonGia.Text = "0 VND";
                            MessageBox.Show("Không tìm thấy đơn giá cho dịch vụ này!");
                        }
                    }
                }

                TinhThanhTien();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy đơn giá: " + ex.Message);
                txtDonGia.Text = "0 VND";
                lblThanhTien.Text = "0 VND";
            }
        }

        private void numSoLuong_ValueChanged(object sender, EventArgs e)
        {
            TinhThanhTien();
        }

        private void TinhThanhTien()
        {
            string donGiaText = txtDonGia.Text.Replace(" VND", "").Replace(",", "");
            if (decimal.TryParse(donGiaText, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal donGia))
            {
                decimal thanhTien = donGia * numSoLuong.Value;
                lblThanhTien.Text = thanhTien.ToString("N0") + " VND";
            }
            else
            {
                lblThanhTien.Text = "0 VND";
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (cboDichVu.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn dịch vụ!");
                return;
            }

            try
            {
                string donGiaText = txtDonGia.Text.Replace(" VND", "").Replace(",", "");
                if (!decimal.TryParse(donGiaText, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal donGia))
                {
                    MessageBox.Show("Đơn giá không hợp lệ!");
                    return;
                }

                int soLuong = (int)numSoLuong.Value;
                decimal thanhTien = donGia * soLuong;

                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    string sql = @"INSERT INTO CHI_TIET_SD_DV (MaDatPhong, MaDV, SoLuong, DonGia)
                               VALUES (@MaDP, @MaDV, @SL, @DG)";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaDP", _maDatPhong);
                        cmd.Parameters.AddWithValue("@MaDV", cboDichVu.SelectedValue);
                        cmd.Parameters.AddWithValue("@SL", soLuong);
                        cmd.Parameters.AddWithValue("@DG", donGia); // Lưu giá tại thời điểm

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Thêm dịch vụ thành công!");
                LoadDichVuDaDung();

                // Reset form
                cboDichVu.SelectedIndex = -1;
                numSoLuong.Value = 1;
                txtDonGia.Text = "";
                lblThanhTien.Text = "0 VND";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm dịch vụ: " + ex.Message);
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void lblTenKhach_Click(object sender, EventArgs e)
        {
            // You can leave this empty or add any logic you want to handle label click
        }

        private void txtDonGia_TextChanged(object sender, EventArgs e)
        {
            // You can add logic here if needed, or leave empty if not required.
        }

        private void dgvDichVuDaDung_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // You can leave this empty or add logic as needed
        }

        private void lblChonDV_Click(object sender, EventArgs e)
        {
            // No action needed, or add your custom logic here if required
        }

        private void lblDonGia_Click(object sender, EventArgs e)
        {
            // You can leave this empty or add logic if needed
        }

        private void lblSoLuong_Click(object sender, EventArgs e)
        {
            // You can leave this empty or add logic if needed
        }

        private void lblThanhTienTiltle_Click(object sender, EventArgs e)
        {
            // You can leave this empty if you don't want any action on click,
            // or add logic here if needed.
        }

        private void txtTenKhach_TextChanged(object sender, EventArgs e)
        {
            // No action needed, or add logic if required
        }

        private void txtSoPhong_TextChanged(object sender, EventArgs e)
        {
            // You can leave this empty if you don't need to handle the event,
            // or add logic here if needed.
        }

        private void lblThanhTien_Click(object sender, EventArgs e)
        {
            // Optionally, leave empty if no action is needed when clicking the label
        }
        // Add this method to your frmThemDichVu class
        private void lblSoPhong_Click(object sender, EventArgs e)
        {
            // You can leave this empty or add logic as needed
        }
    }
}




