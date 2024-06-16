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

        private System.Windows.Forms.Button preOrderButton;
        private System.Windows.Forms.Button inOrderButton;
        private System.Windows.Forms.Button postOrderButton;

        private void InitializeComponent()
        {
            this.preOrderButton = new System.Windows.Forms.Button();
            this.inOrderButton = new System.Windows.Forms.Button();
            this.postOrderButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // preOrderButton
            // 
            this.preOrderButton.Location = new System.Drawing.Point(12, 12);
            this.preOrderButton.Name = "preOrderButton";
            this.preOrderButton.Size = new System.Drawing.Size(100, 30);
            this.preOrderButton.Text = "Pre-Order";
            this.preOrderButton.Click += new System.EventHandler(this.PreOrderButton_Click);
            this.Controls.Add(this.preOrderButton);
            // 
            // inOrderButton
            // 
            this.inOrderButton.Location = new System.Drawing.Point(120, 12);
            this.inOrderButton.Name = "inOrderButton";
            this.inOrderButton.Size = new System.Drawing.Size(100, 30);
            this.inOrderButton.Text = "In-Order";
            this.inOrderButton.Click += new System.EventHandler(this.InOrderButton_Click);
            this.Controls.Add(this.inOrderButton);
            // 
            // postOrderButton
            // 
            this.postOrderButton.Location = new System.Drawing.Point(230, 12);
            this.postOrderButton.Name = "postOrderButton";
            this.postOrderButton.Size = new System.Drawing.Size(100, 30);
            this.postOrderButton.Text = "Post-Order";
            this.postOrderButton.Click += new System.EventHandler(this.PostOrderButton_Click);
            this.Controls.Add(this.postOrderButton);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Name = "Form1";
            this.Text = "Binary Tree Visualization";
            this.ResumeLayout(false);
        }

    }
}