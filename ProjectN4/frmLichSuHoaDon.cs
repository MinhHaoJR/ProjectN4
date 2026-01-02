using ProjectN4.DAL;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO; // Bắt buộc có để xuất file
using System.Windows.Forms;

namespace ProjectN4.GUI
{
    public partial class frmLichSuHoaDon : Form
    {
        string chuoiketNoi = $"Data Source={DbSettings.ServerIP};Initial Catalog={DbSettings.DatabaseName};User ID={DbSettings.UserID};Password={DbSettings.Password};";

        public frmLichSuHoaDon()
        {
            InitializeComponent();
        }

        private void frmLichSuHoaDon_Load(object sender, EventArgs e)
        {
            DateTime today = DateTime.Now;
            dtpTuNgay.Value = new DateTime(today.Year, today.Month, 1);
            dtpDenNgay.Value = today;

            SetupDataGridView();
            LoadDanhSachHoaDon();
        }

        private void SetupDataGridView()
        {
            dgvHoaDon.BackgroundColor = Color.White;
            dgvHoaDon.BorderStyle = BorderStyle.None;
            dgvHoaDon.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(238, 239, 249);
            dgvHoaDon.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;
            dgvHoaDon.DefaultCellStyle.SelectionForeColor = Color.WhiteSmoke;

            dgvHoaDon.EnableHeadersVisualStyles = false;
            dgvHoaDon.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvHoaDon.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 25, 72);
            dgvHoaDon.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvHoaDon.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvHoaDon.ColumnHeadersHeight = 40;

            dgvHoaDon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHoaDon.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHoaDon.ReadOnly = true;

            // Gắn sự kiện Double Click
            dgvHoaDon.CellDoubleClick -= dgvHoaDon_CellDoubleClick;
            dgvHoaDon.CellDoubleClick += dgvHoaDon_CellDoubleClick;
        }

