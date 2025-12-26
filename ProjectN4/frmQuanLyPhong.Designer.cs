namespace ProjectN4.GUI
{
    partial class frmQuanLyPhong
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvPhong = new System.Windows.Forms.DataGridView();
            this.txtMaPhong = new System.Windows.Forms.TextBox();
            this.txtSoPhong = new System.Windows.Forms.TextBox();
            this.txtGiaPhong = new System.Windows.Forms.TextBox();
            this.cboLoaiPhong = new System.Windows.Forms.ComboBox();
            this.cboTrangThai = new System.Windows.Forms.ComboBox();
            this.btnThem = new System.Windows.Forms.Button();
            this.btnSua = new System.Windows.Forms.Button();
            this.btnXoa = new System.Windows.Forms.Button();
            this.btnLamMoi = new System.Windows.Forms.Button();
            this.grpThongTin = new System.Windows.Forms.GroupBox();
            this.txtChiNhanh = new System.Windows.Forms.TextBox();
            this.labelChiNhanh = new System.Windows.Forms.Label();
            this.labelTrangThai = new System.Windows.Forms.Label();
            this.labelLoaiPhong = new System.Windows.Forms.Label();
            this.labelGiaPhong = new System.Windows.Forms.Label();
            this.labelSoPhong = new System.Windows.Forms.Label();
            this.labelMaPhong = new System.Windows.Forms.Label();
            this.pnlChucNang = new System.Windows.Forms.Panel();
            this.btnChiTiet = new System.Windows.Forms.Button();
            this.btnCheckOut = new System.Windows.Forms.Button();
            this.btnCheckIn = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.label1 = new System.Windows.Forms.Label();
            this.cboLocLoaiPhong = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cboLocTrangThai = new System.Windows.Forms.ComboBox();
            this.txtTimCCCD = new System.Windows.Forms.TextBox();
            this.btnTimKiem = new System.Windows.Forms.Button();
            this.grpTimKiem = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhong)).BeginInit();
            this.grpThongTin.SuspendLayout();
            this.pnlChucNang.SuspendLayout();
            this.grpTimKiem.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvPhong
            // 
            this.dgvPhong.AllowUserToAddRows = false;
            this.dgvPhong.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPhong.BackgroundColor = System.Drawing.Color.White;
            this.dgvPhong.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvPhong.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPhong.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPhong.Location = new System.Drawing.Point(0, 310);
            this.dgvPhong.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgvPhong.MultiSelect = false;
            this.dgvPhong.Name = "dgvPhong";
            this.dgvPhong.ReadOnly = true;
            this.dgvPhong.RowHeadersVisible = false;
            this.dgvPhong.RowHeadersWidth = 51;
            this.dgvPhong.RowTemplate.Height = 24;
            this.dgvPhong.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPhong.Size = new System.Drawing.Size(982, 343);
            this.dgvPhong.TabIndex = 0;
            this.dgvPhong.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPhong_CellClick);            // 
            // txtMaPhong
            // 
            this.txtMaPhong.Location = new System.Drawing.Point(150, 27);
            this.txtMaPhong.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtMaPhong.Name = "txtMaPhong";
            this.txtMaPhong.ReadOnly = true;
            this.txtMaPhong.Size = new System.Drawing.Size(250, 30);
            this.txtMaPhong.TabIndex = 1;
            // 
            // txtSoPhong
            // 
            this.txtSoPhong.Location = new System.Drawing.Point(150, 67);
            this.txtSoPhong.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSoPhong.Name = "txtSoPhong";
            this.txtSoPhong.Size = new System.Drawing.Size(250, 30);
            this.txtSoPhong.TabIndex = 2;
            // 
            // txtGiaPhong
            // 
            this.txtGiaPhong.Location = new System.Drawing.Point(150, 107);
            this.txtGiaPhong.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtGiaPhong.Name = "txtGiaPhong";
            this.txtGiaPhong.Size = new System.Drawing.Size(250, 30);
            this.txtGiaPhong.TabIndex = 3;
            // 
            // cboLoaiPhong
            // 
            this.cboLoaiPhong.FormattingEnabled = true;
            this.cboLoaiPhong.Items.AddRange(new object[] {
            "Đơn",
            "Đôi",
            "Vip"});
            this.cboLoaiPhong.Location = new System.Drawing.Point(600, 27);
            this.cboLoaiPhong.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboLoaiPhong.Name = "cboLoaiPhong";
            this.cboLoaiPhong.Size = new System.Drawing.Size(250, 31);
            this.cboLoaiPhong.TabIndex = 4;
            // 
            // cboTrangThai
            // 
            this.cboTrangThai.FormattingEnabled = true;
            this.cboTrangThai.Items.AddRange(new object[] {
            "Trống",
            "Đang ở",
            "Đang dọn"});
            this.cboTrangThai.Location = new System.Drawing.Point(600, 67);
            this.cboTrangThai.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboTrangThai.Name = "cboTrangThai";
            this.cboTrangThai.Size = new System.Drawing.Size(250, 31);
            this.cboTrangThai.TabIndex = 5;
            // 
            // btnThem
            // 
            this.btnThem.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnThem.FlatAppearance.BorderColor = System.Drawing.Color.MediumSeaGreen;
            this.btnThem.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnThem.ForeColor = System.Drawing.Color.White;
            this.btnThem.Location = new System.Drawing.Point(48, 20);
            this.btnThem.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnThem.Name = "btnThem";
            this.btnThem.Size = new System.Drawing.Size(110, 40);
            this.btnThem.TabIndex = 6;
            this.btnThem.Text = "Thêm Mới";
            this.btnThem.UseVisualStyleBackColor = false;
            this.btnThem.Click += new System.EventHandler(this.btnThem_Click);
            // 
            // btnSua
            // 
            this.btnSua.BackColor = System.Drawing.Color.SteelBlue;
            this.btnSua.FlatAppearance.BorderColor = System.Drawing.Color.MediumSeaGreen;
            this.btnSua.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSua.ForeColor = System.Drawing.Color.White;
            this.btnSua.Location = new System.Drawing.Point(178, 20);
            this.btnSua.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSua.Name = "btnSua";
            this.btnSua.Size = new System.Drawing.Size(110, 40);
            this.btnSua.TabIndex = 7;
            this.btnSua.Text = "Cập Nhật";
            this.btnSua.UseVisualStyleBackColor = false;
            this.btnSua.Click += new System.EventHandler(this.btnSua_Click);
            // 
            // btnXoa
            // 
            this.btnXoa.BackColor = System.Drawing.Color.IndianRed;
            this.btnXoa.FlatAppearance.BorderColor = System.Drawing.Color.MediumSeaGreen;
            this.btnXoa.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnXoa.ForeColor = System.Drawing.Color.White;
            this.btnXoa.Location = new System.Drawing.Point(308, 20);
            this.btnXoa.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(110, 40);
            this.btnXoa.TabIndex = 8;
            this.btnXoa.Text = "Xóa Phòng";
            this.btnXoa.UseVisualStyleBackColor = false;
            this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click);
            // 
            // btnLamMoi
            // 
            this.btnLamMoi.BackColor = System.Drawing.Color.LightGray;
            this.btnLamMoi.FlatAppearance.BorderColor = System.Drawing.Color.MediumSeaGreen;
            this.btnLamMoi.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnLamMoi.ForeColor = System.Drawing.Color.Black;
            this.btnLamMoi.Location = new System.Drawing.Point(438, 20);
            this.btnLamMoi.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new System.Drawing.Size(110, 40);
            this.btnLamMoi.TabIndex = 9;
            this.btnLamMoi.Text = "Làm mới";
            this.btnLamMoi.UseVisualStyleBackColor = false;
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);
            // 
            // grpThongTin
            // 
            this.grpThongTin.Controls.Add(this.txtChiNhanh);
            this.grpThongTin.Controls.Add(this.labelChiNhanh);
            this.grpThongTin.Controls.Add(this.labelTrangThai);
            this.grpThongTin.Controls.Add(this.labelLoaiPhong);
            this.grpThongTin.Controls.Add(this.labelGiaPhong);
            this.grpThongTin.Controls.Add(this.labelSoPhong);
            this.grpThongTin.Controls.Add(this.labelMaPhong);
            this.grpThongTin.Controls.Add(this.txtMaPhong);
            this.grpThongTin.Controls.Add(this.txtSoPhong);
            this.grpThongTin.Controls.Add(this.txtGiaPhong);
            this.grpThongTin.Controls.Add(this.cboTrangThai);
            this.grpThongTin.Controls.Add(this.cboLoaiPhong);
            this.grpThongTin.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpThongTin.Location = new System.Drawing.Point(0, 0);
            this.grpThongTin.Name = "grpThongTin";
            this.grpThongTin.Size = new System.Drawing.Size(982, 160);
            this.grpThongTin.TabIndex = 10;
            this.grpThongTin.TabStop = false;
            this.grpThongTin.Text = "Thông tin chi tiết";
            // 
            // txtChiNhanh
            // 
            this.txtChiNhanh.Location = new System.Drawing.Point(600, 110);
            this.txtChiNhanh.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtChiNhanh.Name = "txtChiNhanh";
            this.txtChiNhanh.Size = new System.Drawing.Size(250, 30);
            this.txtChiNhanh.TabIndex = 12;
            // 
            // labelChiNhanh
            // 
            this.labelChiNhanh.AutoSize = true;
            this.labelChiNhanh.Location = new System.Drawing.Point(500, 114);
            this.labelChiNhanh.Name = "labelChiNhanh";
            this.labelChiNhanh.Size = new System.Drawing.Size(93, 23);
            this.labelChiNhanh.TabIndex = 11;
            this.labelChiNhanh.Text = "Chi nhánh:";
            // 
            // labelTrangThai
            // 
            this.labelTrangThai.AutoSize = true;
            this.labelTrangThai.Location = new System.Drawing.Point(500, 70);
            this.labelTrangThai.Name = "labelTrangThai";
            this.labelTrangThai.Size = new System.Drawing.Size(91, 23);
            this.labelTrangThai.TabIndex = 10;
            this.labelTrangThai.Text = "Trạng thái:";
            // 
            // labelLoaiPhong
            // 
            this.labelLoaiPhong.AutoSize = true;
            this.labelLoaiPhong.Location = new System.Drawing.Point(500, 30);
            this.labelLoaiPhong.Name = "labelLoaiPhong";
            this.labelLoaiPhong.Size = new System.Drawing.Size(100, 23);
            this.labelLoaiPhong.TabIndex = 9;
            this.labelLoaiPhong.Text = "Loại phòng:";
            // 
            // labelGiaPhong
            // 
            this.labelGiaPhong.AutoSize = true;
            this.labelGiaPhong.Location = new System.Drawing.Point(50, 110);
            this.labelGiaPhong.Name = "labelGiaPhong";
            this.labelGiaPhong.Size = new System.Drawing.Size(94, 23);
            this.labelGiaPhong.TabIndex = 8;
            this.labelGiaPhong.Text = "Giá phòng:";
            // 
            // labelSoPhong
            // 
            this.labelSoPhong.AutoSize = true;
            this.labelSoPhong.Location = new System.Drawing.Point(50, 70);
            this.labelSoPhong.Name = "labelSoPhong";
            this.labelSoPhong.Size = new System.Drawing.Size(88, 23);
            this.labelSoPhong.TabIndex = 7;
            this.labelSoPhong.Text = "Số phòng:";
            // 
            // labelMaPhong
            // 
            this.labelMaPhong.AutoSize = true;
            this.labelMaPhong.Location = new System.Drawing.Point(50, 30);
            this.labelMaPhong.Name = "labelMaPhong";
            this.labelMaPhong.Size = new System.Drawing.Size(93, 23);
            this.labelMaPhong.TabIndex = 6;
            this.labelMaPhong.Text = "Mã phòng:";
            // 
            // pnlChucNang
            // 
            this.pnlChucNang.Controls.Add(this.btnChiTiet);
            this.pnlChucNang.Controls.Add(this.btnCheckOut);
            this.pnlChucNang.Controls.Add(this.btnThem);
            this.pnlChucNang.Controls.Add(this.btnCheckIn);
            this.pnlChucNang.Controls.Add(this.btnSua);
            this.pnlChucNang.Controls.Add(this.btnXoa);
            this.pnlChucNang.Controls.Add(this.btnLamMoi);
            this.pnlChucNang.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlChucNang.Location = new System.Drawing.Point(0, 160);
            this.pnlChucNang.Name = "pnlChucNang";
            this.pnlChucNang.Size = new System.Drawing.Size(982, 80);
            this.pnlChucNang.TabIndex = 11;
            // 
            // btnChiTiet
            // 
            this.btnChiTiet.BackColor = System.Drawing.Color.Blue;
            this.btnChiTiet.FlatAppearance.BorderColor = System.Drawing.Color.MediumSeaGreen;
            this.btnChiTiet.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnChiTiet.ForeColor = System.Drawing.Color.White;
            this.btnChiTiet.Location = new System.Drawing.Point(698, 20);
            this.btnChiTiet.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnChiTiet.Name = "btnChiTiet";
            this.btnChiTiet.Size = new System.Drawing.Size(110, 40);
            this.btnChiTiet.TabIndex = 12;
            this.btnChiTiet.Text = "Chi Tiết";
            this.btnChiTiet.UseVisualStyleBackColor = false;
            this.btnChiTiet.Click += new System.EventHandler(this.btnChiTiet_Click);
            // 
            // btnCheckOut
            // 
            this.btnCheckOut.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnCheckOut.FlatAppearance.BorderColor = System.Drawing.Color.MediumSeaGreen;
            this.btnCheckOut.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCheckOut.ForeColor = System.Drawing.Color.White;
            this.btnCheckOut.Location = new System.Drawing.Point(828, 20);
            this.btnCheckOut.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCheckOut.Name = "btnCheckOut";
            this.btnCheckOut.Size = new System.Drawing.Size(110, 40);
            this.btnCheckOut.TabIndex = 10;
            this.btnCheckOut.Text = "Check-Out";
            this.btnCheckOut.UseVisualStyleBackColor = false;
            this.btnCheckOut.Click += new System.EventHandler(this.btnCheckOut_Click);
            // 
            // btnCheckIn
            // 
            this.btnCheckIn.BackColor = System.Drawing.Color.Lime;
            this.btnCheckIn.FlatAppearance.BorderColor = System.Drawing.Color.MediumSeaGreen;
            this.btnCheckIn.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCheckIn.ForeColor = System.Drawing.Color.White;
            this.btnCheckIn.Location = new System.Drawing.Point(568, 20);
            this.btnCheckIn.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCheckIn.Name = "btnCheckIn";
            this.btnCheckIn.Size = new System.Drawing.Size(110, 40);
            this.btnCheckIn.TabIndex = 11;
            this.btnCheckIn.Text = "Check-In";
            this.btnCheckIn.UseVisualStyleBackColor = false;
            this.btnCheckIn.Click += new System.EventHandler(this.btnCheckIn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Loại phòng:";
            // 
            // cboLocLoaiPhong
            // 
            this.cboLocLoaiPhong.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLocLoaiPhong.FormattingEnabled = true;
            this.cboLocLoaiPhong.Items.AddRange(new object[] {
            "Tất cả",
            "Đơn",
            "Đôi",
            "Vip"});
            this.cboLocLoaiPhong.Location = new System.Drawing.Point(110, 27);
            this.cboLocLoaiPhong.Name = "cboLocLoaiPhong";
            this.cboLocLoaiPhong.Size = new System.Drawing.Size(130, 31);
            this.cboLocLoaiPhong.TabIndex = 1;
            this.cboLocLoaiPhong.SelectedIndexChanged += new System.EventHandler(this.cboLocLoaiPhong_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(260, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "Trạng thái:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(500, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 23);
            this.label3.TabIndex = 3;
            this.label3.Text = "Tìm CCCD:";
            // 
            // cboLocTrangThai
            // 
            this.cboLocTrangThai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLocTrangThai.FormattingEnabled = true;
            this.cboLocTrangThai.Items.AddRange(new object[] {
            "Tất cả",
            "Trống",
            "Đang ở",
            "Đang dọn"});
            this.cboLocTrangThai.Location = new System.Drawing.Point(340, 27);
            this.cboLocTrangThai.Name = "cboLocTrangThai";
            this.cboLocTrangThai.Size = new System.Drawing.Size(130, 31);
            this.cboLocTrangThai.TabIndex = 5;
            this.cboLocTrangThai.SelectedIndexChanged += new System.EventHandler(this.cboLocTrangThai_SelectedIndexChanged);
            // 
            // txtTimCCCD
            // 
            this.txtTimCCCD.Location = new System.Drawing.Point(590, 27);
            this.txtTimCCCD.Name = "txtTimCCCD";
            this.txtTimCCCD.Size = new System.Drawing.Size(180, 30);
            this.txtTimCCCD.TabIndex = 6;
            this.txtTimCCCD.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTimCCCD_KeyDown);
            // 
            // btnTimKiem
            // 
            this.btnTimKiem.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnTimKiem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTimKiem.ForeColor = System.Drawing.Color.White;
            this.btnTimKiem.Location = new System.Drawing.Point(800, 20);
            this.btnTimKiem.Name = "btnTimKiem";
            this.btnTimKiem.Size = new System.Drawing.Size(120, 38);
            this.btnTimKiem.TabIndex = 7;
            this.btnTimKiem.Text = "TÌM KIẾM";
            this.btnTimKiem.UseVisualStyleBackColor = false;
            this.btnTimKiem.Click += new System.EventHandler(this.btnTimKiem_Click);
            // 
            // grpTimKiem
            // 
            this.grpTimKiem.Controls.Add(this.btnTimKiem);
            this.grpTimKiem.Controls.Add(this.txtTimCCCD);
            this.grpTimKiem.Controls.Add(this.cboLocTrangThai);
            this.grpTimKiem.Controls.Add(this.label3);
            this.grpTimKiem.Controls.Add(this.label2);
            this.grpTimKiem.Controls.Add(this.cboLocLoaiPhong);
            this.grpTimKiem.Controls.Add(this.label1);
            this.grpTimKiem.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpTimKiem.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpTimKiem.Location = new System.Drawing.Point(0, 240);
            this.grpTimKiem.Name = "grpTimKiem";
            this.grpTimKiem.Size = new System.Drawing.Size(982, 70);
            this.grpTimKiem.TabIndex = 12;
            this.grpTimKiem.TabStop = false;
            this.grpTimKiem.Text = "Tìm Kiếm Và Lọc Thông Tin";
            // 
            // frmQuanLyPhong
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(982, 653);
            this.Controls.Add(this.dgvPhong);
            this.Controls.Add(this.grpTimKiem);
            this.Controls.Add(this.pnlChucNang);
            this.Controls.Add(this.grpThongTin);
            this.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmQuanLyPhong";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quản Lý Phòng Khách Sạn";
            this.Load += new System.EventHandler(this.frmQuanLyPhong_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhong)).EndInit();
            this.grpThongTin.ResumeLayout(false);
            this.grpThongTin.PerformLayout();
            this.pnlChucNang.ResumeLayout(false);
            this.grpTimKiem.ResumeLayout(false);
            this.grpTimKiem.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvPhong;
        private System.Windows.Forms.TextBox txtMaPhong;
        private System.Windows.Forms.TextBox txtSoPhong;
        private System.Windows.Forms.TextBox txtGiaPhong;
        private System.Windows.Forms.ComboBox cboLoaiPhong;
        private System.Windows.Forms.ComboBox cboTrangThai;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Button btnSua;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnLamMoi;
        private System.Windows.Forms.GroupBox grpThongTin;
        private System.Windows.Forms.Label labelTrangThai;
        private System.Windows.Forms.Label labelLoaiPhong;
        private System.Windows.Forms.Label labelGiaPhong;
        private System.Windows.Forms.Label labelSoPhong;
        private System.Windows.Forms.Label labelMaPhong;
        private System.Windows.Forms.Panel pnlChucNang;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.TextBox txtChiNhanh;
        private System.Windows.Forms.Label labelChiNhanh;
        private System.Windows.Forms.Button btnChiTiet;
        private System.Windows.Forms.Button btnCheckOut;
        private System.Windows.Forms.Button btnCheckIn;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboLocLoaiPhong;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboLocTrangThai;
        private System.Windows.Forms.TextBox txtTimCCCD;
        private System.Windows.Forms.Button btnTimKiem;
        private System.Windows.Forms.GroupBox grpTimKiem;
    }
}