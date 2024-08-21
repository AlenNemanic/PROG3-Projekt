namespace Projekt
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private System.Windows.Forms.Button PremiPregledGumb;
        private System.Windows.Forms.Button VmesniPregledGumb;
        private System.Windows.Forms.Button ObratniPregledGumb;

        private void InitializeComponent()
        {
            this.PremiPregledGumb = new System.Windows.Forms.Button();
            this.VmesniPregledGumb = new System.Windows.Forms.Button();
            this.ObratniPregledGumb = new System.Windows.Forms.Button();
            this.IzvoziGumb = new System.Windows.Forms.Button();
            this.UvoziGumb = new System.Windows.Forms.Button();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.radioGumbDodaj = new System.Windows.Forms.RadioButton();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.radioGumbOdstrani = new System.Windows.Forms.RadioButton();
            this.textBoxDodaj = new System.Windows.Forms.TextBox();
            this.RazveljaviGumb = new System.Windows.Forms.Button();
            this.ObnoviGumb = new System.Windows.Forms.Button();
            this.textBoxPoisci = new System.Windows.Forms.TextBox();
            this.GumbPoisci = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // PremiPregledGumb
            // 
            this.PremiPregledGumb.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.PremiPregledGumb.Location = new System.Drawing.Point(15, 68);
            this.PremiPregledGumb.Name = "PremiPregledGumb";
            this.PremiPregledGumb.Size = new System.Drawing.Size(103, 30);
            this.PremiPregledGumb.TabIndex = 0;
            this.PremiPregledGumb.Text = "Premi pregled";
            this.PremiPregledGumb.Click += new System.EventHandler(this.PremiPregledGumb_Click);
            // 
            // VmesniPregledGumb
            // 
            this.VmesniPregledGumb.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.VmesniPregledGumb.Location = new System.Drawing.Point(121, 68);
            this.VmesniPregledGumb.Name = "VmesniPregledGumb";
            this.VmesniPregledGumb.Size = new System.Drawing.Size(103, 30);
            this.VmesniPregledGumb.TabIndex = 1;
            this.VmesniPregledGumb.Text = "Vmesni pregled";
            this.VmesniPregledGumb.Click += new System.EventHandler(this.VmesniPregledGumb_Click);
            // 
            // ObratniPregledGumb
            // 
            this.ObratniPregledGumb.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ObratniPregledGumb.Location = new System.Drawing.Point(227, 68);
            this.ObratniPregledGumb.Name = "ObratniPregledGumb";
            this.ObratniPregledGumb.Size = new System.Drawing.Size(103, 30);
            this.ObratniPregledGumb.TabIndex = 2;
            this.ObratniPregledGumb.Text = "Obratni pregled";
            this.ObratniPregledGumb.Click += new System.EventHandler(this.ObratniPregledGumb_Click);
            // 
            // IzvoziGumb
            // 
            this.IzvoziGumb.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.IzvoziGumb.Location = new System.Drawing.Point(15, 22);
            this.IzvoziGumb.Name = "IzvoziGumb";
            this.IzvoziGumb.Size = new System.Drawing.Size(103, 30);
            this.IzvoziGumb.TabIndex = 3;
            this.IzvoziGumb.Text = "Izvozi";
            this.IzvoziGumb.Click += new System.EventHandler(this.IzvoziGumb_Click);
            // 
            // UvoziGumb
            // 
            this.UvoziGumb.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.UvoziGumb.Location = new System.Drawing.Point(121, 22);
            this.UvoziGumb.Name = "UvoziGumb";
            this.UvoziGumb.Size = new System.Drawing.Size(103, 30);
            this.UvoziGumb.TabIndex = 4;
            this.UvoziGumb.Text = "Uvozi";
            this.UvoziGumb.Click += new System.EventHandler(this.UvoziGumb_Click);
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(12, 12);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(327, 253);
            this.pictureBox.TabIndex = 5;
            this.pictureBox.TabStop = false;
            // 
            // radioGumbDodaj
            // 
            this.radioGumbDodaj.AutoSize = true;
            this.radioGumbDodaj.Location = new System.Drawing.Point(6, 19);
            this.radioGumbDodaj.Name = "radioGumbDodaj";
            this.radioGumbDodaj.Size = new System.Drawing.Size(58, 19);
            this.radioGumbDodaj.TabIndex = 9;
            this.radioGumbDodaj.TabStop = true;
            this.radioGumbDodaj.Text = "Dodaj";
            this.radioGumbDodaj.UseVisualStyleBackColor = true;
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.radioGumbOdstrani);
            this.groupBox.Controls.Add(this.radioGumbDodaj);
            this.groupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.groupBox.Location = new System.Drawing.Point(15, 104);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(92, 76);
            this.groupBox.TabIndex = 10;
            this.groupBox.TabStop = false;
            // 
            // radioGumbOdstrani
            // 
            this.radioGumbOdstrani.AutoSize = true;
            this.radioGumbOdstrani.Location = new System.Drawing.Point(6, 44);
            this.radioGumbOdstrani.Name = "radioGumbOdstrani";
            this.radioGumbOdstrani.Size = new System.Drawing.Size(71, 19);
            this.radioGumbOdstrani.TabIndex = 12;
            this.radioGumbOdstrani.TabStop = true;
            this.radioGumbOdstrani.Text = "Odstrani";
            this.radioGumbOdstrani.UseVisualStyleBackColor = true;
            // 
            // textBoxDodaj
            // 
            this.textBoxDodaj.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textBoxDodaj.Location = new System.Drawing.Point(113, 124);
            this.textBoxDodaj.Name = "textBoxDodaj";
            this.textBoxDodaj.Size = new System.Drawing.Size(45, 21);
            this.textBoxDodaj.TabIndex = 11;
            // 
            // RazveljaviGumb
            // 
            this.RazveljaviGumb.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.RazveljaviGumb.Location = new System.Drawing.Point(15, 222);
            this.RazveljaviGumb.Name = "RazveljaviGumb";
            this.RazveljaviGumb.Size = new System.Drawing.Size(103, 30);
            this.RazveljaviGumb.TabIndex = 12;
            this.RazveljaviGumb.Text = "Razveljavi";
            this.RazveljaviGumb.Click += new System.EventHandler(this.RazveljaviGumb_Click);
            // 
            // ObnoviGumb
            // 
            this.ObnoviGumb.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ObnoviGumb.Location = new System.Drawing.Point(124, 222);
            this.ObnoviGumb.Name = "ObnoviGumb";
            this.ObnoviGumb.Size = new System.Drawing.Size(103, 30);
            this.ObnoviGumb.TabIndex = 13;
            this.ObnoviGumb.Text = "Obnovi";
            this.ObnoviGumb.Click += new System.EventHandler(this.ObnoviGumb_Click);
            // 
            // textBoxPoisci
            // 
            this.textBoxPoisci.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textBoxPoisci.Location = new System.Drawing.Point(124, 191);
            this.textBoxPoisci.Name = "textBoxPoisci";
            this.textBoxPoisci.Size = new System.Drawing.Size(45, 21);
            this.textBoxPoisci.TabIndex = 14;
            // 
            // GumbPoisci
            // 
            this.GumbPoisci.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.GumbPoisci.Location = new System.Drawing.Point(15, 186);
            this.GumbPoisci.Name = "GumbPoisci";
            this.GumbPoisci.Size = new System.Drawing.Size(103, 30);
            this.GumbPoisci.TabIndex = 15;
            this.GumbPoisci.Text = "Poišči podatek:";
            this.GumbPoisci.Click += new System.EventHandler(this.GumbPoisci_Click);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(1484, 761);
            this.Controls.Add(this.GumbPoisci);
            this.Controls.Add(this.textBoxPoisci);
            this.Controls.Add(this.ObnoviGumb);
            this.Controls.Add(this.RazveljaviGumb);
            this.Controls.Add(this.textBoxDodaj);
            this.Controls.Add(this.groupBox);
            this.Controls.Add(this.UvoziGumb);
            this.Controls.Add(this.IzvoziGumb);
            this.Controls.Add(this.PremiPregledGumb);
            this.Controls.Add(this.VmesniPregledGumb);
            this.Controls.Add(this.ObratniPregledGumb);
            this.Controls.Add(this.pictureBox);
            this.Name = "Form1";
            this.Text = "Binary Tree Visualization";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Button IzvoziGumb;
        private System.Windows.Forms.Button UvoziGumb;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.RadioButton radioGumbDodaj;
        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.RadioButton radioGumbOdstrani;
        private System.Windows.Forms.TextBox textBoxDodaj;
        private System.Windows.Forms.Button RazveljaviGumb;
        private System.Windows.Forms.Button ObnoviGumb;
        private System.Windows.Forms.TextBox textBoxPoisci;
        private System.Windows.Forms.Button GumbPoisci;
    }
}