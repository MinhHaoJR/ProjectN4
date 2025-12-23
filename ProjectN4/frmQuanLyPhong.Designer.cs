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
            this.labelTrangThai = new System.Windows.Forms.Label();
            this.labelLoaiPhong = new System.Windows.Forms.Label();
            this.labelGiaPhong = new System.Windows.Forms.Label();
            this.labelSoPhong = new System.Windows.Forms.Label();
            this.labelMaPhong = new System.Windows.Forms.Label();
            this.pnlChucNang = new System.Windows.Forms.Panel();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.labelChiNhanh = new System.Windows.Forms.Label();
            this.txtChiNhanh = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhong)).BeginInit();
            this.grpThongTin.SuspendLayout();
            this.pnlChucNang.SuspendLayout();
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
            this.dgvPhong.Location = new System.Drawing.Point(0, 0);
            this.dgvPhong.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgvPhong.MultiSelect = false;
            this.dgvPhong.Name = "dgvPhong";
            this.dgvPhong.ReadOnly = true;
            this.dgvPhong.RowHeadersVisible = false;
            this.dgvPhong.RowHeadersWidth = 51;
            this.dgvPhong.RowTemplate.Height = 24;
            this.dgvPhong.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPhong.Size = new System.Drawing.Size(982, 653);
            this.dgvPhong.TabIndex = 0;
            this.dgvPhong.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPhong_CellClick);
            // 
            // txtMaPhong
            // 
            this.txtMaPhong.Location = new System.Drawing.Point(150, 27);
            this.txtMaPhong.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtMaPhong.Name = "txtMaPhong";
            this.txtMaPhong.ReadOnly = true;
            this.txtMaPhong.Size = new System.Drawing.Size(250, 26);
            this.txtMaPhong.TabIndex = 1;
            // 
            // txtSoPhong
            // 
            this.txtSoPhong.Location = new System.Drawing.Point(150, 67);
            this.txtSoPhong.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSoPhong.Name = "txtSoPhong";
            this.txtSoPhong.Size = new System.Drawing.Size(250, 26);
            this.txtSoPhong.TabIndex = 2;
            this.txtSoPhong.TextChanged += new System.EventHandler(this.txtSoPhong_TextChanged);
            // 
            // txtGiaPhong
            // 
            this.txtGiaPhong.Location = new System.Drawing.Point(150, 107);
            this.txtGiaPhong.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtGiaPhong.Name = "txtGiaPhong";
            this.txtGiaPhong.Size = new System.Drawing.Size(250, 26);
            this.txtGiaPhong.TabIndex = 3;
            this.txtGiaPhong.TextChanged += new System.EventHandler(this.txtGiaPhong_TextChanged);
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
            this.cboLoaiPhong.Size = new System.Drawing.Size(250, 27);
            this.cboLoaiPhong.TabIndex = 4;
            this.cboLoaiPhong.SelectedIndexChanged += new System.EventHandler(this.cboLoaiPhong_SelectedIndexChanged);
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
            this.cboTrangThai.Size = new System.Drawing.Size(250, 27);
            this.cboTrangThai.TabIndex = 5;
            // 
            // btnThem
            // 
            this.btnThem.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnThem.FlatAppearance.BorderColor = System.Drawing.Color.MediumSeaGreen;
            this.btnThem.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnThem.ForeColor = System.Drawing.Color.White;
            this.btnThem.Location = new System.Drawing.Point(250, 20);
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
            this.btnSua.Location = new System.Drawing.Point(380, 20);
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
            this.btnXoa.Location = new System.Drawing.Point(510, 20);
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
            this.btnLamMoi.Location = new System.Drawing.Point(640, 20);
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
            // labelTrangThai
            // 
            this.labelTrangThai.AutoSize = true;
            this.labelTrangThai.Location = new System.Drawing.Point(500, 70);
            this.labelTrangThai.Name = "labelTrangThai";
            this.labelTrangThai.Size = new System.Drawing.Size(73, 19);
            this.labelTrangThai.TabIndex = 10;
            this.labelTrangThai.Text = "Trạng thái:";
            // 
            // labelLoaiPhong
            // 
            this.labelLoaiPhong.AutoSize = true;
            this.labelLoaiPhong.Location = new System.Drawing.Point(500, 30);
            this.labelLoaiPhong.Name = "labelLoaiPhong";
            this.labelLoaiPhong.Size = new System.Drawing.Size(81, 19);
            this.labelLoaiPhong.TabIndex = 9;
            this.labelLoaiPhong.Text = "Loại phòng:";
            // 
            // labelGiaPhong
            // 
            this.labelGiaPhong.AutoSize = true;
            this.labelGiaPhong.Location = new System.Drawing.Point(50, 110);
            this.labelGiaPhong.Name = "labelGiaPhong";
            this.labelGiaPhong.Size = new System.Drawing.Size(76, 19);
            this.labelGiaPhong.TabIndex = 8;
            this.labelGiaPhong.Text = "Giá phòng:";
            this.labelGiaPhong.Click += new System.EventHandler(this.label3_Click);
            // 
            // labelSoPhong
            // 
            this.labelSoPhong.AutoSize = true;
            this.labelSoPhong.Location = new System.Drawing.Point(50, 70);
            this.labelSoPhong.Name = "labelSoPhong";
            this.labelSoPhong.Size = new System.Drawing.Size(71, 19);
            this.labelSoPhong.TabIndex = 7;
            this.labelSoPhong.Text = "Số phòng:";
            // 
            // labelMaPhong
            // 
            this.labelMaPhong.AutoSize = true;
            this.labelMaPhong.Location = new System.Drawing.Point(50, 30);
            this.labelMaPhong.Name = "labelMaPhong";
            this.labelMaPhong.Size = new System.Drawing.Size(76, 19);
            this.labelMaPhong.TabIndex = 6;
            this.labelMaPhong.Text = "Mã phòng:";
            // 
            // pnlChucNang
            // 
            this.pnlChucNang.Controls.Add(this.btnThem);
            this.pnlChucNang.Controls.Add(this.btnSua);
            this.pnlChucNang.Controls.Add(this.btnXoa);
            this.pnlChucNang.Controls.Add(this.btnLamMoi);
            this.pnlChucNang.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlChucNang.Location = new System.Drawing.Point(0, 160);
            this.pnlChucNang.Name = "pnlChucNang";
            this.pnlChucNang.Size = new System.Drawing.Size(982, 80);
            this.pnlChucNang.TabIndex = 11;
            // 
            // labelChiNhanh
            // 
            this.labelChiNhanh.AutoSize = true;
            this.labelChiNhanh.Location = new System.Drawing.Point(500, 114);
            this.labelChiNhanh.Name = "labelChiNhanh";
            this.labelChiNhanh.Size = new System.Drawing.Size(75, 19);
            this.labelChiNhanh.TabIndex = 11;
            this.labelChiNhanh.Text = "Chi nhánh:";
            // 
            // txtChiNhanh
            // 
            this.txtChiNhanh.Location = new System.Drawing.Point(600, 110);
            this.txtChiNhanh.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtChiNhanh.Name = "txtChiNhanh";
            this.txtChiNhanh.Size = new System.Drawing.Size(250, 26);
            this.txtChiNhanh.TabIndex = 12;
            // 
            // frmQuanLyPhong
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(982, 653);
            this.Controls.Add(this.pnlChucNang);
            this.Controls.Add(this.grpThongTin);
            this.Controls.Add(this.dgvPhong);
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
    }
}