namespace CapaPresentacion
{
    partial class frmimprimirserie
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnimprimirselecionadas = new FontAwesome.Sharp.IconButton();
            this.btnimprimirtodo = new FontAwesome.Sharp.IconButton();
            this.dgvSeries = new System.Windows.Forms.DataGridView();
            this.chkSeleccionar = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.NumeroSerie = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Equipo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnbuscar = new FontAwesome.Sharp.IconButton();
            this.label26 = new System.Windows.Forms.Label();
            this.txtnumerodocumento = new System.Windows.Forms.TextBox();
            this.btnborrar = new FontAwesome.Sharp.IconButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSeries)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnimprimirselecionadas);
            this.panel1.Controls.Add(this.btnimprimirtodo);
            this.panel1.Controls.Add(this.dgvSeries);
            this.panel1.Controls.Add(this.btnbuscar);
            this.panel1.Controls.Add(this.label26);
            this.panel1.Controls.Add(this.txtnumerodocumento);
            this.panel1.Controls.Add(this.btnborrar);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Location = new System.Drawing.Point(35, 34);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(503, 436);
            this.panel1.TabIndex = 0;
            // 
            // btnimprimirselecionadas
            // 
            this.btnimprimirselecionadas.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnimprimirselecionadas.IconChar = FontAwesome.Sharp.IconChar.FilePdf;
            this.btnimprimirselecionadas.IconColor = System.Drawing.Color.Black;
            this.btnimprimirselecionadas.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnimprimirselecionadas.IconSize = 17;
            this.btnimprimirselecionadas.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnimprimirselecionadas.Location = new System.Drawing.Point(193, 396);
            this.btnimprimirselecionadas.Name = "btnimprimirselecionadas";
            this.btnimprimirselecionadas.Size = new System.Drawing.Size(134, 21);
            this.btnimprimirselecionadas.TabIndex = 241;
            this.btnimprimirselecionadas.Text = "Imprimir Selecionadas";
            this.btnimprimirselecionadas.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnimprimirselecionadas.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnimprimirselecionadas.UseVisualStyleBackColor = true;
            this.btnimprimirselecionadas.Click += new System.EventHandler(this.btnimprimirselecionadas_Click);
            // 
            // btnimprimirtodo
            // 
            this.btnimprimirtodo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnimprimirtodo.IconChar = FontAwesome.Sharp.IconChar.FilePdf;
            this.btnimprimirtodo.IconColor = System.Drawing.Color.Black;
            this.btnimprimirtodo.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnimprimirtodo.IconSize = 17;
            this.btnimprimirtodo.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnimprimirtodo.Location = new System.Drawing.Point(333, 396);
            this.btnimprimirtodo.Name = "btnimprimirtodo";
            this.btnimprimirtodo.Size = new System.Drawing.Size(134, 21);
            this.btnimprimirtodo.TabIndex = 240;
            this.btnimprimirtodo.Text = "Imprimir Todo";
            this.btnimprimirtodo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnimprimirtodo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnimprimirtodo.UseVisualStyleBackColor = true;
            this.btnimprimirtodo.Click += new System.EventHandler(this.btnimprimirtodo_Click);
            // 
            // dgvSeries
            // 
            this.dgvSeries.AllowUserToAddRows = false;
            this.dgvSeries.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(2);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSeries.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSeries.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSeries.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.chkSeleccionar,
            this.NumeroSerie,
            this.Equipo});
            this.dgvSeries.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvSeries.Location = new System.Drawing.Point(24, 148);
            this.dgvSeries.MultiSelect = false;
            this.dgvSeries.Name = "dgvSeries";
            this.dgvSeries.RowHeadersWidth = 51;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvSeries.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvSeries.RowTemplate.Height = 28;
            this.dgvSeries.Size = new System.Drawing.Size(443, 230);
            this.dgvSeries.TabIndex = 235;
            // 
            // chkSeleccionar
            // 
            this.chkSeleccionar.FalseValue = "false";
            this.chkSeleccionar.HeaderText = "";
            this.chkSeleccionar.Name = "chkSeleccionar";
            this.chkSeleccionar.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.chkSeleccionar.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.chkSeleccionar.TrueValue = "true";
            this.chkSeleccionar.Width = 30;
            // 
            // NumeroSerie
            // 
            this.NumeroSerie.HeaderText = "Numero de Serie";
            this.NumeroSerie.Name = "NumeroSerie";
            this.NumeroSerie.ReadOnly = true;
            this.NumeroSerie.Width = 200;
            // 
            // Equipo
            // 
            this.Equipo.HeaderText = "Equipo";
            this.Equipo.Name = "Equipo";
            this.Equipo.ReadOnly = true;
            this.Equipo.Width = 150;
            // 
            // btnbuscar
            // 
            this.btnbuscar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnbuscar.IconChar = FontAwesome.Sharp.IconChar.MagnifyingGlass;
            this.btnbuscar.IconColor = System.Drawing.Color.Black;
            this.btnbuscar.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnbuscar.IconSize = 17;
            this.btnbuscar.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnbuscar.Location = new System.Drawing.Point(308, 43);
            this.btnbuscar.Name = "btnbuscar";
            this.btnbuscar.Size = new System.Drawing.Size(78, 25);
            this.btnbuscar.TabIndex = 232;
            this.btnbuscar.Text = "Buscar";
            this.btnbuscar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnbuscar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnbuscar.UseVisualStyleBackColor = true;
            this.btnbuscar.Click += new System.EventHandler(this.btnbuscar_Click);
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.BackColor = System.Drawing.Color.White;
            this.label26.Location = new System.Drawing.Point(62, 51);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(105, 13);
            this.label26.TabIndex = 233;
            this.label26.Text = "Numero Documento:";
            // 
            // txtnumerodocumento
            // 
            this.txtnumerodocumento.Location = new System.Drawing.Point(173, 47);
            this.txtnumerodocumento.Name = "txtnumerodocumento";
            this.txtnumerodocumento.Size = new System.Drawing.Size(129, 20);
            this.txtnumerodocumento.TabIndex = 231;
            // 
            // btnborrar
            // 
            this.btnborrar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnborrar.IconChar = FontAwesome.Sharp.IconChar.Eraser;
            this.btnborrar.IconColor = System.Drawing.Color.Black;
            this.btnborrar.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnborrar.IconSize = 20;
            this.btnborrar.Location = new System.Drawing.Point(389, 43);
            this.btnborrar.Name = "btnborrar";
            this.btnborrar.Size = new System.Drawing.Size(78, 25);
            this.btnborrar.TabIndex = 234;
            this.btnborrar.Text = "Limpiar";
            this.btnborrar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnborrar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnborrar.UseVisualStyleBackColor = true;
            this.btnborrar.Click += new System.EventHandler(this.btnborrar_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(19, 120);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(318, 25);
            this.label1.TabIndex = 230;
            this.label1.Text = "Lista de Equipos y codigo de Barra ";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.White;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(3, 13);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(124, 25);
            this.label9.TabIndex = 230;
            this.label9.Text = "Buscar Acta:";
            // 
            // frmimprimirserie
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(575, 495);
            this.Controls.Add(this.panel1);
            this.Name = "frmimprimirserie";
            this.Text = "frmimprimirserie";
            this.Load += new System.EventHandler(this.frmimprimirserie_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSeries)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private FontAwesome.Sharp.IconButton btnbuscar;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.TextBox txtnumerodocumento;
        private FontAwesome.Sharp.IconButton btnborrar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DataGridView dgvSeries;
        private FontAwesome.Sharp.IconButton btnimprimirselecionadas;
        private FontAwesome.Sharp.IconButton btnimprimirtodo;
        private System.Windows.Forms.DataGridViewCheckBoxColumn chkSeleccionar;
        private System.Windows.Forms.DataGridViewTextBoxColumn NumeroSerie;
        private System.Windows.Forms.DataGridViewTextBoxColumn Equipo;
    }
}