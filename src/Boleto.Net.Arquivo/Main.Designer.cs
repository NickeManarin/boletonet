namespace BoletoNet.Arquivo
{
    partial class Main
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
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.lstReturnFields = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.arquivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cNABToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.retornoToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.lerToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.gerarToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.impressãoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBoxBancos = new System.Windows.Forms.GroupBox();
            this.RbSicoob = new System.Windows.Forms.RadioButton();
            this.RbBNB = new System.Windows.Forms.RadioButton();
            this.RbBanrisul = new System.Windows.Forms.RadioButton();
            this.RbSicredi = new System.Windows.Forms.RadioButton();
            this.RbCaixa = new System.Windows.Forms.RadioButton();
            this.RbSantander = new System.Windows.Forms.RadioButton();
            this.RbUnibanco = new System.Windows.Forms.RadioButton();
            this.RbSudameris = new System.Windows.Forms.RadioButton();
            this.RbBancoBrasil = new System.Windows.Forms.RadioButton();
            this.RbReal = new System.Windows.Forms.RadioButton();
            this.RbBradesco = new System.Windows.Forms.RadioButton();
            this.RbHsbc = new System.Windows.Forms.RadioButton();
            this.RbSafra = new System.Windows.Forms.RadioButton();
            this.RbItau = new System.Windows.Forms.RadioButton();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonCNAB240 = new System.Windows.Forms.RadioButton();
            this.radioButtonCNAB400 = new System.Windows.Forms.RadioButton();
            this.menuStrip.SuspendLayout();
            this.groupBoxBancos.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "Arquivos de Retorno (*.ret)|*.ret|Todos Arquivos (*.*)|*.*";
            // 
            // lstReturnFields
            // 
            this.lstReturnFields.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstReturnFields.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader9});
            this.lstReturnFields.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstReturnFields.FullRowSelect = true;
            this.lstReturnFields.GridLines = true;
            this.lstReturnFields.HideSelection = false;
            this.lstReturnFields.Location = new System.Drawing.Point(165, 32);
            this.lstReturnFields.MultiSelect = false;
            this.lstReturnFields.Name = "lstReturnFields";
            this.lstReturnFields.Size = new System.Drawing.Size(834, 402);
            this.lstReturnFields.TabIndex = 2;
            this.lstReturnFields.UseCompatibleStateImageBehavior = false;
            this.lstReturnFields.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Sacado";
            this.columnHeader1.Width = 150;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Data de Vencimento";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader2.Width = 110;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Data do Crédito";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader3.Width = 110;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Valor do Título";
            this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader4.Width = 85;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Valor Pago";
            this.columnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader5.Width = 85;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Cód. de Retorno";
            this.columnHeader6.Width = 100;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Situação";
            this.columnHeader7.Width = 100;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Nosso Número";
            this.columnHeader8.Width = 80;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "Numero Documento";
            this.columnHeader9.Width = 100;
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            this.openFileDialog.Filter = "Arquivos de Retorno (*.ret)|*.ret|Todos Arquivos (*.*)|*.*";
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.arquivoToolStripMenuItem,
            this.impressãoToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1011, 24);
            this.menuStrip.TabIndex = 4;
            this.menuStrip.Text = "menuStrip1";
            // 
            // arquivoToolStripMenuItem
            // 
            this.arquivoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cNABToolStripMenuItem,
            this.retornoToolStripMenuItem1});
            this.arquivoToolStripMenuItem.Name = "arquivoToolStripMenuItem";
            this.arquivoToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.arquivoToolStripMenuItem.Text = "&Arquivo";
            // 
            // cNABToolStripMenuItem
            // 
            this.cNABToolStripMenuItem.Name = "cNABToolStripMenuItem";
            this.cNABToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.cNABToolStripMenuItem.Text = "&Remessa";
            this.cNABToolStripMenuItem.Click += new System.EventHandler(this.RemessaMenuItem_Click);
            // 
            // retornoToolStripMenuItem1
            // 
            this.retornoToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lerToolStripMenuItem1,
            this.gerarToolStripMenuItem1});
            this.retornoToolStripMenuItem1.Name = "retornoToolStripMenuItem1";
            this.retornoToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.retornoToolStripMenuItem1.Text = "R&etorno";
            // 
            // lerToolStripMenuItem1
            // 
            this.lerToolStripMenuItem1.Name = "lerToolStripMenuItem1";
            this.lerToolStripMenuItem1.Size = new System.Drawing.Size(102, 22);
            this.lerToolStripMenuItem1.Text = "&Ler";
            this.lerToolStripMenuItem1.Click += new System.EventHandler(this.RetornoMenuItem_Click);
            // 
            // gerarToolStripMenuItem1
            // 
            this.gerarToolStripMenuItem1.Name = "gerarToolStripMenuItem1";
            this.gerarToolStripMenuItem1.Size = new System.Drawing.Size(102, 22);
            this.gerarToolStripMenuItem1.Text = "&Gerar";
            this.gerarToolStripMenuItem1.Click += new System.EventHandler(this.GeraRetornoMenuItem_Click);
            // 
            // impressãoToolStripMenuItem
            // 
            this.impressãoToolStripMenuItem.Name = "impressãoToolStripMenuItem";
            this.impressãoToolStripMenuItem.Size = new System.Drawing.Size(73, 20);
            this.impressãoToolStripMenuItem.Text = "&Impressão";
            this.impressãoToolStripMenuItem.Click += new System.EventHandler(this.ImpressãoMenuItem_Click);
            // 
            // groupBoxBancos
            // 
            this.groupBoxBancos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxBancos.Controls.Add(this.RbSicoob);
            this.groupBoxBancos.Controls.Add(this.RbBNB);
            this.groupBoxBancos.Controls.Add(this.RbBanrisul);
            this.groupBoxBancos.Controls.Add(this.RbSicredi);
            this.groupBoxBancos.Controls.Add(this.RbCaixa);
            this.groupBoxBancos.Controls.Add(this.RbSantander);
            this.groupBoxBancos.Controls.Add(this.RbUnibanco);
            this.groupBoxBancos.Controls.Add(this.RbSudameris);
            this.groupBoxBancos.Controls.Add(this.RbBancoBrasil);
            this.groupBoxBancos.Controls.Add(this.RbReal);
            this.groupBoxBancos.Controls.Add(this.RbBradesco);
            this.groupBoxBancos.Controls.Add(this.RbHsbc);
            this.groupBoxBancos.Controls.Add(this.RbSafra);
            this.groupBoxBancos.Controls.Add(this.RbItau);
            this.groupBoxBancos.Location = new System.Drawing.Point(12, 83);
            this.groupBoxBancos.Name = "groupBoxBancos";
            this.groupBoxBancos.Padding = new System.Windows.Forms.Padding(10, 5, 10, 10);
            this.groupBoxBancos.Size = new System.Drawing.Size(147, 351);
            this.groupBoxBancos.TabIndex = 5;
            this.groupBoxBancos.TabStop = false;
            this.groupBoxBancos.Text = "Bancos";
            // 
            // RbSicoob
            // 
            this.RbSicoob.AutoSize = true;
            this.RbSicoob.Location = new System.Drawing.Point(13, 275);
            this.RbSicoob.Name = "RbSicoob";
            this.RbSicoob.Size = new System.Drawing.Size(58, 17);
            this.RbSicoob.TabIndex = 32;
            this.RbSicoob.Tag = "756";
            this.RbSicoob.Text = "Sicoob";
            this.RbSicoob.UseVisualStyleBackColor = true;
            // 
            // radioButtonBNB
            // 
            this.RbBNB.AutoSize = true;
            this.RbBNB.Location = new System.Drawing.Point(13, 321);
            this.RbBNB.Name = "RbBNB";
            this.RbBNB.Size = new System.Drawing.Size(68, 17);
            this.RbBNB.TabIndex = 31;
            this.RbBNB.Tag = "4";
            this.RbBNB.Text = "Nordeste";
            this.RbBNB.UseVisualStyleBackColor = true;
            // 
            // radioButtonBanrisul
            // 
            this.RbBanrisul.AutoSize = true;
            this.RbBanrisul.Location = new System.Drawing.Point(13, 298);
            this.RbBanrisul.Name = "RbBanrisul";
            this.RbBanrisul.Size = new System.Drawing.Size(62, 17);
            this.RbBanrisul.TabIndex = 30;
            this.RbBanrisul.Tag = "041";
            this.RbBanrisul.Text = "Banrisul";
            this.RbBanrisul.UseVisualStyleBackColor = true;
            // 
            // radioButtonSicredi
            // 
            this.RbSicredi.AutoSize = true;
            this.RbSicredi.Location = new System.Drawing.Point(13, 251);
            this.RbSicredi.Name = "RbSicredi";
            this.RbSicredi.Size = new System.Drawing.Size(57, 17);
            this.RbSicredi.TabIndex = 29;
            this.RbSicredi.Tag = "748";
            this.RbSicredi.Text = "Sicredi";
            this.RbSicredi.UseVisualStyleBackColor = true;
            // 
            // radioButtonCaixa
            // 
            this.RbCaixa.AutoSize = true;
            this.RbCaixa.Location = new System.Drawing.Point(13, 228);
            this.RbCaixa.Name = "RbCaixa";
            this.RbCaixa.Size = new System.Drawing.Size(51, 17);
            this.RbCaixa.TabIndex = 28;
            this.RbCaixa.Tag = "104";
            this.RbCaixa.Text = "Caixa";
            this.RbCaixa.UseVisualStyleBackColor = true;
            // 
            // radioButtonSantander
            // 
            this.RbSantander.AutoSize = true;
            this.RbSantander.Location = new System.Drawing.Point(13, 205);
            this.RbSantander.Name = "RbSantander";
            this.RbSantander.Size = new System.Drawing.Size(74, 17);
            this.RbSantander.TabIndex = 27;
            this.RbSantander.Tag = "409";
            this.RbSantander.Text = "Santander";
            this.RbSantander.UseVisualStyleBackColor = true;
            // 
            // radioButtonUnibanco
            // 
            this.RbUnibanco.AutoSize = true;
            this.RbUnibanco.Location = new System.Drawing.Point(13, 182);
            this.RbUnibanco.Name = "RbUnibanco";
            this.RbUnibanco.Size = new System.Drawing.Size(71, 17);
            this.RbUnibanco.TabIndex = 21;
            this.RbUnibanco.Tag = "409";
            this.RbUnibanco.Text = "Unibanco";
            this.RbUnibanco.UseVisualStyleBackColor = true;
            // 
            // radioButtonSudameris
            // 
            this.RbSudameris.AutoSize = true;
            this.RbSudameris.Location = new System.Drawing.Point(13, 159);
            this.RbSudameris.Name = "RbSudameris";
            this.RbSudameris.Size = new System.Drawing.Size(74, 17);
            this.RbSudameris.TabIndex = 22;
            this.RbSudameris.Tag = "347";
            this.RbSudameris.Text = "Sudameris";
            this.RbSudameris.UseVisualStyleBackColor = true;
            // 
            // radioButtonBancoBrasil
            // 
            this.RbBancoBrasil.AutoSize = true;
            this.RbBancoBrasil.Location = new System.Drawing.Point(13, 44);
            this.RbBancoBrasil.Name = "RbBancoBrasil";
            this.RbBancoBrasil.Size = new System.Drawing.Size(99, 17);
            this.RbBancoBrasil.TabIndex = 25;
            this.RbBancoBrasil.Tag = "1";
            this.RbBancoBrasil.Text = "Banco do Brasil";
            this.RbBancoBrasil.UseVisualStyleBackColor = true;
            // 
            // radioButtonReal
            // 
            this.RbReal.AutoSize = true;
            this.RbReal.Location = new System.Drawing.Point(13, 113);
            this.RbReal.Name = "RbReal";
            this.RbReal.Size = new System.Drawing.Size(47, 17);
            this.RbReal.TabIndex = 23;
            this.RbReal.Tag = "356";
            this.RbReal.Text = "Real";
            this.RbReal.UseVisualStyleBackColor = true;
            // 
            // radioButtonBradesco
            // 
            this.RbBradesco.AutoSize = true;
            this.RbBradesco.Location = new System.Drawing.Point(13, 67);
            this.RbBradesco.Name = "RbBradesco";
            this.RbBradesco.Size = new System.Drawing.Size(70, 17);
            this.RbBradesco.TabIndex = 26;
            this.RbBradesco.Tag = "237";
            this.RbBradesco.Text = "Bradesco";
            this.RbBradesco.UseVisualStyleBackColor = true;
            // 
            // radioButtonHsbc
            // 
            this.RbHsbc.AutoSize = true;
            this.RbHsbc.Location = new System.Drawing.Point(13, 90);
            this.RbHsbc.Name = "RbHsbc";
            this.RbHsbc.Size = new System.Drawing.Size(54, 17);
            this.RbHsbc.TabIndex = 24;
            this.RbHsbc.Tag = "399";
            this.RbHsbc.Text = "HSBC";
            this.RbHsbc.UseVisualStyleBackColor = true;
            // 
            // radioButtonSafra
            // 
            this.RbSafra.AutoSize = true;
            this.RbSafra.Location = new System.Drawing.Point(13, 136);
            this.RbSafra.Name = "RbSafra";
            this.RbSafra.Size = new System.Drawing.Size(50, 17);
            this.RbSafra.TabIndex = 20;
            this.RbSafra.Tag = "422";
            this.RbSafra.Text = "Safra";
            this.RbSafra.UseVisualStyleBackColor = true;
            // 
            // radioButtonItau
            // 
            this.RbItau.AutoSize = true;
            this.RbItau.Checked = true;
            this.RbItau.Location = new System.Drawing.Point(13, 21);
            this.RbItau.Name = "RbItau";
            this.RbItau.Size = new System.Drawing.Size(43, 17);
            this.RbItau.TabIndex = 0;
            this.RbItau.TabStop = true;
            this.RbItau.Tag = "341";
            this.RbItau.Text = "Itaú";
            this.RbItau.UseVisualStyleBackColor = true;
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.WorkerReportsProgress = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonCNAB240);
            this.groupBox1.Controls.Add(this.radioButtonCNAB400);
            this.groupBox1.Location = new System.Drawing.Point(12, 32);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(147, 45);
            this.groupBox1.TabIndex = 28;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Padrão";
            // 
            // radioButtonCNAB240
            // 
            this.radioButtonCNAB240.AutoSize = true;
            this.radioButtonCNAB240.Location = new System.Drawing.Point(94, 19);
            this.radioButtonCNAB240.Name = "radioButtonCNAB240";
            this.radioButtonCNAB240.Size = new System.Drawing.Size(43, 17);
            this.radioButtonCNAB240.TabIndex = 29;
            this.radioButtonCNAB240.TabStop = true;
            this.radioButtonCNAB240.Tag = "409";
            this.radioButtonCNAB240.Text = "240";
            this.radioButtonCNAB240.UseVisualStyleBackColor = true;
            // 
            // radioButtonCNAB400
            // 
            this.radioButtonCNAB400.AutoSize = true;
            this.radioButtonCNAB400.Checked = true;
            this.radioButtonCNAB400.Location = new System.Drawing.Point(13, 19);
            this.radioButtonCNAB400.Name = "radioButtonCNAB400";
            this.radioButtonCNAB400.Size = new System.Drawing.Size(75, 17);
            this.radioButtonCNAB400.TabIndex = 28;
            this.radioButtonCNAB400.TabStop = true;
            this.radioButtonCNAB400.Tag = "409";
            this.radioButtonCNAB400.Text = "CNAB 400";
            this.radioButtonCNAB400.UseVisualStyleBackColor = true;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1011, 446);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBoxBancos);
            this.Controls.Add(this.lstReturnFields);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Geração do Arquivo Remessa";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.groupBoxBancos.ResumeLayout(false);
            this.groupBoxBancos.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        protected System.Windows.Forms.ListView lstReturnFields;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem arquivoToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBoxBancos;
        private System.Windows.Forms.RadioButton RbItau;
        private System.Windows.Forms.ToolStripMenuItem impressãoToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.RadioButton RbUnibanco;
        private System.Windows.Forms.RadioButton RbSudameris;
        private System.Windows.Forms.RadioButton RbBancoBrasil;
        private System.Windows.Forms.RadioButton RbReal;
        private System.Windows.Forms.RadioButton RbBradesco;
        private System.Windows.Forms.RadioButton RbHsbc;
        private System.Windows.Forms.RadioButton RbSafra;
        private System.Windows.Forms.RadioButton RbSantander;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonCNAB240;
        private System.Windows.Forms.RadioButton radioButtonCNAB400;
        private System.Windows.Forms.ToolStripMenuItem cNABToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem retornoToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem lerToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem gerarToolStripMenuItem1;
        private System.Windows.Forms.RadioButton RbCaixa;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.RadioButton RbSicredi;
        private System.Windows.Forms.RadioButton RbBanrisul;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.RadioButton RbBNB;
        private System.Windows.Forms.RadioButton RbSicoob;
    }
}

