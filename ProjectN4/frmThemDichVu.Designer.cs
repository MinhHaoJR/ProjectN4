namespace ProjectN4
{
    partial class frmThemDichVu
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
            this.lblSoPhong = new System.Windows.Forms.Label();
            this.lblTenKhach = new System.Windows.Forms.Label();
            this.cboDichVu = new System.Windows.Forms.ComboBox();
            this.txtDonGia = new System.Windows.Forms.TextBox();
            this.numSoLuong = new System.Windows.Forms.NumericUpDown();
            this.btnThem = new System.Windows.Forms.Button();
            this.dgvDichVuDaDung = new System.Windows.Forms.DataGridView();
            this.lblChonDV = new System.Windows.Forms.Label();
            this.lblDonGia = new System.Windows.Forms.Label();
            this.lblSoLuong = new System.Windows.Forms.Label();
            this.lblThanhTienTiltle = new System.Windows.Forms.Label();
            this.txtSoPhong = new System.Windows.Forms.TextBox();
            this.txtTenKhach = new System.Windows.Forms.TextBox();
            this.btnDong = new System.Windows.Forms.Button();
            this.lblThanhTien = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numSoLuong)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDichVuDaDung)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSoPhong
            // 
            this.lblSoPhong.AutoSize = true;
            this.lblSoPhong.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSoPhong.Location = new System.Drawing.Point(30, 30);
            this.lblSoPhong.Name = "lblSoPhong";
            this.lblSoPhong.Size = new System.Drawing.Size(98, 23);
            this.lblSoPhong.TabIndex = 0;
            this.lblSoPhong.Text = "Mã phòng :";
            this.lblSoPhong.Click += new System.EventHandler(this.lblSoPhong_Click);
            // 
            // lblTenKhach
            // 
            this.lblTenKhach.AutoSize = true;
            this.lblTenKhach.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblTenKhach.Location = new System.Drawing.Point(30, 70);
            this.lblTenKhach.Name = "lblTenKhach";
            this.lblTenKhach.Size = new System.Drawing.Size(90, 23);
            this.lblTenKhach.TabIndex = 1;
            this.lblTenKhach.Text = "Tên khách:";
            this.lblTenKhach.Click += new System.EventHandler(this.lblTenKhach_Click);
            // 
            // cboDichVu
            // 
            this.cboDichVu.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDichVu.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboDichVu.FormattingEnabled = true;
            this.cboDichVu.Items.AddRange(new object[] {
            "Giặt ủi",
            "Nước suối",
            "Spa"});
            this.cboDichVu.Location = new System.Drawing.Point(151, 123);
            this.cboDichVu.Name = "cboDichVu";
            this.cboDichVu.Size = new System.Drawing.Size(280, 31);
            this.cboDichVu.TabIndex = 2;
            this.cboDichVu.SelectedIndexChanged += new System.EventHandler(this.cboDichVu_SelectedIndexChanged);
            // 
            // txtDonGia
            // 
            this.txtDonGia.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtDonGia.Location = new System.Drawing.Point(150, 170);
            this.txtDonGia.Name = "txtDonGia";
            this.txtDonGia.ReadOnly = true;
            this.txtDonGia.Size = new System.Drawing.Size(280, 30);
            this.txtDonGia.TabIndex = 3;
            this.txtDonGia.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtDonGia.TextChanged += new System.EventHandler(this.txtDonGia_TextChanged);
            // 
            // numSoLuong
            // 
            this.numSoLuong.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.numSoLuong.Location = new System.Drawing.Point(150, 220);
            this.numSoLuong.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSoLuong.Name = "numSoLuong";
            this.numSoLuong.Size = new System.Drawing.Size(280, 30);
            this.numSoLuong.TabIndex = 4;
            this.numSoLuong.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numSoLuong.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSoLuong.ValueChanged += new System.EventHandler(this.numSoLuong_ValueChanged);
            // 
            // btnThem
            // 
            this.btnThem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnThem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThem.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnThem.ForeColor = System.Drawing.Color.White;
            this.btnThem.Location = new System.Drawing.Point(240, 320);
            this.btnThem.Name = "btnThem";
            this.btnThem.Size = new System.Drawing.Size(100, 40);
            this.btnThem.TabIndex = 5;
            this.btnThem.Text = "Thêm";
            this.btnThem.UseVisualStyleBackColor = false;
            this.btnThem.Click += new System.EventHandler(this.btnThem_Click);
            // 
            // dgvDichVuDaDung
            // 
            this.dgvDichVuDaDung.AllowUserToAddRows = false;
            this.dgvDichVuDaDung.AllowUserToDeleteRows = false;
            this.dgvDichVuDaDung.BackgroundColor = System.Drawing.Color.White;
            this.dgvDichVuDaDung.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvDichVuDaDung.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDichVuDaDung.Location = new System.Drawing.Point(470, 30);
            this.dgvDichVuDaDung.Name = "dgvDichVuDaDung";
            this.dgvDichVuDaDung.ReadOnly = true;
            this.dgvDichVuDaDung.RowHeadersWidth = 51;
            this.dgvDichVuDaDung.RowTemplate.Height = 30;
            this.dgvDichVuDaDung.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDichVuDaDung.Size = new System.Drawing.Size(480, 330);
            this.dgvDichVuDaDung.TabIndex = 6;
            this.dgvDichVuDaDung.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDichVuDaDung_CellContentClick);
            // 
            // lblChonDV
            // 
            this.lblChonDV.AutoSize = true;
            this.lblChonDV.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblChonDV.Location = new System.Drawing.Point(13, 123);
            this.lblChonDV.Name = "lblChonDV";
            this.lblChonDV.Size = new System.Drawing.Size(115, 23);
            this.lblChonDV.TabIndex = 7;
            this.lblChonDV.Text = "Chọn dịch vụ:";
            this.lblChonDV.Click += new System.EventHandler(this.lblChonDV_Click);
            // 
            // lblDonGia
            // 
            this.lblDonGia.AutoSize = true;
            this.lblDonGia.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblDonGia.Location = new System.Drawing.Point(30, 173);
            this.lblDonGia.Name = "lblDonGia";
            this.lblDonGia.Size = new System.Drawing.Size(74, 23);
            this.lblDonGia.TabIndex = 8;
            this.lblDonGia.Text = "Đơn giá:";
            this.lblDonGia.Click += new System.EventHandler(this.lblDonGia_Click);
            // 
            // lblSoLuong
            // 
            this.lblSoLuong.AutoSize = true;
            this.lblSoLuong.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSoLuong.Location = new System.Drawing.Point(22, 222);
            this.lblSoLuong.Name = "lblSoLuong";
            this.lblSoLuong.Size = new System.Drawing.Size(82, 23);
            this.lblSoLuong.TabIndex = 9;
            this.lblSoLuong.Text = "Số lượng:";
            this.lblSoLuong.Click += new System.EventHandler(this.lblSoLuong_Click);
            // 
            // lblThanhTienTiltle
            // 
            this.lblThanhTienTiltle.AutoSize = true;
            this.lblThanhTienTiltle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblThanhTienTiltle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblThanhTienTiltle.Location = new System.Drawing.Point(21, 273);
            this.lblThanhTienTiltle.Name = "lblThanhTienTiltle";
            this.lblThanhTienTiltle.Size = new System.Drawing.Size(111, 25);
            this.lblThanhTienTiltle.TabIndex = 10;
            this.lblThanhTienTiltle.Text = "Thành tiền:";
            this.lblThanhTienTiltle.Click += new System.EventHandler(this.lblThanhTienTiltle_Click);
            // 
            // txtSoPhong
            // 
            this.txtSoPhong.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSoPhong.Location = new System.Drawing.Point(150, 27);
            this.txtSoPhong.Name = "txtSoPhong";
            this.txtSoPhong.Size = new System.Drawing.Size(280, 30);
            this.txtSoPhong.TabIndex = 11;
            this.txtSoPhong.TextChanged += new System.EventHandler(this.txtSoPhong_TextChanged);
            // 
            // txtTenKhach
            // 
            this.txtTenKhach.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtTenKhach.Location = new System.Drawing.Point(151, 70);
            this.txtTenKhach.Name = "txtTenKhach";
            this.txtTenKhach.Size = new System.Drawing.Size(280, 30);
            this.txtTenKhach.TabIndex = 12;
            this.txtTenKhach.TextChanged += new System.EventHandler(this.txtTenKhach_TextChanged);
            // 
            // btnDong
            // 
            this.btnDong.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnDong.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDong.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnDong.ForeColor = System.Drawing.Color.White;
            this.btnDong.Location = new System.Drawing.Point(360, 320);
            this.btnDong.Name = "btnDong";
            this.btnDong.Size = new System.Drawing.Size(100, 40);
            this.btnDong.TabIndex = 13;
            this.btnDong.Text = "Đóng";
            this.btnDong.UseVisualStyleBackColor = false;
            this.btnDong.Click += new System.EventHandler(this.btnDong_Click);
            // 
            // lblThanhTien
            // 
            this.lblThanhTien.AutoSize = true;
            this.lblThanhTien.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblThanhTien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblThanhTien.Location = new System.Drawing.Point(174, 270);
            this.lblThanhTien.Name = "lblThanhTien";
            this.lblThanhTien.Size = new System.Drawing.Size(24, 28);
            this.lblThanhTien.TabIndex = 14;
            this.lblThanhTien.Text = "0";
            this.lblThanhTien.Click += new System.EventHandler(this.lblThanhTien_Click);
            // 
            // frmThemDichVu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(244)))), ((int)(((byte)(248)))));
            this.ClientSize = new System.Drawing.Size(980, 400);
            this.Controls.Add(this.lblThanhTien);
            this.Controls.Add(this.btnDong);
            this.Controls.Add(this.txtTenKhach);
            this.Controls.Add(this.txtSoPhong);
            this.Controls.Add(this.lblThanhTienTiltle);
            this.Controls.Add(this.lblSoLuong);
            this.Controls.Add(this.lblDonGia);
            this.Controls.Add(this.lblChonDV);
            this.Controls.Add(this.dgvDichVuDaDung);
            this.Controls.Add(this.btnThem);
            this.Controls.Add(this.numSoLuong);
            this.Controls.Add(this.txtDonGia);
            this.Controls.Add(this.cboDichVu);
            this.Controls.Add(this.lblTenKhach);
            this.Controls.Add(this.lblSoPhong);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frmThemDichVu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Thêm Dịch Vụ Phòng";
            ((System.ComponentModel.ISupportInitialize)(this.numSoLuong)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDichVuDaDung)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSoPhong;
        private System.Windows.Forms.Label lblTenKhach;
        private System.Windows.Forms.ComboBox cboDichVu;
        private System.Windows.Forms.TextBox txtDonGia;
        private System.Windows.Forms.NumericUpDown numSoLuong;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.DataGridView dgvDichVuDaDung;
        private System.Windows.Forms.Label lblChonDV;
        private System.Windows.Forms.Label lblDonGia;
        private System.Windows.Forms.Label lblSoLuong;
        private System.Windows.Forms.Label lblThanhTienTiltle;
        private System.Windows.Forms.TextBox txtSoPhong;
        private System.Windows.Forms.TextBox txtTenKhach;
        private System.Windows.Forms.Button btnDong;
        private System.Windows.Forms.Label lblThanhTien;
    }
}