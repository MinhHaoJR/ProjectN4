using ProjectN4.DAL;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ProjectN4 // Đổi namespace nếu cần
{
    public partial class frmCheckIn : Form
    {
        string chuoiketNoi = $"Data Source={DbSettings.ServerIP};Initial Catalog={DbSettings.DatabaseName};User ID={DbSettings.UserID};Password={DbSettings.Password};";

        public string MaPhongCanCheckIn { get; set; }

        public frmCheckIn()
        {
            InitializeComponent();
        }

        private void frmCheckIn_Load(object sender, EventArgs e)
        {
            txtMaPhong.Text = MaPhongCanCheckIn;
            dtpNgayVao.Value = DateTime.Now;
            txtTienCoc.Text = "0";
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra nhập liệu
            if (string.IsNullOrEmpty(txtTenKhach.Text) || string.IsNullOrEmpty(txtCMND.Text))
            {
                MessageBox.Show("Vui lòng nhập Tên và CMND khách hàng!");
                txtTenKhach.Focus();
                return;
            }

            decimal tienCoc = 0;
            if (!string.IsNullOrEmpty(txtTienCoc.Text))
                decimal.TryParse(txtTienCoc.Text, out tienCoc);

            using (SqlConnection conn = new SqlConnection(chuoiketNoi))
            {
                try
                {
                    conn.Open();
                    int maKhachHang = 0;

                    // ==========================================================
                    // BƯỚC 1: XỬ LÝ KHÁCH HÀNG (QUAN TRỌNG)
                    // ==========================================================

                    // Kiểm tra xem khách có CMND này đã tồn tại chưa?
                    string sqlCheckKH = "SELECT MaKH FROM KHACH_HANG WHERE CCCD_Passport = @CMND";
                    SqlCommand cmdCheck = new SqlCommand(sqlCheckKH, conn);
                    cmdCheck.Parameters.AddWithValue("@CMND", txtCMND.Text);

                    object result = cmdCheck.ExecuteScalar();

                    if (result != null)
                    {
                        // Khách CŨ: Lấy luôn ID cũ
                        maKhachHang = Convert.ToInt32(result);
                    }
                    else
                    {
                        // Khách MỚI: Thêm vào và lấy ID vừa sinh ra
                        // Dùng OUTPUT INSERTED.MaKH để lấy ngay ID mới
                        string sqlInsertKH = @"INSERT INTO KHACH_HANG (HoTen, CCCD_Passport) 
                                               OUTPUT INSERTED.MaKH 
                                               VALUES (@HoTen, @CMND)";
                        SqlCommand cmdInsertKH = new SqlCommand(sqlInsertKH, conn);
                        cmdInsertKH.Parameters.AddWithValue("@HoTen", txtTenKhach.Text);
                        cmdInsertKH.Parameters.AddWithValue("@CMND", txtCMND.Text);

                        maKhachHang = (int)cmdInsertKH.ExecuteScalar();
                    }

                    // ==========================================================
                    // BƯỚC 2: TẠO DÒNG ĐẶT PHÒNG (INSERT VÀO DAT_PHONG)
                    // ==========================================================

                    // Lưu ý: SQL yêu cầu NgayCheckOut không được để trống.
                    // Check-in thì chưa biết bao giờ ra, tạm thời cộng thêm 1 ngày để giữ chỗ.
                    string sqlDatPhong = @"INSERT INTO DAT_PHONG (MaKH, MaPhong, NgayCheckIn, NgayCheckOut, TienCoc, TrangThai) 
                                           VALUES (@MaKH, @MaPhong, @NgayIn, @NgayOut, @TienCoc, N'Đang ở')";

                    SqlCommand cmdDatPhong = new SqlCommand(sqlDatPhong, conn);
                    cmdDatPhong.Parameters.AddWithValue("@MaKH", maKhachHang);
                    cmdDatPhong.Parameters.AddWithValue("@MaPhong", txtMaPhong.Text);
                    cmdDatPhong.Parameters.AddWithValue("@NgayIn", dtpNgayVao.Value);
                    cmdDatPhong.Parameters.AddWithValue("@NgayOut", dtpNgayVao.Value.AddDays(1)); // Tạm tính 1 ngày
                    cmdDatPhong.Parameters.AddWithValue("@TienCoc", tienCoc);

                    cmdDatPhong.ExecuteNonQuery();

                    // ==========================================================
                    // BƯỚC 3: CẬP NHẬT TRẠNG THÁI PHÒNG
                    // ==========================================================
                    string sqlUpdatePhong = "UPDATE PHONG SET TrangThai = N'Đang ở' WHERE MaPhong = @MaPhong";
                    SqlCommand cmdUpdate = new SqlCommand(sqlUpdatePhong, conn);
                    cmdUpdate.Parameters.AddWithValue("@MaPhong", txtMaPhong.Text);
                    cmdUpdate.ExecuteNonQuery();

                    MessageBox.Show("Check-In thành công!");

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi thực thi: " + ex.Message);
                }
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}