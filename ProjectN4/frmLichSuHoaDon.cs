using ProjectN4.DAL; // Nhớ đổi namespace theo project của bạn
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ProjectN4.GUI
{
    public partial class frmLichSuHoaDon : Form
    {
        // Chuỗi kết nối
        string chuoiketNoi = $"Data Source={DbSettings.ServerIP};Initial Catalog={DbSettings.DatabaseName};User ID={DbSettings.UserID};Password={DbSettings.Password};";

        public frmLichSuHoaDon()
        {
            InitializeComponent();
        }

        private void frmLichSuHoaDon_Load(object sender, EventArgs e)
        {
            // Mặc định load dữ liệu từ đầu tháng đến hiện tại
            DateTime today = DateTime.Now;
            dtpTuNgay.Value = new DateTime(today.Year, today.Month, 1); // Ngày 1 đầu tháng
            dtpDenNgay.Value = today; // Hôm nay

            LoadDanhSachHoaDon();
        }

        private void LoadDanhSachHoaDon()
        {
            using (SqlConnection ketNoi = new SqlConnection(chuoiketNoi))
            {
                try
                {
                    ketNoi.Open();

                    // Câu truy vấn JOIN nhiều bảng để lấy thông tin chi tiết
                    // Lưu ý: Tôi giả sử bạn có bảng PHONG (TenPhong) và KHACH_HANG (HoTen)
                    // Nếu tên cột trong DB khác, bạn sửa lại nhé.
                    string sql = @"
                        SELECT 
                            hd.MaHoaDon AS [Mã HĐ],
                            p.MaPhong AS [Phòng],
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

                    // Xử lý tìm kiếm (Nếu ô tìm kiếm không trống)
                    if (!string.IsNullOrEmpty(txtTimKiem.Text))
                    {
                        sql += " AND (kh.HoTen LIKE @TuKhoa OR p.MaPhong LIKE @TuKhoa)";
                    }

                    sql += " ORDER BY hd.NgayLap DESC"; // Mới nhất lên đầu

                    SqlCommand cmd = new SqlCommand(sql, ketNoi);

                    // Set tham số ngày (lấy đầu ngày và cuối ngày để chính xác)
                    cmd.Parameters.AddWithValue("@TuNgay", dtpTuNgay.Value.Date);
                    cmd.Parameters.AddWithValue("@DenNgay", dtpDenNgay.Value.Date.AddDays(1).AddSeconds(-1));

                    cmd.Parameters.AddWithValue("@TuKhoa", "%" + txtTimKiem.Text + "%");

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvHoaDon.DataSource = dt;

                    // Tính tổng doanh thu của danh sách đang hiển thị
                    TinhTongDoanhThu(dt);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
                }
            }
        }

        private void TinhTongDoanhThu(DataTable dt)
        {
            decimal tongTien = 0;
            foreach (DataRow row in dt.Rows)
            {
                if (row["Thực Thu"] != DBNull.Value)
                {
                    tongTien += Convert.ToDecimal(row["Thực Thu"]);
                }
            }
            lblTongDoanhThu.Text = "TỔNG DOANH THU: " + tongTien.ToString("N0") + " VNĐ";
        }

        // Sự kiện nút Xem
        private void btnXem_Click(object sender, EventArgs e)
        {
            LoadDanhSachHoaDon();
        }

        // Sự kiện nút Thoát
        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Sự kiện Format lại hiển thị số tiền cho đẹp (Tùy chọn)
        private void dgvHoaDon_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Nếu là cột tiền (cột 4, 5, 6 theo query trên) thì format N0
            if (e.ColumnIndex >= 4 && e.ColumnIndex <= 6 && e.Value != null)
            {
                if (decimal.TryParse(e.Value.ToString(), out decimal val))
                {
                    e.Value = val.ToString("N0");
                    e.FormattingApplied = true;
                }
            }
        }
    }
}