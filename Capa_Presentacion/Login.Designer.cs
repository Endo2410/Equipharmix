namespace CapaPresentacion
{
    partial class Login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lbl_error = new System.Windows.Forms.Label();
            this.lblcontraseña = new System.Windows.Forms.Label();
            this.lblusuario = new System.Windows.Forms.Label();
            this.btnentrar = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pblcandado = new System.Windows.Forms.PictureBox();
            this.txtpassword = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pbusuario = new System.Windows.Forms.PictureBox();
            this.txtusuario = new System.Windows.Forms.TextBox();
            this.linrecuperacion = new System.Windows.Forms.LinkLabel();
            this.lbwelcome = new System.Windows.Forms.Label();
            this.pbclose = new System.Windows.Forms.PictureBox();
            this.pnlLogo = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pblcandado)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbusuario)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbclose)).BeginInit();
            this.pnlLogo.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(270, 366);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // lbl_error
            // 
            this.lbl_error.AutoSize = true;
            this.lbl_error.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_error.ForeColor = System.Drawing.Color.Red;
            this.lbl_error.Location = new System.Drawing.Point(322, 214);
            this.lbl_error.Name = "lbl_error";
            this.lbl_error.Size = new System.Drawing.Size(52, 24);
            this.lbl_error.TabIndex = 51;
            this.lbl_error.Text = "Error";
            // 
            // lblcontraseña
            // 
            this.lblcontraseña.AutoSize = true;
            this.lblcontraseña.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblcontraseña.ForeColor = System.Drawing.Color.White;
            this.lblcontraseña.Location = new System.Drawing.Point(322, 138);
            this.lblcontraseña.Name = "lblcontraseña";
            this.lblcontraseña.Size = new System.Drawing.Size(106, 24);
            this.lblcontraseña.TabIndex = 50;
            this.lblcontraseña.Text = "Contraseña";
            // 
            // lblusuario
            // 
            this.lblusuario.AutoSize = true;
            this.lblusuario.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblusuario.ForeColor = System.Drawing.Color.White;
            this.lblusuario.Location = new System.Drawing.Point(321, 73);
            this.lblusuario.Name = "lblusuario";
            this.lblusuario.Size = new System.Drawing.Size(74, 24);
            this.lblusuario.TabIndex = 49;
            this.lblusuario.Text = "Usuario";
            // 
            // btnentrar
            // 
            this.btnentrar.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnentrar.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnentrar.BackColor = System.Drawing.Color.LimeGreen;
            this.btnentrar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnentrar.BackgroundImage")));
            this.btnentrar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnentrar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnentrar.FlatAppearance.BorderColor = System.Drawing.Color.LimeGreen;
            this.btnentrar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LimeGreen;
            this.btnentrar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LimeGreen;
            this.btnentrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnentrar.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnentrar.ForeColor = System.Drawing.Color.White;
            this.btnentrar.Location = new System.Drawing.Point(355, 242);
            this.btnentrar.Name = "btnentrar";
            this.btnentrar.Size = new System.Drawing.Size(173, 57);
            this.btnentrar.TabIndex = 48;
            this.btnentrar.Text = "Entrar";
            this.btnentrar.UseVisualStyleBackColor = false;
            this.btnentrar.Click += new System.EventHandler(this.btnentrar_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.pblcandado);
            this.panel2.Controls.Add(this.txtpassword);
            this.panel2.Location = new System.Drawing.Point(322, 165);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(3);
            this.panel2.Size = new System.Drawing.Size(239, 26);
            this.panel2.TabIndex = 47;
            // 
            // pblcandado
            // 
            this.pblcandado.Dock = System.Windows.Forms.DockStyle.Left;
            this.pblcandado.Image = ((System.Drawing.Image)(resources.GetObject("pblcandado.Image")));
            this.pblcandado.Location = new System.Drawing.Point(3, 3);
            this.pblcandado.Name = "pblcandado";
            this.pblcandado.Padding = new System.Windows.Forms.Padding(5);
            this.pblcandado.Size = new System.Drawing.Size(26, 20);
            this.pblcandado.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pblcandado.TabIndex = 1;
            this.pblcandado.TabStop = false;
            // 
            // txtpassword
            // 
            this.txtpassword.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtpassword.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.txtpassword.Location = new System.Drawing.Point(33, 10);
            this.txtpassword.Name = "txtpassword";
            this.txtpassword.PasswordChar = '*';
            this.txtpassword.Size = new System.Drawing.Size(200, 13);
            this.txtpassword.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.pbusuario);
            this.panel1.Controls.Add(this.txtusuario);
            this.panel1.Location = new System.Drawing.Point(322, 100);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(3);
            this.panel1.Size = new System.Drawing.Size(239, 26);
            this.panel1.TabIndex = 46;
            // 
            // pbusuario
            // 
            this.pbusuario.Dock = System.Windows.Forms.DockStyle.Left;
            this.pbusuario.Image = ((System.Drawing.Image)(resources.GetObject("pbusuario.Image")));
            this.pbusuario.Location = new System.Drawing.Point(3, 3);
            this.pbusuario.Margin = new System.Windows.Forms.Padding(5);
            this.pbusuario.Name = "pbusuario";
            this.pbusuario.Size = new System.Drawing.Size(26, 20);
            this.pbusuario.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbusuario.TabIndex = 1;
            this.pbusuario.TabStop = false;
            // 
            // txtusuario
            // 
            this.txtusuario.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtusuario.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtusuario.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.txtusuario.Location = new System.Drawing.Point(33, 7);
            this.txtusuario.Name = "txtusuario";
            this.txtusuario.Size = new System.Drawing.Size(200, 13);
            this.txtusuario.TabIndex = 0;
            // 
            // linrecuperacion
            // 
            this.linrecuperacion.AutoSize = true;
            this.linrecuperacion.BackColor = System.Drawing.Color.Indigo;
            this.linrecuperacion.LinkColor = System.Drawing.Color.White;
            this.linrecuperacion.Location = new System.Drawing.Point(385, 203);
            this.linrecuperacion.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.linrecuperacion.Name = "linrecuperacion";
            this.linrecuperacion.Size = new System.Drawing.Size(0, 13);
            this.linrecuperacion.TabIndex = 52;
            this.linrecuperacion.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linrecuperacion_LinkClicked);
            // 
            // lbwelcome
            // 
            this.lbwelcome.AutoSize = true;
            this.lbwelcome.Font = new System.Drawing.Font("Cooper Black", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbwelcome.ForeColor = System.Drawing.Color.White;
            this.lbwelcome.Location = new System.Drawing.Point(352, 36);
            this.lbwelcome.Name = "lbwelcome";
            this.lbwelcome.Size = new System.Drawing.Size(176, 31);
            this.lbwelcome.TabIndex = 45;
            this.lbwelcome.Text = "Bienvenido!";
            // 
            // pbclose
            // 
            this.pbclose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbclose.Image = ((System.Drawing.Image)(resources.GetObject("pbclose.Image")));
            this.pbclose.Location = new System.Drawing.Point(547, 0);
            this.pbclose.Name = "pbclose";
            this.pbclose.Size = new System.Drawing.Size(39, 36);
            this.pbclose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbclose.TabIndex = 44;
            this.pbclose.TabStop = false;
            this.pbclose.Click += new System.EventHandler(this.pbclose_Click);
            // 
            // pnlLogo
            // 
            this.pnlLogo.BackColor = System.Drawing.Color.Black;
            this.pnlLogo.Controls.Add(this.pictureBox1);
            this.pnlLogo.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLogo.Location = new System.Drawing.Point(0, 0);
            this.pnlLogo.Name = "pnlLogo";
            this.pnlLogo.Size = new System.Drawing.Size(270, 366);
            this.pnlLogo.TabIndex = 43;
            // 
            // Login
            // 
            this.AcceptButton = this.btnentrar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LimeGreen;
            this.ClientSize = new System.Drawing.Size(598, 366);
            this.Controls.Add(this.lbl_error);
            this.Controls.Add(this.lblcontraseña);
            this.Controls.Add(this.lblusuario);
            this.Controls.Add(this.btnentrar);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.linrecuperacion);
            this.Controls.Add(this.lbwelcome);
            this.Controls.Add(this.pbclose);
            this.Controls.Add(this.pnlLogo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.Load += new System.EventHandler(this.Login_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pblcandado)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbusuario)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbclose)).EndInit();
            this.pnlLogo.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lbl_error;
        private System.Windows.Forms.Label lblcontraseña;
        private System.Windows.Forms.Label lblusuario;
        private System.Windows.Forms.Button btnentrar;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pblcandado;
        private System.Windows.Forms.TextBox txtpassword;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pbusuario;
        private System.Windows.Forms.TextBox txtusuario;
        private System.Windows.Forms.LinkLabel linrecuperacion;
        private System.Windows.Forms.Label lbwelcome;
        private System.Windows.Forms.PictureBox pbclose;
        private System.Windows.Forms.Panel pnlLogo;
    }
}