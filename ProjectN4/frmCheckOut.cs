using ProjectN4.DAL;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ProjectN4.GUI
{
    public partial class frmCheckOut : Form
    {
        // =================================================================================
        // 1. KHAI BÁO BIẾN & KẾT NỐI
        // =================================================================================
        string chuoiketNoi = $"Data Source={DbSettings.ServerIP};Initial Catalog={DbSettings.DatabaseName};User ID={DbSettings.UserID};Password={DbSettings.Password};";

        // Biến nhận Mã Phòng (VD: "101") từ Form Quản Lý
        public string MaPhongCanCheckOut { get; set; }

        // Các biến toàn cục để lưu trữ giá trị tính toán
        int _maDatPhong;               // ID phiếu đặt phòng trong DB
        decimal _tongGiaTriSuDung = 0; // Tổng tiền khách phải trả (Sau khi +/- điều chỉnh)
        decimal _thucThu = 0;          // Số tiền khách cần móc ví trả (Sau khi trừ cọc)
        decimal _tienGiamGiaDB = 0;    // Số tiền giảm giá để lưu vào DB (nếu có)

        public frmCheckOut()
        {
            InitializeComponent();
        }

        // =================================================================================
        // 2. FORM LOAD - TẢI DỮ LIỆU
        // =================================================================================
        private void frmCheckOut_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(MaPhongCanCheckOut))
            {
                MessageBox.Show("Lỗi: Không nhận được mã phòng cần trả!", "Thông báo");
                this.Close();
                return;
            }

            // Hiển thị tên phòng lên tiêu đề (nếu có label)
            if (lblTenPhong != null) lblTenPhong.Text = "TRẢ PHÒNG: " + MaPhongCanCheckOut;

            // Cài đặt mặc định các ComboBox
            SetupComboBox();

            // Tải dữ liệu từ CSDL
            LayThongTinThanhToan();
        }

        private void SetupComboBox()
        {
            // Hình thức thanh toán
            if (cboHinhThuc.Items.Count == 0)
            {
                cboHinhThuc.Items.Add("Tiền mặt");
                cboHinhThuc.Items.Add("Chuyển khoản");
            }
            if (cboHinhThuc.Items.Count > 0) cboHinhThuc.SelectedIndex = 0;

            // Loại điều chỉnh (Phụ thu / Giảm giá)
            cboLoaiDieuChinh.Items.Clear();
            cboLoaiDieuChinh.Items.Add("Phụ thu (+)");
            cboLoaiDieuChinh.Items.Add("Giảm giá (-)");
            cboLoaiDieuChinh.SelectedIndex = 0;

            // Giá trị điều chỉnh mặc định 0%
            txtGiaTri.Text = "0";
        }

        // =================================================================================
        // 3. TRUY VẤN CSDL (LẤY TIỀN PHÒNG & DỊCH VỤ)
        // =================================================================================
        private void LayThongTinThanhToan()
        {
            using (SqlConnection ketNoi = new SqlConnection(chuoiketNoi))
            {
                try
                {
                    ketNoi.Open();

                    // --- BƯỚC 1: LẤY THÔNG TIN PHÒNG & KHÁCH ---
                    string sqlInfo = @"SELECT dp.MaDatPhong, dp.NgayCheckIn, dp.TienCoc, p.GiaPhong, kh.HoTen
                                     FROM DAT_PHONG dp
                                     JOIN PHONG p ON dp.MaPhong = p.MaPhong
                                     JOIN KHACH_HANG kh ON dp.MaKH = kh.MaKH
                                     WHERE dp.MaPhong = @MaPhong AND dp.TrangThai = N'Đang ở'";

                    SqlCommand cmd = new SqlCommand(sqlInfo, ketNoi);

                    // Kiểm tra kiểu dữ liệu MaPhong (Sửa lại chỗ này nếu MaPhong là string)
                    if (int.TryParse(MaPhongCanCheckOut, out int maPhongInt))
                        cmd.Parameters.AddWithValue("@MaPhong", maPhongInt);
                    else
                        cmd.Parameters.AddWithValue("@MaPhong", MaPhongCanCheckOut); // Dùng string nếu nó là P01

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        // Lấy dữ liệu thô
                        _maDatPhong = int.Parse(reader["MaDatPhong"].ToString());
                        string tenKhach = reader["HoTen"].ToString();
                        DateTime ngayVao = DateTime.Parse(reader["NgayCheckIn"].ToString());
                        DateTime ngayRa = DateTime.Now;
                        decimal giaPhong = decimal.Parse(reader["GiaPhong"].ToString());
                        decimal tienCoc = reader["TienCoc"] != DBNull.Value ? decimal.Parse(reader["TienCoc"].ToString()) : 0;

                        reader.Close(); // Đóng reader để chạy lệnh tiếp theo

                        // Tính số ngày ở (Làm tròn lên)
                        TimeSpan thoiGianO = ngayRa - ngayVao;
                        double soNgay = Math.Ceiling(thoiGianO.TotalDays);
                        if (soNgay < 1) soNgay = 1; // Ít nhất tính 1 ngày

                        decimal tongTienPhong = (decimal)soNgay * giaPhong;

                        // Hiển thị lên giao diện
                        if (lblTenKhach != null) lblTenKhach.Text = tenKhach;
                        if (lblNgayVao != null) lblNgayVao.Text = ngayVao.ToString("dd/MM/yyyy HH:mm");
                        if (lblNgayRa != null) lblNgayRa.Text = ngayRa.ToString("dd/MM/yyyy HH:mm");

                        txtTienPhong.Text = tongTienPhong.ToString("N0");
                        txtDaCoc.Text = tienCoc.ToString("N0");

                        // --- BƯỚC 2: TÍNH TỔNG TIỀN DỊCH VỤ ---
                        string sqlDV = "SELECT SUM(DonGia * SoLuong) FROM CHI_TIET_SD_DV WHERE MaDatPhong = @MaDP";
                        SqlCommand cmdDV = new SqlCommand(sqlDV, ketNoi);
                        cmdDV.Parameters.AddWithValue("@MaDP", _maDatPhong);

                        object ketQuaDV = cmdDV.ExecuteScalar();
                        decimal tongTienDV = (ketQuaDV != DBNull.Value) ? Convert.ToDecimal(ketQuaDV) : 0;

                        txtTienDichVu.Text = tongTienDV.ToString("N0");

                        // --- BƯỚC 3: TÍNH TOÁN TỔNG HỢP ---
                        CapNhatTongTien();
                    }
                    else
                    {
                        MessageBox.Show("Lỗi: Không tìm thấy thông tin check-in của phòng này!", "Cảnh báo");
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi lấy dữ liệu: " + ex.Message);
                }
            }
        }

        // =================================================================================
        // 4. LOGIC TÍNH TOÁN TIỀN (CORE LOGIC)
        // =================================================================================
        private void CapNhatTongTien()
        {
            try
            {
                // A. Lấy giá trị từ các ô nhập liệu
                decimal tPhong = ParseTienTe(txtTienPhong.Text);
                decimal tDV = ParseTienTe(txtTienDichVu.Text);
                decimal tCoc = ParseTienTe(txtDaCoc.Text);

                // B. Tính THÀNH TIỀN GỐC (Phòng - Cọc + Dịch vụ)
                decimal tongCoBan = tPhong - tCoc + tDV;

                // Hiển thị thành tiền gốc
                if (txtThanhTienGoc != null)
                    txtThanhTienGoc.Text = tongCoBan.ToString("N0") + " VNĐ";

                // C. Tính Phụ thu / Giảm giá
                decimal phanTram = 0;
                decimal.TryParse(txtGiaTri.Text, out phanTram);

                // Tính ra số tiền biến động
                decimal tienBienDong = tongCoBan * (phanTram / 100);

                if (cboLoaiDieuChinh.SelectedIndex == 0) // Phụ thu (+)
                {
                    _tongGiaTriSuDung = tongCoBan + tienBienDong;
                    _tienGiamGiaDB = 0;
                }
                else // Giảm giá (-)
                {
                    _tongGiaTriSuDung = tongCoBan - tienBienDong;
                    _tienGiamGiaDB = tienBienDong;
                }

                // D. Hiển thị kết quả cuối cùng (Không trừ cọc nữa vì đã trừ ở trên)
                _thucThu = _tongGiaTriSuDung;

                if (_thucThu < 0)
                {
                    lblTongTien.ForeColor = Color.Red;
                    lblTongTien.Text = "Trả lại khách: " + Math.Abs(_thucThu).ToString("N0") + " VNĐ";
                }
                else
                {
                    lblTongTien.ForeColor = Color.Blue;
                    lblTongTien.Text = _thucThu.ToString("N0") + " VNĐ";
                }
            }
            catch
            {
                // Bỏ qua lỗi khi đang gõ text chưa xong
            }
        }

        // Hàm hỗ trợ chuyển đổi chuỗi tiền tệ (ví dụ "100,000") thành số (100000)
        private decimal ParseTienTe(string input)
        {
            if (string.IsNullOrEmpty(input)) return 0;
            // Loại bỏ dấu phẩy, dấu chấm phân cách ngàn để parse
            // Giả sử định dạng N0 dùng dấu phẩy hoặc chấm tùy theo Culture của máy
            // Cách an toàn nhất là xóa hết ký tự không phải số
            string cleanInput = new string(input.Where(char.IsDigit).ToArray());
            if (string.IsNullOrEmpty(cleanInput)) return 0;
            return decimal.Parse(cleanInput);
        }

        // --- SỰ KIỆN TỰ ĐỘNG TÍNH LẠI ---
        private void txtGiaTri_TextChanged(object sender, EventArgs e)
        {
            CapNhatTongTien();
        }

        private void cboLoaiDieuChinh_SelectedIndexChanged(object sender, EventArgs e)
        {
            CapNhatTongTien();
        }

        // =================================================================================
        // 5. NÚT THANH TOÁN (LƯU VÀO CSDL)
        // =================================================================================
        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            string thongBao = $"Tổng cộng: {_tongGiaTriSuDung:N0}\n" +
                              $"Đã cọc: -{ParseTienTe(txtDaCoc.Text):N0}\n" +
                              $"--------------------------\n" +
                              $"KHÁCH CẦN TRẢ: {_thucThu:N0} VNĐ\n\n" +
                              $"Xác nhận thanh toán và trả phòng?";

            if (MessageBox.Show(thongBao, "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            using (SqlConnection ketNoi = new SqlConnection(chuoiketNoi))
            {
                ketNoi.Open();
                SqlTransaction trans = ketNoi.BeginTransaction(); // Dùng transaction để đảm bảo an toàn dữ liệu

                try
                {
                    // 1. Thêm Hóa Đơn
                    string sqlHD = @"INSERT INTO HOA_DON (MaDatPhong, MaNV, NgayLap, TongTien, GiamGia, ThueVAT, ThucThu, GhiChu) 
                                   VALUES (@MaDP, @MaNV, GETDATE(), @TongTien, @GiamGia, 0, @ThucThu, @GhiChu)";

                    SqlCommand cmdHD = new SqlCommand(sqlHD, ketNoi, trans);
                    cmdHD.Parameters.AddWithValue("@MaDP", _maDatPhong);
                    cmdHD.Parameters.AddWithValue("@MaNV", 1); // Giả sử ID Admin là 1
                    cmdHD.Parameters.AddWithValue("@TongTien", _tongGiaTriSuDung);
                    cmdHD.Parameters.AddWithValue("@GiamGia", _tienGiamGiaDB);
                    cmdHD.Parameters.AddWithValue("@ThucThu", _thucThu);

                    string ghiChu = $"TT: {cboHinhThuc.Text} | {cboLoaiDieuChinh.Text} {txtGiaTri.Text}%";
                    cmdHD.Parameters.AddWithValue("@GhiChu", ghiChu);

                    cmdHD.ExecuteNonQuery();

                    // 2. Cập nhật DatPhong -> Đã trả
                    string sqlDatPhong = "UPDATE DAT_PHONG SET TrangThai = N'Đã trả', NgayCheckOut = GETDATE() WHERE MaDatPhong = @MaDP";
                    SqlCommand cmdDP = new SqlCommand(sqlDatPhong, ketNoi, trans);
                    cmdDP.Parameters.AddWithValue("@MaDP", _maDatPhong);
                    cmdDP.ExecuteNonQuery();

                    // 3. Cập nhật Phong -> Trống
                    // Lưu ý: Cần dùng đúng MaPhong (dạng int hoặc string tùy DB)
                    string sqlPhong = "UPDATE PHONG SET TrangThai = N'Trống' WHERE MaPhong = @MaPhong";
                    SqlCommand cmdPhong = new SqlCommand(sqlPhong, ketNoi, trans);

                    if (int.TryParse(MaPhongCanCheckOut, out int mpInt))
                        cmdPhong.Parameters.AddWithValue("@MaPhong", mpInt);
                    else
                        cmdPhong.Parameters.AddWithValue("@MaPhong", MaPhongCanCheckOut);

                    cmdPhong.ExecuteNonQuery();

                    // Hoàn tất
                    trans.Commit();
                    MessageBox.Show("Thanh toán thành công! Phòng đã trống.");
                    this.DialogResult = DialogResult.OK; // Báo cho form cha biết để load lại danh sách
                    this.Close();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    MessageBox.Show("Lỗi thanh toán: " + ex.Message);
                }
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Hàm này được thêm vào để Designer tìm thấy nó và không báo lỗi nữa
        private void grpThongTin_Enter(object sender, EventArgs e)
        {
            // Để trống, không cần làm gì cả
        }

        private void lblTongTien_Click(object sender, EventArgs e)
        {

        }
    }
}