        private void LoadDanhSachHoaDon()
        {
            using (SqlConnection ketNoi = new SqlConnection(chuoiketNoi))
            {
                try
                {
                    ketNoi.Open();
                    string sql = @"
                        SELECT 
                            hd.MaHoaDon AS [Mã HĐ],
                            p.SoPhong AS [Phòng],
                            kh.HoTen AS [Khách Hàng],
                            hd.NgayLap AS [Ngày Lập],
                            hd.TongTien AS [Tổng Trị Giá],
                            hd.GiamGia AS [Giảm Giá],
                            hd.ThucThu AS [Thực Thu],
                            hd.GhiChu AS [Ghi Chú]
                        FROM HOA_DON hd
                        JOIN DAT_PHONG dp ON hd.MaDatPhong = dp.MaDatPhong
                        JOIN PHONG p ON dp.MaPhong = p.MaPhong
                        JOIN KHACH_HANG kh ON dp.MaKH = kh.MaKH
                        WHERE hd.NgayLap BETWEEN @TuNgay AND @DenNgay
                    ";

                    if (!string.IsNullOrEmpty(txtTimKiem.Text))
                    {
                        sql += " AND (kh.HoTen LIKE @TuKhoa OR p.SoPhong LIKE @TuKhoa OR hd.GhiChu LIKE @TuKhoa)";
                    }
                    sql += " ORDER BY hd.NgayLap DESC";

                    SqlCommand cmd = new SqlCommand(sql, ketNoi);
                    cmd.Parameters.AddWithValue("@TuNgay", dtpTuNgay.Value.Date);
                    cmd.Parameters.AddWithValue("@DenNgay", dtpDenNgay.Value.Date.AddDays(1).AddSeconds(-1));
                    cmd.Parameters.AddWithValue("@TuKhoa", "%" + txtTimKiem.Text.Trim() + "%");

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvHoaDon.DataSource = dt;

                    // Định dạng hiển thị
                    if (dgvHoaDon.Columns.Count > 0)
                    {
                        dgvHoaDon.Columns["Mã HĐ"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dgvHoaDon.Columns["Phòng"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dgvHoaDon.Columns["Ngày Lập"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";

                        dgvHoaDon.Columns["Tổng Trị Giá"].DefaultCellStyle.Format = "N0";
                        dgvHoaDon.Columns["Tổng Trị Giá"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                        dgvHoaDon.Columns["Giảm Giá"].DefaultCellStyle.Format = "N0";
                        dgvHoaDon.Columns["Giảm Giá"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                        dgvHoaDon.Columns["Thực Thu"].DefaultCellStyle.Format = "N0";
                        dgvHoaDon.Columns["Thực Thu"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                    TinhTongDoanhThu(dt);
                }
                catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
            }
        }

        private void TinhTongDoanhThu(DataTable dt)
        {
            decimal tongTien = 0;
            foreach (DataRow row in dt.Rows)
            {
                if (row["Thực Thu"] != DBNull.Value) tongTien += Convert.ToDecimal(row["Thực Thu"]);
            }
            lblTongDoanhThu.Text = "TỔNG DOANH THU: " + tongTien.ToString("N0") + " VNĐ";
            lblTongDoanhThu.ForeColor = Color.Red;
        }

        // === CHỨC NĂNG 1: XUẤT DANH SÁCH RA EXCEL (CSV) ===
        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            if (dgvHoaDon.Rows.Count == 0) { MessageBox.Show("Không có dữ liệu để xuất!"); return; }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel CSV (*.csv)|*.csv";
            sfd.FileName = "BaoCaoDoanhThu_" + DateTime.Now.ToString("ddMMyyyy_HHmm") + ".csv";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(sfd.FileName, false, System.Text.Encoding.UTF8))
                    {
                        // Ghi tiêu đề cột
                        string[] headers = new string[dgvHoaDon.Columns.Count];
                        for (int i = 0; i < dgvHoaDon.Columns.Count; i++) headers[i] = dgvHoaDon.Columns[i].HeaderText;
                        sw.WriteLine(string.Join(",", headers));

                        // Ghi dữ liệu dòng
                        foreach (DataGridViewRow row in dgvHoaDon.Rows)
                        {
                            string[] cells = new string[dgvHoaDon.Columns.Count];
                            for (int i = 0; i < dgvHoaDon.Columns.Count; i++)
                            {
                                string val = row.Cells[i].Value?.ToString() ?? "";
                                cells[i] = val.Replace(",", " "); // Xử lý dấu phẩy để tránh lỗi cột
                            }
                            sw.WriteLine(string.Join(",", cells));
                        }
                    }
                    MessageBox.Show("Xuất file thành công! Bạn có thể mở bằng Excel.");
                }
                catch (Exception ex) { MessageBox.Show("Lỗi xuất file: " + ex.Message); }
            }
        }

        // === CHỨC NĂNG 2: IN HÓA ĐƠN (BILL LẺ) ===
        private void btnInHoaDon_Click(object sender, EventArgs e)
        {
            if (dgvHoaDon.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một hóa đơn để in!", "Thông báo");
                return;
            }

            // Lấy Mã Hóa Đơn đang chọn
            string maHD = dgvHoaDon.SelectedRows[0].Cells["Mã HĐ"].Value.ToString();

            // Code in ấn sẽ viết ở đây (Crystal Report hoặc PrintDocument)
            // Hiện tại thông báo tạm
            MessageBox.Show($"Đang gửi lệnh in cho Hóa đơn #{maHD}...\n(Chức năng in giấy sẽ được cập nhật trong module Báo cáo)", "In Hóa Đơn");
        }

        private void dgvHoaDon_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string maHD = dgvHoaDon.Rows[e.RowIndex].Cells["Mã HĐ"].Value.ToString();
                string ghiChu = dgvHoaDon.Rows[e.RowIndex].Cells["Ghi Chú"].Value.ToString();
                MessageBox.Show($"Chi tiết hóa đơn #{maHD}:\n{ghiChu}", "Thông tin");
            }
        }

        private void btnXem_Click(object sender, EventArgs e) => LoadDanhSachHoaDon();
        private void btnThoat_Click(object sender, EventArgs e) => this.Close();
    }
}