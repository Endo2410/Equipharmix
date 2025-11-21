namespace CapaPresentacion
{
    partial class frmpermiso
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
            System.Windows.Forms.TreeNode treeNode45 = new System.Windows.Forms.TreeNode("Guardar");
            System.Windows.Forms.TreeNode treeNode46 = new System.Windows.Forms.TreeNode("Limpiar");
            System.Windows.Forms.TreeNode treeNode47 = new System.Windows.Forms.TreeNode("Descargar");
            System.Windows.Forms.TreeNode treeNode48 = new System.Windows.Forms.TreeNode("Usuarios", new System.Windows.Forms.TreeNode[] {
            treeNode45,
            treeNode46,
            treeNode47});
            System.Windows.Forms.TreeNode treeNode49 = new System.Windows.Forms.TreeNode("Roles");
            System.Windows.Forms.TreeNode treeNode50 = new System.Windows.Forms.TreeNode("Permisos");
            System.Windows.Forms.TreeNode treeNode51 = new System.Windows.Forms.TreeNode("Seguridad", new System.Windows.Forms.TreeNode[] {
            treeNode48,
            treeNode49,
            treeNode50});
            System.Windows.Forms.TreeNode treeNode52 = new System.Windows.Forms.TreeNode("Guardar");
            System.Windows.Forms.TreeNode treeNode53 = new System.Windows.Forms.TreeNode("Limpiar");
            System.Windows.Forms.TreeNode treeNode54 = new System.Windows.Forms.TreeNode("Marca", new System.Windows.Forms.TreeNode[] {
            treeNode52,
            treeNode53});
            System.Windows.Forms.TreeNode treeNode55 = new System.Windows.Forms.TreeNode("Guardar");
            System.Windows.Forms.TreeNode treeNode56 = new System.Windows.Forms.TreeNode("Limpiar");
            System.Windows.Forms.TreeNode treeNode57 = new System.Windows.Forms.TreeNode("Descargar");
            System.Windows.Forms.TreeNode treeNode58 = new System.Windows.Forms.TreeNode("Equipo", new System.Windows.Forms.TreeNode[] {
            treeNode55,
            treeNode56,
            treeNode57});
            System.Windows.Forms.TreeNode treeNode59 = new System.Windows.Forms.TreeNode("Mantenedor", new System.Windows.Forms.TreeNode[] {
            treeNode54,
            treeNode58});
            System.Windows.Forms.TreeNode treeNode60 = new System.Windows.Forms.TreeNode("Entrada");
            System.Windows.Forms.TreeNode treeNode61 = new System.Windows.Forms.TreeNode("Salida / Acta");
            System.Windows.Forms.TreeNode treeNode62 = new System.Windows.Forms.TreeNode("Farmacia");
            System.Windows.Forms.TreeNode treeNode63 = new System.Windows.Forms.TreeNode("Autorizacion");
            System.Windows.Forms.TreeNode treeNode64 = new System.Windows.Forms.TreeNode("Equipos asignados");
            System.Windows.Forms.TreeNode treeNode65 = new System.Windows.Forms.TreeNode("Reportes");
            System.Windows.Forms.TreeNode treeNode66 = new System.Windows.Forms.TreeNode("Acerca de");
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnguardar = new FontAwesome.Sharp.IconButton();
            this.cboRoles = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tvPermisos = new System.Windows.Forms.TreeView();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(268, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(201, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "GESTION DE PERMISOS ";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.btnguardar);
            this.panel1.Controls.Add(this.cboRoles);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.tvPermisos);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(44, 27);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(660, 738);
            this.panel1.TabIndex = 1;
            // 
            // btnguardar
            // 
            this.btnguardar.BackColor = System.Drawing.Color.ForestGreen;
            this.btnguardar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnguardar.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnguardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnguardar.ForeColor = System.Drawing.Color.White;
            this.btnguardar.IconChar = FontAwesome.Sharp.IconChar.FloppyDisk;
            this.btnguardar.IconColor = System.Drawing.Color.White;
            this.btnguardar.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnguardar.IconSize = 16;
            this.btnguardar.Location = new System.Drawing.Point(288, 675);
            this.btnguardar.Name = "btnguardar";
            this.btnguardar.Size = new System.Drawing.Size(192, 40);
            this.btnguardar.TabIndex = 80;
            this.btnguardar.Text = "Guardar";
            this.btnguardar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnguardar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnguardar.UseVisualStyleBackColor = false;
            this.btnguardar.Click += new System.EventHandler(this.btnguardar_Click);
            // 
            // cboRoles
            // 
            this.cboRoles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRoles.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboRoles.FormattingEnabled = true;
            this.cboRoles.Location = new System.Drawing.Point(99, 42);
            this.cboRoles.Name = "cboRoles";
            this.cboRoles.Size = new System.Drawing.Size(179, 28);
            this.cboRoles.TabIndex = 3;
            this.cboRoles.SelectedIndexChanged += new System.EventHandler(this.cboRoles_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(56, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "Permisos:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(56, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Rol:";
            // 
            // tvPermisos
            // 
            this.tvPermisos.CheckBoxes = true;
            this.tvPermisos.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tvPermisos.Location = new System.Drawing.Point(59, 104);
            this.tvPermisos.Name = "tvPermisos";
            treeNode45.Name = "btnguardar";
            treeNode45.Text = "Guardar";
            treeNode46.Name = "btnlimpiar";
            treeNode46.Text = "Limpiar";
            treeNode47.Name = "btndescargar";
            treeNode47.Text = "Descargar";
            treeNode48.Name = "submenuusuario";
            treeNode48.Text = "Usuarios";
            treeNode49.Name = "submenuroles";
            treeNode49.Text = "Roles";
            treeNode50.Name = "submenupermisos";
            treeNode50.Text = "Permisos";
            treeNode51.Name = "menuseguridad";
            treeNode51.Text = "Seguridad";
            treeNode52.Name = "btnguardar";
            treeNode52.Text = "Guardar";
            treeNode53.Name = "btnLimpiar";
            treeNode53.Text = "Limpiar";
            treeNode54.Name = "submenumarca";
            treeNode54.Text = "Marca";
            treeNode55.Name = "btnguardar";
            treeNode55.Text = "Guardar";
            treeNode56.Name = "btnlimpiar";
            treeNode56.Text = "Limpiar";
            treeNode57.Name = "btnDescargar";
            treeNode57.Text = "Descargar";
            treeNode58.Name = "submenuequipo";
            treeNode58.Text = "Equipo";
            treeNode59.Name = "menumantenedor";
            treeNode59.Text = "Mantenedor";
            treeNode60.Name = "menuregistrar";
            treeNode60.Text = "Entrada";
            treeNode61.Name = "menuacta";
            treeNode61.Text = "Salida / Acta";
            treeNode62.Name = "menufarmacia";
            treeNode62.Text = "Farmacia";
            treeNode63.Name = "menuautorizacion";
            treeNode63.Text = "Autorizacion";
            treeNode64.Name = "menuasiganados";
            treeNode64.Text = "Equipos asignados";
            treeNode65.Name = "menureportes";
            treeNode65.Text = "Reportes";
            treeNode66.Name = "menuacercade";
            treeNode66.Text = "Acerca de";
            this.tvPermisos.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode51,
            treeNode59,
            treeNode60,
            treeNode61,
            treeNode62,
            treeNode63,
            treeNode64,
            treeNode65,
            treeNode66});
            this.tvPermisos.Size = new System.Drawing.Size(421, 552);
            this.tvPermisos.TabIndex = 1;
            this.tvPermisos.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvPermisos_AfterCheck);
            // 
            // frmpermiso
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(929, 798);
            this.Controls.Add(this.panel1);
            this.Name = "frmpermiso";
            this.Text = "frmpermiso";
            this.Load += new System.EventHandler(this.frmpermiso_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TreeView tvPermisos;
        private System.Windows.Forms.ComboBox cboRoles;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private FontAwesome.Sharp.IconButton btnguardar;
    }
}