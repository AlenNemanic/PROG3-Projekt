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
            this.radioButtonDodaj = new System.Windows.Forms.RadioButton();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.radioButtonPovezi = new System.Windows.Forms.RadioButton();
            this.radioButtonOdstrani = new System.Windows.Forms.RadioButton();
            this.radioButtonPremakni = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // PremiPregledGumb
            // 
            this.PremiPregledGumb.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.PremiPregledGumb.Location = new System.Drawing.Point(15, 102);
            this.PremiPregledGumb.Name = "PremiPregledGumb";
            this.PremiPregledGumb.Size = new System.Drawing.Size(103, 30);
            this.PremiPregledGumb.TabIndex = 0;
            this.PremiPregledGumb.Text = "Premi pregled";
            this.PremiPregledGumb.Click += new System.EventHandler(this.PremiPregledGumb_Click);
            // 
            // VmesniPregledGumb
            // 
            this.VmesniPregledGumb.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.VmesniPregledGumb.Location = new System.Drawing.Point(121, 102);
            this.VmesniPregledGumb.Name = "VmesniPregledGumb";
            this.VmesniPregledGumb.Size = new System.Drawing.Size(103, 30);
            this.VmesniPregledGumb.TabIndex = 1;
            this.VmesniPregledGumb.Text = "Vmesni pregled";
            this.VmesniPregledGumb.Click += new System.EventHandler(this.VmesniPregledGumb_Click);
            // 
            // ObratniPregledGumb
            // 
            this.ObratniPregledGumb.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ObratniPregledGumb.Location = new System.Drawing.Point(227, 102);
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
            // 
            // UvoziGumb
            // 
            this.UvoziGumb.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.UvoziGumb.Location = new System.Drawing.Point(121, 22);
            this.UvoziGumb.Name = "UvoziGumb";
            this.UvoziGumb.Size = new System.Drawing.Size(103, 30);
            this.UvoziGumb.TabIndex = 4;
            this.UvoziGumb.Text = "Uvozi";
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(15, 476);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(350, 500);
            this.pictureBox.TabIndex = 5;
            this.pictureBox.TabStop = false;
            // 
            // radioButtonDodaj
            // 
            this.radioButtonDodaj.AutoSize = true;
            this.radioButtonDodaj.Location = new System.Drawing.Point(6, 19);
            this.radioButtonDodaj.Name = "radioButtonDodaj";
            this.radioButtonDodaj.Size = new System.Drawing.Size(58, 19);
            this.radioButtonDodaj.TabIndex = 9;
            this.radioButtonDodaj.TabStop = true;
            this.radioButtonDodaj.Text = "Dodaj";
            this.radioButtonDodaj.UseVisualStyleBackColor = true;
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.radioButtonPovezi);
            this.groupBox.Controls.Add(this.radioButtonOdstrani);
            this.groupBox.Controls.Add(this.radioButtonPremakni);
            this.groupBox.Controls.Add(this.radioButtonDodaj);
            this.groupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.groupBox.Location = new System.Drawing.Point(15, 138);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(92, 124);
            this.groupBox.TabIndex = 10;
            this.groupBox.TabStop = false;
            // 
            // radioButtonPovezi
            // 
            this.radioButtonPovezi.AutoSize = true;
            this.radioButtonPovezi.Location = new System.Drawing.Point(6, 94);
            this.radioButtonPovezi.Name = "radioButtonPovezi";
            this.radioButtonPovezi.Size = new System.Drawing.Size(61, 19);
            this.radioButtonPovezi.TabIndex = 13;
            this.radioButtonPovezi.TabStop = true;
            this.radioButtonPovezi.Text = "Poveži";
            this.radioButtonPovezi.UseVisualStyleBackColor = true;
            // 
            // radioButtonOdstrani
            // 
            this.radioButtonOdstrani.AutoSize = true;
            this.radioButtonOdstrani.Location = new System.Drawing.Point(6, 69);
            this.radioButtonOdstrani.Name = "radioButtonOdstrani";
            this.radioButtonOdstrani.Size = new System.Drawing.Size(71, 19);
            this.radioButtonOdstrani.TabIndex = 12;
            this.radioButtonOdstrani.TabStop = true;
            this.radioButtonOdstrani.Text = "Odstrani";
            this.radioButtonOdstrani.UseVisualStyleBackColor = true;
            // 
            // radioButtonPremakni
            // 
            this.radioButtonPremakni.AutoSize = true;
            this.radioButtonPremakni.Location = new System.Drawing.Point(6, 44);
            this.radioButtonPremakni.Name = "radioButtonPremakni";
            this.radioButtonPremakni.Size = new System.Drawing.Size(78, 19);
            this.radioButtonPremakni.TabIndex = 10;
            this.radioButtonPremakni.TabStop = true;
            this.radioButtonPremakni.Text = "Premakni";
            this.radioButtonPremakni.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(1184, 561);
            this.Controls.Add(this.groupBox);
            this.Controls.Add(this.UvoziGumb);
            this.Controls.Add(this.IzvoziGumb);
            this.Controls.Add(this.PremiPregledGumb);
            this.Controls.Add(this.VmesniPregledGumb);
            this.Controls.Add(this.ObratniPregledGumb);
            this.Controls.Add(this.pictureBox);
            this.Name = "Form1";
            this.Text = "Binary Tree Visualization";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Button IzvoziGumb;
        private System.Windows.Forms.Button UvoziGumb;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.RadioButton radioButtonDodaj;
        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.RadioButton radioButtonPremakni;
        private System.Windows.Forms.RadioButton radioButtonOdstrani;
        private System.Windows.Forms.RadioButton radioButtonPovezi;
    }
}