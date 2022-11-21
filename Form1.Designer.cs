namespace EnsambladorSicXE
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.nuevoToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.abrirToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.guardarToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.GuardarcomoToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.LblProgramSize = new System.Windows.Forms.Label();
            this.dgridArchivo = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.editorCodigo = new FastColoredTextBoxNS.FastColoredTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtBoxErrores = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tBoxObjFile = new System.Windows.Forms.TextBox();
            this.panelTabSim = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ObjFileGrid = new System.Windows.Forms.DataGridView();
            this.addObjFile = new System.Windows.Forms.Button();
            this.cargarButton = new System.Windows.Forms.Button();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgridArchivo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editorCodigo)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ObjFileGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.SystemColors.MenuBar;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nuevoToolStripButton,
            this.abrirToolStripButton,
            this.guardarToolStripButton,
            this.GuardarcomoToolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1342, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // nuevoToolStripButton
            // 
            this.nuevoToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.nuevoToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("nuevoToolStripButton.Image")));
            this.nuevoToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.nuevoToolStripButton.Name = "nuevoToolStripButton";
            this.nuevoToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.nuevoToolStripButton.Text = "&Nuevo";
            this.nuevoToolStripButton.Click += new System.EventHandler(this.nuevoToolStripButton_Click);
            // 
            // abrirToolStripButton
            // 
            this.abrirToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.abrirToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("abrirToolStripButton.Image")));
            this.abrirToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.abrirToolStripButton.Name = "abrirToolStripButton";
            this.abrirToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.abrirToolStripButton.Text = "&Abrir";
            this.abrirToolStripButton.Click += new System.EventHandler(this.abrirToolStripButton_Click);
            // 
            // guardarToolStripButton
            // 
            this.guardarToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.guardarToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("guardarToolStripButton.Image")));
            this.guardarToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.guardarToolStripButton.Name = "guardarToolStripButton";
            this.guardarToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.guardarToolStripButton.Text = "&Guardar";
            this.guardarToolStripButton.Click += new System.EventHandler(this.guardarToolStripButton_Click);
            // 
            // GuardarcomoToolStripButton
            // 
            this.GuardarcomoToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.GuardarcomoToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("GuardarcomoToolStripButton.Image")));
            this.GuardarcomoToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.GuardarcomoToolStripButton.Name = "GuardarcomoToolStripButton";
            this.GuardarcomoToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.GuardarcomoToolStripButton.Text = "&Guardar como";
            this.GuardarcomoToolStripButton.Click += new System.EventHandler(this.GuardarcomoToolStripButton_Click);
            // 
            // LblProgramSize
            // 
            this.LblProgramSize.AutoSize = true;
            this.LblProgramSize.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblProgramSize.Location = new System.Drawing.Point(1127, 643);
            this.LblProgramSize.Name = "LblProgramSize";
            this.LblProgramSize.Size = new System.Drawing.Size(22, 16);
            this.LblProgramSize.TabIndex = 18;
            this.LblProgramSize.Text = "0H";
            this.LblProgramSize.Click += new System.EventHandler(this.LblProgramSize_Click);
            // 
            // dgridArchivo
            // 
            this.dgridArchivo.AllowUserToAddRows = false;
            this.dgridArchivo.AllowUserToDeleteRows = false;
            this.dgridArchivo.AllowUserToResizeColumns = false;
            this.dgridArchivo.AllowUserToResizeRows = false;
            this.dgridArchivo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgridArchivo.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgridArchivo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgridArchivo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgridArchivo.Location = new System.Drawing.Point(361, 53);
            this.dgridArchivo.Name = "dgridArchivo";
            this.dgridArchivo.RowHeadersVisible = false;
            this.dgridArchivo.RowHeadersWidth = 20;
            this.dgridArchivo.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Teal;
            this.dgridArchivo.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgridArchivo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgridArchivo.Size = new System.Drawing.Size(634, 434);
            this.dgridArchivo.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(358, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 16);
            this.label1.TabIndex = 11;
            this.label1.Text = "Archivo Intermedio";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 16);
            this.label2.TabIndex = 12;
            this.label2.Text = "Programa fuente";
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(259, 489);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(94, 23);
            this.button2.TabIndex = 13;
            this.button2.Text = "Ensamblar";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // editorCodigo
            // 
            this.editorCodigo.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.editorCodigo.AutoScrollMinSize = new System.Drawing.Size(27, 14);
            this.editorCodigo.BackBrush = null;
            this.editorCodigo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.editorCodigo.CharHeight = 14;
            this.editorCodigo.CharWidth = 8;
            this.editorCodigo.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.editorCodigo.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.editorCodigo.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.editorCodigo.ForeColor = System.Drawing.SystemColors.ControlText;
            this.editorCodigo.IsReplaceMode = false;
            this.editorCodigo.Location = new System.Drawing.Point(6, 53);
            this.editorCodigo.Name = "editorCodigo";
            this.editorCodigo.Paddings = new System.Windows.Forms.Padding(0);
            this.editorCodigo.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.editorCodigo.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("editorCodigo.ServiceColors")));
            this.editorCodigo.Size = new System.Drawing.Size(347, 434);
            this.editorCodigo.TabIndex = 14;
            this.editorCodigo.Zoom = 100;
            this.editorCodigo.TextChanged += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.editorCodigo_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(1003, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(164, 16);
            this.label3.TabIndex = 16;
            this.label3.Text = "Tablas de símbolos y bloques";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(1008, 643);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(125, 16);
            this.label4.TabIndex = 17;
            this.label4.Text = "Tamaño del programa:";
            // 
            // txtBoxErrores
            // 
            this.txtBoxErrores.Enabled = false;
            this.txtBoxErrores.Location = new System.Drawing.Point(6, 525);
            this.txtBoxErrores.Multiline = true;
            this.txtBoxErrores.Name = "txtBoxErrores";
            this.txtBoxErrores.Size = new System.Drawing.Size(347, 131);
            this.txtBoxErrores.TabIndex = 19;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(3, 499);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(131, 16);
            this.label5.TabIndex = 20;
            this.label5.Text = "Errores en el programa:";
            // 
            // tBoxObjFile
            // 
            this.tBoxObjFile.Location = new System.Drawing.Point(359, 525);
            this.tBoxObjFile.Multiline = true;
            this.tBoxObjFile.Name = "tBoxObjFile";
            this.tBoxObjFile.Size = new System.Drawing.Size(634, 157);
            this.tBoxObjFile.TabIndex = 21;
            // 
            // panelTabSim
            // 
            this.panelTabSim.AutoScroll = true;
            this.panelTabSim.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelTabSim.Location = new System.Drawing.Point(1006, 53);
            this.panelTabSim.Name = "panelTabSim";
            this.panelTabSim.Size = new System.Drawing.Size(330, 564);
            this.panelTabSim.TabIndex = 23;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1350, 712);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.toolStrip1);
            this.tabPage1.Controls.Add(this.tBoxObjFile);
            this.tabPage1.Controls.Add(this.panelTabSim);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.txtBoxErrores);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.editorCodigo);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.button2);
            this.tabPage1.Controls.Add(this.dgridArchivo);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(1342, 686);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Compilador";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(1342, 686);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "Cargador";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cargarButton);
            this.groupBox1.Controls.Add(this.ObjFileGrid);
            this.groupBox1.Controls.Add(this.addObjFile);
            this.groupBox1.Location = new System.Drawing.Point(3, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(410, 361);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Seleccionar archivos";
            // 
            // ObjFileGrid
            // 
            this.ObjFileGrid.AllowUserToAddRows = false;
            this.ObjFileGrid.AllowUserToDeleteRows = false;
            this.ObjFileGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ObjFileGrid.Location = new System.Drawing.Point(6, 85);
            this.ObjFileGrid.Name = "ObjFileGrid";
            this.ObjFileGrid.ReadOnly = true;
            this.ObjFileGrid.Size = new System.Drawing.Size(384, 190);
            this.ObjFileGrid.TabIndex = 2;
            this.ObjFileGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ObjFileGrid_CellClick);
            // 
            // addObjFile
            // 
            this.addObjFile.Location = new System.Drawing.Point(5, 45);
            this.addObjFile.Name = "addObjFile";
            this.addObjFile.Size = new System.Drawing.Size(174, 23);
            this.addObjFile.TabIndex = 1;
            this.addObjFile.Text = "Agregar archivo";
            this.addObjFile.UseVisualStyleBackColor = true;
            this.addObjFile.Click += new System.EventHandler(this.button1_Click);
            // 
            // cargarButton
            // 
            this.cargarButton.Location = new System.Drawing.Point(6, 295);
            this.cargarButton.Name = "cargarButton";
            this.cargarButton.Size = new System.Drawing.Size(75, 23);
            this.cargarButton.TabIndex = 3;
            this.cargarButton.Text = "Cargar";
            this.cargarButton.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(1350, 714);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.LblProgramSize);
            this.Controls.Add(this.label4);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ensamblador";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgridArchivo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editorCodigo)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ObjFileGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton nuevoToolStripButton;
        private System.Windows.Forms.ToolStripButton abrirToolStripButton;
        private System.Windows.Forms.ToolStripButton guardarToolStripButton;
        private System.Windows.Forms.ToolStripButton GuardarcomoToolStripButton;
        private System.Windows.Forms.Label LblProgramSize;
        public System.Windows.Forms.DataGridView dgridArchivo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button2;
        private FastColoredTextBoxNS.FastColoredTextBox editorCodigo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtBoxErrores;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tBoxObjFile;
        private System.Windows.Forms.Panel panelTabSim;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button addObjFile;
        private System.Windows.Forms.DataGridView ObjFileGrid;
        private System.Windows.Forms.Button cargarButton;
    }
}

