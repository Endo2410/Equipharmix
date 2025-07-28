namespace CapaPresentacion
{
    partial class Inicio
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Inicio));
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.pb_cerrarsecion = new System.Windows.Forms.PictureBox();
            this.pb_minimizar = new System.Windows.Forms.PictureBox();
            this.pb_maximizar = new System.Windows.Forms.PictureBox();
            this.pb_salir = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblusuario = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.menu_titulo = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.contenedor = new System.Windows.Forms.Panel();
            this.menuusuarios = new FontAwesome.Sharp.IconMenuItem();
            this.menumantenedor = new FontAwesome.Sharp.IconMenuItem();
            this.submenumarca = new FontAwesome.Sharp.IconMenuItem();
            this.submenuequipo = new FontAwesome.Sharp.IconMenuItem();
            this.submenunegocio = new System.Windows.Forms.ToolStripMenuItem();
            this.menuacta = new FontAwesome.Sharp.IconMenuItem();
            this.submenuregistraracta = new System.Windows.Forms.ToolStripMenuItem();
            this.submenuverdetalleacta = new System.Windows.Forms.ToolStripMenuItem();
            this.menuregistrar = new FontAwesome.Sharp.IconMenuItem();
            this.submenuregistrarcompra = new FontAwesome.Sharp.IconMenuItem();
            this.submenuverdatallecompra = new FontAwesome.Sharp.IconMenuItem();
            this.menufarmacia = new FontAwesome.Sharp.IconMenuItem();
            this.menuautorizacion = new FontAwesome.Sharp.IconMenuItem();
            this.autorizacionActaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autorizacionBajaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.equiposDeBajaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuasiganados = new FontAwesome.Sharp.IconMenuItem();
            this.menureportes = new FontAwesome.Sharp.IconMenuItem();
            this.submenureporteacta = new System.Windows.Forms.ToolStripMenuItem();
            this.submenureporteregistrar = new System.Windows.Forms.ToolStripMenuItem();
            this.menuacercade = new FontAwesome.Sharp.IconMenuItem();
            this.menu = new System.Windows.Forms.MenuStrip();
            ((System.ComponentModel.ISupportInitialize)(this.pb_cerrarsecion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_minimizar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_maximizar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_salir)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.menu_titulo.SuspendLayout();
            this.menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(22, 76);
            this.toolStripMenuItem1.Text = " ";
            // 
            // pb_cerrarsecion
            // 
            this.pb_cerrarsecion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pb_cerrarsecion.BackColor = System.Drawing.Color.DarkGreen;
            this.pb_cerrarsecion.Image = global::CapaPresentacion.Properties.Resources.icons8_cerrar_sesión_24;
            this.pb_cerrarsecion.Location = new System.Drawing.Point(938, 44);
            this.pb_cerrarsecion.Name = "pb_cerrarsecion";
            this.pb_cerrarsecion.Size = new System.Drawing.Size(30, 19);
            this.pb_cerrarsecion.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_cerrarsecion.TabIndex = 40;
            this.pb_cerrarsecion.TabStop = false;
            this.pb_cerrarsecion.UseWaitCursor = true;
            this.pb_cerrarsecion.Click += new System.EventHandler(this.pb_cerrarsecion_Click_1);
            // 
            // pb_minimizar
            // 
            this.pb_minimizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pb_minimizar.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pb_minimizar.Image = ((System.Drawing.Image)(resources.GetObject("pb_minimizar.Image")));
            this.pb_minimizar.Location = new System.Drawing.Point(890, 0);
            this.pb_minimizar.Name = "pb_minimizar";
            this.pb_minimizar.Size = new System.Drawing.Size(30, 26);
            this.pb_minimizar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_minimizar.TabIndex = 39;
            this.pb_minimizar.TabStop = false;
            this.pb_minimizar.UseWaitCursor = true;
            this.pb_minimizar.Click += new System.EventHandler(this.pb_minimizar_Click_1);
            // 
            // pb_maximizar
            // 
            this.pb_maximizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pb_maximizar.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pb_maximizar.Image = ((System.Drawing.Image)(resources.GetObject("pb_maximizar.Image")));
            this.pb_maximizar.Location = new System.Drawing.Point(916, 0);
            this.pb_maximizar.Name = "pb_maximizar";
            this.pb_maximizar.Size = new System.Drawing.Size(30, 26);
            this.pb_maximizar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_maximizar.TabIndex = 38;
            this.pb_maximizar.TabStop = false;
            this.pb_maximizar.UseWaitCursor = true;
            this.pb_maximizar.Click += new System.EventHandler(this.pb_maximizar_Click);
            // 
            // pb_salir
            // 
            this.pb_salir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pb_salir.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pb_salir.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.pb_salir.Image = ((System.Drawing.Image)(resources.GetObject("pb_salir.Image")));
            this.pb_salir.Location = new System.Drawing.Point(943, 0);
            this.pb_salir.Name = "pb_salir";
            this.pb_salir.Size = new System.Drawing.Size(30, 26);
            this.pb_salir.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_salir.TabIndex = 37;
            this.pb_salir.TabStop = false;
            this.pb_salir.UseWaitCursor = true;
            this.pb_salir.Click += new System.EventHandler(this.pb_salir_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.DarkGreen;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.Control;
            this.label2.Location = new System.Drawing.Point(86, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(272, 47);
            this.label2.TabIndex = 36;
            this.label2.Text = "SISTEMA SABA";
            this.label2.UseWaitCursor = true;
            // 
            // lblusuario
            // 
            this.lblusuario.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblusuario.AutoSize = true;
            this.lblusuario.BackColor = System.Drawing.Color.DarkGreen;
            this.lblusuario.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblusuario.ForeColor = System.Drawing.SystemColors.Control;
            this.lblusuario.Location = new System.Drawing.Point(803, 44);
            this.lblusuario.Name = "lblusuario";
            this.lblusuario.Size = new System.Drawing.Size(70, 17);
            this.lblusuario.TabIndex = 35;
            this.lblusuario.Text = "lblusuario";
            this.lblusuario.UseWaitCursor = true;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.DarkGreen;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(732, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 17);
            this.label1.TabIndex = 34;
            this.label1.Text = "Usuario:";
            this.label1.UseWaitCursor = true;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.DarkGreen;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(0, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(80, 67);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 33;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.UseWaitCursor = true;
            // 
            // menu_titulo
            // 
            this.menu_titulo.AutoSize = false;
            this.menu_titulo.BackColor = System.Drawing.Color.DarkGreen;
            this.menu_titulo.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menu_titulo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2});
            this.menu_titulo.Location = new System.Drawing.Point(0, 0);
            this.menu_titulo.Name = "menu_titulo";
            this.menu_titulo.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menu_titulo.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.menu_titulo.Size = new System.Drawing.Size(977, 80);
            this.menu_titulo.TabIndex = 32;
            this.menu_titulo.Text = "menuStrip3";
            this.menu_titulo.UseWaitCursor = true;
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(22, 76);
            this.toolStripMenuItem2.Text = " ";
            // 
            // contenedor
            // 
            this.contenedor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contenedor.Location = new System.Drawing.Point(0, 153);
            this.contenedor.Name = "contenedor";
            this.contenedor.Size = new System.Drawing.Size(977, 443);
            this.contenedor.TabIndex = 42;
            this.contenedor.UseWaitCursor = true;
            // 
            // menuusuarios
            // 
            this.menuusuarios.AutoSize = false;
            this.menuusuarios.IconChar = FontAwesome.Sharp.IconChar.UsersGear;
            this.menuusuarios.IconColor = System.Drawing.Color.Black;
            this.menuusuarios.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.menuusuarios.IconSize = 40;
            this.menuusuarios.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.menuusuarios.Name = "menuusuarios";
            this.menuusuarios.Size = new System.Drawing.Size(88, 69);
            this.menuusuarios.Text = "Usuarios";
            this.menuusuarios.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.menuusuarios.Click += new System.EventHandler(this.menuusuarios_Click);
            // 
            // menumantenedor
            // 
            this.menumantenedor.AutoSize = false;
            this.menumantenedor.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.submenumarca,
            this.submenuequipo,
            this.submenunegocio});
            this.menumantenedor.IconChar = FontAwesome.Sharp.IconChar.ScrewdriverWrench;
            this.menumantenedor.IconColor = System.Drawing.Color.Black;
            this.menumantenedor.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.menumantenedor.IconSize = 40;
            this.menumantenedor.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.menumantenedor.Name = "menumantenedor";
            this.menumantenedor.Size = new System.Drawing.Size(88, 69);
            this.menumantenedor.Text = "Mantenedor";
            this.menumantenedor.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // submenumarca
            // 
            this.submenumarca.IconChar = FontAwesome.Sharp.IconChar.None;
            this.submenumarca.IconColor = System.Drawing.Color.Black;
            this.submenumarca.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.submenumarca.Name = "submenumarca";
            this.submenumarca.Size = new System.Drawing.Size(119, 22);
            this.submenumarca.Text = "Marca";
            this.submenumarca.Click += new System.EventHandler(this.submenumarca_Click_1);
            // 
            // submenuequipo
            // 
            this.submenuequipo.IconChar = FontAwesome.Sharp.IconChar.None;
            this.submenuequipo.IconColor = System.Drawing.Color.Black;
            this.submenuequipo.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.submenuequipo.Name = "submenuequipo";
            this.submenuequipo.Size = new System.Drawing.Size(119, 22);
            this.submenuequipo.Text = "Equipo";
            this.submenuequipo.Click += new System.EventHandler(this.submenuequipo_Click_1);
            // 
            // submenunegocio
            // 
            this.submenunegocio.Name = "submenunegocio";
            this.submenunegocio.Size = new System.Drawing.Size(119, 22);
            this.submenunegocio.Text = "Negocio";
            this.submenunegocio.Click += new System.EventHandler(this.submenunegocio_Click_1);
            // 
            // menuacta
            // 
            this.menuacta.AutoSize = false;
            this.menuacta.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.submenuregistraracta,
            this.submenuverdetalleacta});
            this.menuacta.IconChar = FontAwesome.Sharp.IconChar.FileText;
            this.menuacta.IconColor = System.Drawing.Color.Black;
            this.menuacta.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.menuacta.IconSize = 40;
            this.menuacta.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.menuacta.Name = "menuacta";
            this.menuacta.Size = new System.Drawing.Size(88, 69);
            this.menuacta.Text = "Salida / Acta";
            this.menuacta.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // submenuregistraracta
            // 
            this.submenuregistraracta.Name = "submenuregistraracta";
            this.submenuregistraracta.Size = new System.Drawing.Size(180, 22);
            this.submenuregistraracta.Text = "Registrar";
            this.submenuregistraracta.Click += new System.EventHandler(this.submenuregistraracta_Click_1);
            // 
            // submenuverdetalleacta
            // 
            this.submenuverdetalleacta.Name = "submenuverdetalleacta";
            this.submenuverdetalleacta.Size = new System.Drawing.Size(180, 22);
            this.submenuverdetalleacta.Text = "Ver detalle";
            this.submenuverdetalleacta.Click += new System.EventHandler(this.submenuverdetalleacta_Click_1);
            // 
            // menuregistrar
            // 
            this.menuregistrar.AutoSize = false;
            this.menuregistrar.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.submenuregistrarcompra,
            this.submenuverdatallecompra});
            this.menuregistrar.IconChar = FontAwesome.Sharp.IconChar.DollyFlatbed;
            this.menuregistrar.IconColor = System.Drawing.Color.Black;
            this.menuregistrar.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.menuregistrar.IconSize = 40;
            this.menuregistrar.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.menuregistrar.Name = "menuregistrar";
            this.menuregistrar.Size = new System.Drawing.Size(88, 69);
            this.menuregistrar.Text = "Entrada";
            this.menuregistrar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // submenuregistrarcompra
            // 
            this.submenuregistrarcompra.IconChar = FontAwesome.Sharp.IconChar.None;
            this.submenuregistrarcompra.IconColor = System.Drawing.Color.Black;
            this.submenuregistrarcompra.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.submenuregistrarcompra.Name = "submenuregistrarcompra";
            this.submenuregistrarcompra.Size = new System.Drawing.Size(184, 26);
            this.submenuregistrarcompra.Text = "Registrar";
            this.submenuregistrarcompra.Click += new System.EventHandler(this.submenuregistrarcompra_Click_1);
            // 
            // submenuverdatallecompra
            // 
            this.submenuverdatallecompra.IconChar = FontAwesome.Sharp.IconChar.None;
            this.submenuverdatallecompra.IconColor = System.Drawing.Color.Black;
            this.submenuverdatallecompra.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.submenuverdatallecompra.Name = "submenuverdatallecompra";
            this.submenuverdatallecompra.Size = new System.Drawing.Size(184, 26);
            this.submenuverdatallecompra.Text = "Ver detalle";
            this.submenuverdatallecompra.Click += new System.EventHandler(this.submenuverdatallecompra_Click_1);
            // 
            // menufarmacia
            // 
            this.menufarmacia.AutoSize = false;
            this.menufarmacia.IconChar = FontAwesome.Sharp.IconChar.HospitalWide;
            this.menufarmacia.IconColor = System.Drawing.Color.Black;
            this.menufarmacia.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.menufarmacia.IconSize = 40;
            this.menufarmacia.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.menufarmacia.Name = "menufarmacia";
            this.menufarmacia.Size = new System.Drawing.Size(88, 69);
            this.menufarmacia.Text = "Farmacias";
            this.menufarmacia.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.menufarmacia.Click += new System.EventHandler(this.menufarmacia_Click);
            // 
            // menuautorizacion
            // 
            this.menuautorizacion.AutoSize = false;
            this.menuautorizacion.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.autorizacionActaToolStripMenuItem,
            this.autorizacionBajaToolStripMenuItem,
            this.equiposDeBajaToolStripMenuItem});
            this.menuautorizacion.IconChar = FontAwesome.Sharp.IconChar.FileSignature;
            this.menuautorizacion.IconColor = System.Drawing.Color.Black;
            this.menuautorizacion.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.menuautorizacion.IconSize = 40;
            this.menuautorizacion.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.menuautorizacion.Name = "menuautorizacion";
            this.menuautorizacion.Size = new System.Drawing.Size(88, 69);
            this.menuautorizacion.Text = "Autorizacion";
            this.menuautorizacion.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // autorizacionActaToolStripMenuItem
            // 
            this.autorizacionActaToolStripMenuItem.Name = "autorizacionActaToolStripMenuItem";
            this.autorizacionActaToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.autorizacionActaToolStripMenuItem.Text = "Autorizacion Acta";
            this.autorizacionActaToolStripMenuItem.Click += new System.EventHandler(this.autorizacionActaToolStripMenuItem_Click_1);
            // 
            // autorizacionBajaToolStripMenuItem
            // 
            this.autorizacionBajaToolStripMenuItem.Name = "autorizacionBajaToolStripMenuItem";
            this.autorizacionBajaToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.autorizacionBajaToolStripMenuItem.Text = "Autorizacion Baja";
            this.autorizacionBajaToolStripMenuItem.Click += new System.EventHandler(this.autorizacionBajaToolStripMenuItem_Click);
            // 
            // equiposDeBajaToolStripMenuItem
            // 
            this.equiposDeBajaToolStripMenuItem.Name = "equiposDeBajaToolStripMenuItem";
            this.equiposDeBajaToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.equiposDeBajaToolStripMenuItem.Text = "Equipos de Baja";
            this.equiposDeBajaToolStripMenuItem.Click += new System.EventHandler(this.equiposDeBajaToolStripMenuItem_Click_1);
            // 
            // menuasiganados
            // 
            this.menuasiganados.AutoSize = false;
            this.menuasiganados.IconChar = FontAwesome.Sharp.IconChar.BoxesStacked;
            this.menuasiganados.IconColor = System.Drawing.Color.Black;
            this.menuasiganados.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.menuasiganados.IconSize = 40;
            this.menuasiganados.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.menuasiganados.Name = "menuasiganados";
            this.menuasiganados.Size = new System.Drawing.Size(88, 69);
            this.menuasiganados.Text = "Asignados";
            this.menuasiganados.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.menuasiganados.Click += new System.EventHandler(this.menuasiganados_Click_1);
            // 
            // menureportes
            // 
            this.menureportes.AutoSize = false;
            this.menureportes.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.submenureporteacta,
            this.submenureporteregistrar});
            this.menureportes.IconChar = FontAwesome.Sharp.IconChar.ChartBar;
            this.menureportes.IconColor = System.Drawing.Color.Black;
            this.menureportes.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.menureportes.IconSize = 50;
            this.menureportes.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.menureportes.Name = "menureportes";
            this.menureportes.Size = new System.Drawing.Size(122, 69);
            this.menureportes.Text = "Reportes";
            this.menureportes.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // submenureporteacta
            // 
            this.submenureporteacta.Name = "submenureporteacta";
            this.submenureporteacta.Size = new System.Drawing.Size(177, 22);
            this.submenureporteacta.Text = "Reporte de Acta";
            this.submenureporteacta.Click += new System.EventHandler(this.submenureporteacta_Click);
            // 
            // submenureporteregistrar
            // 
            this.submenureporteregistrar.Name = "submenureporteregistrar";
            this.submenureporteregistrar.Size = new System.Drawing.Size(177, 22);
            this.submenureporteregistrar.Text = "Reporte de Registro";
            this.submenureporteregistrar.Click += new System.EventHandler(this.submenureporteregistrar_Click);
            // 
            // menuacercade
            // 
            this.menuacercade.AutoSize = false;
            this.menuacercade.IconChar = FontAwesome.Sharp.IconChar.CircleInfo;
            this.menuacercade.IconColor = System.Drawing.Color.Black;
            this.menuacercade.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.menuacercade.IconSize = 40;
            this.menuacercade.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.menuacercade.Name = "menuacercade";
            this.menuacercade.Size = new System.Drawing.Size(88, 69);
            this.menuacercade.Text = "Acerca de";
            this.menuacercade.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.menuacercade.Click += new System.EventHandler(this.menuacercade_Click);
            // 
            // menu
            // 
            this.menu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuusuarios,
            this.menumantenedor,
            this.menuregistrar,
            this.menuacta,
            this.menufarmacia,
            this.menuasiganados,
            this.menuautorizacion,
            this.menureportes,
            this.menuacercade});
            this.menu.Location = new System.Drawing.Point(0, 80);
            this.menu.Name = "menu";
            this.menu.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menu.Size = new System.Drawing.Size(977, 73);
            this.menu.TabIndex = 41;
            this.menu.Text = "menuStrip2";
            this.menu.UseWaitCursor = true;
            // 
            // Inicio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(977, 596);
            this.Controls.Add(this.contenedor);
            this.Controls.Add(this.menu);
            this.Controls.Add(this.pb_cerrarsecion);
            this.Controls.Add(this.pb_minimizar);
            this.Controls.Add(this.pb_maximizar);
            this.Controls.Add(this.pb_salir);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblusuario);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.menu_titulo);
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Name = "Inicio";
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "EQUIPHARMIX";
            this.UseWaitCursor = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MDIParent1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pb_cerrarsecion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_minimizar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_maximizar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_salir)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.menu_titulo.ResumeLayout(false);
            this.menu_titulo.PerformLayout();
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.PictureBox pb_cerrarsecion;
        private System.Windows.Forms.PictureBox pb_minimizar;
        private System.Windows.Forms.PictureBox pb_maximizar;
        private System.Windows.Forms.PictureBox pb_salir;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblusuario;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.MenuStrip menu_titulo;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.Panel contenedor;
        private FontAwesome.Sharp.IconMenuItem menuusuarios;
        private FontAwesome.Sharp.IconMenuItem menumantenedor;
        private FontAwesome.Sharp.IconMenuItem submenumarca;
        private FontAwesome.Sharp.IconMenuItem submenuequipo;
        private System.Windows.Forms.ToolStripMenuItem submenunegocio;
        private FontAwesome.Sharp.IconMenuItem menuacta;
        private System.Windows.Forms.ToolStripMenuItem submenuregistraracta;
        private System.Windows.Forms.ToolStripMenuItem submenuverdetalleacta;
        private FontAwesome.Sharp.IconMenuItem menuregistrar;
        private FontAwesome.Sharp.IconMenuItem submenuregistrarcompra;
        private FontAwesome.Sharp.IconMenuItem submenuverdatallecompra;
        private FontAwesome.Sharp.IconMenuItem menufarmacia;
        private FontAwesome.Sharp.IconMenuItem menuautorizacion;
        private FontAwesome.Sharp.IconMenuItem menuasiganados;
        private FontAwesome.Sharp.IconMenuItem menureportes;
        private System.Windows.Forms.ToolStripMenuItem submenureporteacta;
        private System.Windows.Forms.ToolStripMenuItem submenureporteregistrar;
        private FontAwesome.Sharp.IconMenuItem menuacercade;
        private System.Windows.Forms.MenuStrip menu;
        private System.Windows.Forms.ToolStripMenuItem autorizacionActaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autorizacionBajaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem equiposDeBajaToolStripMenuItem;
    }
}



