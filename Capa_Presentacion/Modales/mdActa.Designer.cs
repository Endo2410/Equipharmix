namespace CapaPresentacion.Modales
{
    partial class mdActa
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mdActa));
            this.dgvEquipo = new System.Windows.Forms.DataGridView();
            this.chkSeleccionarTod = new System.Windows.Forms.CheckBox();
            this.btnAutoriza = new FontAwesome.Sharp.IconButton();
            this.btnCancela = new FontAwesome.Sharp.IconButton();
            this.btnRechaza = new FontAwesome.Sharp.IconButton();
            this.chk = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.CodigoEquipo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NombreEquipo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Marca = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Estado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cantidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NumeroSerial = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Caja = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Movimiento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEquipo)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvEquipo
            // 
            this.dgvEquipo.AllowUserToAddRows = false;
            this.dgvEquipo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvEquipo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEquipo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.chk,
            this.CodigoEquipo,
            this.NombreEquipo,
            this.Marca,
            this.Estado,
            this.Cantidad,
            this.NumeroSerial,
            this.Caja,
            this.Movimiento});
            this.dgvEquipo.Location = new System.Drawing.Point(2, 12);
            this.dgvEquipo.Name = "dgvEquipo";
            this.dgvEquipo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvEquipo.Size = new System.Drawing.Size(1093, 273);
            this.dgvEquipo.TabIndex = 0;
            // 
            // chkSeleccionarTod
            // 
            this.chkSeleccionarTod.AutoSize = true;
            this.chkSeleccionarTod.Location = new System.Drawing.Point(12, 300);
            this.chkSeleccionarTod.Name = "chkSeleccionarTod";
            this.chkSeleccionarTod.Size = new System.Drawing.Size(111, 17);
            this.chkSeleccionarTod.TabIndex = 1;
            this.chkSeleccionarTod.Text = "Seleccionar todos";
            this.chkSeleccionarTod.UseVisualStyleBackColor = true;
            this.chkSeleccionarTod.CheckedChanged += new System.EventHandler(this.chkSeleccionarTod_CheckedChanged);
            // 
            // btnAutoriza
            // 
            this.btnAutoriza.IconChar = FontAwesome.Sharp.IconChar.None;
            this.btnAutoriza.IconColor = System.Drawing.Color.Black;
            this.btnAutoriza.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnAutoriza.Location = new System.Drawing.Point(60, 344);
            this.btnAutoriza.Name = "btnAutoriza";
            this.btnAutoriza.Size = new System.Drawing.Size(75, 30);
            this.btnAutoriza.TabIndex = 3;
            this.btnAutoriza.Text = "Autorizar";
            this.btnAutoriza.UseVisualStyleBackColor = true;
            this.btnAutoriza.Click += new System.EventHandler(this.btnAutoriza_Click);
            // 
            // btnCancela
            // 
            this.btnCancela.IconChar = FontAwesome.Sharp.IconChar.None;
            this.btnCancela.IconColor = System.Drawing.Color.Black;
            this.btnCancela.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnCancela.Location = new System.Drawing.Point(248, 344);
            this.btnCancela.Name = "btnCancela";
            this.btnCancela.Size = new System.Drawing.Size(75, 30);
            this.btnCancela.TabIndex = 3;
            this.btnCancela.Text = "Cancelar";
            this.btnCancela.UseVisualStyleBackColor = true;
            this.btnCancela.Click += new System.EventHandler(this.btnCancela_Click);
            // 
            // btnRechaza
            // 
            this.btnRechaza.IconChar = FontAwesome.Sharp.IconChar.None;
            this.btnRechaza.IconColor = System.Drawing.Color.Black;
            this.btnRechaza.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnRechaza.Location = new System.Drawing.Point(141, 344);
            this.btnRechaza.Name = "btnRechaza";
            this.btnRechaza.Size = new System.Drawing.Size(101, 30);
            this.btnRechaza.TabIndex = 3;
            this.btnRechaza.Text = "Rechazar Acta";
            this.btnRechaza.UseVisualStyleBackColor = true;
            this.btnRechaza.Click += new System.EventHandler(this.btnRechaza_Click);
            // 
            // chk
            // 
            this.chk.HeaderText = "";
            this.chk.Name = "chk";
            this.chk.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.chk.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.chk.Width = 60;
            // 
            // CodigoEquipo
            // 
            this.CodigoEquipo.HeaderText = "Codigo ";
            this.CodigoEquipo.Name = "CodigoEquipo";
            this.CodigoEquipo.ReadOnly = true;
            this.CodigoEquipo.Width = 152;
            // 
            // NombreEquipo
            // 
            this.NombreEquipo.HeaderText = "Equipo";
            this.NombreEquipo.Name = "NombreEquipo";
            this.NombreEquipo.ReadOnly = true;
            this.NombreEquipo.Width = 151;
            // 
            // Marca
            // 
            this.Marca.HeaderText = "Marca";
            this.Marca.Name = "Marca";
            this.Marca.ReadOnly = true;
            this.Marca.Width = 142;
            // 
            // Estado
            // 
            this.Estado.HeaderText = "Estado";
            this.Estado.Name = "Estado";
            this.Estado.ReadOnly = true;
            this.Estado.Width = 141;
            // 
            // Cantidad
            // 
            this.Cantidad.HeaderText = "Cantidad";
            this.Cantidad.Name = "Cantidad";
            this.Cantidad.ReadOnly = true;
            this.Cantidad.Width = 122;
            // 
            // NumeroSerial
            // 
            this.NumeroSerial.HeaderText = "Numero Serial";
            this.NumeroSerial.Name = "NumeroSerial";
            this.NumeroSerial.ReadOnly = true;
            this.NumeroSerial.Width = 151;
            // 
            // Caja
            // 
            this.Caja.HeaderText = "Caja";
            this.Caja.Name = "Caja";
            this.Caja.ReadOnly = true;
            this.Caja.Width = 131;
            // 
            // Movimiento
            // 
            this.Movimiento.HeaderText = "Movimiento";
            this.Movimiento.Name = "Movimiento";
            this.Movimiento.Visible = false;
            // 
            // mdActa
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1096, 388);
            this.Controls.Add(this.btnCancela);
            this.Controls.Add(this.btnRechaza);
            this.Controls.Add(this.btnAutoriza);
            this.Controls.Add(this.chkSeleccionarTod);
            this.Controls.Add(this.dgvEquipo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "mdActa";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Detalle Acta";
            this.Load += new System.EventHandler(this.mdActa_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEquipo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvEquipo;
        private System.Windows.Forms.CheckBox chkSeleccionarTod;
        private FontAwesome.Sharp.IconButton btnAutoriza;
        private FontAwesome.Sharp.IconButton btnCancela;
        private FontAwesome.Sharp.IconButton btnRechaza;
        private System.Windows.Forms.DataGridViewCheckBoxColumn chk;
        private System.Windows.Forms.DataGridViewTextBoxColumn CodigoEquipo;
        private System.Windows.Forms.DataGridViewTextBoxColumn NombreEquipo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Marca;
        private System.Windows.Forms.DataGridViewTextBoxColumn Estado;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cantidad;
        private System.Windows.Forms.DataGridViewTextBoxColumn NumeroSerial;
        private System.Windows.Forms.DataGridViewTextBoxColumn Caja;
        private System.Windows.Forms.DataGridViewTextBoxColumn Movimiento;
    }
}