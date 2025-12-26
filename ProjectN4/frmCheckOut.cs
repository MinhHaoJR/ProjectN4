using ProjectN4.DAL;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace ProjectN4.GUI
{
    public partial class frmCheckOut : Form
    {
        // =================================================================================
        // 1. CẤU HÌNH & KHAI BÁO BIẾN
        // =================================================================================
        string chuoiketNoi = $"Data Source={DbSettings.ServerIP};Initial Catalog={DbSettings.DatabaseName};User ID={DbSettings.UserID};Password={DbSettings.Password};";

        // Biến nhận dữ liệu từ Form cha (QuanLyPhong)
        public string MaPhongCanCheckOut { get; set; }

        // Các biến nội bộ để xử lý tính toán
        int _maDatPhong;       // ID phiếu đặt phòng
        decimal _tongGiaTriSuDung = 0; // Tổng tiền CẦN THANH TOÁN (Sau khi đã cộng phụ thu hoặc trừ giảm giá)
        decimal _thucThu = 0;          // Số tiền khách phải trả thực tế (Sau khi trừ cọc)
        decimal _tienGiamGiaDB = 0;    // Biến lưu riêng số tiền giảm giá để ghi vào Database

        public frmCheckOut()
        {
            InitializeComponent();
        }

        // =================================================================================
        // 2. FORM LOAD
        // =================================================================================
        private void frmCheckOut_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(MaPhongCanCheckOut))
            {
                MessageBox.Show("Chưa có thông tin phòng cần trả!", "Lỗi");
                this.Close();
                return;
            }

            if (lblTenPhong != null) lblTenPhong.Text = "PHÒNG " + MaPhongCanCheckOut;

            // Cài đặt mặc định cho ComboBox Hình thức thanh toán
            if (cboHinhThuc.Items.Count > 0) cboHinhThuc.SelectedIndex = 0;

            // --- CÀI ĐẶT MỚI CHO PHẦN ĐIỀU CHỈNH GIÁ ---
            // Đảm bảo ComboBox Điều chỉnh có dữ liệu (nếu chưa thêm ở Design thì thêm ở đây cho chắc)
            if (cboLoaiDieuChinh.Items.Count == 0)
            {
                cboLoaiDieuChinh.Items.Add("Phụ thu (+)");
                cboLoaiDieuChinh.Items.Add("Giảm giá (-)");
            }
            cboLoaiDieuChinh.SelectedIndex = 0; // Mặc định chọn Phụ thu
            txtGiaTri.Text = "0"; // Mặc định 0%

            // Gắn sự kiện (nếu chưa gắn bên Design)
            txtGiaTri.TextChanged += new EventHandler(txtGiaTri_TextChanged);
            cboLoaiDieuChinh.SelectedIndexChanged += new EventHandler(cboLoaiDieuChinh_SelectedIndexChanged);

            // Bắt đầu tính toán
            TinhToanHoaDon();
        }

        // =================================================================================
        // 3. LOGIC TÍNH TOÁN TIỀN (CORE)
        // =================================================================================
        private void TinhToanHoaDon()
        {
            using (SqlConnection ketNoi = new SqlConnection(chuoiketNoi))
            {
                try
                {
                    ketNoi.Open();

                    // --- BƯỚC A: LẤY THÔNG TIN ĐẶT PHÒNG ---
                    string sqlInfo = @"SELECT dp.MaDatPhong, dp.NgayCheckIn, dp.TienCoc, p.GiaPhong, kh.HoTen
                                     FROM DAT_PHONG dp
                                     JOIN PHONG p ON dp.MaPhong = p.MaPhong
                                     JOIN KHACH_HANG kh ON dp.MaKH = kh.MaKH
                                     WHERE dp.MaPhong = @MaPhong AND dp.TrangThai = N'Đang ở'";

                    SqlCommand cmd = new SqlCommand(sqlInfo, ketNoi);
                    cmd.Parameters.AddWithValue("@MaPhong", int.Parse(MaPhongCanCheckOut));

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        // 1. Lấy dữ liệu ra biến
                        _maDatPhong = int.Parse(reader["MaDatPhong"].ToString());
                        string tenKhach = reader["HoTen"].ToString();
                        DateTime ngayVao = DateTime.Parse(reader["NgayCheckIn"].ToString());
                        DateTime ngayRa = DateTime.Now;
                        decimal giaPhong = decimal.Parse(reader["GiaPhong"].ToString());
                        decimal tienCoc = reader["TienCoc"] != DBNull.Value ? decimal.Parse(reader["TienCoc"].ToString()) : 0;

                        reader.Close();

                        // 2. Tính Tiền Phòng (Theo Ngày - Làm tròn lên)
                        TimeSpan thoiGianO = ngayRa - ngayVao;
                        double soNgay = Math.Ceiling(thoiGianO.TotalDays);
                        if (soNgay < 1) soNgay = 1;

                        decimal tongTienPhong = (decimal)soNgay * giaPhong;

                        // 3. Hiển thị thông tin cơ bản
                        if (lblTenKhach != null) lblTenKhach.Text = tenKhach;
                        if (lblNgayVao != null) lblNgayVao.Text = ngayVao.ToString("dd/MM/yyyy HH:mm");
                        if (lblNgayRa != null) lblNgayRa.Text = ngayRa.ToString("dd/MM/yyyy HH:mm");

                        txtTienPhong.Text = tongTienPhong.ToString("N0");
                        txtDaCoc.Text = tienCoc.ToString("N0");

                        // --- BƯỚC B: TÍNH TIỀN DỊCH VỤ ---
                        string sqlDV = "SELECT SUM(ThanhTien) FROM CHI_TIET_SD_DV WHERE MaDatPhong = @MaDP";
                        SqlCommand cmdDV = new SqlCommand(sqlDV, ketNoi);
                        cmdDV.Parameters.AddWithValue("@MaDP", _maDatPhong);

                        object ketQuaDV = cmdDV.ExecuteScalar();
                        decimal tongTienDV = (ketQuaDV != DBNull.Value) ? Convert.ToDecimal(ketQuaDV) : 0;

                        txtTienDichVu.Text = tongTienDV.ToString("N0");

                        // --- BƯỚC C: TỔNG KẾT ---
                        CapNhatTongTien();
                    }
                    else
                    {
                        MessageBox.Show("Lỗi: Phòng này hiện KHÔNG CÓ người ở!", "Cảnh báo");
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi tính toán: " + ex.Message);
                }
            }
        }

        // --- HÀM TÍNH TOÁN MỚI (QUAN TRỌNG) ---
        private void CapNhatTongTien()
        {
            try
            {
                // 1. Lấy giá trị cơ bản
                decimal tPhong = ParseTienTe(txtTienPhong.Text);
                decimal tDV = ParseTienTe(txtTienDichVu.Text);
                decimal tCoc = ParseTienTe(txtDaCoc.Text);

                // Tổng cơ bản (Chưa có phụ thu/giảm giá)
                decimal tongCoBan = tPhong + tDV;

                // 2. Xử lý Phần trăm Điều chỉnh
                decimal phanTram = 0;
                decimal.TryParse(txtGiaTri.Text, out phanTram); // Lấy số từ ô nhập (ví dụ: 10)

                // Tính ra số tiền biến động (ví dụ: 500k * 10% = 50k)
                decimal tienBienDong = tongCoBan * (phanTram / 100);

                // 3. Kiểm tra ComboBox để Cộng hay Trừ
                // Giả sử: Index 0 là "Phụ thu (+)", Index 1 là "Giảm giá (-)"
                if (cboLoaiDieuChinh.SelectedIndex == 0)
                {
                    // Trường hợp PHỤ THU
                    _tongGiaTriSuDung = tongCoBan + tienBienDong;
                    _tienGiamGiaDB = 0; // Không có giảm giá
                }
                else
                {
                    // Trường hợp GIẢM GIÁ
                    _tongGiaTriSuDung = tongCoBan - tienBienDong;
                    _tienGiamGiaDB = tienBienDong; // Lưu số tiền giảm để ghi vào DB
                }

                // 4. Tính Thực Thu (Tiền khách cần trả nốt)
                _thucThu = _tongGiaTriSuDung - tCoc;

                // Hiển thị kết quả
                // lblTongTien nên hiển thị số tiền CẦN TRẢ (Thực thu) hoặc Tổng giá trị tùy ý bạn. 
                // Ở đây tôi để hiển thị số tiền khách Cần Trả Cuối Cùng.
                lblTongTien.Text = _thucThu.ToString("N0") + " VNĐ";
            }
            catch
            {
                // Bỏ qua lỗi khi đang gõ dở dang
            }
        }

        // Sự kiện khi thay đổi số % -> Tính lại ngay
        private void txtGiaTri_TextChanged(object sender, EventArgs e)
        {
            CapNhatTongTien();
        }

        // Sự kiện khi thay đổi loại (Phụ thu/Giảm giá) -> Tính lại ngay
        private void cboLoaiDieuChinh_SelectedIndexChanged(object sender, EventArgs e)
        {
            CapNhatTongTien();
        }

        private decimal ParseTienTe(string input)
        {
            if (string.IsNullOrEmpty(input)) return 0;
            return decimal.Parse(input.Replace(".", "").Replace(",", "").Trim());
        }

        // =================================================================================
        // 4. CHỨC NĂNG THANH TOÁN (LƯU DATABASE)
        // =================================================================================
        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            string thongBao = $"Tổng giá trị (Sau điều chỉnh): {_tongGiaTriSuDung:N0}\n" +
                              $"Đã cọc: -{ParseTienTe(txtDaCoc.Text):N0}\n" +
                              $"--------------------------\n" +
                              $"KHÁCH CẦN TRẢ: {_thucThu:N0} VNĐ\n\n" +
                              $"Bạn có chắc chắn muốn thanh toán?";

            if (MessageBox.Show(thongBao, "Xác nhận Checkout", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            using (SqlConnection ketNoi = new SqlConnection(chuoiketNoi))
            {
                ketNoi.Open();
                SqlTransaction trans = ketNoi.BeginTransaction();

                try
                {
                    // HÀNH ĐỘNG 1: Thêm vào bảng HOA_DON
                    string sqlHD = @"INSERT INTO HOA_DON 
                                    (MaDatPhong, MaNV, NgayLap, TongTien, GiamGia, ThueVAT, ThucThu, GhiChu) 
                                    VALUES 
                                    (@MaDP, @MaNV, GETDATE(), @TongTien, @GiamGia, @ThueVAT, @ThucThu, @GhiChu)";

                    SqlCommand cmdHD = new SqlCommand(sqlHD, ketNoi, trans);
                    cmdHD.Parameters.AddWithValue("@MaDP", _maDatPhong);
                    cmdHD.Parameters.AddWithValue("@MaNV", 1);

                    // Lưu ý logic lưu tiền:
                    // TongTien: Lưu giá trị thực tế sau cùng của hóa đơn
                    cmdHD.Parameters.AddWithValue("@TongTien", _tongGiaTriSuDung);

                    // GiamGia: Lưu số tiền đã giảm (nếu có) để báo cáo biết
                    cmdHD.Parameters.AddWithValue("@GiamGia", _tienGiamGiaDB);

                    cmdHD.Parameters.AddWithValue("@ThueVAT", 0);
                    cmdHD.Parameters.AddWithValue("@ThucThu", _thucThu);

                    // Tạo ghi chú tự động thông minh hơn
                    string loaiDC = cboLoaiDieuChinh.Text;
                    string phanTram = txtGiaTri.Text;
                    string ghiChu = $"TT: {cboHinhThuc.Text} | {loaiDC} {phanTram}%";
                    cmdHD.Parameters.AddWithValue("@GhiChu", ghiChu);

                    cmdHD.ExecuteNonQuery();

                    // HÀNH ĐỘNG 2: Cập nhật DAT_PHONG -> 'Đã trả'
                    string sqlBooking = @"UPDATE DAT_PHONG  
                                          SET TrangThai = N'Đã trả', NgayCheckOut = GETDATE()  
                                          WHERE MaDatPhong = @MaDP";
                    SqlCommand cmdBooking = new SqlCommand(sqlBooking, ketNoi, trans);
                    cmdBooking.Parameters.AddWithValue("@MaDP", _maDatPhong);
                    cmdBooking.ExecuteNonQuery();

                    // HÀNH ĐỘNG 3: Cập nhật PHONG -> 'Trống'
                    string sqlPhong = @"UPDATE PHONG SET TrangThai = N'Trống' WHERE MaPhong = @MaPhong";
                    SqlCommand cmdPhong = new SqlCommand(sqlPhong, ketNoi, trans);
                    cmdPhong.Parameters.AddWithValue("@MaPhong", int.Parse(MaPhongCanCheckOut));
                    cmdPhong.ExecuteNonQuery();

                    trans.Commit();

                    MessageBox.Show("Thanh toán thành công! Phòng đã Trống.");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    MessageBox.Show("Lỗi giao dịch: " + ex.Message, "Lỗi Nghiêm Trọng");
                }
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}